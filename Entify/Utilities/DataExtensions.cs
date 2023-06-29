namespace Entify.Utilities
{
    using System.Data;
    
    public static class DataExtensions
    {
        public static Task<List<T>> DataTableToList<T>(this DataTable dataTable)
            => Task.Run(async () =>
            {
                List<T> data = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    T instance = await DataRowToEntity<T>(row);
                    data.Add(instance);
                }

                return data;
            });

        private static Task<T> DataRowToEntity<T>(this DataRow dr)
            => Task.Run(() =>
            {
                var properties = typeof(T).GetProperties();
                var result = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                    foreach (var pro in properties)
                        if (column.ColumnName.Equals(pro.Name))
                            switch (column.DataType.Name)
                            {
                                case "String":
                                    pro.SetValue(result, !dr.IsNull(column) ? dr.Field<string>(column.ColumnName)?.Trim() : string.Empty, null);
                                    break;
                                default:
                                    pro.SetValue(result, !dr.IsNull(column) ? dr.Field<object>(column.ColumnName) : 0, null);
                                    break;
                            }
                
                return result;
            });

        public static DataTable ListToDataTable<T>(this List<T> list)
        {
            var tableResult = new DataTable();
            var row = tableResult.NewRow();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
                tableResult.Columns.Add(property.Name, property.PropertyType);

            foreach (var item in list)
            {
                foreach (var property in properties)
                    row.SetField(property.Name, property.GetValue(item));

                tableResult.Rows.Add(row);
                row = tableResult.NewRow();
            }
            
            return tableResult;
        }
    }
}
