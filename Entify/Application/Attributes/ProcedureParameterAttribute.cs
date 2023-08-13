namespace Entify.Application.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ProcedureParameterAttribute : Attribute
{
    public string Name;

    public ProcedureParameterAttribute(string name)
    {
        Name = name;
    }
}