using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SoapHelper
{
    public static class SoapAssemblyHelper
    {
        public static IDictionary<string, object> InvokeWebService(this Assembly assembly, string url, string serviceName, params object[] parameters)
        {
            var service = assembly.ExportedTypes.FirstOrDefault(e => e.Name.EndsWith("Service"));
            var methods = service.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var instance = Activator.CreateInstance(service);
            var returnValues = methods.FirstOrDefault(a => a.Name == serviceName)?.Invoke(instance, parameters);

            var responseType = returnValues.GetType();
            return responseType.GetFields().ToDictionary(a => a.Name, a => a.GetValue(returnValues)).Concat(responseType.GetProperties().ToDictionary(a => a.Name, a => a.GetValue(returnValues))).ToDictionary(a => a.Key, a => a.Value);

        }

        public static T InvokeWebService<T>(this Assembly assembly, string url, string serviceName, params object[] parameters) where T : class, new()
        {
            var dic = InvokeWebService(assembly, url, serviceName, parameters);
            var response = new T();
            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            foreach (var field in fields.Where(a=> dic.Keys.Any(k=> k == a.Name)))
            {
                field.SetValue(response, dic[field.Name]);
            }

            foreach (var property in properties.Where(a => dic.Keys.Any(k => k == a.Name)))
            {
                property.SetValue(response, dic[property.Name]);
            }

            return response;

        }
    }
}
