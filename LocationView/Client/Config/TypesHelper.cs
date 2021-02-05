using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocationView.Client.Config
{
    internal class TypesHelper
    {
        public static TName GetName<TKey, TName>(Dictionary<TKey, TName> names, TKey type)
        {
            if (false == names.ContainsKey(type))
                type = default(TKey);

            return names[type];
        }

        public static TKey GetType<TKey, TName>(Dictionary<TKey, TName> names, TName name) where TName : IComparable
        {
            var result = default(TKey);

            try
            {
                foreach (var kvp in names)
                {
                    if (0 == name.CompareTo(kvp.Value))
                    {
                        result = kvp.Key;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
