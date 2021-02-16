namespace NanoTwitchBot.Core.Nano
{
    using System.Text.Json.Serialization;

    public class NanoRequest
    {
        #region Properties

        [JsonPropertyName("action")] public string Action { get; set; }

        #endregion
    }
}