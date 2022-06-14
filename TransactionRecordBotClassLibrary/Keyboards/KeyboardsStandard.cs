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
    public class KeyboardsStandard
    {
        public ReplyKeyboardMarkup Keyboard_Numpad { get; }
        public ReplyKeyboardMarkup Keyboard_YesNo { get; }
        public ReplyKeyboardMarkup Keyboard_CategoryChoice { get; }
        public ReplyKeyboardMarkup Keyboard_Skip { get; }

        public KeyboardsStandard()
        {
            Keyboard_YesNo = new(
                new[]
                {
                    new KeyboardButton(Answers.Yes.ToString()),
                    new KeyboardButton(Answers.No.ToString())
                });
            Keyboard_CategoryChoice = new(
                new[]
                {
                    new[]
                    {
                        new KeyboardButton(ExpensesCategory.Home.ToString()),
                        new KeyboardButton(ExpensesCategory.Food.ToString()),
                        new KeyboardButton(ExpensesCategory.Transportation.ToString())
                    },
                    new[]
                    {
                        new KeyboardButton(ExpensesCategory.Clothes.ToString()),
                        new KeyboardButton(ExpensesCategory.Trips.ToString()),
                        new KeyboardButton(ExpensesCategory.Others.ToString())
                    }
                });

            Keyboard_Skip = new(
                new[]
                {
                    new KeyboardButton(Answers.Skip.ToString())
                });


            Keyboard_Numpad = new(
                new[]
                {
                    new KeyboardButton("1"),
                    new KeyboardButton("2"),
                    new KeyboardButton("3"),
                    new KeyboardButton("4"),
                    new KeyboardButton("5"),
                    new KeyboardButton("6"),
                    new KeyboardButton("7"),
                    new KeyboardButton("8"),
                    new KeyboardButton("9"),
                });

        }
    }
}
