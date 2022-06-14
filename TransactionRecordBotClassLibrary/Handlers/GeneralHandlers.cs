using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using TransactionRecordBotClassLibrary.Models;

namespace TransactionRecordBotClassLibrary.Handlers
{           
    public static class GeneralHandlers
    {
        
        //TODO - add new workflow type to send mail? (check how to send mail from Azure)
        private static Update MessageValidation(Update update)
        {
            //If it is callback then it is safe
            if (update.CallbackQuery != null)
            {
                return update;
            }
            //if message is null then return and it will be skipped in the main script
            else if (update.Message == null)
            {
                return update;
            }
            //If it is not callback and message exists ensure text is not null
            else if (update.Message.Text == null)
            {
                update.Message.Text = " ";
            }
            return update;
        }

        public static async Task UpdateHandlerAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery) await Messages.RemoveInlineKeyboard(botClient, update);
            update = MessageValidation(update);
            if (update.CallbackQuery == null && (update.Message == null || update.Message.From == null)) return;
            //in case it's a valid workflow start word then proceed to check if wf is yet present
            if (update.CallbackQuery == null && Enum.IsDefined(typeof(WorkflowType), update.Message.Text.Substring(1)))
            {
                WorkflowType wft = Enum.Parse<WorkflowType>(update.Message.Text.Substring(1));
                if (WorkflowLog.CheckExistingWorkflow(DataFromMessage.GetUserId(update)))
                {
                    if (wft == WorkflowType.delete)
                    {
                        WorkflowLog.DeleteWorkflow(DataFromMessage.GetUserId(update));
                        await Messages.GeneralMessageAsync(botClient, update, "Workflow deleted correctly");
                    }
                    else
                    {
                        WorkflowModel wf = WorkflowLog.GetWorkflow(update);
                        await Messages.GeneralMessageAsync(botClient, update, $"There is already a workflow open:\n--> {wf.workflowType} at step {wf.step}\nUse /delete to delete it");
                    }
                }
                else
                {
                    if (wft == WorkflowType.delete)
                    {
                        await Messages.GeneralMessageAsync(botClient, update, "There is no open workflow");
                    }
                    else
                    {
                        WorkflowModel wf = WorkflowLog.CreateNewWorkflow(update, wft);
                        await WorkflowHandlers.GoToNextStep(botClient, update, wf);
                    }
                }
            }
            else
            {
                if (WorkflowLog.CheckExistingWorkflow(DataFromMessage.GetUserId(update)))
                {
                    WorkflowModel wf = WorkflowLog.GetWorkflow(update);
                    await WorkflowHandlers.GoToNextStep(botClient, update, wf);
                }
                else
                {
                    await Messages.GeneralMessageAsync(botClient, update, "I do not know what to do with this... please open a workflow with an inline request");
                }
            }
        }

        public static Task ExceptionsHandler(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
        {
            var ErrorMessage = ex switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => ex.ToString()
            };
            return Task.CompletedTask;
        }
    }
}
