using osu.Framework.Bindables;

namespace Funkin.NET.Core.Music.Songs.Beatmaps
{
    public abstract class NoteControlPoint
    {
        public readonly Bindable<double> TimeBindable = new BindableDouble();

        public virtual double Time
        {
            get => TimeBindable.Value;
            set => TimeBindable.Value = value;
        }

        public abstract void OnHit(IBeatmap beatmap);
    }
}