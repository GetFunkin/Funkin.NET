using Funkin.NET.Common.Configuration;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;

namespace Funkin.NET.osuImpl.Graphics.Cursor
{
    /// <summary>
    ///     See: osu!'s MenuCursorContainer.
    /// </summary>
    public class DefaultCursor : CursorContainer
    {
        public Cursor ActiveFunkinCursor { get; protected set; }

        protected override Drawable CreateCursor() => ActiveFunkinCursor = new Cursor();

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            ActiveFunkinCursor.Scale = new Vector2(1f);
            ActiveFunkinCursor.ScaleTo(0.90f, 800D, Easing.OutQuint);
            ActiveFunkinCursor.FadeTo(0.8f, 800D, Easing.OutQuint);
            ActiveFunkinCursor.SetState(true);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (!e.HasAnyButtonPressed)
            {
                ActiveFunkinCursor.FadeTo(1f, 500D, Easing.OutQuint);
                ActiveFunkinCursor.ScaleTo(1f, 500D, Easing.OutElastic);
                ActiveFunkinCursor.SetState(false);
            }

            base.OnMouseUp(e);
        }

        protected override void PopIn()
        {
            base.PopIn();

            ActiveFunkinCursor.FadeTo(1f, 250D, Easing.OutQuint);
            ActiveFunkinCursor.ScaleTo(1f, 400D, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            ActiveFunkinCursor.FadeTo(0f, 250D, Easing.OutQuint);
            ActiveFunkinCursor.ScaleTo(0.6f, 250D, Easing.In);
        }

        public class Cursor : Container
        {
            public const float BaseScale = 0.85f;

            public Container CursorContainer { get; protected set; }

            public Bindable<float> CursorScale { get; protected set; }

            public Sprite CursorDown { get; protected set; }

            public TextureAnimation CursorAnimation { get; protected set; }

            public bool MouseState { get; protected set; }

            public Cursor()
            {
                AutoSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            private void Load(FunkinConfigManager config, TextureStore textures)
            {
                CursorDown = new Sprite
                {
                    Texture = textures.Get("Cursor/arrow click"),
                    AlwaysPresent = true
                };

                CursorAnimation = new TextureAnimation
                {
                    AlwaysPresent = true,
                    IsPlaying = true,
                    Loop = true
                };

                for (int i = 0; i < 6; i++)
                    CursorAnimation.AddFrame(textures.Get($"Cursor/arrow jiggle{i}"), 1D / 24D * 1000D);

                Children = new Drawable[]
                {
                    CursorContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            CursorDown,
                            CursorAnimation
                        }
                    }
                };

                CursorScale = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.CursorSize);
                CursorScale.BindValueChanged(x => CursorContainer.Scale = new Vector2(x.NewValue * BaseScale), true);
            }

            protected override void Update()
            {
                base.Update();

                if (MouseState)
                {
                    CursorDown.Alpha = Alpha;
                    CursorAnimation.Alpha = 0;
                }
                else
                {
                    CursorDown.Alpha = 0;
                    CursorAnimation.Alpha = Alpha;
                }
            }

            public virtual void SetState(bool heldDown)
            {
                MouseState = heldDown;
            }
        }
    }
}