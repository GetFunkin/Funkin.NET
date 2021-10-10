using System;
using System.Collections.Generic;
using Funkin.NET.Intermediary.Injection.Services;
using Funkin.NET.Resources;
using Newtonsoft.Json;

namespace Funkin.NET.Common.Services
{
    public class SplashTextService : IService
    {
        public List<string[]> SplashTextList { get; }

        public SplashTextService(string jsonPath)
        {
            SplashTextList = JsonConvert.DeserializeObject<List<string[]>>(jsonPath);
        }

        public string[] GetSplashText() => SplashTextList[new Random().Next(0, SplashTextList.Count)];

        [ProvidesService]
        public static SplashTextService CreateProvider() => new(PathHelper.Json.IntroTextJson);
    }
}