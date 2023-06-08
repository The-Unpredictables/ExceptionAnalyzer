#region Magnetic Sense Copyright
//              =====================================================================
//               __  __                        _   _       _____
//              |  \/  |                      | | (_)     / ____|
//              | \  / | __ _  __ _ _ __   ___| |_ _  ___| (___   ___ _ __  ___  ___
//              | |\/| |/ _` |/ _` | '_ \ / _ \ __| |/ __|\___ \ / _ \ '_ \/ __|/ _ \
//              | |  | | (_| | (_| | | | |  __/ |_| | (__ ____) |  __/ | | \__ \  __/
//              |_|  |_|\__,_|\__, |_| |_|\___|\__|_|\___|_____/ \___|_| |_|___/\___|
//                             __/ |
//                            |___/
//              =====================================================================
//      Software: BigfixAIProvider | Package: BigfixAIProvider | File: SourceFinder.cs
//       -------------------------------------------------------------------------------------
//             Copyright ©2023        |   Unauthorized copying of this file, via any medium
//           MagneticSense GmbH       |    is strictly prohibited. Permission to use, copy,
//           All Rights Reserved      |      modify and distribute this software and its
//       Proprietary and confidential | documentation without corresponding license are denied
//       -------------------------------------------------------------------------------------
//                            Contact for licensing opportunities
//                  MagneticSense GmbH  |  Kelterstrasse 59  |  72669 Unterensingen
//                  Phone: +49 7022 40590 0     |    E-Mail: info@magnetic-sense.de
//                  Fax: +49 7022 40590 29      |    Website: www.magnetic-sense.de
//                  ---------------------------------------------------------------
#endregion

using ICSharpCode.Decompiler.CSharp.Syntax;

namespace BigfixAIProvider;

public class SourceFinder
{
	private SyntaxTree _syntaxTree;
	public SourceFinder(SyntaxTree syntaxTree) { _syntaxTree = syntaxTree; }

	public string FindMethodByFullName(CodePointer codePointer)
	{
		List<string> nameParts = codePointer.FullName.Split('.').ToList();
		string methodName = string.Empty;
		string typeName;
		string namespaceName;
		switch (codePointer.Type)
		{
			case CodeType.Method:
				if (codePointer.FullName.Count(c => c == '.') < 2)
				{
					throw new ArgumentException("fullName must contain at least two dots");
				}

				methodName = nameParts.Last();
				nameParts.RemoveAt(nameParts.Count - 1);
				typeName = nameParts.Last();
				nameParts.RemoveAt(nameParts.Count - 1);
				namespaceName = string.Join('.', nameParts);

				break;
			case CodeType.Type:
				if (!codePointer.FullName.Contains('.'))
				{
					throw new ArgumentException("fullName must contain at least one dot");
				}

				typeName = nameParts.Last();
				nameParts.RemoveAt(nameParts.Count - 1);
				namespaceName = string.Join('.', nameParts);
				break;
			default: throw new ArgumentException("CodeType not supported");
		}

		NamespaceDeclaration ns = _syntaxTree.Children.OfType<NamespaceDeclaration>().First(ns => ns.Name == namespaceName);
		TypeDeclaration type = ns.Children.OfType<TypeDeclaration>().First(type => type.Name == typeName);
		if (codePointer.Type != CodeType.Method) return type.ToString();
		MethodDeclaration method = type.Children.OfType<MethodDeclaration>().First(method => method.Name == methodName);
		return method.ToString();
	}
}