namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class TransactionFacility : AttributeControlledFacilityBase<TransactionAttribute, TransactionInterceptor>
    {
        // TODO make sure the interceptor gets added to run first!
        public TransactionFacility()
            : base(LifestyleType.Transient)
        {
        }
   }
}