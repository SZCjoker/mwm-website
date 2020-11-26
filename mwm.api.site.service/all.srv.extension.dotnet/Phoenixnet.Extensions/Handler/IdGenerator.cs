using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Phoenixnet.Extensions.Handler
{
    public static class IdGenerator
    {
        private static IdWorker _idWorker;

        static IdGenerator()
        {
            using (var md5Hasher = MD5.Create())
            {
                var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(Environment.MachineName));
                var intval = Math.Abs(BitConverter.ToInt16(hashed, 0));
                var rand = new Random(intval);
                var listLinq = new List<int>(Enumerable.Range(1, 31));
                listLinq = listLinq.OrderBy(num => rand.Next()).ToList();
                var randomise = listLinq.Skip(0).Take(2);
                _idWorker = new IdWorker(randomise.FirstOrDefault(), randomise.Last(), 1);
            }
        }

        public static long GenerateId() => _idWorker.NextId();
    }
}
