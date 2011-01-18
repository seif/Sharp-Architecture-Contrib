namespace SharpArchContrib.Castle
{
    using System;
    using System.Reflection;

    public class AttributeHelper<T>
    {
        public static T[] GetAssemblyLevelAttributes(Type type)
        {
            return type.Assembly.GetCustomAttributes(typeof(T), false) as T[];
        }

        public static T[] GetClassLevelAttributes(Type type)
        {
            return type.GetCustomAttributes(typeof(T), false) as T[];
        }

        public static T[] GetMethodLevelAttributes(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(T), false) as T[];
        }
    }
}