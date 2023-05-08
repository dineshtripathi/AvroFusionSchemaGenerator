//using Xunit;
//using Moq;
//using AvroFusionGenerator.Implementation;
//using AvroFusionGenerator.ServiceInterface;
//using FluentAssertions;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis;
//using System.Reflection;
//using System.ComponentModel.DataAnnotations;
//using System.Data;
//using System.Text;
//using System.Xml;
//using System.Xml.Serialization;
//using AvroFusionGenerator.Implementation.AvroTypeHandlers;
//using Microsoft.CodeAnalysis.Emit;
//using Newtonsoft.Json;
//using Microsoft.CodeAnalysis.CSharp.Syntax;

//namespace AvroFusionGenerator.Tests
//{
//    public class AvroFusionDynamicAssemblyGeneratorTests
//    {
//        private readonly AvroFusionDynamicAssemblyGenerator _dynamicAssemblyGenerator;
//        private readonly Mock<IAvroFusionFileReader> _fileReaderMock;
//        private readonly Mock<IAvroFusionSyntaxTreeManager> _syntaxTreeManagerMock;

//        public AvroFusionDynamicAssemblyGeneratorTests()
//        {
//            _fileReaderMock = new Mock<IAvroFusionFileReader>();
//            _syntaxTreeManagerMock = new Mock<IAvroFusionSyntaxTreeManager>();
//            _dynamicAssemblyGenerator =
//                new AvroFusionDynamicAssemblyGenerator(_fileReaderMock.Object, _syntaxTreeManagerMock.Object);
//        }

//        [Fact]
//        public void GenerateDynamicAssembly_ValidInputs_ReturnsTypes()
//        {
//            // Arrange
//            var baseDirectory = AppContext.BaseDirectory;
//            var sourceDirectory =
//                Path.GetFullPath(Path.Combine(baseDirectory, "..\\..\\..\\..\\SampleAndTest\\TestModels"));


//            var csharpModelParentClass = "ResourceGroup";
//            var sourceFilePaths = new[]
//            {
//                Path.Combine(sourceDirectory, "AppService.cs"),
//                Path.Combine(sourceDirectory, "AppServiceStatus.cs"),
//                Path.Combine(sourceDirectory, "ArmTemplateMetaData.cs"),
//                Path.Combine(sourceDirectory, "AzureResource.cs"),
//                Path.Combine(sourceDirectory, "Container.cs"),
//                Path.Combine(sourceDirectory, "CosmosDb.cs"),
//                Path.Combine(sourceDirectory, "Database.cs"),
//                Path.Combine(sourceDirectory, "ResourceGroup.cs"),
//                Path.Combine(sourceDirectory, "DataFactoryPipeline.cs"),
//                Path.Combine(sourceDirectory, "EventGrid.cs"),
//                Path.Combine(sourceDirectory, "EventHub.cs"),
//                Path.Combine(sourceDirectory, "Schema.cs"),
//                Path.Combine(sourceDirectory, "SchemaGroup.cs"),
//                Path.Combine(sourceDirectory, "StorageAccount.cs"),
//                Path.Combine(sourceDirectory, "VirtualMachine.cs"),
//                Path.Combine(sourceDirectory, "VirtualMachineStatus.cs")
//            };

//            _fileReaderMock.Setup(x => x.GetFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories))
//                .Returns(sourceFilePaths);

//            // Set up the desired return values for the syntax tree manager mock
//            var syntaxTrees = new List<SyntaxTree>(); 
//            var mainClassSyntaxTree = CSharpSyntaxTree.ParseText(""); 
//            var syntaxTreesNoDup = new List<SyntaxTree>(); 
//            var referencedAssemblies = new List<MetadataReference>(); 
//            var compilation = CSharpCompilation.Create("DynamicAssembly"); 
//            var outputStream = new MemoryStream();

//            var usingDirectives = syntaxTrees.SelectMany(st => st.GetRoot().DescendantNodes()
//                    .OfType<UsingDirectiveSyntax>()
//                    .Select(uds => uds.Name.ToString()))
//                .Distinct()
//                .ToList();
//            var generatedAssembly = GenerateTestAssembly(sourceFilePaths, usingDirectives); 
//            // Provide the desired generated assembly

