using System.ComponentModel;

namespace Funkin.NET.Content.osu.Configuration
{
    /// <summary>
    ///     osu!'s ScalingMode.
    /// </summary>
    public enum ScalingMode
    {
        Off,
        Everything,

        [Description("Excluding overlays")]
        ExcludeOverlays,
        Gameplay,
    }
}