﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Funkin.NET.Common.Providers
{
    public class SplashTextProvider
    {
        public List<string[]> SplashTextList { get; }

        public SplashTextProvider()
        {
            string json = File.ReadAllText(Path.Combine("Json", "IntroText.json"));
            SplashTextList = JsonSerializer.Deserialize<List<string[]>>(json);
        }

        public string[] GetSplashText() => SplashTextList[new Random().Next(0, SplashTextList.Count)];
    }
}