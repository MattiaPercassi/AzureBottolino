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
using System.Net.Mail;
using System.IO;
using System.Reflection;


namespace TransactionRecordBotClassLibrary.Handlers
{
    public static class WorkflowHandlers
    {
        public static async Task GoToNextStep(ITelegramBotClient botClient, Update update, WorkflowModel workflow)
        {
            if (!InputValidation(update, workflow))
            {
                await Messages.GeneralMessageAsync(botClient, update, "Invalid input for current workflow");
                return;
            }
            switch (workflow.workflowType)
            {
                case WorkflowType.start:
                    if (!SQLConnector.CheckUserExists(DataFromMessage.GetUserId(update)))
                    {
                        UserModel user = new UserModel(DataFromMessage.GetUserId(update), DataFromMessage.GetUserFirstName(update), DataFromMessage.GetUserLastName(update),"");
                        SQLConnector.SaveUser(user);
                    }
                    WorkflowLog.DeleteWorkflow(DataFromMessage.GetUserId(update));
                    await Messages.GeneralMessageAsync(botClient, update, Properties.Resources.Introduction);
                    break;
                case WorkflowType.insert:
                    switch (workflow.step)
                    {
                        case 0:
                            await Messages.GeneralMessageAsync(botClient, update, "How much did you spend?");
                            workflow.step++;
                            break;
                        case 1:
                            workflow.transaction.amount = decimal.Parse(update.Message.Text);
                            await Messages.CategoryMessageAsync(botClient, update);
                            workflow.step++;
                            break;
                        case 2:
                            await Messages.RemoveCustomKeyboard(botClient, update);
                            workflow.transaction.category = (ExpensesCategory)Enum.Parse(typeof(ExpensesCategory), update.Message.Text);
                            await Messages.ReplyOrSkip(botClient,update, "Please send the location or skip");
                            workflow.step++;
                            break;
                        case 3:
                            await Messages.RemoveCustomKeyboard(botClient, update);
                            if (update.Message.Text == Answers.Skip.ToString())
                            {
                                workflow.transaction.place = new PlaceModel();
                            }
                            else
                            {
                                workflow.transaction.place = DataFromMessage.GetPlace(update);
                            }
                            await Messages.TransactionConfirmationMessageAsync(botClient, update, workflow.transaction);
                            workflow.step++;
                            break;
                        case 4:
                            await Messages.RemoveCustomKeyboard(botClient, update);
                            if (update.Message.Text == Answers.Yes.ToString())
                            {
                                SQLConnector.SaveTransaction(workflow);
                                await Messages.GeneralMessageAsync(botClient, update, "Transaction recorded");
                            }
                            WorkflowLog.DeleteWorkflow(DataFromMessage.GetUserId(update));
                            if (update.Message.Text == Answers.No.ToString())
                            {
                                await Messages.GeneralMessageAsync(botClient, update, "Transaction canceled"); 
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case WorkflowType.updateemail:
                    switch (workflow.step)
                    {
                        case 0:
                            await Messages.GeneralMessageAsync(botClient, update, "What is your email address?");
                            workflow.step++;
                            break;
                        case 1:
                            if (!SQLConnector.CheckUserExists(DataFromMessage.GetUserId(update)))
                            {
                                UserModel user = new UserModel(DataFromMessage.GetUserId(update), DataFromMessage.GetUserFirstName(update), DataFromMessage.GetUserLastName(update), update.Message.Text.Trim());
                                SQLConnector.SaveUser(user);
                            }
                            else
                            {
                                SQLConnector.UpdateEmail(DataFromMessage.GetUserId(update),update.Message.Text.Trim());
                            }
                            await Messages.GeneralMessageAsync(botClient, update, $"Mail registered!\n{update.Message.Text.Trim()}");
                            WorkflowLog.DeleteWorkflow(DataFromMessage.GetUserId(update));
                            break;
                        default:
                            break;
                    }
                    break;
                case WorkflowType.report:
                    switch (workflow.step)
                    {
                        case 0:
                            Messages.ReportChooseMessage(botClient, update);
                            workflow.step++;
                            break;
                        case 1:
                            string msg = "No transactions found..\nUse /insert to record a new transaction";
                            List<TransactionModel> transactions = new List<TransactionModel>();
                            switch (Enum.Parse<Reports>(update.Message.Text))
                            {
                                case Reports.AllTime:
                                    transactions = SQLConnector.GetTransactionsByUserid(DataFromMessage.GetUserId(update));
                                    break;
                                case Reports.LastMonth:
                                    transactions = SQLConnector.GetTransactionsByUserid_LastMonth(DataFromMessage.GetUserId(update));
                                    break;
                                default:
                                    break;
                            }
                            if (transactions.Count != 0)
                            {
                                msg = DataManipulation.GroupTransactionsByCategory(transactions);
                            }
                            Messages.GeneralMessageAsync(botClient, update, msg);
                            break;
                            WorkflowLog.DeleteWorkflow(DataFromMessage.GetUserId(update));
                        default:
                            break;
                    }
                    break;
                case WorkflowType.delete:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Checks if the data received with the update is in a correct format for the next workflow step
        /// </summary>
        /// <param name="update"></param>
        /// <param name="workflow"></param>
        /// <returns>A true if we can proceed with this message for the next step </returns>
        private static bool InputValidation(Update update, WorkflowModel workflow)
        {
            bool validation = false;

            switch (workflow.workflowType)
            {
                case WorkflowType.start:
                    validation = true;
                    break;
                case WorkflowType.insert:
                    switch (workflow.step)
                    {
                        // for the first interaction at step 0 just give the ok to proceed
                        case 0:
                            validation = true;
                            break;
                        case 1:
                            decimal amount;
                            validation = decimal.TryParse(update.Message.Text, out amount);
                            break;
                        case 2:
                            validation = Enum.IsDefined(typeof(ExpensesCategory), update.Message.Text);
                            break;
                        case 3:
                            if (update.Message.Text == Answers.Skip.ToString())
                            {
                                validation = true;
                            }
                            else if (update.Message.Venue != null)
                            {
                                validation = true;
                            }
                            else if (update.Message.Location != null)
                            {
                                validation = true;
                            }
                            break;
                        case 4:
                            validation = Enum.IsDefined(typeof(Answers), update.Message.Text);
                            break;
                        default:
                            break;
                    }
                    break;
                case WorkflowType.updateemail:
                    switch (workflow.step)
                    {
                        case 0:
                            validation = true;
                            break;
                        case 1:
                            try
                            {
                                MailAddress mail = new MailAddress(update.Message.Text.Trim());
                                validation = true;
                            }
                            catch (Exception)
                            {
                                //intentionally left blank, if we cannot validate the email then we
                                //keep the validation at false
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case WorkflowType.report:
                    switch (workflow.step)
                    {
                        case 0:
                            //report does not take any input other than the workflow type
                            validation = true;
                            break;
                        case 1:
                            if (Enum.IsDefined(typeof(Reports),update.Message.Text))
                            {
                                validation = true;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }


            return validation;
        }
    }
}
