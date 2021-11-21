using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osuTK;

namespace Funkin.NET.Graphics.Cursor
{
    public class BasicCursor : CursorContainer
    {
        public new Cursor? ActiveCursor;

        protected override Drawable CreateCursor() => ActiveCursor = new Cursor();

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (ActiveCursor is null)
                return base.OnMouseDown(e);

            ActiveCursor.Scale = new Vector2(1f);
            ActiveCursor.ScaleTo(0.9f, 800D, Easing.OutQuint);
            ActiveCursor.FadeTo(0.8f, 800D, Easing.OutQuint);
            ActiveCursor.SetState(true);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (!e.HasAnyButtonPressed && ActiveCursor is not null)
            {
                ActiveCursor.FadeTo(1f, 500D, Easing.OutQuint);
                ActiveCursor.ScaleTo(1f, 500D, Easing.OutElastic);
                ActiveCursor.SetState(false);
            }

            base.OnMouseUp(e);
        }

        protected override void PopIn()
        {
            base.PopIn();

            if (ActiveCursor is null)
                return;

            ActiveCursor.FadeTo(1f, 250D, Easing.OutQuint);
            ActiveCursor.ScaleTo(1f, 400D, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            if (ActiveCursor is null)
                return;

            ActiveCursor.FadeTo(0f, 250D, Easing.OutQuint);
            ActiveCursor.ScaleTo(0.6f, 250D, Easing.In);
        }
    }
}