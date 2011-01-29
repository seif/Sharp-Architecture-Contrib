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

    public abstract class AttributeControlledFacilityBase<TAttribute, TInterceptor> : AbstractFacility
        where TInterceptor : IInterceptor
        where TAttribute : Attribute
    {
        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(LifestyleType lifestyleType)
        {
            this.lifestyleType = lifestyleType;
        }

        protected override void Init()
        {
            this.RegisterInterceptor();
            this.AddContributor();
        }

        protected virtual void RegisterInterceptor()
        {
            this.Kernel.Register(Component.For<TInterceptor>().LifeStyle.Is(this.lifestyleType));
        }

        protected virtual void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new AttributeControlledComponentModelConstruction<TAttribute, TInterceptor>());
        }
    }
}