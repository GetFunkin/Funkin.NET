using System.Linq;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Input.Bindings;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;
using osuTK;

namespace Funkin.NET.Screens
{
    public class SimpleKeyScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        // TODO: handle input
        // TODO: draw characters
        // TODO: draw enemy keys
        // TODO: draw all song keys

        public override double ExpectedBpm { get; }

        public Song Song { get; }

        private ArrowKeyDrawable[] _arrows;
        private bool _initialized;

        public readonly UniversalAction[] ArrowValues =
        {
            UniversalAction.Down, 
            UniversalAction.Left, 
            UniversalAction.Right, 
            UniversalAction.Up
        };

        public SimpleKeyScreen(Song song)
        {
            Song = song;
            ExpectedBpm = Song.Bpm;
        }
        
        protected override void Update()
        {
            if (_initialized) return;

            _initialized = true;

            foreach (ArrowKeyDrawable drawable in _arrows)
                AddInternal(drawable);
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio, Storage storage)
        {
            Music = new DrawableTrack(audio.Tracks.Get(@"Songs/Bopeebo/Bopeebo_Inst.ogg"));
            Music.Stop();
            Music.Looping = true;
            Music.Start();
            Music.VolumeTo(1D);

            _arrows = new ArrowKeyDrawable[ArrowValues.Length];

            int offset = 20;

            for (int i = 0; i < ArrowValues.Length; i++)
            {
                UniversalAction arrowKey = ArrowValues[i];
                _arrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 80;
            }
        }

        public bool OnPressed(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return false;

            int value = (int) action;
            if (value >= _arrows.Length) return false;

            _arrows[value].ArrowPressAnim.PlaybackPosition = 0;
            _arrows[value].ArrowPressAnim.Show();

            return true;
        }

        public void OnReleased(UniversalAction action)
        {
            if (!ArrowValues.Contains(action))
                return;

            int value = (int) action;
            if (value >= _arrows.Length) return;

            _arrows[value].ArrowPressAnim.Hide();
        }
    }
}