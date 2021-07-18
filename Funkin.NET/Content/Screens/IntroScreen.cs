using System;
using System.Timers;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Screens;

namespace Funkin.NET.Content.Screens
{
    public class IntroScreen : Screen, IBackgroundDependencyLoadable
    {
        public DrawableTrack MenuMusic { get; private set; }

        [Resolved] private AudioManager Audio { get; set; }

        private float _audioLevel;
        private Timer _audioLevelTimer;

        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            MenuMusic = new DrawableTrack(Audio.Tracks.Get(@"Main/FreakyMenu.ogg"));
            MenuMusic.Stop();
            MenuMusic.Looping = true;
            MenuMusic.Start();
            MenuMusic.VolumeTo(_audioLevel);

            _audioLevelTimer = new Timer(100D) {AutoReset = true};
            _audioLevelTimer.Elapsed += (_, _) =>
            {
                if (_audioLevel < 1f)
                    _audioLevel += 0.0025f;
            };
            _audioLevelTimer.Start();
        }

        protected override void Update()
        {
            base.Update();

            MenuMusic.VolumeTo(_audioLevel);
        }

        #endregion
    }
}