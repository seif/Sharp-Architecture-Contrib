namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.Facilities;
    using global::Castle.MicroKernel.Registration;

    using SharpArchContrib.Castle.Logging;
    using SharpArchContrib.Core;

    public abstract class AttributeControlledFacilityBase : AbstractFacility
    {
        private readonly Type interceptorType;

        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(Type interceptorType, LifestyleType lifestyleType)
        {
            ParameterCheck.ParameterRequired(interceptorType, "interceptorType");
        
            this.interceptorType = interceptorType;
            this.lifestyleType = lifestyleType;
        }

        protected override void Init()
        {
            this.Kernel.Register(Component.For<IInterceptor>().LifeStyle.Is(this.lifestyleType).ImplementedBy(this.interceptorType).Named(this.interceptorType.Name));
            this.RegisterModelInterceptorsSelector();
        }

        protected abstract void RegisterModelInterceptorsSelector();

        protected virtual void KernelComponentRegistered(string key, IHandler handler)
        {
            handler.ComponentModel.Interceptors.Add(new InterceptorReference(this.interceptorType.Name));
        }

    }
}