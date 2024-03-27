#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExceptionAInalyzer.Interfaces;
using JetBrains.Annotations;

#endregion

namespace ExceptionAInalyzer.Models
{
	[DebuggerDisplay("UserMessage = {UserMessage}")]
	public class AnalyzedErrorInfo<T> : IErrorInfo where T : IErrorInfo
	{
		public AnalyzedErrorInfo([NotNull] T errorInfo)
        {
            if (errorInfo == null) throw new ArgumentNullException(nameof(errorInfo));
            ErrorInfo = errorInfo;
        }

		public string ErrorAnalysis { get; set; }
		public string UserMessage { get; set; }
		public string DeveloperDetails { get; set; }
		public List<string> Suggestions { get; set; }
		public T ErrorInfo { get; set; }
        public string Message => ErrorInfo.Message;
        public string StackTrace => ErrorInfo.StackTrace;
        public string UserMessageLanguageTwoLetter => ErrorInfo.UserMessageLanguageTwoLetter;

		/// <summary>
		///     Creates and returns a string representation of the current errorInfo.
		/// </summary>
		/// <returns>A string representation of the current errorInfo.</returns>
		public override string ToString() => UserMessage ?? base.ToString();

		internal void MapAnalysis([NotNull] InternalAnalyzedException internalAnalyzedException)
		{
			ErrorAnalysis = internalAnalyzedException.ErrorAnalysis;
			UserMessage = internalAnalyzedException.UserMessage;
			DeveloperDetails = internalAnalyzedException.DeveloperDetails;
			Suggestions = internalAnalyzedException.Solutions;
		}
    }
}