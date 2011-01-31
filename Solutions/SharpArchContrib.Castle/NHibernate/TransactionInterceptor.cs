namespace SharpArchContrib.Castle.NHibernate
{
    using System;
    using System.Reflection;

    using global::Castle.DynamicProxy;

    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core;
    using SharpArchContrib.Core.Logging;
    using SharpArchContrib.Data.NHibernate;

    public class TransactionInterceptor : IInterceptor
    {
        protected readonly IExceptionLogger ExceptionLogger;
        protected readonly ITransactionManager TransactionManager;
        private readonly AttributeSettingsStorage<TransactionAttributeSettings> settingsStorage;

        public TransactionInterceptor(ITransactionManager transactionManager, IExceptionLogger exceptionLogger, AttributeSettingsStorage<TransactionAttributeSettings> settingsStorage)
        {
            ParameterCheck.ParameterRequired(transactionManager, "transactionManager");
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");
            ParameterCheck.ParameterRequired(exceptionLogger, "settingsStorage");

            this.TransactionManager = transactionManager;
            this.ExceptionLogger = exceptionLogger;
            this.settingsStorage = settingsStorage;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

            // we take the settings from the first attribute we find searching method first
            // If there is at least one attribute, the call gets wrapped with a transaction
            TransactionAttributeSettings transactionAttributeSettings = GetTransactionAttributeSettings(methodInfo);
            if (transactionAttributeSettings == null)
            {
                invocation.Proceed();
            }
            else
            {
                var transactionState = this.OnEntry(transactionAttributeSettings, null);
                try
                {
                    invocation.Proceed();
                }
                catch (Exception err)
                {
                    this.CloseUnitOfWork(transactionAttributeSettings, transactionState, err);
                    if (!(err is AbortTransactionException))
                    {
                        this.ExceptionLogger.LogException(
                            err, transactionAttributeSettings.IsExceptionSilent, methodInfo.ReflectedType);
                    }

                    if (this.TransactionManager.TransactionDepth == 0 &&
                        (transactionAttributeSettings.IsExceptionSilent || err is AbortTransactionException))
                    {
                        invocation.ReturnValue = transactionAttributeSettings.ReturnValue;
                        return;
                    }

                    throw;
                }

                transactionState = this.OnSuccess(transactionAttributeSettings, transactionState);
            }
        }

        protected virtual TransactionAttributeSettings GetTransactionAttributeSettings(MethodInfo methodInfo)
        {
            return this.settingsStorage.GetSettingsForMethod(methodInfo);
        }

        protected virtual object CloseUnitOfWork(
            TransactionAttributeSettings transactionAttributeSettings, object transactionState, Exception err)
        {
            var factoryKey = transactionAttributeSettings.FactoryKey;
            if (err == null)
            {
                try
                {
                    NHibernateSession.CurrentFor(factoryKey).Flush();
                    transactionState = this.TransactionManager.CommitTransaction(factoryKey, transactionState);
                }
                catch (Exception)
                {
                    transactionState = this.TransactionManager.RollbackTransaction(factoryKey, transactionState);
                    transactionState = this.TransactionManager.PopTransaction(factoryKey, transactionState);
                    throw;
                }
            }
            else
            {
                transactionState = this.TransactionManager.RollbackTransaction(factoryKey, transactionState);
            }

            transactionState = this.TransactionManager.PopTransaction(factoryKey, transactionState);

            return transactionState;
        }

        private object OnEntry(TransactionAttributeSettings transactionAttributeSettings, object transactionState)
        {
            return this.TransactionManager.PushTransaction(transactionAttributeSettings.FactoryKey, transactionState);
        }

        private object OnSuccess(TransactionAttributeSettings transactionAttributeSettings, object transactionState)
        {
            return this.CloseUnitOfWork(transactionAttributeSettings, transactionState, null);
        }
    }
}