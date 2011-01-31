namespace SharpArchContrib.Castle.NHibernate
{
    using System;

    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.Logging;
    using SharpArchContrib.Data.NHibernate;

    public class UnitOfWorkInterceptor : TransactionInterceptor
    {
        public UnitOfWorkInterceptor(ITransactionManager transactionManager, IExceptionLogger exceptionLogger, AttributeSettingsStorage<TransactionAttributeSettings> settingsStorage)
            : base(transactionManager, exceptionLogger, settingsStorage)
        {
        }

        protected override TransactionAttributeSettings GetTransactionAttributeSettings(System.Reflection.MethodInfo methodInfo)
        {
            return base.GetTransactionAttributeSettings(methodInfo) as UnitOfWorkAttributeSettings;
        }

        protected override object CloseUnitOfWork(
            TransactionAttributeSettings transactionAttributeSettings, object transactionState, Exception err)
        {
            transactionState = base.CloseUnitOfWork(transactionAttributeSettings, transactionState, err);
            if (this.TransactionManager.TransactionDepth == 0)
            {
                var sessionStorage = NHibernateSession.Storage as IUnitOfWorkSessionStorage;
                if (sessionStorage != null)
                {
                    sessionStorage.EndUnitOfWork(
                        ((UnitOfWorkAttributeSettings)transactionAttributeSettings).CloseSessions);
                }
            }

            return transactionState;
        }
    }
}