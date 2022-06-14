using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using TransactionRecordBotClassLibrary;
using TransactionRecordBotClassLibrary.Handlers;
using System.Web;

namespace FunctionApp1
{
    public static class AzureBottolinoFunction
    {
        [FunctionName("AzureBottolino")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Retrieve connection strings and bot token
            GlobalConfig.SQLconnectionstring = AzureBottolinoApp.EnviromentalConfig.CnnValFromSecret("TransactionBotAzure");
            GlobalConfig.BotToken = AzureBottolinoApp.EnviromentalConfig.CnnValFromSecret("Bottolino");

            TelegramBotClient botClient = new TelegramBotClient(GlobalConfig.BotToken);
            using var cts = new CancellationTokenSource();



            string name = req.Query["name"];

            string requestBody = await req.ReadAsStringAsync();
            log.LogInformation(requestBody);
            //requestBody = AzureBottolinoApp.EnviromentalConfig.Trimbody(requestBody);

            try
            {
                Update update = JsonConvert.DeserializeObject<Update>(requestBody);
                await GeneralHandlers.UpdateHandlerAsync(botClient, update, cts.Token);
            }
            catch (Exception ex)
            {
                await GeneralHandlers.ExceptionsHandler(botClient, ex, cts.Token);
            }

            return new OkObjectResult("Request received");
        }
    }
}
