using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace SoapHelper
{
    public class ClientGenerator
    {
        public string GenerateClass(string wsdl)
        {
            ServiceDescription wsdlDescription;
            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(wsdl);
                writer.Flush();
                ms.Position = 0;
                wsdlDescription = ServiceDescription.Read(ms);
            }
            ServiceDescriptionImporter wsdlImporter = new ServiceDescriptionImporter
            {
                ProtocolName = "Soap12"
            };
            wsdlImporter.AddServiceDescription(wsdlDescription, null, null);
            wsdlImporter.Style = ServiceDescriptionImportStyle.Client;

            wsdlImporter.CodeGenerationOptions = CodeGenerationOptions.EnableDataBinding;

            CodeNamespace codeNamespace = new CodeNamespace();
            CodeCompileUnit codeUnit = new CodeCompileUnit();
            codeUnit.Namespaces.Add(codeNamespace);

            ServiceDescriptionImportWarnings importWarning = wsdlImporter.Import(codeNamespace, codeUnit);

            if (importWarning == 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);

                CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
                codeProvider.GenerateCodeFromCompileUnit(codeUnit, stringWriter, new CodeGeneratorOptions());

                stringWriter.Close();

                return stringBuilder.ToString();
            }
            else
            {
                Console.WriteLine(importWarning);
            }
            return "";
        }

        public Assembly Compile(string @class)
        {
            var types = new[] {
                typeof(object),
                typeof(System.ComponentModel.INotifyPropertyChanged),
                typeof(System.Diagnostics.DebuggerStepThroughAttribute),
                typeof(WebServiceBindingAttribute),
                typeof(XmlTypeAttribute),
                typeof(ValueTuple)
            };

            var metadataReferenceLocations = types.Select(a => a.Assembly.Location).Distinct();

            var metadataReferences = metadataReferenceLocations.Select(x =>
            {
                return MetadataReference.CreateFromFile(x);
            }).ToArray();

            var syntaxTree = CSharpSyntaxTree.ParseText(@class, CSharpParseOptions.Default);
            var compilation = CSharpCompilation.Create(
                "DynamicWebService",
                syntaxTrees: new[] { syntaxTree },
                references: metadataReferences,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    return Assembly.Load(ms.ToArray());
                }

                return null;
            }
        }
    }
}
