namespace Entify.Transformations
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;

    public class DataTransformations
    {
        public static Task<List<Entity>> ReaderToEntityList<Entity>(SqlDataReader reader)
        {
            return Task.Run(() =>
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
                                    property.PropertyType.Name == "String"
                                        ? Convert.ToString(reader.GetValue(column))?.Trim()
                                        : reader.GetValue(column));

                    result.Add(row);
                }

                return result;
            });
        }

        public static Task<List<Entity>> DataTableToList<Entity>(DataTable dataT)
        {
            return Task.Run(async () =>
            {
                List<Entity> data = new();

                foreach (DataRow row in dataT.Rows)
                {
                    Entity instance = await DataRowToEntity<Entity>(row);
                    data.Add(instance);
                }

                return data;
            });
        }
        private static Task<Entity> DataRowToEntity<Entity>(DataRow dr)
        {

            return Task.Run(() =>
            {
                PropertyInfo[] properties = typeof(Entity).GetProperties();
                Entity result = Activator.CreateInstance<Entity>();

                foreach (DataColumn column in dr.Table.Columns)
                    foreach (PropertyInfo pro in properties)
                        if (column.ColumnName.Equals(pro.Name))
                            switch (column.DataType.Name)
                            {
                                case "String":
                                    pro.SetValue(result, !dr.IsNull(column) ? dr.Field<string>(column.ColumnName)?.Trim() : "", null);
                                    break;
                                default:
                                    pro.SetValue(result, !dr.IsNull(column) ? dr.Field<object>(column.ColumnName) : 0, null);
                                    break;
                            }
                        else
                            continue;


                return result;
            });
        }

        public static SqlParameter[] EntityToParameters<Entity>(Entity obj)
        {

            PropertyInfo[] props = typeof(Entity).GetProperties();
            SqlParameter[] parameters = new SqlParameter[props.Length];

            int index = 0;
            foreach (PropertyInfo property in props)
            {
                if (property.PropertyType.Name == "IFormFile")
                {
                    byte[] bytes = Array.Empty<byte>();
                    object? val = property.GetValue(obj);
                    if (val != null)
                    {
                        IFormFile file = (IFormFile)val;
                        using Stream fileStream = file.OpenReadStream();
                        if (file.Length > 0)
                        {
                            bytes = new byte[file.Length];
                            fileStream.Read(bytes, 0, (int)file.Length);
                        }
                    }

                    parameters[index] = new SqlParameter(property.Name, bytes);
                }
                else
                    parameters[index] = new SqlParameter(property.Name, property.GetValue(obj));

                index++;
            }

            return parameters;
        }
    }
}