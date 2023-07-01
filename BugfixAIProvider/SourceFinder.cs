using System.Collections.Concurrent;
using System.Reflection;
using BugfixAIProvider.Models;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp.Syntax;

namespace BugfixAIProvider;

public class SourceFinder
{
    private static readonly ConcurrentDictionary<AssemblyName, SyntaxTree?> SyntaxTrees = new ();
    private static readonly DirectoryInfo BaseDirectory;

    static SourceFinder()
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        BaseDirectory = new FileInfo(executingAssembly.Location).Directory!;
        string baseNamespace = executingAssembly.FullName.Split(',').First().Split('.').First();
        SyntaxTrees.TryAdd(executingAssembly.GetName(), new CSharpDecompiler(executingAssembly.Location, new DecompilerSettings()).DecompileWholeModuleAsSingleFile());

        foreach (AssemblyName assembly in executingAssembly.GetReferencedAssemblies().Where(n => n.FullName.StartsWith(baseNamespace)))
        {
            if (SyntaxTrees.ContainsKey(assembly)) continue;
            SyntaxTrees.TryAdd(assembly, new CSharpDecompiler(Assembly.Load(assembly).Location, new DecompilerSettings()).DecompileWholeModuleAsSingleFile());
        }
    }
    
    private void LoadAssembly(string assemblyNamespace)
    {
        foreach (FileInfo fileInfo in BaseDirectory.GetFiles($"{assemblyNamespace}*.dll").Union(BaseDirectory.GetFiles($"{assemblyNamespace}*.exe")))
        {
            AssemblyName assembly = Assembly.LoadFrom(fileInfo.FullName).GetName();
            if (SyntaxTrees.ContainsKey(assembly)) continue;
            SyntaxTrees.TryAdd(assembly, new CSharpDecompiler(fileInfo.FullName, new DecompilerSettings()).DecompileWholeModuleAsSingleFile());
        }
    }

	public string GetByFullName(CodePointer codePointer)
    {
        if (codePointer.FullName == null) throw new ArgumentNullException(nameof(codePointer.FullName));

        List<string> nameParts = codePointer.FullName.Split('.').ToList();
        string methodName = string.Empty;
        string typeName;
        string namespaceName;
        switch (codePointer.CodeType)
        {
            case CodeType.Method:
                if (codePointer.FullName.Count(c => c == '.') < 2)
                {
                    throw new ArgumentException("fullName must contain at least two dots");
                }

                methodName = nameParts.Last();
                nameParts.RemoveAt(nameParts.Count - 1);
                typeName = nameParts.Last();
                nameParts.RemoveAt(nameParts.Count - 1);
                namespaceName = string.Join('.', nameParts);
                break;
            case CodeType.Type:
                if (!codePointer.FullName.Contains('.'))
                {
                    throw new ArgumentException("fullName must contain at least one dot");
                }

                typeName = nameParts.Last();
                nameParts.RemoveAt(nameParts.Count - 1);
                namespaceName = string.Join('.', nameParts);
                break;
            default: throw new ArgumentException("CodeType not supported");
        }

        KeyValuePair<AssemblyName, SyntaxTree?> syntaxTreePair = SyntaxTrees.Where(p => p.Value.Children.OfType<NamespaceDeclaration>().FirstOrDefault(ns => ns.Name == namespaceName) != null).FirstOrDefault();
        if (syntaxTreePair.Value == null) LoadAssembly(nameParts.First());
        syntaxTreePair = SyntaxTrees.Where(p => p.Value.Children.OfType<NamespaceDeclaration>().FirstOrDefault(ns => ns.Name == namespaceName) != null).FirstOrDefault();
        if (syntaxTreePair.Value == null) return null;
        NamespaceDeclaration ns = syntaxTreePair.Value.Children.OfType<NamespaceDeclaration>().First(ns => ns.Name == namespaceName);
        TypeDeclaration type = ns.Children.OfType<TypeDeclaration>().First(type => type.Name == typeName);
        if (codePointer.CodeType != CodeType.Method) return type.ToString();
        MethodDeclaration method = type.Children.OfType<MethodDeclaration>().First(method => method.Name == methodName);
        return method.ToString();
    }
}