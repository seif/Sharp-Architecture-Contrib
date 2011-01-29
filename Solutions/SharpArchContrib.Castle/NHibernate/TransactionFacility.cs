namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class TransactionFacility : AttributeControlledFacilityBase
    {
        public TransactionFacility()
            : base(typeof(TransactionInterceptor), LifestyleType.Transient)
        {
        }

        protected override void RegisterModelInterceptorsSelector()
        {
            this.Kernel.ProxyFactory.AddInterceptorSelector(new AttributeBasedInteceptorSelector<TransactionAttribute>());
        }
    }
}