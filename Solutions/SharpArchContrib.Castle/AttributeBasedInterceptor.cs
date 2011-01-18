namespace SharpArchContrib.Castle.Logging
{
    using System;

    using global::Castle.Core.Interceptor;

    public abstract class AttributeBasedInterceptor<T> : IInterceptor where T : Attribute
    {
        public abstract void Intercept(IInvocation invocation);
    }
}