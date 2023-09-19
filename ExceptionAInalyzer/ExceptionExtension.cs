#region Usings

using System;
using System.Threading.Tasks;
using ExceptionAInalyzer.Models;
using JetBrains.Annotations;

#endregion

namespace ExceptionAInalyzer;

public static class ExceptionExtension
{

	private static readonly ExceptionService ExceptionService = new ();
	
	public static async Task<AnalyzedException<T>> GetAnalyzedExceptionAsync<T>([NotNull] this T exception) where T : Exception => await ExceptionService.GetAnalyzedException(exception);
	public static AnalyzedException<T> GetAnalyzedException<T>([NotNull] this T exception) where T : Exception => ExceptionService.GetAnalyzedException(exception).Result;
}

