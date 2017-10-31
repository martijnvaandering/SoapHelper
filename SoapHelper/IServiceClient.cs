using System.ServiceModel;

namespace SoapHelper
{
    public interface IServiceClient
    {
        TClient CreateInstance<TClient>(HttpBindingBase binding, string location) where TClient : class;
        HttpBindingBase GetBasicHttpBinding();
        TClient GetServiceClient<TClient, TInterface>(string location) where TClient : class where TInterface : class;
    }
}