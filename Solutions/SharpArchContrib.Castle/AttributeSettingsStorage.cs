namespace SharpArchContrib.Castle
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;

    public class AttributeSettingsStorage<TAttSettings>
    {
        private readonly IDictionary<MethodInfo, TAttSettings> methodAttributeSettings = new ConcurrentDictionary<MethodInfo, TAttSettings>();

        public virtual void RegisterSettings(MethodInfo methodInfo, TAttSettings attributeSettings)
        {
            this.methodAttributeSettings.Add(methodInfo, attributeSettings);
        }

        public virtual TAttSettings GetSettingsForMethod(MethodInfo methodInfo)
        {
            if (this.methodAttributeSettings.ContainsKey(methodInfo))
            {
                return this.methodAttributeSettings[methodInfo];
            }

            return default(TAttSettings);
        }
    }
}
