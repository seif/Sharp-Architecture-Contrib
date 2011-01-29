namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::Castle.DynamicProxy;

    public class AttributeBasedHook<T> : IProxyGenerationHook
        where T : Attribute
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return type != typeof(T) && AttributeHelper<T>.ShouldInterceptMethod(methodInfo);
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            return;
        }

        public void MethodsInspected()
        {
            return;
        }
    }
}