using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionRecordBotClassLibrary.Models;

namespace TransactionRecordBotClassLibrary
{
    public static class DataManipulation
    {
        public static string GroupTransactionsByCategory(List<TransactionModel> transactions)
        {
            string output = "";
            int categoryCount = Enum.GetNames(typeof(ExpensesCategory)).Count();
            decimal[] categoryAmount = new decimal[categoryCount];
            decimal total = 0;
            foreach (TransactionModel transaction in transactions)
            {
                categoryAmount[(int)transaction.category] += transaction.amount;
                total += transaction.amount;
            }
            int counter = 0;
            foreach (var cat in Enum.GetNames(typeof(ExpensesCategory)))
            {
                output += $"{cat}: {Math.Round(categoryAmount[counter])}\n";
                counter++;
            }
            output += $"Total: {Math.Round(total)}";
            return output;
        }
    }
}
