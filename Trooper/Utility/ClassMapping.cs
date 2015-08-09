using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Utility
{
    public class ClassMapping
    {
        public Type Source { get; private set; }

        public Type ResolveTo { get; private set; }

        public bool CommonType { get; set; }

        private ClassMapping()
        {
        }

        public static ClassMapping Make<TSource, TResolveTo>(bool commonType = false)
            where TSource : class
            where TResolveTo : class, TSource, new()
        {
            return new ClassMapping { Source = typeof(TSource), ResolveTo = typeof(TResolveTo), CommonType = commonType };
        }

        public static ClassMapping Make<TSource>(bool commonType = false)
            where TSource : class, new()
        {
            return new ClassMapping { Source = typeof(TSource), ResolveTo = typeof(TSource), CommonType = commonType };
        }
    }
}
