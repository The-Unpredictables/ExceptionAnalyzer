using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExceptionAnalyzer.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Rystem.OpenAi;
using Rystem.OpenAi.Chat;

[assembly:InternalsVisibleTo("ExceptionAnalyzer.Tests")]

namespace ExceptionAnalyzer;

public class ExceptionService
{
	[NotNull] private static readonly List<ChatMessage> ChatMessages = new ();
    [NotNull] internal static IOpenAi OpenAi => OpenAiService.Factory.Create();
    public static string ApiKey
    {
        set
        {
            OpenAiService.Instance.AddOpenAi(settings =>
                                             {
                                                 settings.ApiKey = value;
                                             });
        }
    }

	public ExceptionService()
	{
		
		ChatMessages.Add(new ChatMessage {Role = ChatRole.System, Content = "As an AI language model in this role, your purpose is to analyze, interpret, and provide useful information from complex data such as error stack traces, while offering actionable insights to users and developers. Your primary objective is to assist in troubleshooting and error resolution by processing and presenting the relevant details in a clear, concise, and user-friendly manner. Your key responsibilities include: Analyzing stack traces of exceptions and identifying the root causes of errors. Generating a detailed error analysis that highlights possible causes and affected components. Crafting user-friendly messages to help non-technical users understand the occurred error without overwhelming them with technical jargon. Providing technical descriptions of the error for software developers, including information about affected classes, methods, and line numbers in the code. Suggesting one or more potential solutions to fix the error based on your conducted analysis. Through these responsibilities, your aim is to streamline the troubleshooting process, improve communication between technical and non-technical stakeholders, and expedite error resolution by providing targeted, actionable insights. At the beginning of the message you will receive a country code (e.g. EN or en-GB). Reply in this language."});
		ChatMessages.Add(new ChatMessage {Role = ChatRole.User, Content = "en-US:\r\nAnalyze the following stacktrace of an exception and return the results in a JSON structure. The JSON structure must contain the following fields:\r\n\"errorAnalysis\": A detailed analysis of the error, including possible causes and affected components.\r\n\"userMessage\": An easy-to-understand message for the user, providing an overview of the occurred error without being too technical.\r\n\"developerDetails\": A technical description of the error for software developers, including affected classes, methods, and line numbers in the code.\r\nExample:\r\n\"solutions\": One or more possible solution suggestions to fix the error based on the analysis.\r\n{\r\n  \"errorAnalysis\": \"The exception was triggered by an invalid input in the 'calculate' function.\",\r\n  \"userMessage\": \"An error has occurred: Invalid input. Please check your input and try again.\",\r\n  \"developerDetails\": \"The invalid input led to an IllegalArgumentException in the 'calculate' function of the 'Calculator' class on line 42.\",\r\n  \"solutions\": [\r\n    \"Ensure the input values are within the valid range.\",\r\n    \"Add input value validation to catch invalid values early.\"\r\n  ]\r\n}"});
		ChatMessages.Add(new ChatMessage {Role = ChatRole.Assistant, Content = "Ok, I got it. Please give me the error message and the stack trace."});
		ChatMessages.Add(new ChatMessage {Role = ChatRole.User, Content = "en-US:\r\nApplication: Magnetic.exe\r\nFramework Version: v4.0.30319\r\nDescription: The process was terminated due to an unhandled exception.\r\nException Info: System.MissingFieldException\r\n   at Magnetic.Presentation.ViewModels.ViewModelState..ctor(Prism.Events.IEventAggregator, Magnetic.Core.Interfaces.IDeviceHandlerContainer, Unity.IUnityContainer, Magnetic.BusinessObjects.Interfaces.ISignalCollectionInformation, Magnetic.Core.Interfaces.IConnectionManager"});
		ChatMessages.Add(new ChatMessage {Role = ChatRole.Assistant, Content = "{\r\n\"errorAnalysis\": \"The exception is due to a System.MissingFieldException in the ViewModelState constructor, which led to a chain of exceptions, causing the application to terminate. This could be caused by a missing field, property or parameter in the ViewModelState class or its dependencies.\",\r\n\"userMessage\": \"An error has occurred while initializing the application. Please contact the support team for assistance.\",\r\n\"developerDetails\": \"The System.MissingFieldException occurred in the ViewModelState constructor at Magnetic.Presentation.ViewModels.ViewModelState..ctor. This caused a series of ResolutionFailedExceptions, ActivationExceptions, XamlParseExceptions, RegionCreationExceptions, and UpdateRegionsExceptions throughout the application. The root cause likely lies in the ViewModelState class or its dependencies.\",\r\n\"solutions\": [\r\n\"Check the ViewModelState class and its dependencies for any missing fields, properties or parameters.\",\r\n\"Ensure the correct version of dependencies is being used and are compatible with each other.\",\r\n\"Verify that the ViewModelState constructor is receiving the correct parameters during object creation.\"\r\n]\r\n}"});
	}

	public async Task<AnalyzedException<T>> GetAnalyzedException<T>([NotNull] T exception) where T : Exception => await GetAnalyzedExceptionInternalAsync(exception);

	internal async Task<AnalyzedException<T>> GetAnalyzedExceptionInternalAsync<T>([NotNull] T exception) where T : Exception
	{
		if (exception == null) throw new ArgumentNullException(nameof(exception));
		List<ChatMessage> currentMessages = ChatMessages.ToList();
		currentMessages.Add(new ChatMessage{Role = ChatRole.User, Content = exception.GetExceptionTextForAi()});
		string response = null;
		try
        {
            ChatRequestBuilder requestBuilder = OpenAi.Chat.RequestCollection(currentMessages)?.WithModel(TextModelType.Gpt4_Snapshot.ToModelId()).WithTemperature(1).SetMaxTokens(3000);
            ChatResult chatResult = await requestBuilder?.ExecuteAsync().AsTask()!;
            ChatMessage openAiMessage = chatResult?.Choices?.FirstOrDefault()?.Message;
			response = openAiMessage?.Content;
		} catch (Exception e)
		{
			Console.WriteLine(e);
            throw e;
        }

		AnalyzedException<T> analyzedException = new (exception);
		if (!string.IsNullOrWhiteSpace(response)) analyzedException.MapAnalysis(JsonConvert.DeserializeObject<InternalAnalyzedException>(response));
		return analyzedException;
	}
}