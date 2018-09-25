using System;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinTransaction
{
    public class RawTransactionInput
    {
        public string PreviousTransactionHash { get; }
        public string Script { get; }
        public UInt32 PreviousTransactionOutputIndex { get; }
        public int Size { get; }
        public UInt32 SequenceNumber { get; }
        private IList<string> witness = new List<string>();
        public string[] Witness { get { return witness.ToArray(); } }

        internal void AddWitness(string val)
        {
            witness.Add(val);
        }

        public RawTransactionInput(byte[] data, int offset)
        {
            var savedOffset = offset;
            // First 32 bytes holds previous transaction hash
            PreviousTransactionHash = data.ReadHexString(offset, 32);
            offset += 32;

            // Following by 4 bytes holding output transaction index
            PreviousTransactionOutputIndex = BitConverter.ToUInt32(data, offset);
            offset += 4;

            // Following by varing for transaction script length
            var transactionScriptLength = new VarInt(data, offset);
            offset += transactionScriptLength.Size;

            // Following by transaction script itself
            Script = data.ReadHexString(offset, transactionScriptLength.Value);
            offset += transactionScriptLength.Value;

            // following by 4 bytes sequence number
            SequenceNumber = BitConverter.ToUInt32(data, offset);
            offset += 4;

            Size = offset - savedOffset;
        }
    }
}