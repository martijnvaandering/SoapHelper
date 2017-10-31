namespace SoapHelper
{
    using System;
    using System.ServiceModel;
    public class ServiceClient : IServiceClient
    {
        public static ServiceClient Basic
        {
            get
            {
                return new ServiceClient();
            }
        }

        public virtual TClient GetServiceClient<TClient, TInterface>(string location) where TClient : class where TInterface : class
        {
            if (!(HttpUrlValidation.IsValidHttpAddress(location) && Uri.IsWellFormedUriString(location, UriKind.RelativeOrAbsolute)))
            {
                throw new Exception($"Location: {location} is not a valid value.");
            }

            var binding = GetBasicHttpBinding();
            TClient tClient = CreateInstance<TClient>(binding, location);
            ClientBase<TInterface> clientBase = tClient as ClientBase<TInterface>;
            clientBase.Endpoint.Address = new EndpointAddress(location);
            return tClient;
        }

        public virtual HttpBindingBase GetBasicHttpBinding()
        {
            var binding = new BasicHttpBinding
            {
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = long.MaxValue,
                MaxReceivedMessageSize = int.MaxValue
            };
            return binding;
        }

        public virtual TClient CreateInstance<TClient>(HttpBindingBase binding, string location) where TClient : class
        {
            TClient tClient = Activator.CreateInstance(typeof(TClient), binding, new EndpointAddress(location)) as TClient;
            return tClient;
        }
    }
}
