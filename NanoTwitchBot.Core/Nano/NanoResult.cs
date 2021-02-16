namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Net;

    public class NanoResult<T> where T : NanoResponse
    {
        #region Properties

        public Exception Exception { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string Json { get; set; }

        public T Response { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        #endregion

        #region Methods

        public bool IsError(out string error)
        {
            error = null;
            if (this.Exception != null)
            {
                error = $"Exception: {this.Exception}";
            }

            if (!this.IsSuccessStatusCode)
            {
                error = $"Status Code: {this.StatusCode}";
            }

            if (this.Response == null)
            {
                error = $"Deserialization: {this.Json}";
            }

            if (this.Response?.Error != null)
            {
                error = $"Error: {this.Response?.Error}";
            }

            return error != null;
        }

        #endregion
    }
}