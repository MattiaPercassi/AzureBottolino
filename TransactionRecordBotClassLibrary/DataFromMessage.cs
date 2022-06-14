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

namespace TransactionRecordBotClassLibrary
{
    public static class DataFromMessage
    {
        public static long GetUserId(Update update)
        {
            long id;
            if (update.CallbackQuery != null)
            {
                id = update.CallbackQuery.From.Id;
            }
            else
            {
                id = update.Message.From.Id;
            }
            return id;
        }
        public static string GetUserFirstName(Update update)
        {
            string? firstName;
            if (update.CallbackQuery != null)
            {
                firstName = update.CallbackQuery.From.FirstName;
            }
            else
            {
                firstName = update.Message.From.FirstName;
            }
            if (firstName == null) firstName = "";
            return firstName;
        }

        public static string GetUserLastName(Update update)
        {
            string? lastName;
            if (update.CallbackQuery != null)
            {
                lastName = update.CallbackQuery.From.LastName;
            }
            else
            {
                lastName = update.Message.From.LastName;
            }
            if (lastName == null) lastName = "";
            return lastName;
        }
        public static long GetCHatId(Update update)
        {
            long id;
            if (update.CallbackQuery != null)
            {
                id = update.CallbackQuery.Message.Chat.Id;
            }
            else
            {
                id = update.Message.Chat.Id;
            }
            return id;
        }
        /// <summary>
        /// Returns a PlaceModel from an update
        /// Location or Venue field of update must not be null
        /// </summary>
        /// <param name="update">Update received from bot</param>
        /// <returns></returns>
        public static PlaceModel GetPlace(Update update)
        {
            if (update.Message.Venue != null)
            {
                PlaceModel place = new PlaceModel(update.Message.Venue.Title, update.Message.Venue.Location.Latitude, update.Message.Venue.Location.Longitude); ;
                return place;
            }
            else
            {
                PlaceModel place = new PlaceModel("Unnamed Shop",update.Message.Location.Latitude,update.Message.Location.Longitude);
                return place;
            }
        }
    }
}
