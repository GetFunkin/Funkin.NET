using System;
using System.Collections.Generic;
using System.Text.Json;
using Funkin.NET.Intermediary.Injection.Services;
using Funkin.NET.Resources;

namespace Funkin.NET.Common.Providers
{
    public class SplashTextProvider : IService
    {
        public List<string[]> SplashTextList { get; }

        public SplashTextProvider(string jsonPath)
        {
            SplashTextList = JsonSerializer.Deserialize<List<string[]>>(jsonPath);
        }

        public string[] GetSplashText() => SplashTextList[new Random().Next(0, SplashTextList.Count)];

        public static SplashTextProvider CreateProvider() => new(PathHelper.Json.IntroTextJson);
    }
}