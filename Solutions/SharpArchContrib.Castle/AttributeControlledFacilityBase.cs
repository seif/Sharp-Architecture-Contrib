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

    public abstract class AttributeControlledFacilityBase<TAttSettings, TInterceptor> : AbstractFacility
        where TInterceptor : IInterceptor
        where TAttSettings : class, new()
    {
        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(LifestyleType lifestyleType)
        {
            this.lifestyleType = lifestyleType;
        }

        protected override void Init()
        {
            this.Kernel.Register(Component.For<AttributeSettingsStorage<TAttSettings>>());
            this.RegisterInterceptor();
            this.AddContributor();
        }

        protected virtual void RegisterInterceptor()
        {
            this.Kernel.Register(Component.For<TInterceptor>().LifeStyle.Is(this.lifestyleType));
        }

        protected abstract void AddContributor();
    }
}