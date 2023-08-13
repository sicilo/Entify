using System.Reflection;
using Entify.Application.Exceptions;
using Entify.Application.Resources;

namespace Entify.Application.Helpers;

public static class AttributeHelper
{
    public static bool HasClassAttribute<TAtt>(this Type entity) where TAtt : Attribute
    {
        return Attribute.IsDefined(entity, typeof(TAtt));
    }
    
    public static TAtt GetClassAttribute<TAtt>(this Type entity) where TAtt : Attribute
    {
        var attributeType = typeof(TAtt);

        if (entity.GetCustomAttribute(attributeType, false) is not TAtt attribute)
            throw new EntifyException(ExceptionMessages.NullEntityException);

        return attribute;
    }
    
    public static bool HasPropertyAttribute<TAtt>(this PropertyInfo property) where TAtt : Attribute
    {
        return Attribute.IsDefined(property, typeof(TAtt));
    }

    public static TAtt GetPropertyAttribute<TAtt>(this PropertyInfo property) where TAtt : Attribute
    {
        var attributeType = typeof(TAtt);

        if (property.GetCustomAttribute(attributeType) is not TAtt attribute)
            throw new EntifyException(ExceptionMessages.NullEntityException);

        return attribute;
    }
}