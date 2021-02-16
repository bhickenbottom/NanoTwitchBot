namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Text.Json.Serialization;

    public class BlockInfo
    {
        #region Properties

        [JsonPropertyName("amount")] public string Amount { get; set; }

        [JsonPropertyName("balance")] public string Balance { get; set; }

        [JsonPropertyName("block_account")] public string BlockAccount { get; set; }

        [JsonPropertyName("confirmed")] public string Confirmed { get; set; }

        [JsonPropertyName("contents")] public BlockContents Contents { get; set; }

        [JsonPropertyName("height")] public string Height { get; set; }

        [JsonPropertyName("local_timestamp")] public string LocalTimestamp { get; set; }

        [JsonPropertyName("subtype")] public string Subtype { get; set; }

        #endregion
    }
}