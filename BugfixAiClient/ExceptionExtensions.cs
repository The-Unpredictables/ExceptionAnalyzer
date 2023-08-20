using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using BugfixAiClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace BugfixAiClient;

public static class ExceptionExtensions
{
	private static readonly ConnectionSetup ConnectionSetup = new ();

    public static async Task<BugfixResult?> TryDevelopBugfix<T>(this T exception, CancellationToken cancellationToken = default) where T : Exception
        => await ConnectionSetup.BugfixDevelopConnection.InvokeAsync<BugfixResult>("TryDevelopBugfix", exception.GetExceptionTextForAi(), cancellationToken);
    public static string GetExceptionTextForAi<T>([NotNull] this T exception) where T : Exception => $"{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}:\r\n{exception.Message}\r\n{exception.StackTrace}";
}