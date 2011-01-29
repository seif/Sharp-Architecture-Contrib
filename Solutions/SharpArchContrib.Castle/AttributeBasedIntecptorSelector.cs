namespace SharpArchContrib.Castle
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;
    using global::Castle.MicroKernel.Proxy;

    using SharpArchContrib.Castle.Logging;

    public class AttributeBasedInteceptorSelector<T> : IModelInterceptorsSelector where T : Attribute
    {
        public bool HasInterceptors(ComponentModel model)
        {
            return AttributeHelper<T>.ShouldInterceptType(model.Implementation) ||
                   (model.Implementation != model.Service && AttributeHelper<T>.ShouldInterceptType(model.Service));
        }

        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
        {
            var selectedInterceptors = new InterceptorReference[interceptors.Length + 1];
            interceptors.CopyTo(selectedInterceptors, 0);
            selectedInterceptors[selectedInterceptors.Length - 1] =
                new InterceptorReference(typeof(LogInterceptor).Name);
            return selectedInterceptors;
        }
    }
}