namespace ExceptionAInalyzer.Interfaces;

public interface IErrorInfo
{
    string Message { get; }
    string StackTrace { get; }
    string UserMessageLanguageTwoLetter { get; }
}