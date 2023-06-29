
using System.Data.Common;

namespace Entify.Utilities
{
    using Microsoft.AspNetCore.Http;
    using System.Data.SqlClient;
    using System.Reflection;

    public static class SqlClientExtensions
    {

        public static Task<List<T>> ReaderToList<T>(this DbDataReader reader)
            => Task.Run(() =>
            {
                List<T> result = new();
                var properties = typeof(T).GetProperties();

                var columns = reader.FieldCount;

                while (reader.Read())
                {
                    T row = Activator.CreateInstance<T>();

                    for (var column = 0; column < columns; column++)
                        foreach (PropertyInfo property in properties)
                            if (reader.GetName(column) == property.Name && !reader.IsDBNull(column) && property.CanWrite)
                                property.SetValue(row,
                                    property.PropertyType == typeof(string)
                                        ? Convert.ToString(reader.GetValue(column))?.Trim()
                                        : reader.GetValue(column));

                    result.Add(row);
                }

                return result;
            });
        

        public static IEnumerable<DbParameter> ToDbParameters(this object obj, object? addtionalParameters = null)
        {
            var props = obj.GetType().GetProperties();
            List<DbParameter> parameters = new();

            foreach (var property in props)
                if (property.PropertyType == typeof(IFormFile))
                    parameters.Add(new SqlParameter(property.Name, ((IFormFile?)property.GetValue(obj))?.GetFileBytes()));
                else
                    parameters.Add(new SqlParameter(property.Name, property.GetValue(obj)));


            if (addtionalParameters is not null)
                parameters.AddRange(addtionalParameters.ToDbParameters());

            return parameters.ToArray();
        }

        private static byte[] GetFileBytes(this IFormFile file)
        {
            var bytes = Array.Empty<byte>();
            using Stream fileStream = file.OpenReadStream();
            if (file.Length > 0)
            {
                bytes = new byte[file.Length];
                fileStream.Read(bytes, 0, (int)file.Length);
            }

            return bytes;
        }
    }
}
