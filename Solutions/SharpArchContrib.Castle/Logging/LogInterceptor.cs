namespace SharpArchContrib.Castle.Logging
{
    using System;

    using global::Castle.DynamicProxy;

    using SharpArchContrib.Core;
    using SharpArchContrib.Core.Logging;

    public class LogInterceptor : IInterceptor
    {
        private readonly IMethodLogger methodLogger;

        private readonly AttributeSettingsStorage<LogAttributeSettings> settingsStorage;

        public LogInterceptor(IMethodLogger methodLogger, AttributeSettingsStorage<LogAttributeSettings> settingsStorage)
        {
            ParameterCheck.ParameterRequired(methodLogger, "methodLogger");
            ParameterCheck.ParameterRequired(methodLogger, "settingsStorage");

            this.methodLogger = methodLogger;
            this.settingsStorage = settingsStorage;
        }
        
        public virtual void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

            var logAttributeSettings = settingsStorage.GetSettingsForMethod(methodInfo);

            if (logAttributeSettings == null)
            {
                invocation.Proceed();
            }
            else
            {
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
        }
    }
}