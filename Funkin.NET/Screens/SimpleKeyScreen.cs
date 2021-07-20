using System;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Input.Bindings.ArrowKeys;
using Funkin.NET.Songs;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Screens
{
    public class SimpleKeyScreen : MusicScreen, IKeyBindingHandler<ArrowKeyAction>
    {
        // TODO: handle input
        // TODO: draw characters
        // TODO: draw enemy keys
        // TODO: draw all song keys

        public override double ExpectedBpm { get; }

        public Song Song { get; }

        private ArrowKeyDrawable[] _arrows;
        private bool _initialized;

        public SimpleKeyScreen(Song song)
        {
            Song = song;
            ExpectedBpm = Song.Bpm;
        }

        protected override void LoadComplete()
        {
            AddInternal(new ArrowKeyBindingContainer(Game));
        }

        protected override void Update()
        {
            if (_initialized) return;

            _initialized = true;

            foreach (ArrowKeyDrawable drawable in _arrows)
                AddInternal(drawable);
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            Music = new DrawableTrack(audio.Tracks.Get(@"Songs/Bopeebo/Bopeebo_Inst.ogg"));
            Music.Stop();
            Music.Looping = true;
            Music.Start();
            Music.VolumeTo(1D);

            ArrowKeyAction[] arrowValues = Enum.GetValues<ArrowKeyAction>();
            _arrows = new ArrowKeyDrawable[arrowValues.Length];

            int offset = 20;

            for (int i = 0; i < arrowValues.Length; i++)
            {
                ArrowKeyAction arrowKey = arrowValues[i];
                _arrows[i] = new ArrowKeyDrawable(arrowKey)
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(offset, -200),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 170;
            }
        }

        public bool OnPressed(ArrowKeyAction action)
        {
            int value = (int) action;
            if (value >= _arrows.Length) return false;

            _arrows[value].ArrowPressAnim.PlaybackPosition = 0;
            _arrows[value].ArrowPressAnim.Show();

            return true;
        }

        public void OnReleased(ArrowKeyAction action)
        {
            int value = (int) action;
            if (value >= _arrows.Length) return;

            _arrows[value].ArrowPressAnim.Hide();
        }
    }
}