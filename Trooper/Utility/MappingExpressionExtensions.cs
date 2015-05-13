using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Utility
{
    /// <summary>
    /// http://stackoverflow.com/questions/15337883/automapper-ignore-all-items-of-ienumerableselectlistitem
    /// </summary>
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest>
            IgnorePropertiesOfType<TSource, TDest>(
            this IMappingExpression<TSource, TDest> mappingExpression,
            Type typeToIgnore
            )
        {
            var destInfo = new AutoMapper.TypeInfo(typeof(TDest));
            foreach (var destProperty in destInfo.GetPublicWriteAccessors()
                .OfType<PropertyInfo>()
                .Where(p => p.PropertyType == typeToIgnore))
            {
                mappingExpression = mappingExpression
                    .ForMember(destProperty.Name, opt => opt.Ignore());
            }

            return mappingExpression;
        }
    }
}
