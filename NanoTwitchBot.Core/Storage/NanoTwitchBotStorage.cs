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
                return JsonSerializer.Deserialize<NanoTwitchBotStorage>(json);
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
            this.PollingIntervalInSeconds = 10;
        }

        #endregion

        #region Properties

        public List<Alias> Aliases { get; set; }

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