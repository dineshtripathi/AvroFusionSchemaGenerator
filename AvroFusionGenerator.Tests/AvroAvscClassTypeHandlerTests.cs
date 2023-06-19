using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AvroFusionGenerator.Tests;

public class AvroAvscClassTypeHandlerTests
{
    private readonly AvroAvscClassTypeHandler _handler;

    public AvroAvscClassTypeHandlerTests()
    {
        Mock<IAvroFusionSchemaGenerator> avroSchemaGeneratorMock = new();
        _handler = new AvroAvscClassTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => avroSchemaGeneratorMock.Object));
    }

    [Theory]
    [InlineData(typeof(TestClass), true)]
    [InlineData(typeof(string), false)]
    public void IfCanHandleAvroAvscTypeClass_ShouldReturnExpectedResult(Type type, bool expectedResult)
    {
        // Act
        var result = _handler.IfCanHandleAvroAvscType(type);

        // Assert
        result.Should().Be(expectedResult);
    }

   
    private class TestClass
    {
        public string? Property1 { get; set; }
        public int Property2 { get; set; }
    }
}