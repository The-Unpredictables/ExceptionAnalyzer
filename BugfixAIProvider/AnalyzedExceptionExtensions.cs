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
//      Software: ExceptionAnalyzer | Package: BugfixAIProvider | File: ProviderExtensions.cs
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

using BigfixAIProvider;
using BugfixAIProvider.Helper;
using ExceptionAnalyzer.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace BugfixAIProvider;

public static class AnalyzedExceptionExtensions
{
	private static readonly Setup Setup;
	
	static AnalyzedExceptionExtensions()
	{
		Setup = new Setup();
	}

	public static async Task<BugFixResult?> TryDevelopBugfix<T>(this AnalyzedException<T> analyzedException, CancellationToken cancellationToken = default) where T : Exception
	{
		Guid tan = Guid.NewGuid();
		await Setup.Connection.InvokeAsync("TryBugfixing", tan, analyzedException, cancellationToken);
		BugFixResult? result = null;
		await AsyncHelper.Loop(() => Setup.BugFixResults.TryGetValue(tan, out result), 1000, () => Setup.BugFixResults.ContainsKey(tan));
		return result;
	}
}