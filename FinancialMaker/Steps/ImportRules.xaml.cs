using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FinancialMaker.Logic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinancialMaker.Steps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImportRules : Page, IStep
    {
        private readonly MainPage _page;
        private readonly List<Transaction> _transactions;

        public ImportRules(MainPage page, List<Transaction> transactions)
        {
            _page = page;
            _transactions = transactions;
            this.InitializeComponent();
            LoadRules();
        }

        public void LoadRules()
        {
            Memory mem = new Memory(ApplicationData.Current.RoamingFolder.Path + "\\");
            RulesBox.Text = mem.ReadFile("Rules.txt");
        }

        public string StepName => "Step 2 : Import your rules";
        public Color StepColor => Color.FromArgb(255, 240, 163, 10);

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    List<Rule> rules = new List<Rule>();
                    foreach (string s in await FileIO.ReadLinesAsync(file))
                    {
                        Rule newRule = RuleParser.Parse(s);
                        if (newRule != null)
                        {
                            rules.Add(newRule);
                        }
                    }

                    _page.SetPage(new CalendarStep(_page, _transactions, rules));
                }
                catch (Exception e2)
                {
                    await (new MessageDialog(e2.Message)).ShowAsync();
                }
            }
            else
            {
                await (new MessageDialog("Operation cancelled.")).ShowAsync();
            }
        }
    }
}
