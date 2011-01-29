namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class AttributeHelper<T>
        where T : Attribute
    {
        public static T[] GetAssemblyLevelAttributes(Assembly assembly)
        {
            return (T[])assembly.GetCustomAttributes(typeof(T), false);
        }

        public static T[] GetTypeLevelAttributes(Type type)
        {
            return (T[])type.GetCustomAttributes(typeof(T), false);
        }

        public static T[] GetMethodLevelAttributesForType(Type type)
        {
            List<T> attributes = new List<T>();
            foreach (var method in type.GetMethods())
            {
                attributes.AddRange(GetMethodLevelAttributes(method));
            }
            return attributes.ToArray();
        }

        public static T[] GetMethodLevelAttributes(MethodInfo methodInfo)
        {
            return (T[])methodInfo.GetCustomAttributes(typeof(T), false);
        }

        public static bool ShouldInterceptMethod(MethodInfo methodInfo)
        {
            return GetAssemblyLevelAttributes(methodInfo.ReflectedType.Assembly).Any() ||
                   GetTypeLevelAttributes(methodInfo.ReflectedType).Any() ||
                   GetMethodLevelAttributes(methodInfo).Any();
        }

        public static bool ShouldInterceptType(Type service)
        {
            return GetAssemblyLevelAttributes(service.Assembly).Any() || 
                   GetTypeLevelAttributes(service).Any() ||
                   GetMethodLevelAttributesForType(service).Any();
        }
    }
}