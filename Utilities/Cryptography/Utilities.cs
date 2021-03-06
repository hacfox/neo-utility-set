﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace Utilities.Cryptography
{
    public static class Utilities
    {
        private static readonly ThreadLocal<SHA256> _sha256 = new ThreadLocal<SHA256>(SHA256.Create);
        private static readonly ThreadLocal<RIPEMD160Managed> _ripemd160 = new ThreadLocal<RIPEMD160Managed>(() => new RIPEMD160Managed());


        /// <summary>
        /// 求字节数组的sha256散列值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>返回该散列值</returns>
        public static byte[] Sha256(this IEnumerable<byte> value)
        {
            return _sha256.Value.ComputeHash(value.ToArray());
        }

        /// <summary>
        /// 求字节数组的sha256散列值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="offset">偏移量，散列计算时从该偏移量处开始</param>
        /// <param name="count">要计算散列值的字节数量</param>
        /// <returns>返回该散列值</returns>
        public static byte[] Sha256(this byte[] value, int offset, int count)
        {
            return _sha256.Value.ComputeHash(value, offset, count);
        }

        /// <summary>
        /// 求字节数组的ripemd160散列值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>返回该散列值</returns>
        public static byte[] RIPEMD160(this IEnumerable<byte> value)
        {
            return _ripemd160.Value.ComputeHash(value.ToArray());
        }

        public static byte[] Base58CheckDecode(this string input)
        {
            byte[] buffer = Base58.Decode(input);
            if (buffer.Length < 4) throw new FormatException();
            byte[] checksum = buffer.Sha256(0, buffer.Length - 4).Sha256();
            if (!buffer.Skip(buffer.Length - 4).SequenceEqual(checksum.Take(4)))
                throw new FormatException();
            return buffer.Take(buffer.Length - 4).ToArray();
        }
    }
}
