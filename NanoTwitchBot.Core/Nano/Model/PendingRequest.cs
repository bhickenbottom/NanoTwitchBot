namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Text.Json.Serialization;

    public class PendingRequest : NanoRequest
    {
        #region Constructors

        public PendingRequest()
        {
            this.Action = "pending";
        }

        #endregion

        #region Properties

        [JsonPropertyName("account")] public string Account { get; set; }

        [JsonPropertyName("count")] public string Count { get; set; }

        #endregion
    }
}