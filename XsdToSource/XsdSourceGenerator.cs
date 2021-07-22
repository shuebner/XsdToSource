using Microsoft.CodeAnalysis;
using System;
using System.CodeDom;
using System.IO;
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
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add("mysamplenamespace", XmlReader.Create(typeof(XsdSourceGenerator).Assembly.GetManifestResourceStream("XsdToSource.sample_schema.xsd")));

            var generator = new Generator();
            MemoryOutputWriter memoryOutputWriter = new MemoryOutputWriter();
            generator.OutputWriter = memoryOutputWriter;
            generator.Generate(schemaSet);
            context.AddSource("pocos", memoryOutputWriter.Content);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // do nothing
        }
    }
}
