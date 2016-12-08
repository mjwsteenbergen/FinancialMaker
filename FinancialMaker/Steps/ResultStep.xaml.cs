using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using FinancialMaker.Logic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FinancialMaker.Steps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultStep : Page, IStep
    {
        private readonly MainPage _page;
        private readonly List<Transaction> _transactions;
        private readonly List<Rule> _rules;
        private readonly DateTime _pickedMonth;

        public ResultStep(MainPage _page, List<Transaction> transactions, List<Rule> rules, DateTime pickedMonth)
        {
            this._page = _page;
            _transactions = transactions;
            _rules = rules;
            _pickedMonth = pickedMonth;
            this.InitializeComponent();

            Loaded += CreateResult;
        }

        private void CreateResult(object sender, RoutedEventArgs eventArgs)
        {
//            Montly.Text =
//                "alshfkasjkjlhkaskhlahaasfjkasfjklasfjkasfjkasfjklasfjgkasfkjlasflgjkasjklfaskgjlasfkjasfjkasfkgjasfkgjlasfgjkasfkjaskjlasf";
//            Other.Text = "kjaskjdskjaskhasf; oiuadlifhsdkluhclAHCLKKyasclyKBIYRFLUAGCD YGFQIUHLUYALSKUDVYRLKYSDLKVYCZKU";
//            return;

            List<ExcelObject> excelLines = new List<ExcelObject>();
            foreach (Transaction t in _transactions)
            {
                excelLines.Add(ExcelObject.ConvertToExcel(t));
            }

            foreach (ExcelObject e in excelLines)
            {
                e.Description = e.Name + " Account: " + e.AccountNumber;
            }

            foreach (Rule r in _rules)
            {
                if (r == null)
                {
                    continue;
                }
                excelLines = r.Apply(excelLines);
            }

            excelLines.Reverse();

            string res = "";
            foreach (ExcelObject e in excelLines)
            {
                if (e.TransactionType == TransactionType.Random && _pickedMonth.Month == e.date.Month)
                {
                    res += e.ToExcelRandomLines();
                }
            }

            string monthlyRes = "";

            foreach (Rule r in _rules)
            {
                
                foreach (ExcelObject e in excelLines)
                {
                    if (r.CheckConditions(e) && e.TransactionType == TransactionType.Montly && _pickedMonth.Month == e.date.Month)
                    {
                        monthlyRes += e.ToExcelMonthlyLines();
                    }
                }
                if(r.Consequences.FindAll(consequence => consequence.Column == Column.TransactionType && consequence.Replacement == "monthly").Count > 0)
                {
                    monthlyRes += "\n";
                }

            }

            Montly.Text = monthlyRes;

            Other.Text = res;
        }

        public string StepName => "Step 4 : Copy your results";
        public Color StepColor => Color.FromArgb(255, 229, 20, 0);

        private void GotoRules(object sender, RoutedEventArgs e)
        {
            _page.SetPage(new ImportRules(_page, _transactions));
        }

        private void GotoCalendar(object sender, RoutedEventArgs e)
        {
            _page.SetPage(new CalendarStep(_page, _transactions, _rules));
        }
    }
}
