namespace SharpArchContrib.Castle
{
    using System;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;

    public class AddFirstComponentModelContruction<TAttribute, TInterceptor> : AttributeControlledComponentModelConstruction<TAttribute, TInterceptor>
        where TInterceptor : IInterceptor 
        where TAttribute : Attribute
    {
        protected override void AddInterceptor(ComponentModel model)
        {
            model.Interceptors.AddFirst(new InterceptorReference(typeof(TInterceptor)));
        }
    }
}