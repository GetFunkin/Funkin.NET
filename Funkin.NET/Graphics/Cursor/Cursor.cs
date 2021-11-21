using Funkin.NET.Intermediary.ResourceStores;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Funkin.NET.Graphics.Cursor
{
    public class Cursor : Container
    {
        public const float DefaultScale = 0.15f;

        public Container? Container;
        public Bindable<float>? CursorScale;
        public Sprite? CursorDown;
        public TextureAnimation? CursorAnimation;
        public bool MouseState;

        public Cursor()
        {
            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void Load(SparrowAtlasStore textures)
        {
            CursorDown = new Sprite
            {
                Texture = textures.GetTexture(Textures.General.Cursors.Cursor, "arrow click", 0),
                AlwaysPresent = true
            };

            CursorAnimation = new TextureAnimation
            {
                AlwaysPresent = true,
                IsPlaying = true,
                Loop = true
            };

            for (int i = 0; i < 6; i++)
                CursorAnimation.AddFrame(
                    textures.GetTexture(Textures.General.Cursors.Cursor, "arrow jiggle", i),
                    (1D / 24D) * 1000D
                );

            Children = new Drawable[]
            {
                Container = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        CursorDown,
                        CursorAnimation
                    }
                }
            };
            
            // TODO: Configurable cursor scale.
            CursorScale = new BindableFloat(DefaultScale);
            CursorScale.BindValueChanged(x => Container.Scale = new Vector2(x.NewValue * DefaultScale), true);
        }

        protected override void Update()
        {
            base.Update();

            if (CursorDown is null || CursorAnimation is null)
                return;

            if (MouseState)
            {
                CursorDown.Alpha = Alpha;
                CursorAnimation.Alpha = 0f;
            }
            else
            {
                CursorDown.Alpha = 0f;
                CursorAnimation.Alpha = Alpha;
            }
        }

        public virtual void SetState(bool heldDown) => MouseState = heldDown;
    }
}