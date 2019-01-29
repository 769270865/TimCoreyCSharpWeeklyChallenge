using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PillReminder.Utlitie
{
    public static class LinqExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource,TKey>(this IEnumerable<TSource> source,Func<TSource,TKey> keySelector)
        {
            HashSet<TKey> keySeenList = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (keySeenList.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }
}
