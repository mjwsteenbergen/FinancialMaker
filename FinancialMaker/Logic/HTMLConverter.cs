using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace FinancialMaker.Logic
{
    static class HTMLConverter
    {
        public static string Parse(string input)
        {
            var convertToObject = ConvertToObject(input);
            convertToObject.Reverse();
            List<Rule> rules = new List<Rule>();
            //string res = ConvertToExcelObject(convertToObject, rules);
            Rule rule = new Rule(new List<Condition>(), new List<Consequence>());
            //rule.Apply();
            foreach (Transaction trans in convertToObject)
            {

            }
            return "";
        }

        public static string ConvertToLine()
        {
            return "";
        }

        public static List<Transaction> ConvertToObject(string input)
        {
            List<Transaction> transactions = new List<Transaction>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(input);

            if (doc.DocumentNode.ChildNodes[0].ChildNodes.Count == 0)
            {
                throw new ArgumentException("We could not detect a single transaction");
            }

            foreach (HtmlNode node in doc.DocumentNode.ChildNodes[0].ChildNodes)
            {
                if (node.Name == "li" && node.InnerText != "Eerdere bij- en afschrijvingen")
                {
                    transactions.Add(ConvertNode(node));
                }
            } 

            return transactions;
        }

        public static Transaction ConvertNode(HtmlNode node)
        {
            HtmlNode dateNode = node.ChildNodes[0];
            HtmlNode fromNode = node.ChildNodes[1].ChildNodes[1];
            HtmlNode minusNode = node.ChildNodes[2];
            HtmlNode plusNode = node.ChildNodes[3];

            DateTime date = DateTime.MinValue;

            try
            {
                date = DateTime.Parse(dateNode.InnerText.Replace("`", "20").Replace("okt", "oct"));
            }
            catch (Exception e)
            {
                bool b = true;
                throw;
            }

            string from = fromNode.ChildNodes[0].InnerText;

            string fromAccount = "";
            if (fromNode.ChildNodes.Count > 2)
            {
                fromAccount = fromNode.ChildNodes[2].InnerText;//.Replace("\r\n","");
            }

            string minusText = minusNode.InnerText.Replace(".", "").Replace(",",".");
            string plusText  = plusNode.InnerText.Replace(".", "").Replace(",", ".");

            double amount = plusText != ""
                ? double.Parse(plusText.Replace("+ ", ""))
                : double.Parse(minusText.Replace("- ", "")) * -1;

            return new Transaction {AccountNumber = fromAccount, Amount = amount, date = date, Name = from};
        }
    }
}
