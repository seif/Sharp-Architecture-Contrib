namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;
    using global::Castle.MicroKernel.ModelBuilder;

    using SharpArchContrib.Data.NHibernate;

    public class TransactionFacility : AttributeControlledFacilityBase<TransactionAttributeSettings, TransactionInterceptor>
    {
        public TransactionFacility()
            : base(LifestyleType.Transient)
        {
        }

        protected override void AddContributor()
        {
            this.Kernel.ComponentModelBuilder.AddContributor(new TransactionComponentInspector<TransactionAttribute>(typeof(TransactionInterceptor)));
        }
    }
}