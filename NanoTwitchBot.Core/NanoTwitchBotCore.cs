namespace NanoTwitchBot.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NanoTwitchBot.Core.Nano;
    using NanoTwitchBot.Core.Storage;
    using TwitchLib.Client;
    using TwitchLib.Client.Models;
    using TwitchLib.Communication.Clients;
    using TwitchLib.Communication.Models;

    public class NanoTwitchBotCore
    {
        #region Constructors

        public NanoTwitchBotCore(NanoTwitchBotStorage storage, string twitchAuthToken)
        {
            // Preconditions
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (twitchAuthToken == null || string.IsNullOrWhiteSpace(twitchAuthToken))
            {
                throw new ArgumentNullException(nameof(twitchAuthToken));
            }

            // Storage
            this.Storage = storage;

            // Messages
            this.messages = new List<NanoTwitchBotMessage>();

            // Nano
            NanoClient nanoClient = new NanoClient();
            this.Listener = new NanoListener(nanoClient, this.Storage.NanoAccount, TimeSpan.FromSeconds(this.Storage.PollingIntervalInSeconds), 100);
            this.Listener.Start(this.OnNanoListenerTransaction, this.OnNanoListenerError);

            // Twitch
            ConnectionCredentials credentials = new ConnectionCredentials(this.Storage.TwitchUsername, twitchAuthToken);
            ClientOptions clientOptions = new ClientOptions { MessagesAllowedInPeriod = 100, ThrottlingPeriod = TimeSpan.FromSeconds(30) };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            this.Twitch = new TwitchClient(customClient);
            this.Twitch.Initialize(credentials, this.Storage.TwitchChannel);
            this.Twitch.Connect();

            // Twitch Log
            this.Twitch.OnLog += (s, e) =>
            {
                lock (this.messages)
                {
                    string message = $"{e.DateTime}: {e.Data}";
                    this.messages.Add(new NanoTwitchBotMessage(NanoTwitchBotMessageType.Information, message));
                }
            };

            // Twitch Connected
            this.Twitch.OnConnected += (s, e) =>
            {
                if (this.Twitch.IsConnected)
                {
                    this.Twitch.SendMessage(new JoinedChannel(this.Storage.TwitchChannel), "Nano bot online!");
                }
            };

            // Twitch Message Received
            this.Twitch.OnMessageReceived += (s, e) =>
            {
                // Message
                string message = e.ChatMessage.Message;

                // !nano Command
                if (message.StartsWith("!nano"))
                {
                    if (this.Twitch.IsConnected)
                    {
                        this.Twitch.SendMessage(new JoinedChannel(this.Storage.TwitchChannel), $"This channel accepts Nano donations. Nano is an instant, feeless, eco-friendly cryptocurrency. Send Nano donations to {this.Storage.NanoAccount}. !nano_add <account> adds an alias. !nano_delete deletes an alias. To get started, download the Natrium app.");
                    }
                }

                // !nano_add Command
                if (message.StartsWith("!nano_add"))
                {
                    string account = message.Substring("!nano_add".Length);
                    account = account.Trim();
                    this.AddAddress(e.ChatMessage.DisplayName, account, true);
                    if (this.Twitch.IsConnected)
                    {
                        this.Twitch.SendMessage(new JoinedChannel(this.Storage.TwitchChannel), $"Added Nano alias for {e.ChatMessage.DisplayName}.");
                    }
                }

                // !nano_delete Command
                if (message.StartsWith("!nano_delete"))
                {
                    this.DeleteAddress(e.ChatMessage.DisplayName, true);
                    if (this.Twitch.IsConnected)
                    {
                        this.Twitch.SendMessage(new JoinedChannel(this.Storage.TwitchChannel), $"Deleted Nano alias for {e.ChatMessage.DisplayName}.");
                    }
                }
            };
        }

        #endregion

        #region Fields

        private List<NanoTwitchBotMessage> messages;

        #endregion

        #region Properties

        public NanoListener Listener { get; private set; }

        public NanoTwitchBotStorage Storage { get; private set; }

        public TwitchClient Twitch { get; private set; }

        #endregion

        #region Methods

        private void AddAddress(string displayName, string account, bool save)
        {
            this.DeleteAddress(displayName, false);
            Alias alias = new Alias();
            alias.DisplayName = displayName;
            alias.Account = account;
            this.Storage.Aliases.Add(alias);
            if (save)
            {
                this.Storage.Save();
            }
        }

        private void DeleteAddress(string displayName, bool save)
        {
            int i = 0;
            while (i < this.Storage.Aliases.Count)
            {
                Alias alias = this.Storage.Aliases[i];
                if (alias.DisplayName == displayName)
                {
                    this.Storage.Aliases.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            if (save)
            {
                this.Storage.Save();
            }
        }

        public IEnumerable<NanoTwitchBotMessage> GetMessages()
        {
            lock (this.messages)
            {
                List<NanoTwitchBotMessage> copy = this.messages.ToList();
                this.messages.Clear();
                return copy;
            }
        }

        public void OnNanoListenerError(string error)
        {
            lock (this.messages)
            {
                this.messages.Add(new NanoTwitchBotMessage(NanoTwitchBotMessageType.Error, error));
            }
        }

        public void OnNanoListenerTransaction(NanoListenerTransaction transaction)
        {
            // Message
            string message = $"Anonymous donated {transaction.FriendlyNanoAmount} Nano!";

            // Aliased Message
            Alias aliasForSender = null;
            foreach (Alias alias in this.Storage.Aliases)
            {
                if (alias.Account == transaction.Sender)
                {
                    aliasForSender = alias;
                }
            }

            if (aliasForSender != null)
            {
                message = $"{aliasForSender.DisplayName} donated {transaction.FriendlyNanoAmount} Nano!";
            }

            // Bot Message
            lock (this.messages)
            {
                this.messages.Add(new NanoTwitchBotMessage(NanoTwitchBotMessageType.Success, message));
            }

            // Twitch Message
            if (transaction.FriendlyNanoAmount >= this.Storage.MinimumDonationAmount)
            {
                if (this.Twitch.IsConnected)
                {
                    this.Twitch.SendMessage(new JoinedChannel(this.Storage.TwitchChannel), message);
                }
            }
        }

        #endregion
    }
}