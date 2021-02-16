namespace NanoTwitchBot.Core.Nano
{
    using System.Text.Json.Serialization;

    public class NanoResponse
    {
        #region Properties

        [JsonPropertyName("error")] public string Error { get; set; }

        #endregion
    }
}