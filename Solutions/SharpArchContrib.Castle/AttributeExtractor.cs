namespace SharpArchContrib.Castle
{
    using System;
    using System.Reflection;

    public class AttributeExtractor<T> : IAttributeExtractor<T>
        where T : Attribute
    {
        public T[] GetMethodAttributes(MethodInfo methodInfo)
        {
            return (T[])methodInfo.GetCustomAttributes(typeof(T), false);
        }

        public T[] GetClassAttributes(MethodInfo methodInfo)
        {
            return (T[])methodInfo.ReflectedType.GetCustomAttributes(typeof(T), false);
        }

        public T[] GetAssemblyAttributes(MethodInfo methodInfo)
        {
            return (T[])methodInfo.ReflectedType.Assembly.GetCustomAttributes(typeof(T), false);
        }
    }
}