// See https://aka.ms/new-console-template for more information
using BugfixAIProvider.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace BugfixAIProvider;

internal class ConnectionSetup
{

	internal static readonly Guid ClientGuid = Guid.NewGuid();
    internal static HubConnection BugfixDevelopConnection { get; }
    internal static HubConnection SourceCodeResponseConnection { get; }
    private static readonly SourceFinder SourceFinder = new ();
	
    static ConnectionSetup()
    {
		//string findMethodByFullName = sourceFinder.GetByFullName(new CodePointer {FullName ="BugfixAIProvider.Example.ExampleType", Type = CodeType.Type});
		//Console.WriteLine(findMethodByFullName);

		HubConnectionBuilder builder = new();
        BugfixDevelopConnection = builder
                                  .AddNewtonsoftJsonProtocol()
                                  .WithUrl("https://localhost:7181/BugfixerHub")
                                  .WithAutomaticReconnect()
                                  .Build();
        builder = new();
        SourceCodeResponseConnection = builder
                                       .AddNewtonsoftJsonProtocol()
                                       .WithUrl("https://localhost:7181/SourceCodeHub")
                                       .WithAutomaticReconnect()
                                       .Build();
        BugfixDevelopConnection.On<CodePointer>("RequestSourceCode", RequestSourceCode);
        BugfixDevelopConnection.StartAsync().Wait();
        SourceCodeResponseConnection.StartAsync().Wait();
        BugfixDevelopConnection.SendAsync("Register", ClientGuid).Wait();
    }

    public static Task RequestSourceCode(CodePointer codePointer)
    {
        string sourceCode = SourceFinder.GetByFullName(codePointer);
        if (string.IsNullOrEmpty(sourceCode)) sourceCode = "Error: Source code was not found.";
        return SourceCodeResponseConnection.SendAsync("SendSourceCode", codePointer, sourceCode);
    }
}
