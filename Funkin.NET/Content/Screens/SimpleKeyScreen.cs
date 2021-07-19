using System;
using Funkin.NET.Common.KeyBinds.ArrowKeys;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Core.BackgroundDependencyLoading;
using Funkin.NET.Core.Models;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osuTK;

namespace Funkin.NET.Content.Screens
{
    public class SimpleKeyScreen : MusicScreen, IBackgroundDependencyLoadable
    {
        public override double ExpectedBpm { get; }

        public Song Song { get; }

        private ArrowKeyDrawable[] _arrows;
        private bool _initialized;
        
        public SimpleKeyScreen()
        {
            Song = Song.GetSongFromFile("Json/bopeebo/bopeebo.json");
            ExpectedBpm = Song.Bpm;
        }

        protected override void Update()
        {
            if (!_initialized)
            {
                _initialized = true;

                foreach (ArrowKeyDrawable drawable in _arrows) AddInternal(drawable);
            }
        }


        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            Music = new DrawableTrack(AudioManager.Tracks.Get(@"Bopeebo/Bopeebo_Inst.ogg"));
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
                    Position = new Vector2(offset, -400),
                    Origin = Anchor.Centre,
                    AlwaysPresent = true,
                    Alpha = 1f
                };

                offset += 170;
            }
        }
    }
}