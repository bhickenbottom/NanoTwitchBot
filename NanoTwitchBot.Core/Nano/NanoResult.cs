namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Net;
    using System.Text;

    public class NanoResult
    {
        #region Properties

        public Exception Exception { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string Json { get; set; }

        public string Node { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        #endregion

        #region Virtual Methods

        public virtual bool IsError()
        {
            return !this.IsSuccessStatusCode || this.Json == null || this.Exception != null;
        }

        #endregion

        #region Method Overrides

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{this.Node} {this.StatusCode}");
            if (this.Json != null)
            {
                stringBuilder.AppendLine($"{this.Json}");
            }

            if (this.Exception != null)
            {
                stringBuilder.AppendLine($"{this.Exception}");
            }

            return stringBuilder.ToString();
        }

        #endregion
    }

    public class NanoResult<T> : NanoResult where T : NanoResponse
    {
        #region Properties

        public T Response { get; set; }

        #endregion

        #region Method Overrides

        public override bool IsError()
        {
            return base.IsError() || this.Response == null || this.Response.Error != null;
        }

        #endregion
    }
}