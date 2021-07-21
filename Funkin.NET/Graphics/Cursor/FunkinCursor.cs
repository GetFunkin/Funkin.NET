using Funkin.NET.Configuration;
using JetBrains.Annotations;
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

namespace Funkin.NET.Graphics.Cursor
{
    /// <summary>
    ///     See: osu!'s MenuCursorContainer.
    /// </summary>
    public class FunkinCursor : CursorContainer
    {
        private readonly IBindable<bool> _screenShotCursorVisibility = new Bindable<bool>(true);
        private Cursor _activeCursor;

        public override bool IsPresent => _screenShotCursorVisibility.Value && base.IsPresent;

        protected override Drawable CreateCursor() => _activeCursor = new Cursor();

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            _activeCursor.Scale = new Vector2(1f);
            _activeCursor.ScaleTo(0.90f, 800D, Easing.OutQuint);
            _activeCursor.FadeTo(0.8f, 800D, Easing.OutQuint);
            _activeCursor.SetState(true);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (!e.HasAnyButtonPressed)
            {
                _activeCursor.FadeTo(1f, 500D, Easing.OutQuint);
                _activeCursor.ScaleTo(1f, 500D, Easing.OutElastic);
                _activeCursor.SetState(false);
            }

            base.OnMouseUp(e);
        }

        protected override void PopIn()
        {
            base.PopIn();

            _activeCursor.FadeTo(1f, 250D, Easing.OutQuint);
            _activeCursor.ScaleTo(1f, 400D, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();

            _activeCursor.FadeTo(0f, 250D, Easing.OutQuint);
            _activeCursor.ScaleTo(0.6f, 250D, Easing.In);
        }

        public class Cursor : Container
        {
            private Container _cursorContainer;
            private Bindable<float> _cursorScale;
            private Sprite _cursorDown;
            private TextureAnimation _cursorAnimation;

            public const float BaseScale = 0.85f;

            public Cursor()
            {
                AutoSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            [UsedImplicitly]
            private void Load(FunkinConfigManager config, TextureStore textures)
            {
                _cursorDown = new Sprite
                {
                    Texture = textures.Get("Cursor/arrow click"),
                    AlwaysPresent = true
                };
                _cursorDown.Hide();

                _cursorAnimation = new TextureAnimation
                {
                    AlwaysPresent = true,
                    IsPlaying = true,
                    Loop = true
                };

                for (int i = 0; i < 6; i++)
                    _cursorAnimation.AddFrame(textures.Get($"Cursor/arrow jiggle{i}"), 1D / 24D * 1000D);

                Children = new Drawable[]
                {
                    _cursorContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            _cursorDown,
                            _cursorAnimation
                        }
                    }
                };

                _cursorScale = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.CursorSize);
                _cursorScale.BindValueChanged(x => _cursorContainer.Scale = new Vector2(x.NewValue * BaseScale), true);
            }

            protected override void Update()
            {
                base.Update();

                _cursorAnimation.Alpha = _cursorDown.Alpha = Alpha;
            }

            public void SetState(bool heldDown)
            {
                if (heldDown)
                {
                    _cursorDown.Show();
                    _cursorAnimation.Hide();
                }
                else
                {
                    _cursorDown.Hide();
                    _cursorAnimation.Show();
                    _cursorAnimation.GotoAndPlay(0);
                    _cursorAnimation.Loop = true;
                }
            }
        }
    }
}