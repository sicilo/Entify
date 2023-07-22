using System.Data.Common;
using Microsoft.AspNetCore.Http;
using Entify.Resources;

namespace Entify.Utilities;

public static class DbExtensions
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
                    if (reader.GetName(column) == property.Name && !reader.IsDBNull(column) && property.CanWrite)
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
        
        reader.Close();
    }

    public static IEnumerable<DbParameter> ToDbParameters(this DbConnection connection, object obj)
    {
        if (obj is null)
        {
            throw new Exception(string.Format(Messages.NullObjectException, nameof(obj)));
        }

        var props = obj.GetType().GetProperties();

        foreach (var property in props)
        {
            var parameter = connection.CreateCommand().CreateParameter();
            parameter.ParameterName = property.Name;
            parameter.Value = property.PropertyType == typeof(IFormFile)
                ? ((IFormFile?)property.GetValue(obj))?.GetFileBytes()
                : property.GetValue(obj);

            yield return parameter;
        }
    }

    private static byte[] GetFileBytes(this IFormFile file)
    {
        var bytes = Array.Empty<byte>();
        using var fileStream = file.OpenReadStream();
        if (file.Length <= 0) return bytes;
        bytes = new byte[file.Length];
        _ = fileStream.Read(bytes, 0, (int)file.Length);

        return bytes;
    }
}