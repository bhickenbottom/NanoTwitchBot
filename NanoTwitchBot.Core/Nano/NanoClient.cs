namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class NanoClient
    {
        #region Constructors

        public NanoClient()
        {
            // Nano Node List
            NanoNodeList nodeList = NanoNodeList.Load();

            // Node URI
            this.Nodes = nodeList.GetNodes();

            // Http
            this.Http = new HttpClient();

            // Random 
            this.random = new Random();
        }

        #endregion

        #region Fields

        private Random random;

        #endregion

        #region Properties

        public HttpClient Http { get; private set; }

        public List<Uri> Nodes { get; private set; }

        #endregion

        #region Methods

        public async Task<NanoResult<T>> SendAsync<T>(NanoRequest request) where T : NanoResponse, new()
        {
            NanoResult<T> result = new NanoResult<T>();
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = this.Nodes[this.random.Next(0, this.Nodes.Count)];
                requestMessage.Method = HttpMethod.Post;
                string requestJson = JsonSerializer.Serialize(request, request.GetType());
                requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await this.Http.SendAsync(requestMessage);
                result.StatusCode = responseMessage.StatusCode;
                result.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
                if (responseMessage.IsSuccessStatusCode)
                {
                    result.Json = await responseMessage.Content.ReadAsStringAsync();
                    try
                    {
                        result.Response = JsonSerializer.Deserialize<T>(result.Json);
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }

        #endregion
    }
}