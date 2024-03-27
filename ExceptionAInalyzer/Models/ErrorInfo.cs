using System;
using System.Globalization;
using ExceptionAInalyzer.Interfaces;
using JetBrains.Annotations;

namespace ExceptionAInalyzer.Models;

public class ErrorInfo : IErrorInfo
{
    public ErrorInfo([NotNull] IErrorInfo errorInfo) : this(errorInfo.Message, errorInfo.StackTrace, errorInfo.UserMessageLanguageTwoLetter)
    {
        if (errorInfo == null) throw new ArgumentNullException(nameof(errorInfo));
    }

    public ErrorInfo([NotNull] string message, [NotNull] string stackTrace, string userMessageLanguageTwoLetter = null)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        StackTrace = stackTrace ?? throw new ArgumentNullException(nameof(stackTrace));
        UserMessageLanguageTwoLetter = userMessageLanguageTwoLetter ?? CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    }

    public string Message { get; }
    public string StackTrace { get; }
    public string UserMessageLanguageTwoLetter { get; }
}