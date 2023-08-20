using BugfixAiClient.Models;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.Syntax;
using ICSharpCode.Decompiler.TypeSystem;
using System.Collections.Concurrent;
using System.Reflection;

namespace BugfixAiClient;

public class SourceFinder
{
    private static readonly ConcurrentDictionary<AssemblyName, SyntaxTree?> SyntaxTrees = new ();
    private static readonly DirectoryInfo BaseDirectory;

    static SourceFinder()
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        BaseDirectory = new FileInfo(executingAssembly.Location).Directory!;
        string? baseNamespace = executingAssembly.FullName?.Split(',').First().Split('.').FirstOrDefault();
        if (string.IsNullOrWhiteSpace(baseNamespace)) throw new ArgumentNullException(nameof(baseNamespace));
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
            DecompilerSettings decompilerSettings = new();
            decompilerSettings.UsingDeclarations = false;
            SyntaxTrees.TryAdd(assembly, new CSharpDecompiler(fileInfo.FullName, decompilerSettings).DecompileWholeModuleAsSingleFile());
        }
    }

	public string GetByFullName(CodePointer codePointer)
    {
        if (codePointer.FullName == null) throw new ArgumentNullException(nameof(codePointer.FullName));

        List<string> nameParts = codePointer.FullName.Split('.').ToList();
        string typeName;
        string namespaceName;
        switch (codePointer.CodeType)
        {
            case CodeType.Method:
                if (codePointer.FullName.Count(c => c == '.') < 2)
                {
                    throw new ArgumentException("fullName must contain at least two dots");
                }

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

        KeyValuePair<AssemblyName, SyntaxTree?> syntaxTreePair = SyntaxTrees.FirstOrDefault(p => p.Value?.Children.OfType<NamespaceDeclaration>().FirstOrDefault(ns => ns.Name == namespaceName) != null);
        if (syntaxTreePair.Value == null) LoadAssembly(nameParts.First());
        syntaxTreePair = SyntaxTrees.FirstOrDefault(p => p.Value?.Children.OfType<NamespaceDeclaration>().FirstOrDefault(ns => ns.Name == namespaceName) != null);
        
        //Do not return null. What happens here? @Lukas.Schachner
        if (syntaxTreePair.Value == null) return null;
        
        NamespaceDeclaration ns = syntaxTreePair.Value.Children.OfType<NamespaceDeclaration>().First(ns => ns.Name == namespaceName);
        TypeDeclaration type = ns.Children.OfType<TypeDeclaration>().First(type => type.Name == typeName);
        if (codePointer.CodeType != CodeType.Method) return type.ToString();
        List<MethodDeclaration> methods = type.Children.OfType<MethodDeclaration>().ToList();
        IEnumerable<AstType>? astTypes = codePointer.ParameterTypeFullNames?.Select(AstType.Create).ToList();
        foreach (ParameterDeclaration parameter in methods.Last().Parameters)
        {
            ISymbol symbol = parameter.Type.GetSymbol() ?? throw new ArgumentNullException(nameof(symbol));
            ITypeDefinition? typeDefinition = symbol as ITypeDefinition;
        }
        MethodDeclaration method = type.Children.OfType<MethodDeclaration>().ToList()[2];
        return method.ToString();
    }
}