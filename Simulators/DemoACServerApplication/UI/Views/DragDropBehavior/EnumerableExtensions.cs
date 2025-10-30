using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoServerApplication.UI.Views.DragDropBehavior
{
    internal static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty(this IEnumerable lst)
        {
            if (lst == null) return true;
            return !lst.GetEnumerator().MoveNext();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> lst)
        {
            if (lst == null) return true;
            return !lst.Any();
        }
    }
}
