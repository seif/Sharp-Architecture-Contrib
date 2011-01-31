namespace SharpArchContrib.Castle.NHibernate
{
    using System;

    using global::Castle.Core;
    using global::Castle.MicroKernel;

    using SharpArchContrib.Data.NHibernate;

    public class TransactionComponentInspector<T> : AttributeControlledInspector<T, TransactionAttributeSettings>
        where T : Attribute, ITransactionAttributeSettings
    {
        public TransactionComponentInspector(Type interceptorType)
            : base(interceptorType)
        {
        }

        protected override void RegisterInterceptor(IKernel kernel, ComponentModel model)
        {
            model.Interceptors.AddLast(new InterceptorReference(InterceptorType));
        }

        protected override TransactionAttributeSettings GetAttributeSettings(T[] assemblyAttributes, T[] classAttributes, T[] methodAttributes)
        {
            TransactionAttributeSettings transactionAttributeSettings = CreateTransactionAttributeSettings();
            if (methodAttributes.Length > 0)
            {
                transactionAttributeSettings = methodAttributes[methodAttributes.Length - 1].Settings;
            }
            else if (classAttributes.Length > 0)
            {
                transactionAttributeSettings = classAttributes[classAttributes.Length - 1].Settings;
            }

            return transactionAttributeSettings;
        }

        protected virtual TransactionAttributeSettings CreateTransactionAttributeSettings()
        {
            return new TransactionAttributeSettings();
        }
    }
}