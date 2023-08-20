using BugfixAiClient.Models;
using Xunit.Sdk;

namespace BugfixAiClient.Tests
{
    public class UnitTestAnalyzedExceptionExtension
    {
        [Fact]
        public void DecompileSourceCode_CallTheBugfixAIApiWithAnException_GetTheBugfixForTheException()
        {
            SourceFinder sourceFinder = new();
            CodePointer codePointer = new () { FullName = "BugfixAiClient.Tests.ExampleType.ExampleMethod", CodeType = CodeType.Method, ParameterTypeFullNames = new List<string> { "System.String", "BugfixAiClient.Tests.ForeignNamespace.ForeignType" }};
            sourceFinder.GetByFullName(codePointer);
        }
        
        [Fact]
        public void TryDevelopBugfix_CallTheBugfixAIApiWithAnException_GetTheBugfixForTheException()
        {
            #region Arrange
            Exception? exception = null;
            try
            {
                GenerateCustomNumber(0);
            } catch (Exception e)
            {
                exception = e;
            }

            #endregion Arrange

            #region Act

            if (exception == null) throw new NotNullException();

            Task<BugfixResult?> bugfixTask = exception.TryDevelopBugfix();
            bugfixTask.Wait(90000000);
            BugfixResult? bugfixResult = bugfixTask.Result;

            #endregion Act

            #region Assert


            #endregion Assert
        }

        public static void GenerateCustomNumber(int a)
        {
            Console.WriteLine($"Start Methode: {nameof(GenerateCustomNumber)}");
  
            int b = 10;
  
            int c = b / a;
            
            Console.WriteLine($"End Methode: {nameof(GenerateCustomNumber)}");
        }
    }
}