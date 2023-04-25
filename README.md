[![.NET 7 AVRO Fusion Generator](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/AvroFusionGenerator.yml/badge.svg)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/AvroFusionGenerator.yml)[![.NET 7 Release](https://img.shields.io/badge/.NET%207-Release-blue)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg?label=Release)  [![.NET 7 Debug](https://img.shields.io/badge/.NET%207-Debug-yellow)](https://github.com/dineshtripathi/AvroFusionSchemaGenerator/actions/workflows/dotnet-desktop.yml/badge.svg?label=debug)

Welcome to the AvroFusionSchemaGenerator wiki!

# Avro Fusion Generator
Avro Fusion Generator is a tool to generate Avro schemas from C# classes. It uses a combination of reflection and type strategies to create Avro schemas that represent the structure of the input C# types.

It Follows the AVRO Schema Specfication guidlines .See the specification here [Avro Schema Spefication -Apache](https://avro.apache.org/docs/1.11.1/specification/)

####  .Net to Avro Schema mapping for Primitive type and Collection Type

| .NET Type                                     | Avro Type |
| ----------------------------------------------| --------- |
| `bool` / `Boolean`                            | boolean   |
| `byte` / `Byte`                               | int       |
| `sbyte` / `SByte`                             | int       |
| `short` / `Int16`                             | int       |
| `ushort` / `UInt16`                           | int       |
| `int` / `Int32`                               | int       |
| `uint` / `UInt32`                             | long             |
| `long` / `Int64`                              | long             |
| `ulong` / `UInt64`                            | long             |
| `float` / `Single`                            | float            |
| `double` / `Double`                           | double           |
| `decimal` / `Decimal`                         | bytes (decimal)  |
| `char` / `Char`                               | int              |
| `string` / `String`                           | string           |
| `DateTime`                                    | long (timestamp) |
| `DateTimeOffset`                              | long (timestamp) |
| `TimeSpan`                                    | long (time)      |
| `Guid`                                        | fixed (16 bytes) |
| `Nullable<T>`                                 | union {null, T}  |
| `Enum`                                        | enum             |
| `Class`                                       | record           |
| `Struct`                                      | record           |
| `Tuple` / `ValueTuple`                        | Custom "tuple" - minor issues   |
| `Array`, `List<T>`, `IList<T>`                | array            |
| `Dictionary`, `IDictionary<K, V>`             | map              |
| `IEqualityComparer<T>`                        | Work in progress -Failing   |


### Nuget package (Dot net tool)
To download the latest release of AvroFusionGenerator, 

#### Download Avro Fusion Generator Nuget package from the list 


| Download Package| Package Name    | Package Version | Release Number | Release Type |
|-----------------|-----------------|-----------------|----------------|--------------|



## Features
* Support for primitive types, nullable types, enums, lists, and dictionaries.
* Strategy pattern to handle different C# types easily and extendibly.
* Dependency Injection for better maintainability.
## Getting Started
To use the Avro Fusion Generator, follow these steps:

1. Clone the repository.
1. Build the solution.
1. Run the console application with your input C# types as arguments.

Create a Model Class File 

Example `GenerateAvscSchemaFromThisModel.cs`
And Create a Model

Example
Consider the following C# class:

 ```csharp
 File Name - `GenerateAvscSchemaFromThisModel.cs`
        namespace TestModels
        {
            /// <summary>
            /// 
            /// </summary>
            public class ResourceGroup
            {
                public string? ResourceGroupName { get; set; }
                public string? ResourceGroupSubscriptionId { get; set; }
                public DateTime CreationDate { get; set; }
                public List<AppService>? AppServices { get; set; }
                public Dictionary<string, string>? ResourceGroupTags { get; set; }
                public Dictionary<string, StorageAccount>? StorageAccounts { get; set; }
                public Dictionary<string, VirtualMachine>? VirtualMachines { get; set; }
                public List<DataFactoryPipeline>? DataFactoryPipelines { get; set; }
                [AvroUnionType(typeof(int), typeof(double))]
                public object? DataFactoryPipelineCount { get; set; }
                public List<EventHub>? EventHubs { get; set; }
                public List<EventGrid>? EventGrids { get; set; }
                public List<SchemaGroup>? SchemaGroups { get; set; }
                public List<Schema>? Schemas { get; set; }
                public Dictionary<string, CosmosDb>? CosmosDBs { get; set; }
                public AzureResource? AzureResources { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class AppService
            {
                public string? AppServiceName { get; set; }
                public string? Plan { get; set; }
                public string? OperatingSystem { get; set; }
                public string? RuntimeStack { get; set; }
                public AppServiceStatus Status { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class StorageAccount
            {
                public string? StorageAccountName { get; set; }
                public string? AccountType { get; set; }
                public string? AccessTier { get; set; }
                public bool IsSecureTransferEnabled { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class VirtualMachine
            {
                public string? VirtualMachineName { get; set; }
                public string? Size { get; set; }
                public string? OperatingSystem { get; set; }
                public VirtualMachineStatus Status { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public enum AppServiceStatus
            {
                Running,
                Stopped,
                Restarting,
                Scaling
            }
            /// <summary>
            /// 
            /// </summary>
            public enum VirtualMachineStatus
            {
                Running,
                Stopped,
                Deallocated,
                Deleting
            }
            /// <summary>
            /// 
            /// </summary>
            public class DataFactoryPipeline
            {
                public string? PipelineName { get; set; }
                public string? Description { get; set; }
                public DateTime LastModified { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class EventHub
            {
                public string? EventHubName { get; set; }
                public int PartitionCount { get; set; }
                public TimeSpan MessageRetention { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class EventGrid
            {
                public string? EventGridName { get; set; }
                public string? TopicType { get; set; }
                public string? InputSchema { get; set; }
            }
            /// <summary>
            /// 
            /// </summary>
            public class SchemaGroup
            {
                public string? GroupName { get; set; }
                public string? Description { get; set; }
                public List<Schema>? Schemas { get; set; }
            }

            public record Schema(string? SchemaName, string? SchemaVersion, string? SerializationType) : AzureResource;

            /// <summary>
            /// The cosmos d b.
            /// </summary>
            /// <param name="CosmosDbName"></param>
            /// <param name="AccountType"></param>
            /// <param name="APIType"></param>
            /// <param name="Databases"></param>
            public record CosmosDb(string? CosmosDbName, string? AccountType, string? APIType, List<Database>? Databases) : AzureResource;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="DatabaseName"></param>
            /// <param name="Containers"></param>
            public record Database(string? DatabaseName, List<Container>? Containers) : AzureResource;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="ContainerName"></param>
            /// <param name="PartitionKey"></param>
            public record Container(string? ContainerName, string? PartitionKey) : AzureResource;
            /// <summary>
            /// 
            /// </summary>
            public record AzureResource
            {
                public string? Name { get; set; }
                public string? ResourceType { get; set; }
                public string? Location { get; set; }
                public string? ResourceGroup { get; set; }
                [AvroDuplicateFieldAlias("AzureResourceSubscriptionId")]
                public string? SubscriptionId { get; set; }
                public string? ETag { get; set; }
                public IDictionary<string, string>? AzureResourceTags { get; set; }
                [AvroUnionType(typeof(int), typeof(double))]
                public object? ProvisioningState { get; set; }
            }

        }
 ```

Run the Avro Fusion Generator with `GenerateAvscSchemaFromThisModel.cs` file as input:

```AvroFusionGenerator generate <path\`GenerateAvscSchemaFromThisModel.cs`> --<outputDirectory>```

The generated Avro schema would be:

```avsc
{
  "type": "record",
  "namespace": "TestModels",
  "name": "ResourceGroup",
  "fields": [
    {
      "name": "ResourceGroupName",
      "type": "string"
    },
    {
      "name": "ResourceGroupSubscriptionId",
      "type": "string"
    },
    {
      "name": "CreationDate",
      "type": "long"
    },
    {
      "name": "AppServices",
      "type": {
        "type": "array",
        "items": {
          "type": "record",
          "name": "AppService",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "AppServiceName",
              "type": "string"
            },
            {
              "name": "Plan",
              "type": "string"
            },
            {
              "name": "OperatingSystem",
              "type": "string"
            },
            {
              "name": "RuntimeStack",
              "type": "string"
            },
            {
              "name": "Status",
              "type": {
                "type": "enum",
                "name": "AppServiceStatus",
                "namespace": "TestModels",
                "symbols": [
                  "Running",
                  "Stopped",
                  "Restarting",
                  "Scaling"
                ]
              }
            }
          ]
        }
      }
    },
    {
      "name": "ResourceGroupTags",
      "type": {
        "type": "map",
        "values": "string"
      }
    },
    {
      "name": "StorageAccounts",
      "type": {
        "type": "map",
        "values": {
          "type": "record",
          "name": "StorageAccount",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "StorageAccountName",
              "type": "string"
            },
            {
              "name": "AccountType",
              "type": "string"
            },
            {
              "name": "AccessTier",
              "type": "string"
            },
            {
              "name": "IsSecureTransferEnabled",
              "type": "boolean"
            }
          ]
        }
      }
    },
    {
      "name": "VirtualMachines",
      "type": {
        "type": "map",
        "values": {
          "type": "record",
          "name": "VirtualMachine",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "VirtualMachineName",
              "type": "string"
            },
            {
              "name": "Size",
              "type": "string"
            },
            {
              "name": "OperatingSystem",
              "type": "string"
            },
            {
              "name": "Status",
              "type": {
                "type": "enum",
                "name": "VirtualMachineStatus",
                "namespace": "TestModels",
                "symbols": [
                  "Running",
                  "Stopped",
                  "Deallocated",
                  "Deleting"
                ]
              }
            }
          ]
        }
      }
    },
    {
      "name": "DataFactoryPipelines",
      "type": {
        "type": "array",
        "items": {
          "type": "record",
          "name": "DataFactoryPipeline",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "PipelineName",
              "type": "string"
            },
            {
              "name": "Description",
              "type": "string"
            },
            {
              "name": "LastModified",
              "type": "long"
            }
          ]
        }
      }
    },
    {
      "name": "DataFactoryPipelineCount",
      "type": [
        "int",
        "double",
        "null"
      ]
    },
    {
      "name": "EventHubs",
      "type": {
        "type": "array",
        "items": {
          "type": "record",
          "name": "EventHub",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "EventHubName",
              "type": "string"
            },
            {
              "name": "PartitionCount",
              "type": "int"
            },
            {
              "name": "MessageRetention",
              "type": "long"
            }
          ]
        }
      }
    },
    {
      "name": "EventGrids",
      "type": {
        "type": "array",
        "items": {
          "type": "record",
          "name": "EventGrid",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "EventGridName",
              "type": "string"
            },
            {
              "name": "TopicType",
              "type": "string"
            },
            {
              "name": "InputSchema",
              "type": "string"
            }
          ]
        }
      }
    },
    {
      "name": "SchemaGroups",
      "type": {
        "type": "array",
        "items": {
          "type": "record",
          "name": "SchemaGroup",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "GroupName",
              "type": "string"
            },
            {
              "name": "Description",
              "type": "string"
            },
            {
              "name": "Schemas",
              "type": {
                "type": "array",
                "items": {
                  "type": "record",
                  "name": "Schema",
                  "namespace": "TestModels",
                  "fields": [
                    {
                      "name": "SchemaName",
                      "type": "string"
                    },
                    {
                      "name": "SchemaVersion",
                      "type": "string"
                    },
                    {
                      "name": "SerializationType",
                      "type": "string"
                    },
                    {
                      "name": "Name",
                      "type": "string"
                    },
                    {
                      "name": "ResourceType",
                      "type": "string"
                    },
                    {
                      "name": "Location",
                      "type": "string"
                    },
                    {
                      "name": "ResourceGroup",
                      "type": "string"
                    },
                    {
                      "name": "SubscriptionId",
                      "type": "string",
                      "aliases": [
                        "AzureResourceSubscriptionId"
                      ]
                    },
                    {
                      "name": "ETag",
                      "type": "string"
                    },
                    {
                      "name": "AzureResourceTags",
                      "type": {
                        "type": "map",
                        "values": "string"
                      }
                    },
                    {
                      "name": "ProvisioningState",
                      "type": [
                        "int",
                        "double",
                        "null"
                      ]
                    }
                  ]
                }
              }
            }
          ]
        }
      }
    },
    {
      "name": "Schemas",
      "type": {
        "type": "array",
        "items": "TestModels.Schema"
      }
    },
    {
      "name": "CosmosDBs",
      "type": {
        "type": "map",
        "values": {
          "type": "record",
          "name": "CosmosDB",
          "namespace": "TestModels",
          "fields": [
            {
              "name": "CosmosDBName",
              "type": "string"
            },
            {
              "name": "AccountType",
              "type": "string"
            },
            {
              "name": "APIType",
              "type": "string"
            },
            {
              "name": "Databases",
              "type": {
                "type": "array",
                "items": {
                  "type": "record",
                  "name": "Database",
                  "namespace": "TestModels",
                  "fields": [
                    {
                      "name": "DatabaseName",
                      "type": "string"
                    },
                    {
                      "name": "Containers",
                      "type": {
                        "type": "array",
                        "items": {
                          "type": "record",
                          "name": "Container",
                          "namespace": "TestModels",
                          "fields": [
                            {
                              "name": "ContainerName",
                              "type": "string"
                            },
                            {
                              "name": "PartitionKey",
                              "type": "string"
                            },
                            {
                              "name": "Name",
                              "type": "string"
                            },
                            {
                              "name": "ResourceType",
                              "type": "string"
                            },
                            {
                              "name": "Location",
                              "type": "string"
                            },
                            {
                              "name": "ResourceGroup",
                              "type": "string"
                            },
                            {
                              "name": "SubscriptionId",
                              "type": "string",
                              "aliases": [
                                "AzureResourceSubscriptionId"
                              ]
                            },
                            {
                              "name": "ETag",
                              "type": "string"
                            },
                            {
                              "name": "AzureResourceTags",
                              "type": {
                                "type": "map",
                                "values": "string"
                              }
                            },
                            {
                              "name": "ProvisioningState",
                              "type": [
                                "int",
                                "double",
                                "null"
                              ]
                            }
                          ]
                        }
                      }
                    },
                    {
                      "name": "Name",
                      "type": "string"
                    },
                    {
                      "name": "ResourceType",
                      "type": "string"
                    },
                    {
                      "name": "Location",
                      "type": "string"
                    },
                    {
                      "name": "ResourceGroup",
                      "type": "string"
                    },
                    {
                      "name": "SubscriptionId",
                      "type": "string",
                      "aliases": [
                        "AzureResourceSubscriptionId"
                      ]
                    },
                    {
                      "name": "ETag",
                      "type": "string"
                    },
                    {
                      "name": "AzureResourceTags",
                      "type": {
                        "type": "map",
                        "values": "string"
                      }
                    },
                    {
                      "name": "ProvisioningState",
                      "type": [
                        "int",
                        "double",
                        "null"
                      ]
                    }
                  ]
                }
              }
            },
            {
              "name": "Name",
              "type": "string"
            },
            {
              "name": "ResourceType",
              "type": "string"
            },
            {
              "name": "Location",
              "type": "string"
            },
            {
              "name": "ResourceGroup",
              "type": "string"
            },
            {
              "name": "SubscriptionId",
              "type": "string",
              "aliases": [
                "AzureResourceSubscriptionId"
              ]
            },
            {
              "name": "ETag",
              "type": "string"
            },
            {
              "name": "AzureResourceTags",
              "type": {
                "type": "map",
                "values": "string"
              }
            },
            {
              "name": "ProvisioningState",
              "type": [
                "int",
                "double",
                "null"
              ]
            }
          ]
        }
      }
    },
    {
      "name": "AzureResources",
      "type": {
        "type": "record",
        "name": "AzureResource",
        "namespace": "TestModels",
        "fields": [
          {
            "name": "Name",
            "type": "string"
          },
          {
            "name": "ResourceType",
            "type": "string"
          },
          {
            "name": "Location",
            "type": "string"
          },
          {
            "name": "ResourceGroup",
            "type": "string"
          },
          {
            "name": "SubscriptionId",
            "type": "string",
            "aliases": [
              "AzureResourceSubscriptionId"
            ]
          },
          {
            "name": "ETag",
            "type": "string"
          },
          {
            "name": "AzureResourceTags",
            "type": {
              "type": "map",
              "values": "string"
            }
          },
          {
            "name": "ProvisioningState",
            "type": [
              "int",
              "double",
              "null"
            ]
          }
        ]
      }
    }
  ]
}
```

## Extending the Generator
To support additional C# types or customize the Avro schema generation, you can create new classes that implement the `IAvroAvscTypeHandler` interface and register them in the `ServiceCollection.

