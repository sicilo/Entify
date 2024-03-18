using Entify.Application.Helpers;
using Entify.Domain.Exceptions;
using Entify.Domain.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Entify.Infrastructure.Extensions;

public static class DataExtensions
{
    public static IEnumerable<T> DataTableToList<T>(this DataTable dataTable)
    {
        return from DataRow row in dataTable.Rows select row.DataRowToEntity<T>();
    }

    private static T DataRowToEntity<T>(this DataRow dr)
    {
        var properties = typeof(T).GetProperties();
        var result = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (var pro in properties)
            {
                if (column.ColumnName.Equals(pro.Name))
                {
                    switch (column.DataType.Name)
                    {
                        case nameof(String):
                            pro.SetValue(result,
                                !dr.IsNull(column)
                                    ? dr.Field<string>(column.ColumnName)
                                    : string.Empty);
                            break;
                        default:
                            pro.SetValue(result,
                                !dr.IsNull(column)
                                    ? dr.Field<object>(column.ColumnName)
                                    : 0);
                            break;
                    }
                }
            }
        }

        return result;
    }

    public static DataTable ListToDataTable<T>(this IEnumerable<T> list)
    {
        var tableResult = new DataTable();
        var row = tableResult.NewRow();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            tableResult.Columns.Add(property.Name, property.PropertyType);
        }

        foreach (var item in list)
        {
            foreach (var property in properties)
            {
                var propColumnName =
                    property.HasPropertyAttribute<ColumnAttribute>()
                        ? property.GetPropertyAttribute<ColumnAttribute>().Name
                        : property.Name;

                if (string.IsNullOrEmpty(propColumnName))
                    throw new EntifyException(ExceptionMessages.NullReferenceException);

                row.SetField(propColumnName, property.GetValue(item));
            }

            tableResult.Rows.Add(row);
            row = tableResult.NewRow();
        }

        return tableResult;
    }
}