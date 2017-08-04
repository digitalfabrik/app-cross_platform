using System;
using Plugin.Settings.Abstractions;

namespace Integreat.Shared.Utilities
{
    public static class PreferencesExtension
    {
        /// <summary>
        /// Invokes the GetValueOrDefault method of a ISettings instance, but catches every exception and if so, also returns the default value.
        /// </summary>
        /// <typeparam name="T">Type for the value of the setting.</typeparam>
        /// <param name="settings">The settings instance.</param>
        /// <param name="key">The key for the value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value of type T, or it's default if it wasn't found or an error occurred.</returns>
        public static T GetValueOrDefaultExceptionSafe<T>(this ISettings settings, string key, T defaultValue = default(T))
        {
            try
            {
                // invoke the method
                return settings.GetValueOrDefault(key, defaultValue);
            }
            catch (Exception)
            {
                // if any exception occurs, return the default value
                return defaultValue;
            }
        }
    }
}
