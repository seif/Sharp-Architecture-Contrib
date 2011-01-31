namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::Castle.Core;

    public class AttributeHelper<T>
        where T : Attribute
    {
        public static T[] GetAssemblyLevelAttributes(Assembly assembly)
        {
            return (T[])assembly.GetCustomAttributes(typeof(T), false);
        }

        public static T[] GetTypeLevelAttributes(Type type)
        {
            return (T[])type.GetCustomAttributes(typeof(T), false);
        }

        public static T[] GetMethodLevelAttributesForType(Type type)
        {
            List<T> attributes = new List<T>();
            foreach (var method in type.GetMethods())
            {
                attributes.AddRange(GetMethodLevelAttributes(method));
            }

            return attributes.ToArray();
        }

        public static T[] GetMethodLevelAttributes(MethodInfo methodInfo)
        {
            return (T[])methodInfo.GetCustomAttributes(typeof(T), false);
        }

        /// <summary>
        /// Adds AttributeSettings for methods in this model to the storage.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="model"></param>
        /// <returns>true if the implementation is decorated with the attribute</returns>
        public static bool PopulateAttributeSettings<TAttSettings>(
            AttributeSettingsStorage<TAttSettings> storage,
            ComponentModel model,
            Func<T[], T[], T[], TAttSettings> getAttributeSettings)
        {
            bool shouldAddInterceptor = false;
            var assemblyAttributes =
                GetAssemblyLevelAttributes(model.Implementation.Assembly);
            var classAttributes =
                GetTypeLevelAttributes(model.Implementation);
            foreach (var method in model.Implementation.GetMethods())
            {
                T[] methodAttributes = GetMethodLevelAttributes(method);
                
                if (assemblyAttributes.Length > 0 || classAttributes.Length > 0 || methodAttributes.Length > 0)
                {
                    shouldAddInterceptor = true;
                    TAttSettings attSettings = getAttributeSettings(assemblyAttributes, classAttributes, methodAttributes);
                    storage.RegisterSettings(method, attSettings);
                }
            }

            return shouldAddInterceptor;
        }
    }
}