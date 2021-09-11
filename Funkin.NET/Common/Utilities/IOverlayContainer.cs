using System.Collections.Generic;
using Funkin.NET.Intermediary.Utilities;
using osu.Framework.Graphics.Containers;

namespace Funkin.NET.Common.Utilities
{
    public interface IOverlayContainer : IScheduler
    {
        List<OverlayContainer> VisibleBlockingOverlays { get; }

        void UpdateBlockingOverlayFade();

        void AddBlockingOverlay(OverlayContainer overlay)
        {
            if (!VisibleBlockingOverlays.Contains(overlay))
                VisibleBlockingOverlays.Add(overlay);

            UpdateBlockingOverlayFade();
        }

        void RemoveBlockingOverlay(OverlayContainer overlay) => ScheduleTask(() =>
        {
            VisibleBlockingOverlays.Remove(overlay);
            UpdateBlockingOverlayFade();
        });
    }
}