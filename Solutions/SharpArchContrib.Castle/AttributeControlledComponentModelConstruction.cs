namespace SharpArchContrib.Castle
{
    using System;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ModelBuilder;
    using global::Castle.MicroKernel.Proxy;

    public class AttributeControlledComponentModelConstruction<TAttribute, TInterceptor> : IContributeComponentModelConstruction 
        where TInterceptor : IInterceptor 
        where TAttribute : Attribute
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (AttributeHelper<TAttribute>.ShouldInterceptType(model.Service) ||
                (model.Implementation != model.Service && AttributeHelper<TAttribute>.ShouldInterceptType(model.Implementation)))
            {
                var proxyOptions = ProxyUtil.ObtainProxyOptions(model, true);
                proxyOptions.Hook = new InstanceReference<IProxyGenerationHook>(new AttributeControlledHook<TAttribute>());
                AddInterceptor(model);
            }
        }

        protected virtual void AddInterceptor(ComponentModel model)
        {
            model.Interceptors.Add(new InterceptorReference(typeof(TInterceptor)));
        }
    }
}