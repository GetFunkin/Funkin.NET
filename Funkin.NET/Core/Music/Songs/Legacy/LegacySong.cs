using System.Collections.Generic;
using System.Reflection;
using Funkin.NET.Resources;
using Newtonsoft.Json;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacySong
    {
        [JsonProperty("player1")] public virtual string Player1 { get; set; }

        [JsonProperty("player2")] public virtual string Player2 { get; set; }

        [JsonProperty("speed")] public virtual double Speed { get; set; }

        [JsonProperty("needsVoices")] public virtual bool NeedsVoices { get; set; }

        [JsonProperty("sectionLengths")] public virtual List<object> SectionLengths { get; set; }

        [JsonProperty("song")] public virtual string SongName { get; set; }
        
        [JsonProperty("notes")]
        public virtual List<LegacySection> Sections { get; set; }

        [JsonProperty("bpm")] public virtual double Bpm { get; set; }

        [JsonProperty("sections")] public virtual int NumSections { get; set; }

        public static LegacySong GetSong(string json)
        {
            LegacyTrack track = LegacyTrack.GetTrack(json);
            return track.Song;
        }

        public static LegacySong GetSongFromFile(string filePath, Assembly assembly = null) => GetSong(
            PathHelper.Json.GetEmbeddedJson(PathHelper.EmbeddedResource.SanitizeForEmbeds(filePath, assembly),
                assembly));
    }
}