using System.Data;
using System.Net;
using System.Text;
using AvroFusionGenerator.Implementation.AvroTypeHandlers;
using AvroFusionGenerator.ServiceInterface;
using FluentAssertions;
using Moq;
using Xunit;

namespace AvroFusionGenerator.Tests;

public class AvroAvscAllClassTypeHandlerTests
{

    private readonly Mock<IAvroFusionSchemaGenerator> _avroFusionSchemaGeneratorMock;
    private readonly AvroAvscClassTypeHandler _avroAvscClassTypeHandler;
    public AvroAvscAllClassTypeHandlerTests()
    {
        _avroFusionSchemaGeneratorMock = new Mock<IAvroFusionSchemaGenerator>();
        _avroAvscClassTypeHandler = new AvroAvscClassTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => _avroFusionSchemaGeneratorMock.Object));
    }
    [Theory]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(Exception), false)]
    [InlineData(typeof(Uri), false)]
    [InlineData(typeof(TestModel), true)]
    public void IfCanHandleAvroAvscType_ShouldReturnExpectedResult(Type inputType, bool expectedResult)
    {
        // Arrange
        var avroSchemaGeneratorMock = new Mock<IAvroFusionSchemaGenerator>();
        var avroAvscClassTypeHandler = new AvroAvscClassTypeHandler(new Lazy<IAvroFusionSchemaGenerator>(() => avroSchemaGeneratorMock.Object));

        // Act
        var result = avroAvscClassTypeHandler.IfCanHandleAvroAvscType(inputType);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(typeof(TestModel))]
    [InlineData(typeof(NestedTestModel))]
    [InlineData(typeof(CustomTupleTestModel))]
    [InlineData(typeof(UnionTestModel))]
    public void IfCanHandleAvroAvscType_ShouldReturnTrue_ForValidClassType(Type type)
    {
        Assert.True(_avroAvscClassTypeHandler.IfCanHandleAvroAvscType(type));
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(string))]
    public void IfCanHandleAvroAvscType_ShouldReturnFalse_ForNonClassType(Type type)
    {
        Assert.False(_avroAvscClassTypeHandler.IfCanHandleAvroAvscType(type));
    }

    [Fact]
    public void ThenCreateAvroAvscType_ShouldCallGenerateAvroFusionAvscAvroType_ForEachProperty()
    {
        var testModelType = typeof(TestModel);
        var forAvroAvscGeneratedTypes = new HashSet<string>();

        _avroAvscClassTypeHandler.ThenCreateAvroAvscType(testModelType, forAvroAvscGeneratedTypes);

        _avroFusionSchemaGeneratorMock.Verify(x => x.GenerateAvroFusionAvscAvroType(typeof(int), forAvroAvscGeneratedTypes), Times.Once);
        _avroFusionSchemaGeneratorMock.Verify(x => x.GenerateAvroFusionAvscAvroType(typeof(string), forAvroAvscGeneratedTypes), Times.Once);
    }

    [Fact]
    public void ThenCreateAvroAvscType_ShouldHandleNestedTestModel()
    {
        var nestedTestModelType = typeof(NestedTestModel);
        var forAvroAvscGeneratedTypes = new HashSet<string>();

        _avroAvscClassTypeHandler.ThenCreateAvroAvscType(nestedTestModelType, forAvroAvscGeneratedTypes);

        _avroFusionSchemaGeneratorMock.Verify(x => x.GenerateAvroFusionAvscAvroType(typeof(int), forAvroAvscGeneratedTypes), Times.Once);
        _avroFusionSchemaGeneratorMock.Verify(x => x.GenerateAvroFusionAvscAvroType(typeof(TestModel), forAvroAvscGeneratedTypes), Times.Once);
    }

    [Fact]
    public void ThenCreateAvroAvscType_ShouldHandleCustomTupleTestModel()
    {
        // Arrange
        var customTupleTestModelType = typeof(CustomTupleTestModel);
        var forAvroAvscGeneratedTypes = new HashSet<string>();

        // Act
        var result = _avroAvscClassTypeHandler.ThenCreateAvroAvscType(customTupleTestModelType, forAvroAvscGeneratedTypes);

        // Assert
        result.Should().NotBeNull();
        var avscType = result as Dictionary<string, object?>;
        avscType.Should().NotBeNull();
        avscType?["type"].Should().Be("record");
        avscType?["name"].Should().Be("CustomTupleTestModel");
        avscType?["namespace"].Should().Be(customTupleTestModelType.Namespace);

        var fields = avscType?["fields"] as List<Dictionary<string, object?>>;
        fields.Should().NotBeNull();
        fields.Should().HaveCount(3);

        var expectedTypes = new List<Type>
        {
            typeof(int),
            typeof(string),
            typeof(int),
            typeof(string),
            typeof(int),
            typeof(string),
            typeof(DateTime)
        };

        _avroFusionSchemaGeneratorMock.Invocations
            .Select(invocation => invocation.Arguments[0] as Type)
            .Should()
            .Equal(expectedTypes);

    }


    [Fact]
    public void ThenCreateAvroAvscType_ShouldHandleTestModelContainer()
    {
        // Arrange
        var forAvroAvscGeneratedTypes = new HashSet<string>();
        var type = typeof(TestModelContainer);

        _avroFusionSchemaGeneratorMock
            .Setup(x => x.GenerateAvroFusionAvscAvroType(typeof(TestModel), forAvroAvscGeneratedTypes))
            .Returns("TestModel");

        // Act
        var result = _avroAvscClassTypeHandler.ThenCreateAvroAvscType(type, forAvroAvscGeneratedTypes);

        // Assert
        result.Should().NotBeNull();

        var avscType = result as Dictionary<string, object?>;
        avscType.Should().NotBeNull();
        avscType?["type"].Should().Be("record");
        avscType?["name"].Should().Be("TestModelContainer");
        avscType?["namespace"].Should().Be(type.Namespace);

        var fields = avscType?["fields"] as List<Dictionary<string, object?>>;
        fields.Should().NotBeNull();
        fields.Should().HaveCount(4);

        var idField = fields?.FirstOrDefault(f => f["name"] as string == "TestModel");
        idField.Should().NotBeNull();

        fields.Should().HaveCount(4);

        var testModelField = fields?.FirstOrDefault(f => f["name"] as string == "TestModel");
        testModelField.Should().NotBeNull();

        var nestedTestModelField = fields?.FirstOrDefault(f => f["name"] as string == "NestedTestModel");
        nestedTestModelField.Should().NotBeNull();

        var customTupleTestModelField = fields?.FirstOrDefault(f => f["name"] as string == "CustomTupleTestModel");
        customTupleTestModelField.Should().NotBeNull();

        var unionTestModelField = fields?.FirstOrDefault(f => f["name"] as string == "UnionTestModel");
        unionTestModelField.Should().NotBeNull();

        _avroFusionSchemaGeneratorMock.Verify(x => x.GenerateAvroFusionAvscAvroType(typeof(TestModel), forAvroAvscGeneratedTypes), Times.Once);
    }


    public class NestedTestModel
    {
        public int Id { get; set; }
        public TestModel? InnerTestModel { get; set; }
    }

    public class CustomTupleTestModel
    {
        public Tuple<int, string>? Tuple2 { get; set; }
        public ValueTuple<int, string> ValueTuple2 { get; set; }
        public ValueTuple<int, string, DateTime> ValueTuple3 { get; set; }
    }

   
    public class UnionTestModel
    {
        [AvroUnionType(typeof(string), typeof(int))]
        public object? Value { get; set; }
    }

    public class TestModelContainer
    {
        public TestModel? TestModel { get; set; }
        public NestedTestModel? NestedTestModel { get; set; }
        public CustomTupleTestModel? CustomTupleTestModel { get; set; }
        public UnionTestModel? UnionTestModel { get; set; }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Exception? Exception { get; set; }
        public Uri? Uri { get; set; }
        public Stream? Stream { get; set; }
        public Task? Task { get; set; }
        public ValueTask ValueTask { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public Encoding? Encoding { get; set; }
        public MemoryStream? MemoryStream { get; set; }
        public WebRequest? WebRequest { get; set; }
        public WebResponse? WebResponse { get; set; }
        public HttpRequestMessage? HttpRequestMessage { get; set; }
        public HttpResponseMessage? HttpResponseMessage { get; set; }
        public IQueryable? Queryable { get; set; }
        public IDbConnection? DbConnection { get; set; }
        public IDbCommand? DbCommand { get; set; }
        public IDbTransaction? DbTransaction { get; set; }
        public IEnumerable<string>? Enumerable { get; set; }
    }

}