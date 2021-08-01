using Microsoft.CodeAnalysis;
using System;
using System.CodeDom;
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
            //var schema = context.AdditionalFiles.First(t => t.Path.EndsWith(".xsd"));
            AdditionalText schema;
            try
            {
                schema = context.AdditionalFiles.First();
            }
            catch (Exception e)
            {
                context.AddSource("errors", e.Message);
                return;
            }

            var schemaStr = schema.GetText().ToString();
            var stringReader = new StringReader(schemaStr);

            var schemaSet = new XmlSchemaSet();
            schemaSet.Add("mysamplenamespace", XmlReader.Create(stringReader));

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
