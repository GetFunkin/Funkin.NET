using System;

namespace Funkin.NET.Intermediary.Screens.Backgrounds
{
    public interface IBackgroundProvider
    {
        IBackgroundScreen OwnedBackground { get; }

        IBackgroundScreen Background { get; }

        BackgroundScreenStack BackgroundStack { get; }

        public void ApplyToBackground(Action<IBackgroundScreen> action)
        {
            if (BackgroundStack == null)
                throw new InvalidOperationException(
                    "Attempted to apply to background without a background stack being available.");

            if (Background == null)
                throw new InvalidOperationException("Attempted to apply to background before screen is pushed.");

            Background.ApplyToBackground(action);
        }

        IBackgroundScreen CreateBackground();
    }
}