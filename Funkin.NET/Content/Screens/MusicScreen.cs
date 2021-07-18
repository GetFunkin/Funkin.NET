using System;
using System.Linq;
using Funkin.NET.Content.Conductor;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Audio;
using osu.Framework.Screens;

namespace Funkin.NET.Content.Screens
{
    /// <summary>
    ///     Music-playing screen.
    /// </summary>
    public abstract class MusicScreen : Screen
    {
        public abstract double ExpectedBpm { get; }

        public virtual DrawableTrack Music { get; protected set; }

        public double CurrentBeat;
        public double CurrentStep;

        [Resolved] private AudioManager Audio { get; set; }

        protected AudioManager AudioManager => Audio;

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            MusicConductor.Bpm = ExpectedBpm;
        }

        protected override void Update()
        {
            base.Update();

            double oldStep = CurrentStep;

            UpdateCurrentStep();
            UpdateBeat();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldStep != CurrentStep && CurrentStep > 0)
                StepHit();
        }

        protected virtual void UpdateCurrentStep()
        {
            MusicConductor.SongPosition = Music.CurrentTime;

            BpmChange lastChange = new(0, 0D, 0D);

            foreach (BpmChange bpmChange in MusicConductor.BpmChangeMap.Where(bpmChange =>
                MusicConductor.SongPosition >= bpmChange.SongTime))
                lastChange = bpmChange;

            double flooredCrochet =
                Math.Floor((MusicConductor.SongPosition - lastChange.SongTime) / MusicConductor.StepCrochet);

            CurrentStep = lastChange.StepTime + flooredCrochet;
        }

        protected virtual void UpdateBeat() => CurrentBeat = Math.Floor(CurrentStep / 4D);

        protected virtual void StepHit()
        {
            if (CurrentStep % 4 == 0)
                BeatHit();
        }

        protected virtual void BeatHit()
        {
        }
    }
}