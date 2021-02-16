namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    public class NanoNodeList
    {
        #region Static Methods

        public static NanoNodeList Load()
        {
            try
            {
                string json = File.ReadAllText("NanoNodes.json");
                return JsonSerializer.Deserialize<NanoNodeList>(json);
            }
            catch
            {
                return new NanoNodeList();
            }
        }

        #endregion

        #region Properties

        public List<string> Nodes { get; set; }

        #endregion

        #region Methods

        public List<Uri> GetNodes()
        {
            List<Uri> result = new List<Uri>();
            if (this.Nodes != null)
            {
                foreach (string node in this.Nodes)
                {
                    try
                    {
                        result.Add(new Uri(node));
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }
            }

            return result;
        }

        #endregion
    }
}