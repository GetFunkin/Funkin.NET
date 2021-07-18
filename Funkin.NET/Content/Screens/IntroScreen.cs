using System;
using System.Timers;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Audio;
using osu.Framework.Screens;

namespace Funkin.NET.Content.Screens
{
    public class IntroScreen : Screen, IBackgroundDependencyLoadable
    {
        public DrawableTrack MenuMusic { get; private set; }

        [Resolved] private AudioManager Audio { get; set; }

        private float _audioLevel;
        private double _lastUpdatedTime;

        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            MenuMusic = new DrawableTrack(Audio.Tracks.Get(@"Main/FreakyMenu.ogg"));
            MenuMusic.Stop();
            MenuMusic.Looping = true;
            MenuMusic.Start();
            MenuMusic.VolumeTo(_audioLevel);
        }

        protected override void Update()
        {
            base.Update();

            TimeSpan time = TimeSpan.FromMilliseconds(Clock.CurrentTime - _lastUpdatedTime);

            if (!(MenuMusic.Volume.Value < 1D) || time.Milliseconds < 100) 
                return;

            _lastUpdatedTime = Clock.CurrentTime;
            MenuMusic.Volume.Value += 0.0025D;
        }

        #endregion
    }
}