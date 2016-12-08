using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FinancialMaker.Logic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinancialMaker.Steps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalendarStep : Page, IStep
    {
        private MainPage _page;
        private List<Transaction> _transactions;
        private List<Rule> rules;
        private DateTime pickedMonth;

        public CalendarStep(MainPage _page, List<Transaction> transactions, List<Rule> rules)
        {
            this.InitializeComponent();

            this._page = _page;
            this._transactions = transactions;
            this.rules = rules;
        }

        public string StepName => "Step 3 : Pick the month you want to view";
        public Color StepColor => Color.FromArgb(255, 164, 196, 0);

        private void CalendarDatePicker_OnClosed(object sender, object e)
        {
            CalendarDatePicker picker = sender as CalendarDatePicker;

            if (picker.Date.HasValue)
            {
                MonthBox.Text = "You have picked " + picker.Date.Value.ToString("MMMM");
                pickedMonth = picker.Date.Value.DateTime;
                _page.EnableContinue(new ResultStep(_page, _transactions, rules, pickedMonth));
            }
            else
            {
                MonthBox.Text = "You did not pick a month";
            }

        }
    }
}
