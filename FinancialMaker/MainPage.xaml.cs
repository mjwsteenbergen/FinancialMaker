using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using FinancialMaker.Logic;
using FinancialMaker.Steps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FinancialMaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IStep nextFrame;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += (o, e) =>
            {
//                SetPage(new CalendarStep(this, new List<Transaction>(), new List<Rule>()));
                SetPage(new ImportHTML(this));
                ContinueButton.Click += (e2, o2) =>
                {
                    SetPage(nextFrame);
                    ResetButton();
                };
                ResetButton();
            };
        }

        public static void ChangeColor(DependencyObject target, Color color)
        {
            ColorAnimation changeColorAnimation = new ColorAnimation
            {
                To = color,
                Duration = TimeSpan.FromSeconds(1)
            };
            Storyboard.SetTarget(changeColorAnimation, target);
            Storyboard.SetTargetName(changeColorAnimation, "MyAnimatedBrush");
            Storyboard.SetTargetProperty(
                changeColorAnimation, "(ContentControl.Background).(SolidColorBrush.Color)");
            Storyboard changeColorStoryboard = new Storyboard();
            changeColorStoryboard.Children.Add(changeColorAnimation);
            changeColorStoryboard.Begin();
        }

        public static Color ChangeBrightness(Color color, double brightness)
        {
            return Color.FromArgb((byte)color.A, (byte)(color.R * brightness), (byte)(color.G * brightness), (byte)(color.B * brightness));
        }

        public void SetPage(IStep frame)
        {
            StepFrame.Content = frame;
            StepName.Text = frame.StepName;
            SetColor(frame.StepColor);
        }

        public void SetColor(Color color)
        {
            this.MainGrid.Background = new SolidColorBrush(color);
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = ChangeBrightness(color, 0.9);
            titleBar.ButtonBackgroundColor = ChangeBrightness(color, 0.9);
        }

        public void ResetButton()
        {
            ContinueButton.IsEnabled = false;
            ContinueButton.Background = null;
            ContinueTextBlock.Text = "";
        }

        public void EnableContinue(IStep nextStep)
        {
            ContinueButton.IsEnabled = true;
            ContinueButton.Background = new SolidColorBrush(Colors.DarkGreen);
            ContinueTextBlock.Text = "Continue";
            nextFrame = nextStep;
        }

        public void Error(string s)
        {
            ContinueButton.IsEnabled = false;
            ContinueTextBlock.Text = s;
        }
    }
}
