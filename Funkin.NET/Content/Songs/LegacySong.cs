using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Funkin.NET.Content.Songs
{
    /// <summary>
    ///     Implementation of regular the FNF song format.
    /// </summary>
    public class LegacySong : ISong<LegacyNote>
    {
        [JsonProperty("song")] public string SongName { get; }

        [JsonProperty("notes")] public LegacyNote[] Notes { get; }

        [JsonProperty("bpm")] public double Bpm { get; }

        [JsonProperty("needsVoices")] public bool NeedsVoices { get; set; } = true;

        [JsonProperty("speed")] public float Speed { get; set; } = 1f;

        [JsonProperty("play1")] public string PlayerCharacter { get; set; } = "boyfriend";

        [JsonProperty("player2")] public string OpponentCharacter { get; set; } = "dad";

        [JsonIgnore] public bool ValidScore { get; set; } = true;

        public LegacySong(string songName, LegacyNote[] notes, double bpm)
        {
            SongName = songName;
            Notes = notes;
            Bpm = bpm;
        }

        public static LegacySong FromJson(string path, bool expectDictionaryFormat = true)
        {
            string json = File.ReadAllText(path);

            // TODO: what the hell is this?
            /*while (!rawJson.endsWith("}"))
            {
                rawJson = rawJson.substr(0, rawJson.length - 1);
                // LOL GOING THROUGH THE BULLSHIT TO CLEAN IDK WHATS STRANGE
            }*/

            return ParseJson(json, expectDictionaryFormat);
        }

        public static LegacySong ParseJson(string json, bool expectDictionaryFormat = true)
        {
            // haxe/haxeflixel/whatever json (de)serializer
            // that ninjamuffin uses unfortunately writes
            // starting objects in a dictionary-like format:
            // { "key" { /* contents that you would expect at the top level */ } }
            LegacySong song = expectDictionaryFormat
                ? JsonConvert.DeserializeObject<Dictionary<string, LegacySong>>(json)?.Values.First()
                : JsonConvert.DeserializeObject<LegacySong>(json);

            // if it fails to load then other issues would
            // arise down the line, better to go nuclear
            // here instead of pretending that everything
            // is okay for a short while longer when it isn't
            if (song is null)
                throw new Exception($"Failed to parse and load an {nameof(ISong)} " +
                                    $"in the {nameof(LegacySong)} format using JSON!");

            song.ValidScore = true; // just to be safe
            return song;
        }
    }
}