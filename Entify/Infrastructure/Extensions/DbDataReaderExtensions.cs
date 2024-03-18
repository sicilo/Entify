using Entify.Application.Helpers;
using Entify.Domain.Exceptions;
using Entify.Domain.Resources;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;

namespace Entify.Infrastructure.Extensions;

public static class DbDataReaderExtensions
{
    public static IEnumerable<T> ReaderToList<T>(this DbDataReader reader)
    {
        var result = Activator.CreateInstance<List<T>>();
        var properties = typeof(T).GetProperties();

        var columns = reader.FieldCount;

        while (reader.Read())
        {
            var row = Activator.CreateInstance<T>();

            for (var column = 0; column < columns; column++)
            {
                foreach (var property in properties)
                {
                    var propColumnName =
                        property.HasPropertyAttribute<ColumnAttribute>()
                            ? property.GetPropertyAttribute<ColumnAttribute>().Name
                            : property.Name;

                    if (reader.GetName(column).Equals(propColumnName) && !reader.IsDBNull(column) && property.CanWrite)
                    {
                        property.SetValue(row,
                            property.PropertyType == typeof(string)
                                ? Convert.ToString(reader.GetValue(column))?.Trim()
                                : reader.GetValue(column));
                    }
                }
            }

            result.Add(row);
        }

        return result;
    }

    public static T ReaderToEntity<T>(this DbDataReader reader)
    {
        var properties = typeof(T).GetProperties();

        var columns = reader.FieldCount;
        var row = Activator.CreateInstance<T>();

        while (reader.Read())
        {
            for (var column = 0; column < columns; column++)
            {
                foreach (var property in properties)
                {
                    var propColumnName =
                        property.HasPropertyAttribute<ColumnAttribute>()
                            ? property.GetPropertyAttribute<ColumnAttribute>().Name
                            : property.Name;

                    if (reader.GetName(column).Equals(propColumnName) && !reader.IsDBNull(column) && property.CanWrite)
                    {
                        property.SetValue(row,
                            property.PropertyType == typeof(string)
                                ? Convert.ToString(reader.GetValue(column))?.Trim()
                                : reader.GetValue(column));
                    }
                }
            }

            break;
        }

        return row;
    }

    public static T ReaderToScalar<T>(this DbDataReader reader)
    {
        object? value = default;

        if (reader.FieldCount > 1)
            throw new EntifyException(ExceptionMessages.UnexpectedDataReaderException);


        while (reader.Read())
        {
            value = reader.GetValue(0);
            break;
        }

        if (value is null)
            throw new EntifyException(ExceptionMessages.NullDataReaderColumnException);

        return value.ConvertTo<T>();
    }

    public static TResult MultiResultReaderToEntity<TResult>(this DbDataReader reader)
    {
        var resultType = typeof(TResult);
        var resultInstance = Activator.CreateInstance<TResult>();
        var resultProperties = resultType.GetProperties();

        foreach (var property in resultProperties)
        {
            var propertyType = property.PropertyType;
            var readerExtensions = typeof(DbDataReaderExtensions);
            var methodName = string.Empty;
            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(propertyType);

            if (isEnumerable)
            {
                methodName = nameof(ReaderToList);
            }
            else if (propertyType.IsClass)
            {
                methodName = nameof(ReaderToEntity);
            }
            else if (propertyType.IsPrimitive)
            {
                methodName = nameof(ReaderToScalar);
            }

            var genericArgument = isEnumerable ? propertyType.GetGenericArguments().First() : propertyType;

            var method = readerExtensions
                .GetMethod(methodName, BindingFlags.Static | BindingFlags.Public)
                ?.MakeGenericMethod(genericArgument);

            if (method is null)
            {
                var message = string.Format(ExceptionMessages.NullMethodException, methodName);
                throw new EntifyException(message);
            }

            try
            {
                var methodResult = method.Invoke(null, new object[] { reader });

                var result = isEnumerable ? methodResult as IEnumerable : methodResult;
                property.SetValue(resultInstance, result);
            }
            catch (Exception e)
            {
                throw new EntifyException(e.Message);
            }

            reader.NextResult();
        }

        return resultInstance;
    }
}