using BugfixAIProvider.Tests.ForeignNamespace;

namespace BugfixAIProvider.Tests;

public class ExampleType
{
	
	/// <summary>
	///		xmldoc
	/// </summary>
	public string ExampleProperty { get; set; }
	
	public ForeignType ForeignProperty { get; set; }

	public string ExampleMethod()
	{
		return "Example";
	}
	
	public string ExampleMethod(string parameter)
	{
		return parameter;
	}
	
	public string ExampleMethod(string parameter1, ForeignType parameter2)
	{
		return parameter1 + parameter2;
	}
	
}