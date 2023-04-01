#region Usings

using System.Globalization;
using ExceptionAnalyzer.ApiKeyBalancer;
using ExceptionAnalyzer.Models;
using Xunit.Abstractions;

#endregion

namespace ExceptionAnalyzer.Tests
{
	public class UnitTestExceptionExtension
	{
		public UnitTestExceptionExtension(ITestOutputHelper testOutputHelper) { _testOutputHelper = testOutputHelper; }

		private readonly ITestOutputHelper _testOutputHelper;

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