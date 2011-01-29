namespace SharpArchContrib.Castle.NHibernate
{
    using global::Castle.Core;

    public class UnitOfWorkFacility : AttributeControlledFacilityBase<UnitOfWorkAttribute, UnitOfWorkInterceptor>
    {
        public UnitOfWorkFacility()
            : base(LifestyleType.Transient)
        {
        }
    }
}