namespace SharpArchContrib.Castle.Logging
{
    using System;

    using global::Castle.DynamicProxy;

    using SharpArchContrib.Core;
    using SharpArchContrib.Core.Logging;

    public class LogInterceptor : IInterceptor
    {
        private readonly IMethodLogger methodLogger;

        public LogInterceptor(IMethodLogger methodLogger)
        {
            ParameterCheck.ParameterRequired(methodLogger, "methodLogger");

            this.methodLogger = methodLogger;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            // we take the most permissive log settings from the attributes we find
            // If there is at least one attribute, the call gets wrapped with a transaction
            var assemblyAttributes = AttributeHelper<LogAttribute>.GetAssemblyLevelAttributes(methodInfo.ReflectedType);
            var classAttributes = AttributeHelper<LogAttribute>.GetClassLevelAttributes(methodInfo.ReflectedType);
            var methodAttributes = AttributeHelper<LogAttribute>.GetMethodLevelAttributes(methodInfo);

            var logAttributeSettings = this.GetLoggingLevels(
                    assemblyAttributes, classAttributes, methodAttributes);
            this.methodLogger.LogEntry(methodInfo, invocation.Arguments, logAttributeSettings.EntryLevel);
            try
            {
                invocation.Proceed();
            }
            catch (Exception err)
            {
                this.methodLogger.LogException(methodInfo, err, logAttributeSettings.ExceptionLevel);
                throw;
            }

            this.methodLogger.LogSuccess(methodInfo, invocation.ReturnValue, logAttributeSettings.SuccessLevel);

        }

        private LogAttributeSettings GetLoggingLevels(
            LogAttribute[] assemblyLogAttributes, LogAttribute[] classLogAttributes, LogAttribute[] methodLogAttributes)
        {
            var logAttributeSettings = new LogAttributeSettings();
            logAttributeSettings = this.GetLoggingLevels(assemblyLogAttributes, logAttributeSettings);
            logAttributeSettings = this.GetLoggingLevels(classLogAttributes, logAttributeSettings);
            logAttributeSettings = this.GetLoggingLevels(methodLogAttributes, logAttributeSettings);

            return logAttributeSettings;
        }

        private LogAttributeSettings GetLoggingLevels(
            LogAttribute[] logAttributes, LogAttributeSettings logAttributeSettings)
        {
            foreach (var logAttribute in logAttributes)
            {
                if (logAttribute.Settings.EntryLevel > logAttributeSettings.EntryLevel)
                {
                    logAttributeSettings.EntryLevel = logAttribute.Settings.EntryLevel;
                }

                if (logAttribute.Settings.SuccessLevel > logAttributeSettings.SuccessLevel)
                {
                    logAttributeSettings.SuccessLevel = logAttribute.Settings.SuccessLevel;
                }

                if (logAttribute.Settings.ExceptionLevel > logAttributeSettings.ExceptionLevel)
                {
                    logAttributeSettings.ExceptionLevel = logAttribute.Settings.ExceptionLevel;
                }
            }

            return logAttributeSettings;
        }
    }
}