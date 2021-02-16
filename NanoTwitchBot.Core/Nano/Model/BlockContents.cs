namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Text.Json.Serialization;

    public class BlockContents
    {
        #region Properties

        [JsonPropertyName("account")] public string Account { get; set; }

        [JsonPropertyName("balance")] public string Balance { get; set; }

        [JsonPropertyName("link")] public string Link { get; set; }

        [JsonPropertyName("link_as_account")] public string LinkAsAccount { get; set; }

        [JsonPropertyName("previous")] public string Previous { get; set; }

        [JsonPropertyName("representative")] public string Representative { get; set; }

        [JsonPropertyName("signature")] public string Signature { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("work")] public string Work { get; set; }

        #endregion
    }
}