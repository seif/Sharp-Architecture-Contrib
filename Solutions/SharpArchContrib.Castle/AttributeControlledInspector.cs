namespace SharpArchContrib.Castle
{
    using System;

    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.ModelBuilder;

    public abstract class AttributeControlledInspector<TAttribute, TAttSettings> : IContributeComponentModelConstruction
        where TAttribute : Attribute
        where TAttSettings : class, new()
    {
        private readonly Type interceptorType;
        private AttributeSettingsStorage<TAttSettings> attributeSettingsStorage;

        protected AttributeControlledInspector(Type interceptorType)
        {
            this.interceptorType = interceptorType;
        }

        protected Type InterceptorType
        {
            get
            {
                return this.interceptorType;
            }
        }

        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (this.attributeSettingsStorage == null)
            {
                this.attributeSettingsStorage = kernel.Resolve<AttributeSettingsStorage<TAttSettings>>();
            }

            if (AttributeHelper<TAttribute>.PopulateAttributeSettings(this.attributeSettingsStorage, model, this.GetAttributeSettings))
            {
                this.RegisterInterceptor(kernel, model);
            }
        }

        protected virtual void RegisterInterceptor(IKernel kernel, ComponentModel model)
        {
            model.Interceptors.Add(new InterceptorReference(this.interceptorType));
        }

        protected virtual void AddInterceptor(ComponentModel model)
        {
            model.Interceptors.Add(new InterceptorReference(this.InterceptorType));
        }

        protected abstract TAttSettings GetAttributeSettings(
            TAttribute[] assemblyAttributes, TAttribute[] classAttributes, TAttribute[] methodAttributes);
    }
}