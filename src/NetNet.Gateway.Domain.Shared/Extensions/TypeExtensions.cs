using System.Reflection;

namespace NetNet.Gateway.Extensions;

public static class TypeExtensions
{
    public static object? GetDefaultValue(this Type type)
    {
        return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
    }
}
