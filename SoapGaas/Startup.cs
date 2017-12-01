using Microsoft.Owin;
using Owin;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

[assembly: OwinStartup(typeof(SoapGaas.Startup))]

namespace SoapGaas
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(async ctx =>
            {
                if (!ctx.Request.Query.Any(a => a.Key == "wsdl"))
                {
                    await ctx.Response.WriteAsync("wsdl parameter not found");
                    return;
                }
                var wsdl = ctx.Request.Query["wsdl"];
                var generator = new SoapHelper.ClientGenerator();
                wsdl = new WebClient().DownloadString(wsdl);
                var code = generator.GenerateClass(wsdl);
                MemoryStream ms = generator.CompileStream(code) as MemoryStream;
                var ass = generator.CompileAssembly(ms);
                var name = ass.GetTypes().FirstOrDefault(a => a.Name.ToLower().EndsWith("service"))?.Name ?? "FooBar";
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        var codeFile = archive.CreateEntry(name + ".cs");

                        using (var entryStream = codeFile.Open())
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            streamWriter.Write(code);
                        }

                        var assemblyFile = archive.CreateEntry(name + ".dll");
                        using (var entryStream = assemblyFile.Open())
                        {
                            ms.Position = 0;
                            ms.CopyTo(entryStream);
                            entryStream.Flush();
                        }
                    }

                    memoryStream.Position = 0;
                    memoryStream.CopyTo(ctx.Response.Body);
                }
                ctx.Response.Headers["Content-Disposition"] = $"attachment; filename=\"{name}.zip\"";
                await ctx.Response.WriteAsync("");

            });
        }
    }
}
