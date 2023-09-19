#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

#endregion

namespace ExceptionAInalyzer.Models
{
	[DebuggerDisplay("UserMessage = {UserMessage}")]
	public class AnalyzedException<T> : Exception where T : Exception
	{
		public AnalyzedException([NotNull] T exception) : base(exception.Message, exception) { }

		public string ErrorAnalysis { get; set; }
		public string UserMessage { get; set; }
		public string DeveloperDetails { get; set; }
		public List<string> Suggestions { get; set; }
		public new T InnerException => (T) base.InnerException;

		/// <summary>
		///     Creates and returns a string representation of the current exception.
		/// </summary>
		/// <returns>A string representation of the current exception.</returns>
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