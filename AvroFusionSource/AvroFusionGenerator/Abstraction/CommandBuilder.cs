using System.CommandLine;

namespace AvroFusionGenerator.Abstraction;

public abstract class CommandBuilder
{
    public abstract RootCommand BuildRootCommand();
}