namespace Christophilus.Extensions
{
    using System.Web.Mvc;

    /// <summary>
    /// The extension class for the ModelState object.
    /// </summary>
    public static class ModelStateEx
    {
        /// <summary>
        /// Adds a default error message.
        /// </summary>
        /// <param name="ms">The model state.</param>
        /// <param name="error">The error message to be added.</param>
        public static void AddDefaultError(this ModelStateDictionary ms, string error)
        {
            ms.AddModelError(string.Empty, error);
        }

        /// <summary>
        /// Determines whether or not the ModelState has errors and initializes
        /// the default validation summary if none exists.
        /// </summary>
        /// <param name="modelState">The model state.</param>
        /// <param name="validationSummary">
        /// The validation summary to be used if there is no default one 
        /// already in the ModelState.
        /// </param>
        /// <returns>True or false.</returns>
        public static bool HasErrors(
            this ModelStateDictionary modelState,
            string validationSummary = null)
        {
            if (!modelState.IsValid && !modelState.ContainsKey(string.Empty))
            {
                modelState.AddDefaultError(
                    validationSummary ?? "Please correct the following entries.");
            }

            return !modelState.IsValid;
        }
    }
}