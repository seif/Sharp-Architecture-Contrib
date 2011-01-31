namespace SharpArchContrib.Castle.NHibernate
{
    using System;

    using SharpArchContrib.Data.NHibernate;

    public class UnitOfWorkComponentInspector : TransactionComponentInspector<UnitOfWorkAttribute>
    {
        public UnitOfWorkComponentInspector()
            : base(typeof(UnitOfWorkInterceptor))
        {
        }

        protected override TransactionAttributeSettings CreateTransactionAttributeSettings()
        {
            return new UnitOfWorkAttributeSettings();
        }
    }
}