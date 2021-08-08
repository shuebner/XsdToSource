using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using XmlSchemaClassGenerator;

namespace XsdToSource
{
    [Generator]
    public class XsdSourceGenerator : ISourceGenerator
    {
        internal class MemoryOutputWriter : OutputWriter
        {
            public string Content { get; set; }

            public override void Write(CodeNamespace cn)
            {
                var cu = new CodeCompileUnit();
                cu.Namespaces.Add(cn);

                using (var writer = new StringWriter())
                {
                    Write(writer, cu);
                    Content = writer.ToString();
                }
            }
        }

        public void Execute(GeneratorExecutionContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
#endif
            var configurations = GetConfigurations(context);

            foreach (var (schemaFile, @namespace) in configurations)
            {
                var schemaStr = schemaFile.GetText().ToString();
                var stringReader = new StringReader(schemaStr);

                var schemaSet = new XmlSchemaSet();
                schemaSet.Add("mysamplenamespace", XmlReader.Create(stringReader));

                var generator = new Generator();
                generator.NamespaceProvider.Add(new NamespaceKey("mysamplenamespace"), @namespace);
                MemoryOutputWriter memoryOutputWriter = new MemoryOutputWriter();
                generator.OutputWriter = memoryOutputWriter;
                generator.Generate(schemaSet);
                context.AddSource("Pocos", memoryOutputWriter.Content);
            }            
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // do nothing
        }

        static IEnumerable<(AdditionalText SchemaFile, string Namespace)> GetConfigurations(GeneratorExecutionContext context)
        {
            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (Path.GetExtension(file.Path).Equals(".xsd", StringComparison.OrdinalIgnoreCase))
                {
                    AnalyzerConfigOptions analyzerConfigOptions = context.AnalyzerConfigOptions.GetOptions(file);
                    if (analyzerConfigOptions.TryGetValue("build_metadata.AdditionalFiles.XsdToSource_RootNamespace", out string @namespace))
                    {
                        yield return (file, @namespace);
                    }

                }
            }
        }
    }
}
