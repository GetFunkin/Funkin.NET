using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace Funkin.NET.Graphics.Sprites
{
    public class MenuButton : ClickableContainer
    {
        public Sprite ButtonGraphic { get; }

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

            ButtonGraphic.ScaleTo(1.15f, 650D, Easing.OutBounce);
            return true;

        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            if (Alpha >= 1f && ButtonGraphic.Alpha >= 1f) 
                ButtonGraphic.ScaleTo(1f, 300D, Easing.OutBounce);
        }

        protected override bool OnClick(ClickEvent e) => Alpha > 0f && ButtonGraphic.Alpha > 0f && base.OnClick(e);
    }
}