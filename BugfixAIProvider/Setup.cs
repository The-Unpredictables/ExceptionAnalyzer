// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Microsoft.AspNetCore.SignalR.Client;
using BigfixAIProvider;

public class Setup
{
	public HubConnection Connection { get; }

	public readonly ConcurrentDictionary<Guid, BugFixResult> BugFixResults = new();
	public Setup()
	{
		
		CSharpDecompiler decompiler = new(Assembly.GetEntryAssembly()?.Location!, new DecompilerSettings());
		SourceFinder sourceFinder = new (decompiler.DecompileWholeModuleAsSingleFile());
		string findMethodByFullName = sourceFinder.FindMethodByFullName(new CodePointer {FullName ="BugfixAIProvider.Example.ExampleType", Type = CodeType.Type});
		Console.WriteLine(findMethodByFullName);

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

	private void OnBugFixResult(BugFixResult bugFixResult, Guid tan)
	{
		BugFixResults.TryAdd(tan, bugFixResult);
		Console.WriteLine($"Received message: {bugFixResult}");
	}
}
