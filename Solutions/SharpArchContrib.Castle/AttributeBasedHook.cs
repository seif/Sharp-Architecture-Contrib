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
            return this.GetAttributes(type, methodInfo).Any();
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            return;
        }

        public void MethodsInspected()
        {
            return;
        }

        protected List<Attribute> GetAttributes(Type type, MethodInfo methodInfo)
        {
            var attributes = new List<Attribute>();
            attributes.AddRange(AttributeHelper<T>.GetAssemblyLevelAttributes(type));
            attributes.AddRange(AttributeHelper<T>.GetClassLevelAttributes(type));
            attributes.AddRange(AttributeHelper<T>.GetMethodLevelAttributes(methodInfo));

            return attributes;
        }
    }
}