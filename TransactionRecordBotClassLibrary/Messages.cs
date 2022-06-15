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
using TransactionRecordBotClassLibrary.Keyboards;
using Telegram.Bot.Types.ReplyMarkups;

namespace TransactionRecordBotClassLibrary
{
    public static class Messages
    {
        public static async Task GeneralMessageAsync(ITelegramBotClient botClient,Update update, string text)
        {
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), text);
        }
        public static async Task CategoryMessageAsync(ITelegramBotClient botClient, Update update)
        {
            KeyboardsStandard keyboards = new KeyboardsStandard();
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), "In which category?", replyMarkup: keyboards.Keyboard_CategoryChoice);
        }

        public static async Task ReportChooseMessage(ITelegramBotClient botClient, Update update)
        {
            //TODO - implement function
            throw new NotImplementedException();
        }
        public static async Task TransactionConfirmationMessageAsync(ITelegramBotClient botClient, Update update, TransactionModel transaction)
        {
            string msg = $"{transaction.amount} JPY for {transaction.category} at {transaction.place.Name} (lat: {transaction.place.Latitude} lon: {transaction.place.Longitude})\nConfirm?";
            KeyboardsStandard keyboards = new KeyboardsStandard();
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), msg, replyMarkup: keyboards.Keyboard_YesNo);
        }

        public static async Task RemoveInlineKeyboard(ITelegramBotClient botClient, Update update)
        {
            //TODO - do method for retrieving message ID always
            await botClient.EditMessageTextAsync(DataFromMessage.GetCHatId(update), update.CallbackQuery.Message.MessageId, update.CallbackQuery.Data, replyMarkup: null);
        }
        
        public static async Task RemoveCustomKeyboard(ITelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), "Got it", replyMarkup: new ReplyKeyboardRemove());
        }

        public static async Task NumpadAsync(ITelegramBotClient botClient, Update update, string text)
        {
            KeyboardsStandard keyboard = new KeyboardsStandard();
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), text, replyMarkup: keyboard.Keyboard_Numpad);
        }
        public static async Task ReplyOrSkip(ITelegramBotClient botClient, Update update, string text)
        {
            KeyboardsStandard keyboards = new KeyboardsStandard();
            await botClient.SendTextMessageAsync(DataFromMessage.GetCHatId(update), text, replyMarkup: keyboards.Keyboard_Skip);
        }
    }
}
