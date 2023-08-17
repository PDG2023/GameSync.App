using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Security.Cryptography;
using System.Text;

namespace GameSync.Api.Persistence
{ // Taken from https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
    internal class AlphaNumericValueGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => throw new NotImplementedException();

        public override string Next(EntityEntry entry)
        {
            return GetUniqueKey(12); 
        }

        private static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        private static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

    }
}
