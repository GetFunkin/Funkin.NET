using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Funkin.NET.Intermediary.Json;
using Funkin.NET.Resources;

namespace Funkin.NET.Core.Music.Songs.Legacy
{
    public class LegacySong : ISong
    {
        [JsonPropertyName("player1")] public virtual string Player1 { get; set; }

        [JsonPropertyName("player2")] public virtual string Player2 { get; set; }

        [JsonPropertyName("speed")] public virtual double Speed { get; set; }

        [JsonPropertyName("needsVoices")] public virtual bool NeedsVoices { get; set; }

        [JsonPropertyName("sectionLengths")] public virtual List<object> SectionLengths { get; set; }

        [JsonPropertyName("song")] public virtual string SongName { get; set; }

        [JsonConverter(typeof(ListInterfaceConverter<LegacySection, ISection>))]
        [JsonPropertyName("notes")] public virtual List<ISection> Sections { get; set; }

        [JsonPropertyName("bpm")] public virtual double Bpm { get; set; }

        [JsonPropertyName("sections")] public virtual int NumSections { get; set; }

        public static ISong GetSong(string json)
        {
            LegacyTrack track = LegacyTrack.GetTrack(json);
            return track.Song;
        }

        public static ISong GetSongFromFile(string filePath, Assembly assembly = null) => GetSong(
            PathHelper.Json.GetEmbeddedJson(PathHelper.EmbeddedResource.SanitizeForEmbeds(filePath, assembly),
                assembly));
    }
}