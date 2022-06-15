using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionRecordBotClassLibrary
{
    public enum WorkflowType
    {
        start,
        insert,
        delete,
        updateemail,
        report
    }
    public enum ExpensesCategory
    {
        Home,
        Food,
        Transportation,
        Clothes,
        Trips,
        Others
    }
    public enum Answers
    {
        Yes,
        No,
        Skip
    }
    public enum Reports
    {
        AllTime,
        CurrentMonth
    }

}
