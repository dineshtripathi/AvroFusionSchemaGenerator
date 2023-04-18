[![.NET Core Desktop](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml)

[![Debug Build](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg?label=Debug)

[![Release Build](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg?label=Release)

Welcome to the AvroFusionSchemaGenerator wiki!

# Avro Fusion Generator
Avro Fusion Generator is a tool to generate Avro schemas from C# classes. It uses a combination of reflection and type strategies to create Avro schemas that represent the structure of the input C# types.

## Features
* Support for primitive types, nullable types, enums, lists, and dictionaries.
* Strategy pattern to handle different C# types easily and extendibly.
* Dependency Injection for better maintainability.
## Getting Started
To use the Avro Fusion Generator, follow these steps:

1. Clone the repository.
1. Build the solution.
1. Run the console application with your input C# types as arguments.
Example
Consider the following C# class:
```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
    public List<string> Hobbies { get; set; }
}
```

Run the Avro Fusion Generator with `Person` class as input:

```dotnet run --project AvroFusionGenerator -- Person```

The generated Avro schema would be:

```avsc
{
  "type": "record",
  "name": "Person",
  "fields": [
    {
      "name": "Name",
      "type": "string"
    },
    {
      "name": "Age",
      "type": "int"
    },
    {
      "name": "BirthDate",
      "type": "long"
    },
    {
      "name": "Hobbies",
      "type": {
        "type": "array",
        "items": "string"
      }
    }
  ]
}
```

## Extending the Generator
To support additional C# types or customize the Avro schema generation, you can create new classes that implement the `IAvroTypeStrategy` interface and register them in the `ServiceCollection.

For example, to handle a custom type `MyCustomType`, create a new `class MyCustomTypeStrategy`:

```csharp
public class MyCustomTypeStrategy : IAvroTypeStrategy
{
    public bool CanHandle(Type type)
    {
        return type == typeof(MyCustomType);
    }

    public object CreateAvroType(Type type, HashSet<string> generatedTypes)
    {
        // Implement the Avro schema generation logic for MyCustomType
    }
}
```
Then, register the new strategy in the `ServiceCollection`:

```csharp
services.AddSingleton<IAvroTypeStrategy, MyCustomTypeStrategy>();
```

## License
This project is licensed under the MIT License. See the [LICENSE](https://chat.openai.com/c/LICENSE) file for details.

## Contributing
1. Fork the repository.
1. Create your feature branch (git checkout -b feature/my-new-feature).
1. Commit your changes (git commit -am 'Add my new feature').
1. Push to the branch (git push origin feature/my-new-feature).
1. Create a new Pull Request.

