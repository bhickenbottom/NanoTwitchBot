namespace NanoTwitchBot.Core.Storage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    public class NanoTwitchBotStorage
    {
        #region Static Methods

        public static NanoTwitchBotStorage Load()
        {
            try
            {
                string json = File.ReadAllText("Data.json");
                NanoTwitchBotStorage storage = JsonSerializer.Deserialize<NanoTwitchBotStorage>(json);
                storage.Fix();
                return storage;
            }
            catch
            {
                return new NanoTwitchBotStorage();
            }
        }

        #endregion

        #region Constructors

        public NanoTwitchBotStorage()
        {
            this.Aliases = new List<Alias>();
            this.KnownHashes = new List<string>();
            this.PollingIntervalInSeconds = 10;

            // Chat Messages
            this.ChatMessageInfo = "This channel accepts Nano donations. Nano is an instant, feeless, eco-friendly cryptocurrency. Send Nano donations to {0}. !nano_add <account> adds an alias. !nano_delete deletes an alias. To get started, download the Natrium app.";
            this.ChatMessageOnline = "Nano bot is online!";
            this.ChatMessageAdded = "Added Nano alias for {0}.";
            this.ChatMessageDeleted = "Deleted Nano alias for {0}.";
            this.ChatMessageAnonymousDonation = "Anonymous donated {0} Nano!";
            this.ChatMessageAliasedDonation = "{1} donated {0} Nano!";
        }

        #endregion

        #region Properties

        public List<Alias> Aliases { get; set; }

        public string ChatMessageAdded { get; set; }

        public string ChatMessageAliasedDonation { get; set; }

        public string ChatMessageAnonymousDonation { get; set; }

        public string ChatMessageDeleted { get; set; }

        public string ChatMessageInfo { get; set; }

        public string ChatMessageOnline { get; set; }

        public List<string> KnownHashes { get; set; }

        public double MinimumDonationAmount { get; set; }

        public string NanoAccount { get; set; }

        public int PollingIntervalInSeconds { get; set; }

        public string TwitchChannel { get; set; }

        public string TwitchUsername { get; set; }

        #endregion

        #region Methods

        public NanoTwitchBotStorage Clone()
        {
            try
            {
                string json = JsonSerializer.Serialize(this);
                return JsonSerializer.Deserialize<NanoTwitchBotStorage>(json);
            }
            catch
            {
                return null;
            }
        }

        public void Fix()
        {
            if (this.Aliases == null)
            {
                this.Aliases = new List<Alias>();
            }

            if (this.KnownHashes == null)
            {
                this.KnownHashes = new List<string>();
            }
        }

        public bool Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this);
                File.WriteAllText("Data.json", json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}