using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMaker.Logic
{
    public class Rule
    {
        public List<Condition> Conditions { get; set; }
        public List<Consequence> Consequences { get; set; }

        public Rule(List<Condition> conditions, List<Consequence> consequence)
        {
            Conditions = conditions;
            Consequences = consequence;
        }

        public List<ExcelObject> Apply(List<ExcelObject> transs)
        {
            List<ExcelObject> excelRows = new List<ExcelObject>();
            foreach(ExcelObject trans in transs)
            {
                excelRows.Add(CheckConditions(trans) ? Replace(trans) : trans);
            }
             
            return excelRows;
        }


        public bool CheckConditions(Transaction trans)
        {
            foreach (Condition cond in Conditions)
            {
                var propertyInfo = trans.GetType().GetProperty(cond.Item.ToString());
                string transValue = propertyInfo.GetValue(trans).ToString().Replace(" ", "");
                switch (cond.Form)
                {
                    case Sign.Equals:
                        if (cond.Query.ToLower() != transValue.ToLower())
                        {
                            return false;
                        }
                        break;
                    case Sign.Contains:
                        if (!transValue.ToLower().Contains(cond.Query.ToLower()))
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        public ExcelObject Replace(ExcelObject e)
        { 
            foreach(Consequence conq in Consequences)
            {
                var propertyInfo = e.GetType().GetProperty(conq.Column.ToString());
                switch(propertyInfo.Name)
                {
                    case "TransactionType":
                        switch (conq.Replacement.ToLower())
                        {
                            case "monthly":
                                e.TransactionType = TransactionType.Montly;
                                break;
                            case "random":
                                e.TransactionType = TransactionType.Random;
                                break;
                            default:
                                throw new Exception("Something really weird is going on. Transaction type is not monthly/random");

                        }
                        break;
                    default:
                        propertyInfo.SetValue(e, conq.Replacement);
                        break;

                }
            }

            return e;
        }
    }

    public class Condition
    {
        public Category Item { get; set; } 
        public Sign Form { get; set; }
        public string Query
        {
            get;
            set;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Condition tobj = (Condition)obj;

            return tobj.Form == Form && tobj.Item == Item && tobj.Query == Query;
        }
    }

    public class Consequence
    {
        public Column Column { get; set; }
        public string Replacement         {
            get;
            set;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Consequence tobj = (Consequence)obj;

            return tobj.Column == Column && tobj.Replacement == Replacement;
        }
    }

    

    public enum Sign { Equals, Contains }
    public enum Category { Amount, Date, AccountNumber, Name }
    public enum Column { TransactionType, Date, Description, Category, Amount,
        Name
    }
}
