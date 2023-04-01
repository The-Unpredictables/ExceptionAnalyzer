#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

#endregion

namespace ExceptionAnalyzer.ApiKeyBalancer
{
	internal class ApiKey
	{
		public string Key { get; set; }

		public DateTime LastUsed { get; set; }

		public bool IsUsed { get; set; }
	}

	public static class ApiKeyService
	{
		[NotNull] private static readonly HashSet<ApiKey> ApiKeys = new HashSet<ApiKey>();

		public static string Lend()
		{
			ApiKey candidateKey = ApiKeys.OrderBy(key => key.LastUsed).First(key => key.IsUsed == false);
			candidateKey.IsUsed = true;
			return candidateKey.Key;
		}

		public static void Register(string apiKey) { ApiKeys.Add(new ApiKey {IsUsed = false, Key = apiKey, LastUsed = DateTime.UtcNow}); }

		public static void Release(string apiKey)
		{
			ApiKey releaseKey = ApiKeys.Single(k => k.Key == apiKey);
			releaseKey.IsUsed = false;
			releaseKey.LastUsed = DateTime.UtcNow;
		}
	}
}