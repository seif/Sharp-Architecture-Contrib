namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class UnitOfWorkFacility : AttributeControlledFacilityBase
    {
        public UnitOfWorkFacility()
            : base(typeof(UnitOfWorkInterceptor), LifestyleType.Transient)
        {
        }


        protected override void RegisterModelInterceptorsSelector()
        {
            this.Kernel.ProxyFactory.AddInterceptorSelector(new AttributeBasedInteceptorSelector<UnitOfWorkAttribute>());
        }
    }
}