using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMaker.Logic
{
    public class RuleParser
    {
        public static Rule Parse(string input)
        {
            if (input == "" || input.StartsWith("//") || input.StartsWith("#"))
            {
                return null;
            }
            string[] sides = input.Split(new string[] { "=>" }, StringSplitOptions.None);
            if(sides.Length != 2)
            {
                throw new ParseException("You did not use the arrow correctly");
            }

            string conditions = sides[0];
            string consequences = sides[1];

            return new Rule(ParseConditions(conditions), ParseConsequences(consequences));
        }

        public static List<Consequence> ParseConsequences(string consequences)
        {
            List<Consequence> result = new List<Consequence>();
            string[] consequenceArray = consequences.Split(' ');
            foreach (string consequence in consequenceArray)
            {
                if (consequence == "")
                {
                    continue;
                }
                result.Add(ParseConsequence(consequence));
            }
            return result;
        }

        public static Consequence ParseConsequence(string consequence)
        {
            Consequence conq = new Consequence();
            string[] pieces = consequence.Split(':');
            if (pieces.Length != 2)
            {
                throw new ParseException("The consequence " + consequence + " was not made correctly");
            }
            conq.Replacement = pieces[1];
            conq.Column = ConvertToColumn(pieces[0].ToLower(), conq);

            return conq;
        }

        public static Column ConvertToColumn(string s, Consequence c)
        {
            switch (s)
            {
                case "type":
                    if (!(c.Replacement.ToLower() == "monthly" || c.Replacement.ToLower() == "random"))
                    {
                        throw new ParseException("Transaction type is not monthly/random");
                    }
                    return Column.TransactionType;
                case "desc":
                    return Column.Description;
                case "cat":
                    return Column.Category;
                case "name":
                    return Column.Name;
                default:
                    throw new ParseException("Could not find the right Column");
            }
        }

        public static List<Condition> ParseConditions(string conditions)
        {
            List<Condition> result = new List<Condition>();
            string[] conditionArray = conditions.Split(' ');
            foreach(string condition in conditionArray)
            {
                if(condition == "")
                {
                    continue;
                }
                result.Add(ParseCondition(condition));
            }
            return result;
        }

        public static Condition ParseCondition(string condition)
        {
            Condition cond = new Condition();
            string[] pieces = condition.Split(':');
            if(!(pieces.Length == 2 || pieces.Length == 3))
            {
                throw new ParseException("The condition was not made correctly");
            }
            cond.Form = Sign.Equals;

            foreach (Category c in Enum.GetValues(typeof(Category)))
            {
                string cName = c.ToString();
                cName = cName.Length > 7 ? cName.Substring(0, 7) : cName;
                if (cName.ToLower() == pieces[0].ToLower())
                {
                    cond.Item = c;
                    break;
                }

            }

            if (pieces.Length == 3)
            {
                switch(pieces[1])
                {
                    case "e":
                        cond.Form = Sign.Equals;
                        break;
                    case "c":
                        cond.Form = Sign.Contains;
                        break;
                    default:
                        throw new ParseException("Wrong sign");
                }
                cond.Query = pieces[2];
                
            }
            else
            {
                cond.Form = Sign.Equals;
                cond.Query = pieces[1];
            }

            return cond;
        }
    }


    public class ParseException : InvalidOperationException
    {
        public ParseException() : base() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, System.Exception inner) : base(message, inner) { }
    }
}
