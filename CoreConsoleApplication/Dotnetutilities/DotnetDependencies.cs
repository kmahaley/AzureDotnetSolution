using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleApplication.Dotnetutilities
{
    public static class DotnetDependencies
    {
        /// <summary>
        /// - Open terminal
        /// - Go to.csporj or .sln file location
        /// - Make sure msbuild path is setup in environment variable.
        /// - Run cmd: dotnet msbuild UserProjectName.csproj /t:GenerateRestoreGraphFile /p:RestoreGraphOutputPath=C:\Users\kamahale\Downloads\UserProjectName.md
        /// - Provide path in 
        /// </summary>
        /// <example>
        /// 	dotnet list package
        /// 	dotnet list UserProjectName.sln package --include-transitive > C:\Users\kamahale\Downloads\UserProjectName_packages.md

        /// </example>

        public static void PrintProjectDependencyTreeUsingMSBuildGraph()
        {
            var projectGraphOutput = @"C:\Users\kamahale\Downloads\UserProjectName.md";
            string dependencyGraphText = File.ReadAllText(projectGraphOutput);
            var dependencyGraph = DependencyGraphSpec.Load(projectGraphOutput);

            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {
                Console.WriteLine(project.Name);

                foreach (var targetFramework in project.TargetFrameworks)
                {
                    Console.WriteLine($"  [{targetFramework.FrameworkName}]");

                    foreach (var dependency in targetFramework.Dependencies)
                    {
                        Console.WriteLine($"  {dependency.Name}, v{dependency.LibraryRange.VersionRange.ToShortString()}");
                    }
                }
            }
        }

        //https://www.jerriepelser.com/blog/analyze-dotnet-project-dependencies-part-2/
        /*public static void PrintProjectDependencyTreeUsingDotnetRestore()
        {
            var projectGraphOutput = @"C:\Users\kamahale\source\repos\kmahaley\AzureDotnetSolution\CoreConsoleApplication\obj\project.assets.json";
            string dependencyGraphText = File.ReadAllText(projectGraphOutput);
            var dependencyGraph = DependencyGraphSpec.Load(projectGraphOutput);

            foreach (var project in dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference))
            {

                Console.WriteLine(project.Name);

                foreach (var targetFramework in project.TargetFrameworks)
                {
                    Console.WriteLine($"  [{targetFramework.FrameworkName}]");

                    var lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));
                    if (lockFileTargetFramework != null)
                    {
                        foreach (var dependency in targetFramework.Dependencies)
                        {
                            var projectLibrary = lockFileTargetFramework.Libraries.FirstOrDefault(library => library.Name == dependency.Name);

                            ReportDependency(projectLibrary, lockFileTargetFramework, 1);
                        }
                    }

                }
            }
        }*/

    }
}
