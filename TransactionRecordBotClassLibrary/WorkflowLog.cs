using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionRecordBotClassLibrary.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;

namespace TransactionRecordBotClassLibrary
{
    public class WorkflowLog
    {
        public static List<WorkflowModel> Workflows = new List<WorkflowModel>();
        
        public static bool CheckExistingWorkflow(long userid)
        {
            return Workflows.Exists(x => x.UserId == userid);
        }

        public static WorkflowModel CreateNewWorkflow(Update update, WorkflowType workflowType)
        {
            WorkflowModel wf = new WorkflowModel(DataFromMessage.GetUserId(update),workflowType);
            Workflows.Add(wf);
            return wf;
        }

        public static WorkflowModel GetWorkflow(Update update)
        {
            return Workflows[Workflows.FindIndex(x => x.UserId == DataFromMessage.GetUserId(update))];
        }

        public static void DeleteWorkflow(long key)
        {
            Workflows.RemoveAt(Workflows.FindIndex((x) => x.UserId == key));
        }
    }
}
