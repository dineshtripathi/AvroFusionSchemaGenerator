using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;

namespace AvroFusionGenerator.Implementation;
/// <summary>
/// The reference assembly load context.
/// </summary>

public class ReferenceAssemblyLoadContext : AssemblyLoadContext
{
    private readonly Dictionary<string, string> _assemblyPaths;

    public ReferenceAssemblyLoadContext(IEnumerable<MetadataReference> referencedAssemblies)
    {
        _assemblyPaths = new Dictionary<string, string>();

        foreach (var reference in referencedAssemblies)
            if (reference is PortableExecutableReference peReference)
                _assemblyPaths.Add(Path.GetFileNameWithoutExtension(peReference.FilePath), peReference.FilePath);
    }

    /// <summary>
    /// Loads the.
    /// </summary>
    /// <param name="assemblyName">The assembly name.</param>
    /// <returns>An Assembly.</returns>
    protected override Assembly Load(AssemblyName assemblyName)
    {
        if (_assemblyPaths.TryGetValue(assemblyName.Name, out var assemblyPath))
            return LoadFromAssemblyPath(assemblyPath);

        return null;
    }
}