//            _syntaxTreeManagerMock.Setup(x => x.CreateSyntaxTrees(sourceFilePaths)).Returns(syntaxTrees);
//            _syntaxTreeManagerMock.Setup(x => x.FindMainClassSyntaxTree(syntaxTrees, csharpModelParentClass))
//                .Returns(mainClassSyntaxTree);
//            _syntaxTreeManagerMock.Setup(x => x.RemoveDuplicateAssemblyAttributes(syntaxTrees))
//                .Returns(syntaxTreesNoDup);
//            _syntaxTreeManagerMock.Setup(x => x.GetReferencedAssemblies(syntaxTrees)).Returns(referencedAssemblies);
//            _syntaxTreeManagerMock.Setup(x => x.CreateCompilationCode(syntaxTreesNoDup, referencedAssemblies))
//                .Returns(compilation);
//            _syntaxTreeManagerMock.Setup(x => x.LoadCompilationCode(compilation)).Returns(outputStream);
//            _syntaxTreeManagerMock.Setup(x => x.LoadRequiredAssembly(outputStream)).Returns(generatedAssembly);

//            // Act
//            var types = _dynamicAssemblyGenerator.GenerateDynamicAssembly(sourceDirectory, csharpModelParentClass);

//            // Assert
//            types.Should().NotBeNullOrEmpty();
//            types.Count().Should().BeGreaterThan(0);
//        }


//private Assembly GenerateTestAssembly(string[] sourceFilePaths, List<string> usingDirectives)
//    {
//        // Read the content of each source file
//        string[] sourceCodes = sourceFilePaths.Select(File.ReadAllText).ToArray();
       
//            // Create syntax trees from the source code
//            SyntaxTree[] syntaxTrees = sourceCodes.Select(x => CSharpSyntaxTree.ParseText(x)).ToArray();

//        // Compile the syntax trees
//        CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
//        var references = new List<MetadataReference>
//        {
//        MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(Dictionary<,>).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(DateTime).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(TimeSpan).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(decimal).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(AvroUnionTypeAttribute).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(JsonConvert).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(XmlReader).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(DataSet).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(ValidationAttribute).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(XmlSerializer).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(HttpClient).Assembly.Location),
//        MetadataReference.CreateFromFile(typeof(AvroFusionGeneratorEntryPoint).GetTypeInfo().Assembly.Location),
//        MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location)
//        };
//        if (usingDirectives.Contains("System.Collections.Generic"))
//        {
//            references.Add(MetadataReference.CreateFromFile(typeof(HashSet<>).Assembly.Location));
//            references.Add(MetadataReference.CreateFromFile(typeof(IEnumerable<>).Assembly.Location));
//            references.Add(MetadataReference.CreateFromFile(typeof(IList<>).Assembly.Location));
//        }

//        if (usingDirectives.Contains("System.Linq"))
//            references.Add(MetadataReference.CreateFromFile(typeof(IQueryable<>).Assembly.Location));

//            CSharpCompilation compilation = CSharpCompilation.Create("DynamicTestAssembly", syntaxTrees, references, compilationOptions);

//        // Emit the assembly
//        using (MemoryStream stream = new MemoryStream())
//        {
//            EmitResult emitResult = compilation.Emit(stream);

//            if (!emitResult.Success)
//            {
//                IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(diagnostic =>
//                    diagnostic.IsWarningAsError ||
//                    diagnostic.Severity == DiagnosticSeverity.Error);

//                StringBuilder errorMessage = new StringBuilder();
//                errorMessage.AppendLine("Failed to compile the provided C# source code:");

//                foreach (Diagnostic diagnostic in failures)
//                {
//                    errorMessage.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
//                }

//                throw new InvalidOperationException(errorMessage.ToString());
//            }

//            stream.Seek(0, SeekOrigin.Begin);
//            Assembly generatedAssembly = Assembly.Load(stream.ToArray());
//            return generatedAssembly;
//        }
//    }


//}
//}

