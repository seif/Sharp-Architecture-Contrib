namespace SharpArchContrib.Castle.Logging
{
    using SharpArchContrib.Core.Logging;

    public class LogComponentInspector : AttributeControlledInspector<LogAttribute, LogAttributeSettings>
    {
        public LogComponentInspector()
            : base(typeof(LogInterceptor))
        {
        }

        protected override LogAttributeSettings GetAttributeSettings(LogAttribute[] assemblyAttributes, LogAttribute[] classAttributes, LogAttribute[] methodAttributes)
        {
            var logAttributeSettings = new LogAttributeSettings();
            logAttributeSettings = this.GetLoggingLevels(assemblyAttributes, logAttributeSettings);
            logAttributeSettings = this.GetLoggingLevels(classAttributes, logAttributeSettings);
            logAttributeSettings = this.GetLoggingLevels(methodAttributes, logAttributeSettings);

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