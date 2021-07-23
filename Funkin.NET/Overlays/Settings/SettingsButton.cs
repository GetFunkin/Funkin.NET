using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Graphics.UserInterface;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays.Settings
{
    public class SettingsButton : FunkinButton, IHasTooltip
    {
        public SettingsButton()
        {
            RelativeSizeAxes = Axes.X;
            Padding = new MarginPadding {Left = SettingsPanel.ContentMargins, Right = SettingsPanel.ContentMargins};
        }

        public LocalisableString TooltipText { get; set; }

        public override IEnumerable<string> FilterTerms => TooltipText != default
            ? base.FilterTerms.Append(TooltipText.ToString())
            : base.FilterTerms;
    }
}