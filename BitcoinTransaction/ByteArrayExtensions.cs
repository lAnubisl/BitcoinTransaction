using System;

namespace BitcoinTransaction
{
    internal static class ByteArrayExtensions
    {
        internal static string ReadHexString(this byte[] data, int offset, int length)
        {
            return BitConverter.ToString(data, offset, length).Replace("-", "");
        }
    }
}