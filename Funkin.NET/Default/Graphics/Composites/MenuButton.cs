using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace Funkin.NET.Default.Graphics.Composites
{
    /// <summary>
    ///     Simple button class that allows for beat hit-pulsing and hovering.
    /// </summary>
    public class MenuButton : ClickableContainer
    {
        public Sprite ButtonGraphic { get; }

        public float TargetScale = 1f;
        protected bool OverrideTargetScale;

        public MenuButton(Sprite buttonGraphic)
        {
            ButtonGraphic = buttonGraphic;
            Enabled.Value = true;

            // ReSharper disable once VirtualMemberCallInConstructor
            AddInternal(buttonGraphic);
        }

        protected override void Update()
        {
            base.Update();

            Width = ButtonGraphic.Width;
            Height = ButtonGraphic.Height;

        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Alpha < 1f || ButtonGraphic.Alpha < 1f)
                return false;

            OverrideTargetScale = true;
            ButtonGraphic.ScaleTo(TargetScale + 0.15f, 650D, Easing.OutBounce);
            return true;

        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (Alpha < 1f || ButtonGraphic.Alpha < 1f)
                return;

            ButtonGraphic.ScaleTo(1f, 300D, Easing.OutBounce);

            Scheduler.AddDelayed(() => { OverrideTargetScale = false; }, 300D);
        }

        public virtual void BeatHit()
        {
            if (Alpha < 1f || ButtonGraphic.Alpha < 1f)
                return;

            TargetScale = 1.25f;

            if (!OverrideTargetScale)
                ButtonGraphic.ScaleTo(TargetScale, 100D);

            Scheduler.AddDelayed(() =>
            {
                if (!OverrideTargetScale)
                    ButtonGraphic.ScaleTo(1f, 250D);
            }, 100D);
        }

        protected override bool OnClick(ClickEvent e) => Alpha > 0f && ButtonGraphic.Alpha > 0f && base.OnClick(e);
    }
}