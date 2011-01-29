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
        

        public virtual void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            LogAttribute[] assemblyLogAttributes = AttributeHelper<LogAttribute>.GetAssemblyLevelAttributes(methodInfo.ReflectedType.Assembly);
            LogAttribute[] classLogAttributes = AttributeHelper<LogAttribute>.GetTypeLevelAttributes(methodInfo.ReflectedType);
            LogAttribute[] methodLogAttributes = AttributeHelper<LogAttribute>.GetMethodLevelAttributes(methodInfo);

            var logAttributeSettings = this.GetLoggingLevels(
                assemblyLogAttributes, classLogAttributes, methodLogAttributes);
            
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