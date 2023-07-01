#region Usings

using System;
using System.Globalization;
using ExceptionAnalyzer.Models;
using JetBrains.Annotations;

#endregion

namespace ExceptionAnalyzer;

public static class ExceptionExtension
{
    private static readonly ExceptionService ExceptionService = new ();
    public static AnalyzedException<T> GetAnalyzedException<T>([NotNull] this T exception) where T : Exception => ExceptionService.GetAnalyzedException(exception);
    public static string GetExceptionTextForAi<T>([NotNull] this T exception) where T : Exception => $"{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}:\r\n{exception.Message}\r\n{exception.StackTrace}";
}

