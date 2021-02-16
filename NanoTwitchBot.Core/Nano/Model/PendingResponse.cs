namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class PendingResponse : NanoResponse
    {
        #region Properties

        [JsonPropertyName("blocks")] public List<string> Blocks { get; set; }

        #endregion
    }
}