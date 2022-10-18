# XsdToSource
Generates POCOS from XML schema files (XSD) at compile-time.

The real work was done by Michael Ganss at https://github.com/mganss/XmlSchemaClassGenerator.
I just piggybacked the source generator.

This is a proof-of-concept.
Features like including multiple dependent schema files are not tested or implemented.

You can read about my experience writing it and how to put the pieces together on [my blog](https://svenhuebner-it.com/creating-pocos-from-xsd-with-a-source-generator/).

Please note the [improvement suggestion](https://github.com/shuebner/XsdToSource/issues/1#issue-1410284424) by [JulianBirch](https://github.com/JulianBirch).

# How to use
Add nuget package (enable preview) SvSoft.XsdToSource to your project.
Then configure xsd to use by the generator:

```
<PackageReference Include="SvSoft.XsdToSource" Version="0.1.0-preview" />
<AdditionalFiles Include="myschema.xsd" XsdToSource_RootNamespace="MyRootNamespace" />
```
