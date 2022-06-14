using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionRecordBotClassLibrary.Models
{
    public class TransactionModel
    {
        public decimal amount { get; set; }
        public ExpensesCategory category { get; set; }
        public PlaceModel place { get; set; } = new PlaceModel();
        public DateTime date { get; set; }
    }
}
