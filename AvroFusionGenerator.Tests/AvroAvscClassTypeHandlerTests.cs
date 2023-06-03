using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AvroFusionGenerator.Tests;

public class AvroAvscClassTypeHandlerTests
{
    private readonly Mock<IAvroFusionSchemaGenerator> _avroSchemaGeneratorMock;
    private readonly AvroAvscClassTypeHandler _handler;

    public AvroAvscClassTypeHandlerTests()
    {
        _avroSchemaGeneratorMock = new Mock<IAvroFusionSchemaGenerator>();
        _handler = new AvroAvscClassTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => _avroSchemaGeneratorMock.Object));
    }

    [Theory]
    [InlineData(typeof(TestClass), true)]
    [InlineData(typeof(string), false)]
    public void IfCanHandleAvroAvscTypeClass_ShouldReturnExpectedResult(Type type, bool expectedResult)
    {
        // Act
        bool result = _handler.IfCanHandleAvroAvscType(type);

        // Assert
        result.Should().Be(expectedResult);
    }

   
    private class TestClass
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
    }
}