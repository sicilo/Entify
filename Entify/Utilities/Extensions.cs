﻿namespace Entify.Utilities
{
    using Microsoft.AspNetCore.Http;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    public static class Extensions
    {
        public static Task<List<Entity>> ReaderToList<Entity>(this SqlDataReader reader)
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

        public static Task<List<Entity>> DataTableToList<Entity>(this DataTable dataT)
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

        private static Task<Entity> DataRowToEntity<Entity>(this DataRow dr)
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

        public static SqlParameter[] ToSqlParameters<Entity>(this Entity obj)
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

        public static SqlParameter[] ToSqlParameters<Entity>(this Entity obj, params KeyValuePair<string, object>[] additionalParameters)
        {

            PropertyInfo[] props = typeof(Entity).GetProperties();
            SqlParameter[] parameters = new SqlParameter[props.Length];

            int index = 0;
            foreach (PropertyInfo property in props)
            {
                if (property.PropertyType.Name == "IFormFile")
                {
                    IFormFile? val = (IFormFile?)property.GetValue(obj);

                    parameters[index] = new SqlParameter(property.Name, val?.GetFileBytes());
                }
                else
                    parameters[index] = new SqlParameter(property.Name, property.GetValue(obj));

                index++;
            }

            foreach (KeyValuePair<string, object> pair in additionalParameters)
                parameters[index] = new SqlParameter(pair.Key, pair.Value);

            return parameters;
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


        public static DataTable ListToDataTable<T>(this List<T> listEntity)
        {
            DataTable TableResult = new();
            DataRow Row = TableResult.NewRow();
            PropertyInfo[] Properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in Properties)
                TableResult.Columns.Add(property.Name, property.PropertyType);

            foreach (T item in listEntity)
            {
                foreach (PropertyInfo property in Properties)
                    Row.SetField(property.Name, property.GetValue(item));
                    //Row[property.Name] = property.GetValue(item, null);

                TableResult.Rows.Add(Row);
                Row = TableResult.NewRow();
            }


            return TableResult;
        }
    }
}