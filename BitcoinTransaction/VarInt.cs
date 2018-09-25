using System;

namespace BitcoinTransaction
{
    /// <summary>
    /// Even having definition for varint that says it should be able to hold ulong. 
    /// This implementation supports only int which is actually should be enough to 
    /// handle transaction of size 2147,483647 megabytes.
    /// </summary>
    public struct VarInt
    {
        public int Value { get; }
        public byte Size { get; }

        public VarInt(byte[] data, int offset)
        {
            byte marker = data[offset];
            if (marker < 253)
            {
                Size = 1;
                Value = marker;
            }
            else if (marker == 253)
            {
                Size = 3;
                Value = BitConverter.ToUInt16(data, offset + 1);
            }
            else if (marker == 254)
            {
                Size = 5;
                Value = (int)BitConverter.ToUInt32(data, offset + 1);
            }
            else
            {
                throw new NotSupportedException();
                //Size = 9;
                //Value = (int)BitConverter.ToUInt64(data, offset + 1);
            }
        }
    }
}