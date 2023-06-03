using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AvroFusionGenerator.Tests;

public class AvroStructTypeHandlerTests
{
    private readonly Mock<IAvroFusionSchemaGenerator> _avroSchemaGeneratorMock;
    private readonly AvroStructTypeHandler _handler;

    public AvroStructTypeHandlerTests()
    {
        _avroSchemaGeneratorMock = new Mock<IAvroFusionSchemaGenerator>();
        _handler = new AvroStructTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => _avroSchemaGeneratorMock.Object));
    }

    [Fact]
    public void IfCanHandleAvroAvscType_ShouldHandleStructType()
    {
        // Arrange
        Type structType = typeof(SampleStruct);

        // Act
        bool result = _handler.IfCanHandleAvroAvscType(structType);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ThenCreateAvroAvscType_ShouldCreateAvroAvscTypeForStruct()
    {
        // Arrange
        Type structType = typeof(SampleStruct);
        _avroSchemaGeneratorMock
            .Setup(generator => generator.GenerateAvroFusionAvscAvroType(It.IsAny<Type>(), It.IsAny<HashSet<string>>()))
            .Returns("string");

        // Act
        var avscType = _handler.ThenCreateAvroAvscType(structType, new HashSet<string>());

        // Assert
        avscType.Should().NotBeNull();
        var avscTypeAsDictionary = avscType as Dictionary<string, object?>;
        avscTypeAsDictionary.Should().NotBeNull();
        avscTypeAsDictionary?["name"].Should().Be("SampleStruct");
    }

    public struct SampleStruct
    {
        public string Property1 { get; set; }
    }
}