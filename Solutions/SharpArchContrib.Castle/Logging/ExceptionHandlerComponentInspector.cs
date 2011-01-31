namespace SharpArchContrib.Castle.Logging
{
    using SharpArchContrib.Core.Logging;

    public class ExceptionHandlerComponentInspector : AttributeControlledInspector<ExceptionHandlerAttribute, ExceptionHandlerAttributeSettings>
    {
        public ExceptionHandlerComponentInspector()
            : base(typeof(ExceptionHandlerInterceptor))
        {
        }

        protected override ExceptionHandlerAttributeSettings GetAttributeSettings(ExceptionHandlerAttribute[] assemblyAttributes, ExceptionHandlerAttribute[] classAttributes, ExceptionHandlerAttribute[] methodAttributes)
        {
            if (methodAttributes.Length > 0)
            {
                return methodAttributes[0].Settings;
            }

            if (classAttributes.Length > 0)
            {
                return classAttributes[0].Settings;
            }

            if (assemblyAttributes.Length > 0)
            {
                return assemblyAttributes[0].Settings;
            }
            
            return null;
        }
    }
}