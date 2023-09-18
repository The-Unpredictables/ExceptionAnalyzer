# ExceptionAnalyzer

[![Downloads](https://img.shields.io/nuget/dt/ExceptionAInalyzer?style=flat-square)](https://www.nuget.org/packages/ExceptionAInalyzer)
[![Pipeline status](https://img.shields.io/github/actions/workflow/status/The-Unpredictables/ExceptionAnalyzer/dotnet.yml?branch=main&style=flat-square)](https://github.com/The-Unpredictables/ExceptionAnalyzer/actions/workflows/dotnet.yml)
[![GitHub](https://img.shields.io/github/license/The-Unpredictables/ExceptionAnalyzer?style=flat-square)](https://github.com/The-Unpredictables/ExceptionAnalyzer/blob/main/LICENSE)

| ![Logo](https://raw.githubusercontent.com/The-Unpredictables/ExceptionAnalyzer/main/logo.png) | ExceptionAnalyzer is a C# library that utilizes the OpenAI API to analyze, interpret, and provide useful information from complex data such as error stack traces. It offers actionable insights to both users and developers, assisting in troubleshooting and error resolution by processing and presenting relevant details in a clear, concise, and user-friendly manner. |
| ---------- | ------- | 

## Features

- Analyze stack traces of exceptions and identify root causes of errors
- Generate a detailed error analysis, highlighting possible causes and affected components
- Craft user-friendly messages to help non-technical users understand errors without overwhelming them with technical jargon
- Provide technical descriptions of errors for software developers, including information about affected classes, methods, and line numbers in the code
- Suggest one or more potential solutions to fix the error based on the conducted analysis

## Installation

Include the `ExceptionAnalyzer` project in your solution and add a reference to it in your main project.

## Usage

To use the ExceptionAnalyzer, you will need an API key from OpenAI. Replace the placeholder API key in the `ApiKeyService` class with your own API key.

```csharp
using ExceptionAnalyzer;
using ExceptionAnalyzer.Models;
```

To analyze an exception, call the GetAnalyzedException extension method on an instance of an exception:

```csharp
try
{
    // Your code that may throw an exception
}
catch (Exception ex)
{
    AnalyzedException<Exception> analyzedException = ex.GetAnalyzedException();
    Console.WriteLine(analyzedException.UserMessage);
    Console.WriteLine(analyzedException.DeveloperDetails);
}
```

The `GetAnalyzedException` method returns an instance of `AnalyzedException<T>`, where `T` is the type of the exception. The returned object contains the following properties:


- `ErrorAnalysis`: A detailed analysis of the error, including possible causes and affected components
- `UserMessage`: An easy-to-understand message for the user, providing an overview of the occurred error without being too technical
- `DeveloperDetails`: A technical description of the error for software developers, including affected classes, methods, and line numbers in the code
- `Solutions`: One or more possible solution suggestions to fix the error based on the analysis

## Dependencies

- Newtonsoft.Json
- OpenAI_API
- JetBrains.Annotations

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
