namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class UnitOfWorkFacility : AttributeControlledFacilityBase<UnitOfWorkAttribute, UnitOfWorkInterceptor>
    {
        public UnitOfWorkFacility()
            : base(LifestyleType.Transient)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new AddLastComponentModelContruction<UnitOfWorkAttribute, UnitOfWorkInterceptor>());
        }
    }
}