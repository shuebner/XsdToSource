# XsdToSource
Generates POCOS from XML schema files (XSD) at compile-time.

The real work was done by Michael Ganss at https://github.com/mganss/XmlSchemaClassGenerator.
I just piggybacked the source generator.

This is a proof-of-concept.
Features like including multiple dependent schema files are not tested or implemented.

# How to use
Add nuget package (enable preview) SvSoft.XsdToSource to your project.
Then configure xsd to use by the generator:

```
<PackageReference Include="SvSoft.XsdToSource" Version="0.1.0-preview" />
<AdditionalFiles Include="myschema.xsd" XsdToSource_RootNamespace="MyRootNamespace" />
```
