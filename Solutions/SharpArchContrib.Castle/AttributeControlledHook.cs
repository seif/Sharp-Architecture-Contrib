namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::Castle.DynamicProxy;

    public class AttributeControlledHook<T> : IProxyGenerationHook
        where T : Attribute
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            bool shouldInterceptMethod = type != typeof(T) && AttributeHelper<T>.ShouldInterceptMethod(methodInfo);
            return shouldInterceptMethod;
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