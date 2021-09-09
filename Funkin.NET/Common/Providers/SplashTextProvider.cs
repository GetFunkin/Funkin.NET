using System;
using System.Collections.Generic;
using System.Text.Json;
using Funkin.NET.Resources;

namespace Funkin.NET.Common.Providers
{
    public class SplashTextProvider
    {
        public List<string[]> SplashTextList { get; }

        public SplashTextProvider()
        {
            SplashTextList = JsonSerializer.Deserialize<List<string[]>>(PathHelper.Json.IntroTextJson);
        }

        public string[] GetSplashText() => SplashTextList[new Random().Next(0, SplashTextList.Count)];
    }
}