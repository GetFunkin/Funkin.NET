using Funkin.NET.Common.Screens;
using Funkin.NET.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET.Game.Screens.Gameplay
{
    public class StartupIntroductionScreen : DefaultScreen
    {
        public DefaultScreen NextScreen { get; }

        public TextFlowContainer TextFlow { get; protected set; }

        public FillFlowContainer FillContainer { get; protected set; }

        public Sprite FloatingHead { get; protected set; }

        public override bool CursorVisible => false;

        public StartupIntroductionScreen(DefaultScreen nextScreen)
        {
            NextScreen = nextScreen;
        }

        [BackgroundDependencyLoader]
        private void Load(TextureStore textures)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Colour4.Black,
                    RelativeSizeAxes = Axes.Both
                },

                FloatingHead = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = -50f,
                    Blending = BlendingParameters.Additive
                },

                FillContainer = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopCentre,
                    Children = new Drawable[]
                    {
                        TextFlow = new TextFlowContainer
                        {
                            Width = 680,
                            AutoSizeAxes = Axes.Y,
                            TextAnchor = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Spacing = new Vector2(0, 2)
                        }
                    }
                }
            };

            FloatingHead.Texture = textures.Get(PathHelper.GetTexture("Icons/bf-alive", includeTextures: false));

            static void Format(SpriteText text) => text.Font = new FontUsage("Torus-Regular", 30f);
            static void Bold(SpriteText text) => text.Font = new FontUsage("Torus-Bold", 30f);

            static void AddLine(TextFlowContainer container, string text)
            {
                container.AddText(text, Format);
                container.NewLine();
            }

            AddLine(TextFlow, "Funkin.NET");
            AddLine(TextFlow, "an unfaithful");
            AddLine(TextFlow, "C# FNF port");

            TextFlow.AddText("made with ", Bold);
            TextFlow.AddText("", x => x.Font = new FontUsage("FontAwesome-Solid", 30f));
            TextFlow.AddText(" by tomat", Bold);
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            double delay = 500D;

            foreach (Drawable drawable in TextFlow.Children)
                drawable.FadeTo(0.001f).Delay(delay += 20).FadeIn(500D);

            this.FadeInFromZero(500D).Then(5500D).FadeOut(250D).ScaleTo(0.9f, 250, Easing.InQuint).Finally(_ =>
            {
                this.Push(NextScreen);
            });
        }
    }
}