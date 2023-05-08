using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;
namespace AvroFusionGenerator.Tests;
public class AvroAvscArrayTypeHandlerTests
{
    private readonly Mock<IAvroFusionSchemaGenerator> _avroSchemaGeneratorMock;
    private readonly AvroAvscArrayTypeHandler _handler;

    public AvroAvscArrayTypeHandlerTests()
    {
        _avroSchemaGeneratorMock = new Mock<IAvroFusionSchemaGenerator>();
        _handler = new AvroAvscArrayTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => _avroSchemaGeneratorMock.Object));
    }

    [Theory]
    [InlineData(typeof(List<string>), true)]
    [InlineData(typeof(List<int>), true)]
    [InlineData(typeof(Dictionary<string, object>), false)]
    public void IfCanHandleAvroAvscType_ShouldReturnExpectedResult(Type type, bool expectedResult)
    {
        //Act
        bool result = _handler.IfCanHandleAvroAvscType(type);

        //Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void IfCanHandleAvroAvscType_OpenGenericList_ShouldReturnFalse()
    {
        // Act
        bool result = _handler.IfCanHandleAvroAvscType(typeof(List<>));

        // Assert
        result.Should().BeTrue();
    }
}

