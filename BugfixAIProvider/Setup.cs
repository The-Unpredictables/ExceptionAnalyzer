// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using System.Reflection;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Microsoft.AspNetCore.SignalR.Client;
using BigfixAIProvider;

internal class Setup
{
	private HubConnection Connection { get; }
    private readonly ConcurrentDictionary<Guid, BugFixResult?> _bugFixResults = new();
    private readonly SourceFinder _sourceFinder = new (new CSharpDecompiler(Assembly.GetEntryAssembly()?.Location!, new DecompilerSettings()).DecompileWholeModuleAsSingleFile());
	
    internal Setup()
    {

		//string findMethodByFullName = sourceFinder.FindMethodByFullName(new CodePointer {FullName ="BugfixAIProvider.Example.ExampleType", Type = CodeType.Type});
		//Console.WriteLine(findMethodByFullName);

		HubConnectionBuilder builder = new();
		Connection = builder
					.WithUrl("http://localhost:5039/bugfixerHub")
					.WithAutomaticReconnect()
					.Build();
		Connection.On<CodePointer>("RequestSourceCode", OnCodeRequest);
		Connection.On<BugFixResult, Guid>("SendBugfixResult", OnBugFixResult);

		Task t = Connection.StartAsync();

		t.Wait();
	}
	
	private static void OnCodeRequest(CodePointer codePointer)
	{
		Console.WriteLine($"Received message: {codePointer}");
	}

	private void OnBugFixResult(BugFixResult? bugFixResult, Guid tan)
	{
		_bugFixResults.TryAdd(tan, bugFixResult);
		Console.WriteLine($"Received message: {bugFixResult}");
	}
}
