using System.Globalization;
using BugfixAiClient.Models;
using ExceptionAnalyzer;
using ExceptionAnalyzer.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR.Client;

namespace BugfixAiClient;

public static class ExceptionExtensions
{
	private static readonly ConnectionSetup ConnectionSetup = new ();

    public static async Task<BugfixResult?> TryDevelopBugfix<T>(this AnalyzedException<T> analyzedException, CancellationToken cancellationToken = default) where T : Exception 
        => await ConnectionSetup.BugfixDevelopConnection.InvokeAsync<BugfixResult>("TryDevelopBugfix", analyzedException.GetExceptionTextForAi(), cancellationToken);
    public static async Task<BugfixResult?> TryDevelopBugfix<T>(this T exception, CancellationToken cancellationToken = default) where T : Exception
        => await ConnectionSetup.BugfixDevelopConnection.InvokeAsync<BugfixResult>("TryDevelopBugfix", exception.GetExceptionTextForAi(), cancellationToken);
    public static string GetExceptionTextForAi<T>([NotNull] this AnalyzedException<T> exception) where T : Exception => $"{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}:\r\nError: {exception.Message}\r\nAnalysis: {exception.ErrorAnalysis}\r\nDetails: {exception.DeveloperDetails}\r\nStacktrace: {exception.StackTrace}";
}