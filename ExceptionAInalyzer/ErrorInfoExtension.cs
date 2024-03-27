#region Usings

using System.Threading.Tasks;
using ExceptionAInalyzer.Interfaces;
using ExceptionAInalyzer.Models;
using JetBrains.Annotations;

#endregion

namespace ExceptionAInalyzer;

public static class ErrorInfoExtension
{
	private static readonly ExceptionService ExceptionService = new ();
	
	public static async Task<AnalyzedErrorInfo<T>> GetAnalyzedErrorInfoAsync<T>([NotNull] this T exception) where T : IErrorInfo => await ExceptionService.GetAnalyzedErrorInfo(exception);
	public static AnalyzedErrorInfo<T> GetAnalyzedErrorInfo<T>([NotNull] this T exception) where T : IErrorInfo => ExceptionService.GetAnalyzedErrorInfo(exception).Result;
}

