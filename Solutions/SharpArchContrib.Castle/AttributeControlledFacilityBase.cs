namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.Facilities;

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
            this.Kernel.AddComponent(
                this.interceptorType.Name, typeof(IInterceptor), this.interceptorType, this.lifestyleType);
            this.Kernel.ComponentRegistered += this.KernelComponentRegistered;
        }

        private void KernelComponentRegistered(string key, IHandler handler)
        {
            //TODO how do we hook up the hook???
            handler.ComponentModel.Interceptors.Add(new InterceptorReference(this.interceptorType.Name));
        }
    }
}