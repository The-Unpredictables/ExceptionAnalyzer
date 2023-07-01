using BugfixAIProvider.Models;
using Xunit.Sdk;

namespace BugfixAIProvider.Tests
{
    public class UnitTestAnalyzedExceptionExtension
    {
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
            BugfixResult bugfixResult = bugfixTask.Result;

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