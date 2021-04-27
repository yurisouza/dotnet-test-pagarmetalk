using System;
using System.Collections.Generic;
using System.Linq;

namespace PagarMeTalk.UnitTests.FakerData
{
    public static class FakerHelper
    {
        /// <summary>
        /// Get a random value of Enum
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns></returns>
        public static TEnum RandomEnumValue<TEnum>() where TEnum : struct
        {
            var values = GetEnumValues<TEnum>().ToArray();
            return (TEnum)values.GetValue(new Random().Next(values.Length));
        }

        /// <summary>
        /// Get a random value of Enum different of
        /// </summary>
        /// <param name="differentOf">Value different this.</param>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns></returns>
        public static TEnum RandomEnumValue<TEnum>(TEnum differentOf) where TEnum : struct
        {
            var values = GetEnumValues<TEnum>().Where(v => v.ToString() != differentOf.ToString()).ToArray();
            return (TEnum)values.GetValue(new Random().Next(values.Length));
        }

        /// <summary>
        /// Get values of a enum
        /// </summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <returns></returns>
        public static List<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }
    }
}
