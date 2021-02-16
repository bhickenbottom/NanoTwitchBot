namespace NanoTwitchBot
{
    using System;
    using System.Globalization;
    using NanoTwitchBot.Core;
    using NanoTwitchBot.Core.Storage;

    public class Program
    {
        #region Static Methods

        public static double GetDouble(string name, string instructions, double defaultValue)
        {
            string stringValue = Program.GetString(name, instructions, defaultValue.ToString(CultureInfo.InvariantCulture));
            if (double.TryParse(stringValue, out double parsedValue))
            {
                return parsedValue;
            }

            return defaultValue;
        }

        public static int GetInt(string name, string instructions, int defaultValue)
        {
            string stringValue = Program.GetString(name, instructions, defaultValue.ToString(CultureInfo.InvariantCulture));
            if (int.TryParse(stringValue, out int parsedValue))
            {
                return parsedValue;
            }

            return defaultValue;
        }

        public static string GetString(string name, string instructions, string defaultValue)
        {
            Console.WriteLine(string.Empty);
            Console.ForegroundColor = ConsoleColor.Gray;
            if (defaultValue != null && !string.IsNullOrWhiteSpace(defaultValue))
            {
                Console.WriteLine($"{name}? (Leave blank for {defaultValue})");
            }
            else
            {
                Console.WriteLine($"{name}?");
            }

            if (instructions != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(instructions);
            }

            string stringValue = Console.ReadLine();
            if (stringValue == null || string.IsNullOrWhiteSpace(stringValue))
            {
                return defaultValue;
            }

            return stringValue;
        }

        public static void Main(string[] args)
        {
            // Storage
            NanoTwitchBotStorage storage = NanoTwitchBotStorage.Load();

            // Header
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Nano Twitch Bot version 0.01");

            // Information
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("This bot monitors a Nano account for pending receives");
            Console.WriteLine("and posts donation messages in a channel's Twitch chat.");

            // Nano Account
            storage.NanoAccount = Program.GetString("Nano Account", "The Nano account that will receive donations.", storage.NanoAccount);

            // Polling Interval In Seconds
            storage.PollingIntervalInSeconds = Program.GetInt("Polling Interval In Seconds", "The interval at which the Nano network will be polled.", storage.PollingIntervalInSeconds);

            // Minimum Donation Amount
            storage.MinimumDonationAmount = Program.GetDouble("Minimum Donation Amount", "The minimum doination amount. Donations below this threshold will not be posted to Twitch.", storage.MinimumDonationAmount);

            // Twitch Channel
            storage.TwitchChannel = Program.GetString("Twitch Channel", "The twitch channel for the bot.", storage.TwitchChannel);

            // Twitch Username
            storage.TwitchUsername = Program.GetString("Twitch Username", "The twitch username for the bot.", storage.TwitchUsername);

            // Twitch Auth Token
            string twitchAuthToken = Program.GetString("Twitch Auth Token", "Go to https://twitchapps.com/tmi to get an auth token.", null);

            // Save
            storage.Save();

            // Run
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Starting Bot...");
            try
            {
                NanoTwitchBotCore bot = new NanoTwitchBotCore(storage, twitchAuthToken);
                while (true)
                {
                    foreach (NanoTwitchBotMessage message in bot.GetMessages())
                    {
                        if (message.Type == NanoTwitchBotMessageType.Success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (message.Type == NanoTwitchBotMessageType.Error)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }

                        Console.WriteLine(message.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred.");
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}