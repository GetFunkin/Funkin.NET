using System;
using System.Linq;
using Funkin.NET.Conductor;
using osu.Framework.Graphics.Audio;
using osu.Framework.Screens;

namespace Funkin.NET.Screens
{
    /// <summary>
    ///     Music-playing screen.
    /// </summary>
    public abstract class MusicScreen : FunkinScreen
    {
        public abstract double ExpectedBpm { get; }

        public virtual DrawableTrack Music { get; protected set; }

        public double CurrentBeat;
        public double CurrentStep;

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
            if (Music is null || !Music.IsRunning)
                return;

            MusicConductor.SongPosition = Music.CurrentTime;

            IBpmChange lastChange = new BpmChange(0, 0D, 0D);

            foreach (IBpmChange bpmChange in MusicConductor.ReadonlyChangeCollection.Where(bpmChange =>
                MusicConductor.SongPosition >= bpmChange.SongTime))
                lastChange = bpmChange;

            double crochet = (MusicConductor.SongPosition - lastChange.SongTime) / MusicConductor.StepCrochet;
            double flooredCrochet = Math.Floor(crochet);

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