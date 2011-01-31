namespace SharpArchContrib.Castle.NHibernate
{
    using System;

    using global::Castle.Core;
    using global::Castle.MicroKernel.ModelBuilder;

    using SharpArchContrib.Data.NHibernate;

    public class UnitOfWorkFacility : AttributeControlledFacilityBase<UnitOfWorkAttributeSettings, UnitOfWorkInterceptor>
    {
        public UnitOfWorkFacility()
            : base(LifestyleType.Transient)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new UnitOfWorkComponentInspector());
        }
    }
}