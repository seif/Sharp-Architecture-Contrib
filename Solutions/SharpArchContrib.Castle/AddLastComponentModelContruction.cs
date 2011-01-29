namespace SharpArchContrib.Castle
{
    using System;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;

    public class AddLastComponentModelContruction<TAttribute, TInterceptor> : AttributeControlledComponentModelConstruction<TAttribute, TInterceptor>
        where TInterceptor : IInterceptor 
        where TAttribute : Attribute
    {
        protected override void AddInterceptor(ComponentModel model)
        {
            model.Interceptors.AddLast(new InterceptorReference(typeof(TInterceptor)));
        }
    }
}