namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Numerics;

    public static class NanoUtility
    {
        #region Static Methods

        public static double GetNano(BigInteger raw, int decimalPlaces)
        {
            double total = 0;
            BigInteger dividend = raw;
            for (int i = 0; i < (decimalPlaces + 1); i++)
            {
                total += (double)BigInteger.DivRem(dividend, BigInteger.Pow(10, 30 - i), out dividend) / Math.Pow(10, i);
            }

            return total;
        }

        public static BigInteger GetRaw(string amount)
        {
            return BigInteger.Parse(amount);
        }

        #endregion
    }
}