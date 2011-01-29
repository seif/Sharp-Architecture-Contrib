namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class TransactionFacility : AttributeControlledFacilityBase<TransactionAttribute, TransactionInterceptor>
    {
        public TransactionFacility()
            : base(LifestyleType.Transient)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new AddLastComponentModelContruction<TransactionAttribute, TransactionInterceptor>());
        }
    }
}