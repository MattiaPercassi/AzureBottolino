# Azure Bottolino

[DUE TO FREE TRIAL AZURE ACCOUNT THE BOT IS CURRENTLY UNAVAILABLE]

This is the code for interacting with a Telegram Bot named Bottolino.

You can find the bot by searching for @bottolino_bot in Telegram!

The main purpose of the bot is to record the daily transactions and store them in an SQL database.
To do so the bot will provide a series of messages meant to record the key information being the amount of the transaction, the expenditure category (currently hardcoded) and the location.
Transaction data will be saved in a specific table in the database.
The user name and the email (if available) are stored in a different database table.
User has the following options:
- record a transaction
- change the associated email
- receive a brief summary of the expenses divided by category (either current month either all time) 

The bot runs on Azure functions and it is linked to Telegram servers throgh a webhook
The database the bot uses for data storage is an Azure SQL database.
