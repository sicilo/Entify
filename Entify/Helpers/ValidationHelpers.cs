namespace Entify.Helpers;

public static class ValidationHelpers
{
    
    /// <summary>
    /// Whether the value is null then throws NullReferenceException else returns the value
    /// </summary>
    /// <param name="reference">value that will be validated</param>
    /// <typeparam name="T">Type of the value that will be validated</typeparam>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">In case that the value argument be a null value</exception>
    public static T ThrowsNullReferenceException<T>(this T? reference)
    {
        if (reference is null)
        {
            throw new NullReferenceException("result doesn't contains data");
        }

        return reference;
    }
}