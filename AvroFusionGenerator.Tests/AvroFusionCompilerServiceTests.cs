using Avro.SchemaGeneration.Sample.Model;
using AvroFusionGenerator.Implementation;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AvroFusionGenerator.Tests;
/// <summary>
/// The avro fusion compiler service tests.
/// </summary>

public class AvroFusionCompilerServiceTests
{
    private readonly AvroFusionCompilerService _avroFusionCompilerService;
    private readonly Mock<IAvroFusionDynamicAssemblyGenerator> _mockAvroFusionDynamicAssemblyGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroFusionCompilerServiceTests"/> class.
    /// </summary>
    public AvroFusionCompilerServiceTests()
    {
        _mockAvroFusionDynamicAssemblyGenerator = new Mock<IAvroFusionDynamicAssemblyGenerator>();
        _avroFusionCompilerService = new AvroFusionCompilerService(_mockAvroFusionDynamicAssemblyGenerator.Object);
    }

    /// <summary>
    /// Loads the dot net types from source_ valid parameters_ should return list of types.
    /// </summary>
    /// <param name="sourceDirectoryPath">The source directory path.</param>
    /// <param name="parentClassModelName">The parent class model name.</param>
    [Theory]
    [InlineData("validPath", "validParentClassModelName")]
    [InlineData("validPath2", "validParentClassModelName2")]
    public void LoadDotNetTypesFromSource_ValidParameters_ShouldReturnListOfTypes(string sourceDirectoryPath,
        string parentClassModelName)
    {
        // Arrange
        var expectedTypes = new List<Type> {typeof(string), typeof(int)};
        _mockAvroFusionDynamicAssemblyGenerator
            .Setup(g => g.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName))
            .Returns(expectedTypes);

        // Act
        var result = _avroFusionCompilerService.LoadDotNetTypesFromSource(sourceDirectoryPath, parentClassModelName);

        // Assert
        result.Should().BeEquivalentTo(expectedTypes);
        _mockAvroFusionDynamicAssemblyGenerator.Verify(
            g => g.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName), Times.Once);
    }

    /// <summary>
    /// Loads the dot net types from source_ invalid parameters_ should throw argument exception.
    /// </summary>
    /// <param name="sourceDirectoryPath">The source directory path.</param>
    /// <param name="parentClassModelName">The parent class model name.</param>
    [Theory]
    [InlineData(null, "ResourceGroup")]
    [InlineData("", "ResourceGroup")]
    [InlineData("   ", "ResourceGroup")]
    [InlineData("SampleAndTest/TestModels", null)]
    [InlineData("SampleAndTest/TestModels", "")]
    [InlineData("SampleAndTest/TestModels", "   ")]
    public void LoadDotNetTypesFromSource_InvalidParameters_ShouldThrowArgumentException(string sourceDirectoryPath,
        string parentClassModelName)
    {
        // Act
        Action act = () =>
            _avroFusionCompilerService.LoadDotNetTypesFromSource(sourceDirectoryPath, parentClassModelName);

        // Assert
        act.Should().Throw<ArgumentException>();
        _mockAvroFusionDynamicAssemblyGenerator.Verify(
            g => g.GenerateDynamicAssembly(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void LoadDotNetTypesFromSource_ResourceGroupModel_ShouldReturnListOfResourceGroupRelatedTypes()
    {
        // Arrange
        var sourceDirectoryPath = "validPath";
        var parentClassModelName = "ResourceGroup";

        var expectedTypes = new List<Type>
        {
            typeof(ResourceGroup),
            typeof(AppService),
            typeof(AppServiceStatus),
            typeof(ArmTemplateMetadata),
            typeof(AzureResource),
            typeof(Container),
            typeof(CosmosDb),
            typeof(Database),
            typeof(DataFactoryPipeline),
            typeof(EventGrid),
            typeof(EventHub),
            typeof(EventHubSchema),
            typeof(SchemaGroup),
            typeof(StorageAccount),
            typeof(VirtualMachine),
            typeof(VirtualMachineStatus)
        };

        _mockAvroFusionDynamicAssemblyGenerator
            .Setup(g => g.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName))
            .Returns(expectedTypes);

        // Act
        var result = _avroFusionCompilerService.LoadDotNetTypesFromSource(sourceDirectoryPath, parentClassModelName);

        // Assert
        result.Should().BeEquivalentTo(expectedTypes);
        _mockAvroFusionDynamicAssemblyGenerator.Verify(
            g => g.GenerateDynamicAssembly(sourceDirectoryPath, parentClassModelName), Times.Once);
    }
}