using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Code_Analyzer_Tool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# Code Analyzer Tool\n");

            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Usage: CodeAnalyzerTool <path-to-cs-file-or-folder>");
            //    return;
            //}

            string path = Console.ReadLine();//args[0];
            string[] files;

            if (File.Exists(path) && path.EndsWith(".cs"))
            {
                files = new[] { path };
            }
            else if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            }
            else
            {
                Console.WriteLine("Invalid file or directory path.");
                return;
            }

            foreach (var file in files)
            {
                AnalyzeFile(file);
            }
        }

        static void AnalyzeFile(string filePath)
        {
            var code = File.ReadAllText(filePath);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var methodCount = root.DescendantNodes().OfType<MethodDeclarationSyntax>().Count();
            var classCount = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Count();
            var interfaceCount = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().Count();
            var lineCount = code.Split(new[] { '\'' }, StringSplitOptions.None).Length;

            Console.WriteLine($"File: {Path.GetFileName(filePath)}");
            Console.WriteLine($"  Lines of Code: {lineCount}");
            Console.WriteLine($"  Classes: {classCount}");
            Console.WriteLine($"  Interfaces: {interfaceCount}");
            Console.WriteLine($"  Methods: {methodCount}\n");
        }
    }
}
