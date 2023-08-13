namespace Entify.Application.Helpers;

internal static class ConvertionHelper
{
    internal static TResult ConvertTo<TResult>(this object? value)
    {
        return (TResult)Convert.ChangeType(value: value, typeof(TResult));
    }
}