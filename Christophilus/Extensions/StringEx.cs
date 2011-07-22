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
    }
}