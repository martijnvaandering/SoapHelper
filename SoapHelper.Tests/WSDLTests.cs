﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace SoapHelper.Tests
{
    [TestClass]
    public class WSDLTests
    {
        public ITestOutputHelper TestOutputHelper { get; }
        //public const string wsdl = "<?xml version=\"1.0\" encoding=\"utf-8\"?><wsdl:definitions xmlns:tm=\"http://microsoft.com/wsdl/mime/textMatching/\" xmlns:soapenc=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:mime=\"http://schemas.xmlsoap.org/wsdl/mime/\" xmlns:tns=\"http://www.webservicex.net/\" xmlns:soap=\"http://schemas.xmlsoap.org/wsdl/soap/\" xmlns:s=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://schemas.xmlsoap.org/wsdl/soap12/\" xmlns:http=\"http://schemas.xmlsoap.org/wsdl/http/\" targetNamespace=\"http://www.webservicex.net/\" xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\"><wsdl:types><s:schema elementFormDefault=\"qualified\" targetNamespace=\"http://www.webservicex.net/\"><s:element name=\"GetGeoIP\"><s:complexType><s:sequence><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"IPAddress\" type=\"s:string\" /></s:sequence></s:complexType></s:element><s:element name=\"GetGeoIPResponse\"><s:complexType><s:sequence><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"GetGeoIPResult\" type=\"tns:GeoIP\" /></s:sequence></s:complexType></s:element><s:complexType name=\"GeoIP\"><s:sequence><s:element minOccurs=\"1\" maxOccurs=\"1\" name=\"ReturnCode\" type=\"s:int\" /><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"IP\" type=\"s:string\" /><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"ReturnCodeDetails\" type=\"s:string\" /><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"CountryName\" type=\"s:string\" /><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"CountryCode\" type=\"s:string\" /></s:sequence></s:complexType><s:element name=\"GetGeoIPContext\"><s:complexType /></s:element><s:element name=\"GetGeoIPContextResponse\"><s:complexType><s:sequence><s:element minOccurs=\"0\" maxOccurs=\"1\" name=\"GetGeoIPContextResult\" type=\"tns:GeoIP\" /></s:sequence></s:complexType></s:element><s:element name=\"GeoIP\" nillable=\"true\" type=\"tns:GeoIP\" /></s:schema></wsdl:types><wsdl:message name=\"GetGeoIPSoapIn\"><wsdl:part name=\"parameters\" element=\"tns:GetGeoIP\" /></wsdl:message><wsdl:message name=\"GetGeoIPSoapOut\"><wsdl:part name=\"parameters\" element=\"tns:GetGeoIPResponse\" /></wsdl:message><wsdl:message name=\"GetGeoIPContextSoapIn\"><wsdl:part name=\"parameters\" element=\"tns:GetGeoIPContext\" /></wsdl:message><wsdl:message name=\"GetGeoIPContextSoapOut\"><wsdl:part name=\"parameters\" element=\"tns:GetGeoIPContextResponse\" /></wsdl:message><wsdl:message name=\"GetGeoIPHttpGetIn\"><wsdl:part name=\"IPAddress\" type=\"s:string\" /></wsdl:message><wsdl:message name=\"GetGeoIPHttpGetOut\"><wsdl:part name=\"Body\" element=\"tns:GeoIP\" /></wsdl:message><wsdl:message name=\"GetGeoIPContextHttpGetIn\" /><wsdl:message name=\"GetGeoIPContextHttpGetOut\"><wsdl:part name=\"Body\" element=\"tns:GeoIP\" /></wsdl:message><wsdl:message name=\"GetGeoIPHttpPostIn\"><wsdl:part name=\"IPAddress\" type=\"s:string\" /></wsdl:message><wsdl:message name=\"GetGeoIPHttpPostOut\"><wsdl:part name=\"Body\" element=\"tns:GeoIP\" /></wsdl:message><wsdl:message name=\"GetGeoIPContextHttpPostIn\" /><wsdl:message name=\"GetGeoIPContextHttpPostOut\"><wsdl:part name=\"Body\" element=\"tns:GeoIP\" /></wsdl:message><wsdl:portType name=\"GeoIPServiceSoap\"><wsdl:operation name=\"GetGeoIP\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIP enables you to easily look up countries by IP addresses</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPSoapIn\" /><wsdl:output message=\"tns:GetGeoIPSoapOut\" /></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIPContext enables you to easily look up countries by Context</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPContextSoapIn\" /><wsdl:output message=\"tns:GetGeoIPContextSoapOut\" /></wsdl:operation></wsdl:portType><wsdl:portType name=\"GeoIPServiceHttpGet\"><wsdl:operation name=\"GetGeoIP\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIP enables you to easily look up countries by IP addresses</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPHttpGetIn\" /><wsdl:output message=\"tns:GetGeoIPHttpGetOut\" /></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIPContext enables you to easily look up countries by Context</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPContextHttpGetIn\" /><wsdl:output message=\"tns:GetGeoIPContextHttpGetOut\" /></wsdl:operation></wsdl:portType><wsdl:portType name=\"GeoIPServiceHttpPost\"><wsdl:operation name=\"GetGeoIP\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIP enables you to easily look up countries by IP addresses</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPHttpPostIn\" /><wsdl:output message=\"tns:GetGeoIPHttpPostOut\" /></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><wsdl:documentation xmlns:wsdl=\"http://schemas.xmlsoap.org/wsdl/\">GeoIPService - GetGeoIPContext enables you to easily look up countries by Context</wsdl:documentation><wsdl:input message=\"tns:GetGeoIPContextHttpPostIn\" /><wsdl:output message=\"tns:GetGeoIPContextHttpPostOut\" /></wsdl:operation></wsdl:portType><wsdl:binding name=\"GeoIPServiceSoap\" type=\"tns:GeoIPServiceSoap\"><soap:binding transport=\"http://schemas.xmlsoap.org/soap/http\" /><wsdl:operation name=\"GetGeoIP\"><soap:operation soapAction=\"http://www.webservicex.net/GetGeoIP\" style=\"document\" /><wsdl:input><soap:body use=\"literal\" /></wsdl:input><wsdl:output><soap:body use=\"literal\" /></wsdl:output></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><soap:operation soapAction=\"http://www.webservicex.net/GetGeoIPContext\" style=\"document\" /><wsdl:input><soap:body use=\"literal\" /></wsdl:input><wsdl:output><soap:body use=\"literal\" /></wsdl:output></wsdl:operation></wsdl:binding><wsdl:binding name=\"GeoIPServiceSoap12\" type=\"tns:GeoIPServiceSoap\"><soap12:binding transport=\"http://schemas.xmlsoap.org/soap/http\" /><wsdl:operation name=\"GetGeoIP\"><soap12:operation soapAction=\"http://www.webservicex.net/GetGeoIP\" style=\"document\" /><wsdl:input><soap12:body use=\"literal\" /></wsdl:input><wsdl:output><soap12:body use=\"literal\" /></wsdl:output></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><soap12:operation soapAction=\"http://www.webservicex.net/GetGeoIPContext\" style=\"document\" /><wsdl:input><soap12:body use=\"literal\" /></wsdl:input><wsdl:output><soap12:body use=\"literal\" /></wsdl:output></wsdl:operation></wsdl:binding><wsdl:binding name=\"GeoIPServiceHttpGet\" type=\"tns:GeoIPServiceHttpGet\"><http:binding verb=\"GET\" /><wsdl:operation name=\"GetGeoIP\"><http:operation location=\"/GetGeoIP\" /><wsdl:input><http:urlEncoded /></wsdl:input><wsdl:output><mime:mimeXml part=\"Body\" /></wsdl:output></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><http:operation location=\"/GetGeoIPContext\" /><wsdl:input><http:urlEncoded /></wsdl:input><wsdl:output><mime:mimeXml part=\"Body\" /></wsdl:output></wsdl:operation></wsdl:binding><wsdl:binding name=\"GeoIPServiceHttpPost\" type=\"tns:GeoIPServiceHttpPost\"><http:binding verb=\"POST\" /><wsdl:operation name=\"GetGeoIP\"><http:operation location=\"/GetGeoIP\" /><wsdl:input><mime:content type=\"application/x-www-form-urlencoded\" /></wsdl:input><wsdl:output><mime:mimeXml part=\"Body\" /></wsdl:output></wsdl:operation><wsdl:operation name=\"GetGeoIPContext\"><http:operation location=\"/GetGeoIPContext\" /><wsdl:input><mime:content type=\"application/x-www-form-urlencoded\" /></wsdl:input><wsdl:output><mime:mimeXml part=\"Body\" /></wsdl:output></wsdl:operation></wsdl:binding><wsdl:service name=\"GeoIPService\"><wsdl:port name=\"GeoIPServiceSoap\" binding=\"tns:GeoIPServiceSoap\"><soap:address location=\"http://www.webservicex.net/geoipservice.asmx\" /></wsdl:port><wsdl:port name=\"GeoIPServiceSoap12\" binding=\"tns:GeoIPServiceSoap12\"><soap12:address location=\"http://www.webservicex.net/geoipservice.asmx\" /></wsdl:port><wsdl:port name=\"GeoIPServiceHttpGet\" binding=\"tns:GeoIPServiceHttpGet\"><http:address location=\"http://www.webservicex.net/geoipservice.asmx\" /></wsdl:port><wsdl:port name=\"GeoIPServiceHttpPost\" binding=\"tns:GeoIPServiceHttpPost\"><http:address location=\"http://www.webservicex.net/geoipservice.asmx\" /></wsdl:port></wsdl:service></wsdl:definitions>";

        public WSDLTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GenerateTest()
        {
            string wsdl;
            using (var client = new WebClient())
            {
                wsdl = client.DownloadString("http://www.webservicex.net/geoipservice.asmx?WDSL");
            }
            var generator = new ClientGenerator();
            var x = generator.GenerateClass(wsdl);
            var ass = generator.Compile(x);

            var service = ass.ExportedTypes.FirstOrDefault(e => e.Name.EndsWith("Service"));
            var methods = service.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);


        }

        [Fact]
        public void InvokeTest()
        {
            string wsdl;
            using (var client = new WebClient())
            {
                wsdl = client.DownloadString("http://www.webservicex.net/geoipservice.asmx?wsdl");
            }
            var generator = new ClientGenerator();
            var x = generator.GenerateClass(wsdl);
            var ass = generator.Compile(x);

            var response = ass.InvokeWebService("", "GetGeoIP", "127.0.0.1");
        }

        [Fact]
        public void MappedInvokeTest()
        {
            string wsdl;
            using (var client = new WebClient())
            {
                wsdl = client.DownloadString("http://www.webservicex.net/geoipservice.asmx?wsdl");
            }
            var generator = new ClientGenerator();
            var x = generator.GenerateClass(wsdl);
            var ass = generator.Compile(x);

            var response = ass.InvokeWebService<IpResponse>("", "GetGeoIP", "127.0.0.1");
        }


        public class IpResponse
        {
            public int ReturnCode { get; set; }
            public string IP { get; set; }
            public string ReturnCodeDetails { get; set; }
            public string CountryName;
            public string CountryCode { get; set; }
        }
    }
}
