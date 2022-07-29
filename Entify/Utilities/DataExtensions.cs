namespace Entify.Utilities.Extensions
{
    using System.Data;
    using System.Reflection;
    public static class DataExtensions
    {
        public static Task<List<Entity>> DataTableToList<Entity>(this DataTable dataTable)
            => Task.Run(async () =>
            {
                List<Entity> data = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    Entity instance = await DataRowToEntity<Entity>(row);
                    data.Add(instance);
                }

                return data;
            });

        private static Task<Entity> DataRowToEntity<Entity>(this DataRow dr)
            => Task.Run(() =>
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

                TableResult.Rows.Add(Row);
                Row = TableResult.NewRow();
            }


            return TableResult;
        }
    }
}
