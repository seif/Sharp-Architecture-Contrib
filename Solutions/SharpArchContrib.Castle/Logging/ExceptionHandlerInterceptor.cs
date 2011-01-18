namespace SharpArchContrib.Castle.Logging
{
    using System;

    using global::Castle.DynamicProxy;

    using SharpArchContrib.Core;
    using SharpArchContrib.Core.Logging;

    public class ExceptionHandlerInterceptor : IInterceptor
    {
        private readonly IExceptionLogger exceptionLogger;

        public ExceptionHandlerInterceptor(IExceptionLogger exceptionLogger)
        {
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.exceptionLogger = exceptionLogger;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            // we take the settings from the first attribute we find searching method first
            // If there is at least one attribute, the call gets wrapped with an exception handler
            var assemblyAttributes = AttributeHelper<ExceptionHandlerAttribute>.GetAssemblyLevelAttributes(methodInfo.ReflectedType);
            var classAttributes = AttributeHelper<ExceptionHandlerAttribute>.GetClassLevelAttributes(methodInfo.ReflectedType);
            var methodAttributes = AttributeHelper<ExceptionHandlerAttribute>.GetMethodLevelAttributes(methodInfo);

            var exceptionHandlerAttributeSettings = this.GetExceptionHandlerSettings(
                assemblyAttributes, classAttributes, methodAttributes);
            try
            {
                invocation.Proceed();
            }
            catch (Exception err)
            {
                this.exceptionLogger.LogException(
                    err, exceptionHandlerAttributeSettings.IsSilent, methodInfo.ReflectedType);
                if (exceptionHandlerAttributeSettings.IsSilent)
                {
                    if (exceptionHandlerAttributeSettings.ExceptionType == null ||
                        exceptionHandlerAttributeSettings.ExceptionType == err.GetType())
                    {
                        invocation.ReturnValue = exceptionHandlerAttributeSettings.ReturnValue;
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

        }

        private ExceptionHandlerAttributeSettings GetExceptionHandlerSettings(
            ExceptionHandlerAttribute[] assemblyAttributes,
            ExceptionHandlerAttribute[] classAttributes,
            ExceptionHandlerAttribute[] methodAttributes)
        {
            if (methodAttributes.Length > 0)
            {
                return methodAttributes[0].Settings;
            }

            if (classAttributes.Length > 0)
            {
                return classAttributes[0].Settings;
            }

            return assemblyAttributes[0].Settings;
        }
    }
}