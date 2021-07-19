using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using osu.Framework;

namespace Funkin.NET
{
    /// <summary>
    ///     Base Funkin' game. Contains data shared between the test browser and game implementation.
    /// </summary>
    public class FunkinGame : Game
    {
        public const string ProgramName = "Funkin.NET";

        public static List<string[]> FunnyTextList { get; private set; }

        public static string[] FunnyText { get; private set; }

        public FunkinGame()
        {
            Name = ProgramName;

            string path = Path.Combine("Json", "IntroText.json");
            string text = File.ReadAllText(path);
            FunnyTextList = JsonConvert.DeserializeObject<List<string[]>>(text);
            FunnyText = FunnyTextList?[new Random().Next(0, FunnyTextList.Count)];
        }
    }
}