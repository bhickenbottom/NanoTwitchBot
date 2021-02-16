namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BlocksInfoResponse : NanoResponse
    {
        #region Properties

        [JsonPropertyName("blocks")] public Dictionary<string, BlockInfo> Blocks { get; set; }

        #endregion
    }
}