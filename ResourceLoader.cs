using System;
using System.IO;
using System.Reflection;

namespace MufflerCore;

public class SubmoduleResourceLoader
{
    public static string GetSubmoduleDirectory()
    {
        // Get the assembly that contains the code
        var assembly = Assembly.GetExecutingAssembly();

        // Get the location of the assembly
        var assemblyLocation = assembly.Location;

        // Get the directory of the assembly
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

        return assemblyDirectory;
    }

    public static string GetResourcePath(string relativePath)
    {
        var submoduleDirectory = GetSubmoduleDirectory();
        var fullPath = Path.Combine(submoduleDirectory, relativePath);
        return fullPath;
    }
}
