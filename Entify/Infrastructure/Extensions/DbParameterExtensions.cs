using System.Data.Common;
using Entify.Application.Attributes;
using Entify.Application.Helpers;
using Entify.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Entify.Infrastructure.Extensions;

public static class DbParameterExtensions
{
    public static IEnumerable<DbParameter> ToDbParameters(this DbConnection connection, params object[] objs)
    {
        if (objs is null)
        {
            throw new Exception(string.Format(ExceptionMessages.NullReferenceException, nameof(objs)));
        }

        foreach (var obj in objs)
        {
            var props = obj.GetType().GetProperties();

            foreach (var property in props)
            {
                var propColumnName =
                    property.HasPropertyAttribute<ProcedureParameterAttribute>()
                        ? property.GetPropertyAttribute<ProcedureParameterAttribute>().Name
                        : property.Name;

                var parameter = connection.CreateCommand().CreateParameter();
                parameter.ParameterName = propColumnName;
                parameter.Value = property.PropertyType == typeof(IFormFile)
                    ? ((IFormFile?)property.GetValue(obj))?.GetFileBytes()
                    : property.GetValue(obj);

                yield return parameter;
            }
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