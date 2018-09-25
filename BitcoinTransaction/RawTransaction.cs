using System;

namespace BitcoinTransaction
{
    public class RawTransaction
    {
        private int currentOffset;
        public UInt32 Version { get; }
        public RawTransactionInput[] Inputs { get; }
        public RawTransactionOutput[] Outputs { get; }
        public string[] Witness { get; }
        public UInt32 LockTime { get; }

        public RawTransaction(byte[] data)
        {
            Version = BitConverter.ToUInt32(data, 0);
            currentOffset = 4;
            var witnessDataPresented = GetWitnessDataPresence(data);
            var inCount = ReadVarInt(data);
            Inputs = new RawTransactionInput[inCount];
            for (var i = 0; i < inCount; i++)
            {
                Inputs[i] = BuildInput(data);
            }
            var outCount = ReadVarInt(data);
            Outputs = new RawTransactionOutput[outCount];
            for (var i = 0; i < outCount; i++)
            {
                Outputs[i] = BuildOutput(data);
            }
            if (witnessDataPresented)
            {
                foreach (var input in Inputs)
                {
                    var witnessDataComponentsCount = ReadVarInt(data);
                    for (var i = 0; i < witnessDataComponentsCount; i++)
                    {
                        var witnessDataLength = ReadVarInt(data);
                        input.AddWitness(data.ReadHexString(currentOffset, witnessDataLength));
                        currentOffset += witnessDataLength;
                    }
                }
            }

            LockTime = BitConverter.ToUInt32(data, currentOffset);
        }

        private bool GetWitnessDataPresence(byte[] data)
        {
            var witnessDataPresented = data[currentOffset] == 0 && data[currentOffset + 1] == 1;
            if (witnessDataPresented) currentOffset += 2;
            return witnessDataPresented;
        }

        private int ReadVarInt(byte[] data)
        {
            var count = new VarInt(data, currentOffset);
            currentOffset += count.Size;
            return count.Value;
        }

        private RawTransactionInput BuildInput(byte[] data)
        {
            var input = new RawTransactionInput(data, currentOffset);
            currentOffset += input.Size;
            return input;
        }

        private RawTransactionOutput BuildOutput(byte[] data)
        {
            var output = new RawTransactionOutput(data, currentOffset);
            currentOffset += output.Size;
            return output;
        }
    }
}