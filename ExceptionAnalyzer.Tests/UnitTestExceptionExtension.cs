#region Usings

using System.Globalization;
using ExceptionAInalyzer.ApiKeyBalancer;
using ExceptionAInalyzer.Models;
using Moq;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using Shouldly;
using Xunit.Abstractions;

#endregion

namespace ExceptionAInalyzer.Tests
{
	public class UnitTestExceptionExtension
	{
		public UnitTestExceptionExtension(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

		private readonly ITestOutputHelper _testOutputHelper;

		[Fact] public async void GetAnalyzedException_TestOpenAiApiCall_ResultIsReturned()
		{
			#region Arrange

			const string key = "sk-thisisonlyatestvalue";
			const string response = @"
									{
										""errorAnalysis"": ""Sample error analysis"",
										""userMessage"": ""Sample user message"",
										""developerDetails"": ""Sample developer details"",
										""solutions"": [
											""Sample solution 1"",
											""Sample solution 2""
										]
									}";
			InternalAnalyzedException internalAnalyzedException = JsonConvert.DeserializeObject<InternalAnalyzedException>(response)!;
			ApiKeyService.Register(key);
			ChatResult chatResult = new() {Choices = new[] {new ChatChoice {Message = new ChatMessage {Content = response}}}};
			Mock<IOpenAIAPI> openAiApiMock = new();
			openAiApiMock.SetupProperty(api => api.Auth, new APIAuthentication(key));
			openAiApiMock.Setup(api => api.Chat.CreateChatCompletionAsync(It.IsAny<ChatRequest>())).Returns(Task.FromResult(chatResult));
			ExceptionService exceptionService = new();
			ArgumentNullException testingException = new(nameof(openAiApiMock));

			#endregion Arrange

			#region Act

			AnalyzedException<ArgumentNullException> analyzedExceptionInternal = await exceptionService.GetAnalyzedExceptionInternal(testingException, openAiApiMock.Object!)!;

			#endregion Act

			#region Assert

			internalAnalyzedException!.DeveloperDetails.ShouldBe(analyzedExceptionInternal!.DeveloperDetails);
			internalAnalyzedException.ErrorAnalysis.ShouldBe(analyzedExceptionInternal.ErrorAnalysis);
			internalAnalyzedException.UserMessage.ShouldBe(analyzedExceptionInternal.UserMessage);
			internalAnalyzedException.Solutions.ShouldBe(analyzedExceptionInternal.Suggestions);

			#endregion Assert
		}

		[Fact] public void GetAnalyzedException_SendExceptionToAiForAnalyze_AnalyzedExceptionFieldsAreFilled()
		{
			#region Arrange

			ApiKeyService.Register(@"your key");
			CultureInfo.CurrentCulture = new CultureInfo("");
			Exception exception = null;
			List<int> zahlen = new()
								{
										1, 2, 3, 4,
										13, 5
								}; // 13 wird eine versteckte Exception auslösen
			VersteckteException versteckteException = new();
			try
			{
				int sum = versteckteException.BerechneSumme(zahlen);
				_testOutputHelper.WriteLine("Sum: " + sum);
			} catch (Exception ex)
			{
				exception = ex;
			}

			#endregion

			#region Act

			AnalyzedException<Exception?> analyzedException = exception.GetAnalyzedException();

			#endregion

			#region Assert

			Assert.NotNull(analyzedException);
			Assert.NotNull(analyzedException.DeveloperDetails);
			Assert.NotNull(analyzedException.ErrorAnalysis);
			Assert.NotNull(analyzedException.UserMessage);
			Assert.NotEmpty(analyzedException.Suggestions);

			#endregion
		}
	}

	public class VersteckteException
	{
		public int BerechneSumme(List<int> zahlen)
		{
			int summe = 0;
			try
			{
				for (int i = 0; i < zahlen.Count; i++) summe = AddiereZahlen(summe, zahlen[i]);
			} catch (InvalidOperationException ex)
			{
				Console.WriteLine("Eine versteckte InvalidOperationException ist aufgetreten: " + ex.Message);
				throw;
			} catch (Exception ex)
			{
				Console.WriteLine("Eine unbekannte Exception ist aufgetreten: " + ex.Message);
				throw;
			}

			return summe;
		}

		private int AddiereZahlen(int a, int b)
		{
			if (a == 13 || b == 13) // Beispiel f�r einen versteckten Fehler
				throw new InvalidOperationException("Ung�ltiger Wert: 13 ist nicht erlaubt.");
			return a + b;
		}
	}
}