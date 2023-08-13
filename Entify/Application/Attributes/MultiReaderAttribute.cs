namespace Entify.Application.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MultiReaderAttribute : Attribute
{
    public bool IsMultiReader;

    public MultiReaderAttribute(bool isMultiReader = true)
    {
        
    }
}