using Funkin.NET.Core.Music.Songs.Beatmaps.Accuracy;
using osu.Framework.Bindables;

namespace Funkin.NET.Core.Music.Songs.Beatmaps
{
    public class BeatmapObject
    {
        public readonly Bindable<double> StartTimeBindable = new BindableDouble();
        public readonly Bindable<double> HoldTimeBindable = new BindableDouble();

        public virtual double StartTime
        {
            get => StartTimeBindable.Value;
            set => StartTimeBindable.Value = value;
        }

        public virtual double HoldTime
        {
            get => HoldTimeBindable.Value;
            set => HoldTimeBindable.Value = value;
        }

        public virtual HitWindow HitWindow { get; set; } = new();
    }
}