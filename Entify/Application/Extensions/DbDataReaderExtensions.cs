using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Entify.Application.Attributes;
using Entify.Application.Exceptions;
using Entify.Application.Helpers;
using Entify.Application.Resources;

namespace Entify.Application.Extensions;

public static class DbDataReaderExtensions
{
    public static IEnumerable<T> ReaderToList<T>(this DbDataReader reader)
    {
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

            yield return row;
        }
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
            throw new EntifyException("");
         
        
        while (reader.Read())
        {
            value = reader.GetValue(1);
            break;
        }

        if (value is null)
            throw new EntifyException("");
        
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
            
            if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            {
                methodName = "ReaderToList";
            }
            else if (propertyType.IsClass)
            {
                methodName = "ReaderToEntity";
            }
            else if (propertyType.IsPrimitive)
            {
                methodName = "ReaderToScalar";
            }
            
            var method = readerExtensions
                .GetMethod(methodName)
                ?.MakeGenericMethod(propertyType);

            if (method is null)
            {
                var message = string.Format(ExceptionMessages.NullMethodException, methodName);
                throw new EntifyException(message);
            }

            var methodResult = method.Invoke(null,null);
                
            property.SetValue(resultInstance,methodResult);

            reader.NextResult();
        }

        return resultInstance;
    }
}