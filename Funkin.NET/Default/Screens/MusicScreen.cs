using System;
using Funkin.NET.Intermediary.Screens;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Audio;
using osu.Framework.Screens;

namespace Funkin.NET.Default.Screens
{
    /// <summary>
    ///     Basic Music-playing screen. Allows for conductor integration.
    /// </summary>
    public abstract class MusicScreen : DefaultScreen
    {
        public Bindable<double> MusicBpm { get; }

        public virtual DrawableTrack Music { get; protected set; }

        public new FunkinGame Game => (FunkinGame) base.Game;

        public double CurrentBeat;
        public double CurrentStep;

        protected MusicScreen()
        {
            MusicBpm = new Bindable<double>();
        }

        protected override void Update()
        {
            base.Update();

            double oldStep = CurrentStep;

            UpdateCurrentStep();
            UpdateBeat();

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (oldStep != CurrentStep && CurrentStep > 0D)
                StepHit();
        }

        protected virtual void UpdateCurrentStep()
        {
            if (Music is null || !Music.IsRunning)
                return;

            /*Conductor.CurrentSongPosition = Music.CurrentTime;

            IBpmChange lastChange = new BpmChange(0, 0D, 0D);

            foreach (IBpmChange bpmChange in Conductor.ReadonlyChangeCollection.Where(bpmChange =>
                Conductor.CurrentSongPosition >= bpmChange.SongTime))
                lastChange = bpmChange;

            double crochet = (Conductor.CurrentSongPosition - lastChange.SongTime) / Conductor.StepCrochet;
            double flooredCrochet = Math.Floor(crochet);

            CurrentStep = lastChange.StepTime + flooredCrochet;*/
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

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            Music.Reset();
        }
    }
}