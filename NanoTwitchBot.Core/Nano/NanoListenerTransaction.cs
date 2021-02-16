namespace NanoTwitchBot.Core.Nano
{
    using System.Numerics;
    using NanoTwitchBot.Core.Nano.Model;

    public class NanoListenerTransaction
    {
        #region Constructors

        public NanoListenerTransaction(BlockInfo block)
        {
            this.Block = block;

            // Amounts
            this.RawAmount = NanoUtility.GetRaw(this.Block.Amount);
            this.FriendlyNanoAmount = NanoUtility.GetNano(this.RawAmount, 4);

            // Sender
            this.Sender = this.Block.BlockAccount;
        }

        #endregion

        #region Properties

        public BlockInfo Block { get; private set; }

        public double FriendlyNanoAmount { get; private set; }

        public BigInteger RawAmount { get; private set; }

        public string Sender { get; private set; }

        #endregion
    }
}