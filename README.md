# ExceptionAInalyzer: Error analysis with AI support 🚀

[![Downloads](https://img.shields.io/nuget/dt/ExceptionAInalyzer?style=flat-square)](https://www.nuget.org/packages/ExceptionAInalyzer)
[![Pipeline status](https://img.shields.io/github/actions/workflow/status/The-Unpredictables/ExceptionAnalyzer/dotnet.yml?branch=main&style=flat-square)](https://github.com/The-Unpredictables/ExceptionAnalyzer/actions/workflows/dotnet.yml)
[![GitHub](https://img.shields.io/github/license/The-Unpredictables/ExceptionAnalyzer?style=flat-square)](https://github.com/The-Unpredictables/ExceptionAnalyzer/blob/main/LICENSE)

| ![Logo](https://raw.githubusercontent.com/The-Unpredictables/ExceptionAnalyzer/main/logo.png) | ExceptionAnalyzer is a C# library that utilizes the OpenAI API to analyze, interpret, and provide useful information from complex data such as error stack traces. It offers actionable insights to both users and developers, assisting in troubleshooting and error resolution by processing and presenting relevant details in a clear, concise, and user-friendly manner. |
| ---------- | ------- | 

Dive into the era of AI-supported error handling with ExceptionAInalyzer. An innovation geared towards simplifying software development processes.

## 🌟 Overview

In the age of modern software development, the importance of efficient bug fixing cannot be overstated. But, what if AI could make this process even more streamlined? That's where ExceptionAInalyzer steps in. By harnessing the power of OpenAI's GPT models, ExceptionAInalyzer analyzes exceptions, provides detailed analysis, user-friendly messages, and solution proposals.

## 📜 Error Diagnosis in Software Development

Software developers know the challenges of error diagnosis. Whether it's interpreting error messages or analyzing stack traces, the process can be tedious. ExceptionAInalyzer aims to bridge the gap between technical precision and general understanding. Powered by GPT-4, ExceptionAInalyzer can provide insights like an experienced developer or a support team member, thanks to the vast data it has been trained on.

## 🤖 Features

- Analyze stack traces of exceptions and identify root causes of errors with AI support using OpenAI's GPT model
- Generate a detailed error analysis, highlighting possible causes and affected components
- Craft user-friendly messages to help non-technical users understand errors without overwhelming them with technical jargon
- Provide technical descriptions of errors for software developers, including information about affected classes, methods, and line numbers in the code
- Suggest one or more potential solutions to fix the error based on the conducted analysis

## ⚙️ Installation

Include the `ExceptionAInalyzer` project in your solution and add a reference to it in your main project.

## 📊 Usage

To use the ExceptionAnalyzer, you will need an API key from OpenAI. Use the SetApiKey Methode in ExceptionService.

```csharp
using ExceptionAInalyzer;
using ExceptionAInalyzer.Models;
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

## ⚠️ Limitations

Connection and response times are vital parameters to consider when using the application. Although the primary focus is on improving the software development process, these are essential aspects to keep in mind.

## 🎉 Conclusion

ExceptionAInalyzer is a step towards the future, making error diagnosis easier for both developers and end-users. By intelligently incorporating AI into the software development process, it's revolutionizing the way we approach bug fixing.
## 📦 Dependencies

- Newtonsoft.Json
- OpenAI_API
- JetBrains.Annotations

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
