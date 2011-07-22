using System;
using System.Security.Cryptography;
using System.Text;
namespace Christophilus.Extensions
{
    /// <summary>
    /// Extends the string class.
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// Provides a more convenient string format experience.
        /// </summary>
        /// <param name="str">The string to be formatted.</param>
        /// <param name="args">The arguments to the string.</param>
        /// <returns>The formatted string.</returns>
        public static string Formatted(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// Gets the SHA1 hash of a string.
        /// </summary>
        /// <param name="str">The string to be hashed.</param>
        /// <returns>The Base64 hashed value.</returns>
        public static string Sha1Hash(this string str)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(str)));
            }
        }

        /// <summary>
        /// Slices off the specified range of characters from the string, 
        /// returning the sliced section.
        /// </summary>
        /// <param name="start">The start index of the slice.</param>
        /// <param name="count">The length of the slice.</param>
        /// <returns>The sliced string.</returns>
        public static string Slice(this string str, int start, int length)
        {
            str = str ?? string.Empty;

            if (start < 0 || start >= str.Length)
            {
                return string.Empty;
            }

            return str.Substring(start, Math.Min(str.Length - start, length));
        }
    }
}