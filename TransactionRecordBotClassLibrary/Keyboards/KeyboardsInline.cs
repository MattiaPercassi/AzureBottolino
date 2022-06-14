using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using TransactionRecordBotClassLibrary.Models;

namespace TransactionRecordBotClassLibrary.Keyboards
{
    public class KeyboardsInline
    {
        public InlineKeyboardMarkup Keyboard_YesNo { get; }
        public InlineKeyboardMarkup Keyboard_CategoryChoice { get; }
        public InlineKeyboardMarkup Keyboard_Skip { get; }
        public KeyboardsInline()
        {
            Keyboard_YesNo = new(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Answers.Yes.ToString()),
                    InlineKeyboardButton.WithCallbackData(Answers.No.ToString()),
                }
            );

            Keyboard_Skip = new(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Answers.Skip.ToString())
                }
             );

            Keyboard_CategoryChoice = new(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Home.ToString()),
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Food.ToString()),
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Transportation.ToString()),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Clothes.ToString()),
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Trips.ToString()),
                        InlineKeyboardButton.WithCallbackData(ExpensesCategory.Others.ToString()),
                    },
                }

            );
        }
    }
}
