#region Usings

using System;
using ExceptionAnalyzer.Models;
using JetBrains.Annotations;

#endregion

namespace ExceptionAnalyzer;

public static class ExceptionExtension
{

	private static readonly ExceptionService ExceptionService = new ();
	
	public static AnalyzedException<T> GetAnalyzedException<T>([NotNull] this T exception) where T : Exception => ExceptionService.GetAnalyzedException(exception);
}

