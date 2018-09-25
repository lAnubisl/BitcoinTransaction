using System;

namespace BitcoinTransaction
{
    public class RawTransactionOutput
    {
        internal int Size { get; }
        public long Value { get; }
        public string PkScript { get; }

        public RawTransactionOutput(byte[] data, int offset)
        {
            var savedOffset = offset;
            // The first 8 bits holds output value
            Value = BitConverter.ToInt64(data, offset);
            offset += 8;

            // Following by varint PK script length
            var pkScriptLength = new VarInt(data, offset);
            offset += pkScriptLength.Size;

            // Following by PK script content of length read in previous step
            PkScript = data.ReadHexString(offset, pkScriptLength.Value);
            offset += pkScriptLength.Value;

            Size = offset - savedOffset;
        }
    }
}