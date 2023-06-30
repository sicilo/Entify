namespace Entify.Helpers;

public static class ConvertionHelpers
{
    public static TResult ConvertToGeneric<TResult>(this object? value)
    {
        var val = value.ThrowsNullReferenceException();
        
        return (TResult)Convert.ChangeType(value: val, typeof(TResult));
    }
}