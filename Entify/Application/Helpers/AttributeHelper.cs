using Entify.Domain.Exceptions;
using Entify.Domain.Resources;
using System.Reflection;

namespace Entify.Application.Helpers;

public static class AttributeHelper
{
    public static bool HasClassAttribute<T>(this Type entity) where T : Attribute
    {
        return Attribute.IsDefined(entity, typeof(T));
    }

    public static T GetClassAttribute<T>(this Type entity) where T : Attribute
    {
        var attributeType = typeof(T);

        if (entity.GetCustomAttribute(attributeType, false) is not T attribute)
            throw new EntifyException(ExceptionMessages.NullEntityException);

        return attribute;
    }

    public static bool HasPropertyAttribute<T>(this PropertyInfo property) where T : Attribute
    {
        return Attribute.IsDefined(property, typeof(T));
    }

    public static T GetPropertyAttribute<T>(this PropertyInfo property) where T : Attribute
    {
        var attributeType = typeof(T);

        if (property.GetCustomAttribute(attributeType) is not T attribute)
            throw new EntifyException(ExceptionMessages.NullEntityException);

        return attribute;
    }
}