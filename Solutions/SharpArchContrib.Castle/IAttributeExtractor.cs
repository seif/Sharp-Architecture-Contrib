namespace SharpArchContrib.Castle
{
    using System;
    using System.Reflection;

    public interface IAttributeExtractor<T>
        where T : Attribute
    {
        T[] GetMethodAttributes(MethodInfo methodInfo);

        T[] GetClassAttributes(MethodInfo methodInfo);

        T[] GetAssemblyAttributes(MethodInfo methodInfo);
    }
}