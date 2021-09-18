using System;
using Funkin.NET.Common.Input;
using Funkin.NET.Common.Screens.Backgrounds;
using Funkin.NET.Common.Services;
using Funkin.NET.Game.Graphics.Composites;
using Funkin.NET.Intermediary.Screens.Backgrounds;
using Funkin.NET.Resources;
using Microsoft.Extensions.DependencyInjection;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Input.Bindings;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET.Game.Screens.Gameplay
{
    public class EnterScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        public double TimeOnEntering = TimeSpan.Zero.Milliseconds;
        public bool FinishedTextIntroduction;
        public bool InitializedEnterVisuals;
        public bool FlashBangComplete;
        public bool SelectedEnter;
        public bool InfoFinishedNow;
        public bool BlockSelectionSound;
        public bool EnteredRealMenu;
        public bool BaseMenuInitialized;
        public bool ScheduledEnterText;
        public bool RevealedButtons;

        public TextFlowContainer FlowingText;
        public MenuButton[] Buttons = new MenuButton[3];
        public GirlfriendDanceTitle GirlfriendAnimation;
        public DrawableSample ConfirmSample;
        public LogoTitle LogoAnimation;
        public Box ScreenFlashBang;

        public string[] ChosenSplashText;

        public EnterScreen()
        {
            MusicBpm.Value = 102D;
        }

        protected override void Update()
        {
            base.Update();

            UpdateChecks();

            if (EnteredRealMenu)
            {
                DoCoolMenuStuff();
                return;
            }

            if (!InitializedEnterVisuals && FinishedTextIntroduction)
                InitializeEnterState();
        }

        protected void UpdateChecks()
        {
            InfoFinishedNow = false;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (SelectedEnter && TimeOnEntering == TimeSpan.Zero.Milliseconds)
                TimeOnEntering = Clock.CurrentTime;

            if (SelectedEnter /*&& ScreenFlashBang?.Alpha >= 1f*/)
            {
                // this.Push(new SimpleKeyScreen(RootTrack.GetTrackFromFile("Json/Songs/bopeebo/bopeebo.json").Song));

                // Music.Stop();
                EnteredRealMenu = true;
            }
        }

        protected void DoCoolMenuStuff()
        {
            if (BaseMenuInitialized)
                return;

            BaseMenuInitialized = true;

            const float scale = 0.95f;

            Vector2 offset =
                new(-(GirlfriendAnimation.LeftAnim.CurrentFrame.DisplayHeight * scale / 2f), GirlfriendAnimation
                    .LeftAnim.CurrentFrame.DisplayWidth * scale / 2f);
            GirlfriendAnimation.MoveTo(offset, 1500D, Easing.OutBounce);
            GirlfriendAnimation.ScaleTo(scale, 1500D, Easing.OutBounce);
            LogoAnimation.FadeOut(1500D, Easing.OutCirc);

            foreach (MenuButton button in Buttons)
                button.Position = new Vector2();

            GirlfriendAnimation.OnUpdate += gf =>
            {
                if (RevealedButtons)
                    return;

                if (gf.Scale != new Vector2(scale))
                    return;

                RevealedButtons = true;

                const double delayIncrease = 250D;
                double delay = delayIncrease;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Vector2 gotoPosition = i switch
                    {
                        0 => new Vector2(400f, -150f),
                        1 => new Vector2(400f, 150f),
                        2 => new Vector2(-400f, 0f),
                        _ => new Vector2()
                    };

                    Buttons[i].Delay(delay, sprite =>
                    {
                        sprite.MoveTo(gotoPosition, 500D, Easing.OutBounce);
                        return new TransformSequence<MenuButton>(sprite);
                    });
                    Buttons[i].ButtonGraphic.Delay(delay, sprite =>
                    {
                        sprite.ScaleTo(1f, 500D);
                        sprite.FadeInFromZero(500D);
                        return new TransformSequence<Sprite>(sprite);
                    });

                    delay += delayIncrease;
                }
            };
        }

        public void InitializeEnterState()
        {
            InitializedEnterVisuals = true;

            void Fade(Drawable drawable)
            {
                if (SelectedEnter && !ScheduledEnterText)
                {
                    ScheduledEnterText = true;

                    TransformSequence<Drawable> sequence = drawable.Delay(0);

                    for (int i = 0; i < 9; i++)
                    {
                        if (i % 2 == 0)
                            sequence.FadeOut();
                        else
                            sequence.FadeIn();

                        sequence.Delay(150D);
                    }
                }

                if (SelectedEnter)
                    return;

                if (!FlashBangComplete)
                {
                    drawable.Alpha = 0f;
                    return;
                }

                switch (drawable.Alpha)
                {
                    case 1f:
                        drawable.FadeOutFromOne(1000D);
                        break;

                    case 0f:
                        drawable.FadeInFromZero(1000D);
                        break;
                }
            }

            void CircularOffset(Drawable drawable)
            {
                if (!FlashBangComplete)
                    drawable.Alpha = 0f;
                else
                    drawable.Alpha = 1f / 3f;

                double rotOffset = 0D;
                if (SelectedEnter)
                    rotOffset = (Clock.CurrentTime - TimeOnEntering) / 100D * 55D;

                int offset = 999;

                if (drawable.Colour == Colour4.Blue)
                    offset = 333;

                if (drawable.Colour == Colour4.Green)
                    offset = 666;

                drawable.Position = new Vector2(
                    (float) ((float) Math.Sin((Clock.CurrentTime + offset) / 200f) * (5f + rotOffset)),
                    (float) ((float) Math.Cos((Clock.CurrentTime + offset) / 200f) * (5f + rotOffset) + 280f));
            }

            void MagicallyAppear(Drawable drawable)
            {
                if (ScreenFlashBang.Alpha >= 1f)
                    drawable.Alpha = 1f; // basically activates 'em
            }

            SpriteText GetEnterText()
            {
                SpriteText text = new()
                {
                    Anchor = Anchor.Centre,
                    RelativeAnchorPosition = Size / 2f,
                    Text = "Press Enter to Begin",
                    Position = new Vector2(0f, 280f),
                    Origin = Anchor.Centre,
                    Font = new FontUsage("VCR", 80f),
                    Alpha = 0f,
                    AlwaysPresent = true
                };

                text.OnUpdate += MagicallyAppear;

                return text;
            }

            SpriteText GetColoredEnterText(Colour4 color)
            {
                SpriteText text = GetEnterText();
                text.Colour = color;
                text.Blending = BlendingParameters.Additive;
                text.OnUpdate += CircularOffset;

                return text;
            }

            SpriteText enterText = GetEnterText();
            SpriteText enterRed = GetColoredEnterText(Colour4.Red);
            SpriteText enterBlue = GetColoredEnterText(Colour4.Blue);
            SpriteText enterGreen = GetColoredEnterText(Colour4.Green);

            enterText.OnUpdate += Fade;
            GirlfriendAnimation.OnUpdate += MagicallyAppear;
            LogoAnimation.OnUpdate += MagicallyAppear;

            AddInternal(enterText);
            AddInternal(enterRed);
            AddInternal(enterBlue);
            AddInternal(enterGreen);
            AddInternal(GirlfriendAnimation);
            AddInternal(LogoAnimation);

            ScreenFlashBang = new Box
            {
                Colour = Colour4.White,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0f,
                AlwaysPresent = true
            };

            ScreenFlashBang.OnUpdate += drawable =>
            {
                if (FlashBangComplete)
                    return;

                switch (drawable.Alpha)
                {
                    case >= 1f /*when !PressedEnter*/:
                        drawable.FadeOutFromOne(1500D);
                        FlashBangComplete = true;
                        break;

                    /*case >= 1f when PressedEnter:
                        drawable.FadeOutFromOne(8000D);
                        break;*/

                    case <= 0f:
                        drawable.FadeInFromZero(1000D);
                        break;
                }
            };

            AddInternal(ScreenFlashBang);
        }

        public bool OnPressed(UniversalAction action) => false;

        public void OnReleased(UniversalAction action)
        {
            if (action != UniversalAction.Select)
                return;

            switch (FinishedTextIntroduction)
            {
                case false:
                    FlowingText.Text = "";
                    SelectedEnter = false;
                    FinishedTextIntroduction = true;
                    InfoFinishedNow = true;
                    return;

                case true when !InfoFinishedNow && !BlockSelectionSound:
                    if (ScreenFlashBang != null && ScreenFlashBang.Alpha != 0f)
                        return;

                    BlockSelectionSound = SelectedEnter = true;
                    ConfirmSample.Play();
                    break;
            }
        }

        protected override void BeatHit()
        {
            base.BeatHit();

            GirlfriendAnimation.SwapAnimation();

            foreach (MenuButton button in Buttons)
                button.BeatHit();

            if (FinishedTextIntroduction)
                return;

            static void WithFont(SpriteText text) => text.Font = new FontUsage("Funkin", 40f);

            switch (CurrentBeat)
            {
                case 1D:
                    FlowingText.AddParagraph("TOMAT", WithFont);
                    break;

                case 3D:
                    FlowingText.AddParagraph("PRESENTS", WithFont);
                    break;

                case 4D:
                    FlowingText.Text = "";
                    break;

                case 5D:
                    FlowingText.AddParagraph("UNASSOCIATED WITH", WithFont);
                    break;

                case 7D:
                    FlowingText.AddParagraph("NEWGROUNDS", WithFont);
                    break;

                case 8D:
                    FlowingText.Text = "";
                    break;

                case 9D:
                    FlowingText.AddParagraph(ChosenSplashText[0].ToUpper(), WithFont);
                    break;

                case 11D:
                    FlowingText.AddParagraph(ChosenSplashText[1].ToUpper(), WithFont);
                    break;

                case 12D:
                    FlowingText.Text = "";
                    break;

                case 13D:
                    FlowingText.AddParagraph("FRIDAY", WithFont);
                    break;

                case 14D:
                    FlowingText.AddParagraph("NIGHT", WithFont);
                    break;

                case 15D:
                    FlowingText.AddParagraph("FUNKIN'", WithFont);
                    break;

                case 16D:
                    FlowingText.Text = "";
                    FinishedTextIntroduction = true;
                    InfoFinishedNow = true;
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio, TextureStore textures, FunkinGame game)
        {
            ChosenSplashText = game.ServiceProvider.GetRequiredService<SplashTextService>().GetSplashText();

            Music = new DrawableTrack(audio.Tracks.Get(@"Main/FreakyMenu.ogg")) {Looping = true};
            Music.VolumeTo(0D);
            Music.VolumeTo(1D, 4000D, Easing.In);
            Music.Start();

            ConfirmSample = new DrawableSample(audio.Samples.Get("Main/ConfirmEnter.ogg"));

            GirlfriendAnimation = new GirlfriendDanceTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(0f, 220f),
                Origin = Anchor.Centre,
                Scale = new Vector2(1.5f),
                AlwaysPresent = true,
                Alpha = 0f
            };

            LogoAnimation = new LogoTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(-630f, 190f),
                Origin = Anchor.Centre,
                Scale = new Vector2(1.5f),
                AlwaysPresent = true,
                Alpha = 0f
            };

            AddInternal(new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    FlowingText = new TextFlowContainer
                    {
                        Width = 680f,
                        AutoSizeAxes = Axes.Y,
                        TextAnchor = Anchor.TopCentre,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Spacing = new Vector2(0, 2)
                    }
                }
            });

            for (int i = 0; i < Buttons.Length; i++)
            {
                string request = i switch
                {
                    0 => PathHelper.Texture.StoryModeButton,
                    1 => PathHelper.Texture.FreeplayButton,
                    2 => PathHelper.Texture.OptionsButton,
                    _ => ""
                };

                Action action = i switch
                {
                    0 => () =>
                    {
                        this.Push(BaseKeyPlayScreen.GetPlayScreen("Json/Songs/Bopeebo/bopeebo-normal.json",
                            "Songs/Bopeebo/Inst.ogg", "Songs/Bopeebo/Voices.ogg"));
                    },
                    2 => game.Containers[FunkinContainers.Settings].Show,
                    _ => null
                };

                Sprite button = new()
                {
                    Texture = textures.Get(request),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeAnchorPosition = Size / 2f,
                    AlwaysPresent = true,
                    Scale = new Vector2(),
                    Alpha = 0f
                };

                Buttons[i] = new MenuButton(button)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeAnchorPosition = Size / 2f,
                    Action = action
                };

                AddInternal(Buttons[i]);
            }
        }

        public override IBackgroundScreen CreateBackground() => new DefaultBackgroundScreen(DefaultBackgroundType.Purple);
    }
}