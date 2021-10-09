using System.Collections.Generic;
using osu.Framework.Graphics.Containers;

namespace Funkin.NET.Common.Utilities
{
    public interface IOverlayContainer
    {
        List<OverlayContainer> VisibleBlockingOverlays { get; }

        void UpdateBlockingOverlayFade();

        public void AddBlockingOverlay(OverlayContainer overlay);

        public void RemoveBlockingOverlay(OverlayContainer overlay);
    }
}