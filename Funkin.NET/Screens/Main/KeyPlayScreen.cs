using System.Linq;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Screens.Main
{
    public class KeyPlayScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        // TODO: draw characters
        // TODO: draw keys (player & opponent)
        // TODO: register when keys are hit
        // TODO: score
        // TODO: everything else

        public static readonly UniversalAction[] ArrowValues =
        {
            UniversalAction.Left,
            UniversalAction.Down,
            UniversalAction.Up,
            UniversalAction.Right
        };

        public override double ExpectedBpm { get; }

        public virtual Song Song { get; }

        public virtual string InstrumentalPath { get; }

        public virtual string VocalPath { get; }

        public virtual DrawableTrack VocalTrack { get; protected set; }

        protected ArrowKeyDrawable[] PlayerArrows;
        protected ArrowKeyDrawable[] OpponentArrows;

        public KeyPlayScreen(Song song, string instrumentalPath, string vocalPath)
        {
            Song = song;
            ExpectedBpm = song.Bpm;
            InstrumentalPath = instrumentalPath;
            VocalPath = vocalPath;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Music.Start();
            Music.VolumeTo(1D);

            VocalTrack.Start();
            VocalTrack.VolumeTo(0D);
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            Music = new DrawableTrack(audio.Tracks.Get(InstrumentalPath)) {Looping = true};
            VocalTrack = new DrawableTrack(audio.Tracks.Get(VocalPath)) {Looping = true};
            PlayerArrows = new ArrowKeyDrawable[ArrowValues.Length];
            OpponentArrows = new ArrowKeyDrawable[ArrowValues.Length];

            float offset = 140f;
            for (int i = 0; i < ArrowValues.Length; i++)
            {
                UniversalAction arrowKey = ArrowValues[i];
                PlayerArrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 75f;
            }

            offset = 140f;
            for (int i = ArrowValues.Length - 1; i >= 0; i--)
            {
                UniversalAction arrowKey = ArrowValues[i];
                OpponentArrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(-offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f,

                };

                offset += 75f;
            }

            foreach (ArrowKeyDrawable drawable in PlayerArrows)
                AddInternal(drawable);

            foreach (ArrowKeyDrawable drawable in OpponentArrows)
                AddInternal(drawable);
        }

        public virtual bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return false;

            int value = (int) action;
            if (value >= PlayerArrows.Length) return false;

            PlayerArrows[value].ArrowPressAnim.PlaybackPosition = 0;
            PlayerArrows[value].ArrowPressAnim.Show();
            PlayerArrows[value].ArrowIdleSprite.Hide();

            return true;
        }

        public virtual void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return;

            int value = (int) action;
            if (value >= PlayerArrows.Length) return;

            PlayerArrows[value].ArrowPressAnim.Hide();
            PlayerArrows[value].ArrowIdleSprite.Show();
        }

        public static KeyPlayScreen GetPlayScreen(string json, string instrumental, string voices) =>
            new(Song.GetSongFromFile(json), instrumental, voices);
    }
}