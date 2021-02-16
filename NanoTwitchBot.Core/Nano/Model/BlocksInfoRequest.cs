namespace NanoTwitchBot.Core.Nano.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class BlocksInfoRequest : NanoRequest
    {
        #region Constructors

        public BlocksInfoRequest()
        {
            this.Action = "blocks_info";
            this.JsonBlock = "true";
        }

        #endregion

        #region Properties

        [JsonPropertyName("hashes")] public List<string> Hashes { get; set; }

        [JsonPropertyName("json_block")] public string JsonBlock { get; set; }

        #endregion
    }
}