
namespace Entify.Utilities.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System.Data.SqlClient;
    using System.Reflection;

    public static class SqlClientExtensions
    {

        public static Task<List<Entity>> ReaderToList<Entity>(this SqlDataReader reader)
            => Task.Run(() =>
            {
                List<Entity> result = new();
                PropertyInfo[] properties = typeof(Entity).GetProperties();

                int columns = reader.FieldCount;

                while (reader.Read())
                {
                    Entity row = Activator.CreateInstance<Entity>();

                    for (int column = 0; column < columns; column++)
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
        

        public static SqlParameter[] ToSqlParameters(this object obj, object? addtionalParameters = null)
        {
            PropertyInfo[] props = obj.GetType().GetProperties();
            List<SqlParameter> parameters = new();

            foreach (PropertyInfo property in props)
                if (property.PropertyType == typeof(IFormFile))
                    parameters.Add(new SqlParameter(property.Name, ((IFormFile?)property.GetValue(obj))?.GetFileBytes()));
                else
                    parameters.Add(new SqlParameter(property.Name, property.GetValue(obj)));


            if (addtionalParameters != null)
                parameters.AddRange(addtionalParameters.ToSqlParameters());

            return parameters.ToArray();
        }

        public static byte[] GetFileBytes(this IFormFile file)
        {
            byte[] bytes = Array.Empty<byte>();
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
