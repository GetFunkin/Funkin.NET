using Funkin.NET.Configuration;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
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
            _activeCursor.AdditiveLayer.Alpha = 0;
            _activeCursor.AdditiveLayer.FadeInFromZero(800D, Easing.OutQuint);

            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (!e.HasAnyButtonPressed)
            {
                _activeCursor.AdditiveLayer.FadeOutFromOne(500D, Easing.OutQuint);
                _activeCursor.ScaleTo(1f, 500D, Easing.OutElastic);
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

            public const float BaseScale = 0.15f;

            public Sprite AdditiveLayer;

            public Cursor()
            {
                AutoSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            [UsedImplicitly]
            private void Load(FunkinConfigManager config, TextureStore textures)
            {
                Children = new Drawable[]
                {
                    _cursorContainer = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new Sprite
                            {
                                Texture = textures.Get("Cursor/osu-cursor")
                            },
                            AdditiveLayer = new Sprite
                            {
                                Blending = BlendingParameters.Additive,
                                Colour = Colour4.Purple,
                                Alpha = 0,
                                Texture = textures.Get("Cursor/osu-cursor-additive")
                            }
                        }
                    }
                };

                _cursorScale = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.CursorSize);
                _cursorScale.BindValueChanged(x => _cursorContainer.Scale = new Vector2(x.NewValue * BaseScale), true);
            }
        }
    }
}