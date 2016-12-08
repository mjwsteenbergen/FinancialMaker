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
        private Memory mem;

        public string StepName => "Step 2 : Import your rules";
        public Color StepColor => Color.FromArgb(255, 240, 163, 10);

        public ImportRules(MainPage page, List<Transaction> transactions)
        {
            _page = page;
            _transactions = transactions;
            this.InitializeComponent();
            LoadRules();
            this.Loaded += (o, e) =>
            {
                CheckText(RulesBox.Text);
            };
        }

        public void LoadRules()
        {
            mem = new Memory(ApplicationData.Current.RoamingFolder.Path + "\\");
            RulesBox.Text = mem.ReadFile("Rules.txt");
            
        }

        private async void RulesBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            CheckText(box.Text);
        }

        private void CheckText(string text)
        {
            if (text == "")
            {
                _page.Error("There is no text");
                return;
            }
            try
            {
                List<Rule> rules = new List<Rule>();
                foreach (string s in text.Split('\r'))
                {
                    Rule newRule = RuleParser.Parse(s);
                    if (newRule != null)
                    {
                        rules.Add(newRule);
                    }
                }

                _page.EnableContinue(new CalendarStep(_page, _transactions, rules));
                mem.WriteFile("Rules.txt", text);
            }
            catch (Exception e2)
            {
                _page.Error(e2.Message);
            }
        }
    }
}
