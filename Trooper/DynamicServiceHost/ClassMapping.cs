using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.DynamicServiceHost
{
    public class ClassMapping
    {
        public Type Source { get; private set; }

        public Type ResolveTo { get; private set; }

        public string Alias { get; set; }

        private ClassMapping()
        {
        }

        public static ClassMapping Make<TSource, TResolveTo>(string alias)
            where TSource : class
            where TResolveTo : class, TSource, new()
        {
            return new ClassMapping { Alias = alias, Source = typeof(TSource), ResolveTo = typeof(TResolveTo) };
        }

        public static ClassMapping Make<TSource>(string alias)
            where TSource : class, new()
        {
            return new ClassMapping { Alias = alias, Source = typeof(TSource), ResolveTo = typeof(TSource) };
        }
    }
}
