using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Nebula.Infrastructure
{
    public static class AnonymousTypeExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object anonymousType)
        {
            return TypeDescriptor.GetProperties(anonymousType)
                .Cast<PropertyDescriptor>()
                .ToDictionary(property => property.Name, property => property.GetValue(anonymousType));
        }
    }
}