For example, to handle a custom type `ClassType`, create a new `class AvroAvscClassTypeHandler`:

```csharp
public class AvroAvscClassTypeHandler : IAvroAvscTypeHandler
{
    private readonly Lazy<IAvroSchemaGenerator> _avroSchemaGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvroAvscClassTypeHandler"/> class.
    /// </summary>
    /// <param name="avroSchemaGenerator">The avro schema generator.</param>
    public AvroAvscClassTypeHandler(Lazy<IAvroSchemaGenerator> avroSchemaGenerator)
    {
        _avroSchemaGenerator = avroSchemaGenerator;
    }

    /// <summary>
    /// Ifs the can handle avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    public bool IfCanHandleAvroAvscType(Type? type)
    {
        return type is {IsClass: true} && type != typeof(string);
    }

    /// <summary>
    /// Then the create avro avsc type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="forAvroAvscGeneratedTypes">The for avro avsc generated types.</param>
    /// <returns>An object.</returns>
    public object? ThenCreateAvroAvscType(Type? type, HashSet<string> forAvroAvscGeneratedTypes)
    {
        if (type?.FullName != null && forAvroAvscGeneratedTypes.Contains(type.FullName))
            return type.FullName;

        if (type?.FullName != null) forAvroAvscGeneratedTypes.Add(type.FullName);

        var avroFieldInfo = type?.GetProperties().Where(prop => !IsIgnoredType(prop.PropertyType))
            .Select(prop => new
            {
                name = prop.Name,
                type = GenerateUnionTypeIfRequired(prop, forAvroAvscGeneratedTypes),
                aliasAttribute = prop.GetCustomAttribute<AvroDuplicateFieldAliasAttribute>()
            });

        if (avroFieldInfo != null)
        {
            var createAvroAvscType = new Dictionary<string, object?>
            {
                {"type", "record"},
                {"name", type?.Name},
                {"namespace", type?.Namespace},
                {
                    "fields", avroFieldInfo.Select(fieldInfo =>
                    {
                        var field = new Dictionary<string, object?>
                        {
                            {"name", fieldInfo.name},
                            {"type", fieldInfo.type}
                        };

                        if (fieldInfo.aliasAttribute != null)
                        {
                            field["aliases"] = new List<string> { fieldInfo.aliasAttribute.Alias };
                        }

                        return field;
                    }).ToList()
                }
            };

            return createAvroAvscType;
        }

        return null;
    }

    /// <summary>
    /// Are the ignored type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A bool.</returns>
    private static bool IsIgnoredType(Type type)
    {
        return type.GetInterfaces().Contains(typeof(IEqualityComparer<>)) ||
               type.Name.EndsWith("UnsupportedType", StringComparison.Ordinal) ||
               (type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>)) &&
                type.GetGenericArguments()[0] != typeof(string));
    }

    /// <summary>
    /// Generates the union type if required.
    /// </summary>
    /// <param name="prop">The prop.</param>
    /// <param name="generatedTypes">The generated types.</param>
    /// <returns>An object.</returns>
    private object? GenerateUnionTypeIfRequired(PropertyInfo prop, HashSet<string> generatedTypes)
    {
        var avroUnionAttribute = prop.GetCustomAttribute<AvroUnionTypeAttribute>();

        if (avroUnionAttribute != null)
        {
            List<object?> unionTypes = avroUnionAttribute.UnionTypes.Select(unionType =>
                _avroSchemaGenerator.Value.GenerateAvroAvscType(unionType, generatedTypes)).ToList();
            unionTypes.Add("null");
            return unionTypes;
        }

        return _avroSchemaGenerator.Value.GenerateAvroAvscType(prop.PropertyType, generatedTypes);
    }
}
```
Then, register the new strategy in the `ServiceCollection`:

```csharp
services.AddSingleton<IAvroAvscTypeHandler, AvroAvscClassTypeHandler>();
```

## License
This project is licensed under the MIT License.

## Contributing
1. Fork the repository.
1. Create your feature branch (git checkout -b feature/my-new-feature).
1. Commit your changes (git commit -am 'Add my new feature').
1. Push to the branch (git push origin feature/my-new-feature).
1. Create a new Pull Request.

