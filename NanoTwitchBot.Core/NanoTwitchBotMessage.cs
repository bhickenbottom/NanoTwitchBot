namespace NanoTwitchBot.Core
{
    public class NanoTwitchBotMessage
    {
        #region Constructors

        public NanoTwitchBotMessage(NanoTwitchBotMessageType type, string message)
        {
            this.Type = type;
            this.Message = message;
        }

        #endregion

        #region Properties

        public string Message { get; set; }

        public NanoTwitchBotMessageType Type { get; set; }

        #endregion
    }
}