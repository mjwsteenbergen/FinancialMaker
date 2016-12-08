using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMaker.Logic
{
    public class ExcelObject: Transaction
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public TransactionType TransactionType { get; set; }
        public InOut MoneyKind { get; set; }

        public ExcelObject()
        {
            Description = "";
            Category = "";
            TransactionType = TransactionType.Random;
            MoneyKind = InOut.Expense;
        }

        public string ToExcelRandomLines()
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append(MoneyKind == InOut.Income ? "Inkomen" : "Uitgaven");
            builder.Append("\t");

            //Date
            builder.Append(date);
            builder.Append("\t");

            //Description
            builder.Append(Description);
            builder.Append("\t");

            //Category
            builder.Append(Category);
            builder.Append("\t");

            //Amount
            double value = Amount > 0 ? Amount : Amount *-1;
            IFormatProvider myFormatProvider = new CultureInfo("nl").NumberFormat;
            builder.Append(value.ToString(myFormatProvider));
            builder.AppendLine();

            return builder.ToString();
        }

        public static ExcelObject ConvertToExcel(Transaction trans)
        {
            ExcelObject exc = new ExcelObject();
            exc.AccountNumber = trans.AccountNumber;
            exc.Amount = trans.Amount > 0 ? trans.Amount : trans.Amount * -1;
            exc.MoneyKind = trans.Amount > 0 ? InOut.Income : InOut.Expense;
            exc.date = trans.date;
            exc.Name = trans.Name;
            return exc;
        }

        public string ToString
        {
            get
            {
                return Name;
            }
        }

        internal string ToExcelMonthlyLines()
        {
            IFormatProvider myFormatProvider = new CultureInfo("nl").NumberFormat;
            return Amount.ToString(myFormatProvider);
        }
    }

    public enum InOut { Income, Expense}
    public enum TransactionType { Random, Montly }

    
}
