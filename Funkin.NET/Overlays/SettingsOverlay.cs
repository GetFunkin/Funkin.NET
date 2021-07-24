using System.Collections.Generic;
using System.Linq;
using Funkin.NET.Overlays.Settings;
using Funkin.NET.Overlays.Settings.Sections;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK.Graphics;

namespace Funkin.NET.Overlays
{
    /// <summary>
    ///     See: osu!'s SettingsOverlay.
    /// </summary>
    public class SettingsOverlay : SettingsPanel
    {
        protected override IEnumerable<SettingsSection> CreateSections() => new SettingsSection[]
        {
            new InputSection(CreateSubPanel(new KeyBindingPanel()))
        };

        private readonly List<SettingsSubPanel> _subPanels = new();

        protected override Drawable CreateHeader() => new SettingsHeader("Settings", "lmao stole this menu from osu!");

        protected override Drawable CreateFooter() => new SettingsFooter();

        public SettingsOverlay()
            : base(true)
        {
        }

        public override bool AcceptsFocus => _subPanels.All(s => s.State.Value != Visibility.Visible);

        private T CreateSubPanel<T>(T subPanel)
            where T : SettingsSubPanel
        {
            subPanel.Depth = 1;
            subPanel.Anchor = Anchor.TopRight;
            subPanel.State.ValueChanged += SubPanelStateChanged;

            _subPanels.Add(subPanel);

            return subPanel;
        }

        private void SubPanelStateChanged(ValueChangedEvent<Visibility> state)
        {
            switch (state.NewValue)
            {
                case Visibility.Visible:
                    Sidebar?.FadeColour(Color4.DarkGray, 300, Easing.OutQuint);

                    SectionsContainer.FadeOut(300, Easing.OutQuint);
                    ContentContainer.MoveToX(-Width, 500, Easing.OutQuint);
                    break;

                case Visibility.Hidden:
                    Sidebar?.FadeColour(Color4.White, 300, Easing.OutQuint);

                    SectionsContainer.FadeIn(500, Easing.OutQuint);
                    ContentContainer.MoveToX(0, 500, Easing.OutQuint);
                    break;
            }
        }

        protected override float ExpandedPosition =>
            _subPanels.Any(s => s.State.Value == Visibility.Visible) ? -Width : base.ExpandedPosition;

        [BackgroundDependencyLoader]
        private void Load()
        {
            foreach (SettingsSubPanel s in _subPanels)
                ContentContainer.Add(s);
        }
    }
}