using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapHelper.Tests.GeoIpService;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace SoapHelper.Tests
{
    [TestClass]
    public class ServiceClientTests
    {
        [Fact]
        public void GetBasicHttpClientTestOk()
        {
            var client = ServiceClient.Basic.GetServiceClient<GeoIPServiceSoapClient, GeoIPServiceSoap>("http://www.webservicex.net/geoipservice.asmx");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(client);

        }

        [Fact]
        public void GetBasicHttpClientTestWrongLocation()
        {
            try
            {
                var client = ServiceClient.Basic.GetServiceClient<GeoIPServiceSoapClient, GeoIPServiceSoap>("foobar");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail();
            }
            catch (Exception ex)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(ex.Message == "Location: foobar is not a valid value.");
            }
        }

        [Fact]
        public void E2ETestOk()
        {
            var client = ServiceClient.Basic.GetServiceClient<GeoIPServiceSoapClient, GeoIPServiceSoap>("http://www.webservicex.net/geoipservice.asmx");
            var response = client.GetGeoIP("127.0.0.1");
            Xunit.Assert.True(response.ReturnCode == 1);
            Xunit.Assert.True(response.IP == "127.0.0.1");
            Xunit.Assert.True(response.ReturnCodeDetails == "Success");
            Xunit.Assert.True(response.CountryName == "Reserved");
            Debug.WriteLine(string.Join("\r\n", response.GetType().GetProperties().Select(a => a.Name + ": " + a.GetValue(response)?.ToString())));
        }
    }
}