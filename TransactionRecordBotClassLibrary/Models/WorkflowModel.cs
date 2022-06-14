using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionRecordBotClassLibrary.Models
{
    public class WorkflowModel
    {
        public long UserId { get; set; }
        public WorkflowType workflowType { get; set; }
        public int step { get; set; }
        public TransactionModel transaction { get; set; } = new TransactionModel();

        public WorkflowModel(long uid, WorkflowType wft)
        {
            UserId = uid;
            workflowType = wft;
            step = 0;
        }
        public WorkflowModel()
        {

        }
    }
}
