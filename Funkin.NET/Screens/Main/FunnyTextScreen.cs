using System;
using Funkin.NET.Graphics;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Input.Bindings;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Transforms;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET.Screens.Main
{
    public class FunnyTextScreen : MusicScreen, IKeyBindingHandler<UniversalAction>
    {
        public enum TextDisplayType
        {
            Intro,
            Exit
        }

        public override double ExpectedBpm => 102D;

        public virtual TextDisplayType DisplayType { get; }

        public double LastUpdatedTime;
        public bool IsQuirkyIntroFinished;
        public bool IsEnterPromptInitialized;
        public GirlfriendDanceTitle GirlfriendAnimation;
        public LogoTitle LogoAnimation;
        public Box ScreenFlashBang;
        public bool HasBangCycled;
        public bool PressedEnter;
        public double TimeOnEntering = TimeSpan.Zero.Milliseconds;
        public bool WasIntroFinishedThisCycle;
        public SpriteText[] Text = new SpriteText[3];
        public DrawableSample ConfirmSample;
        public bool DoNotPlayTheSoundAgainAaa;
        public bool HaveWeActuallyEnteredTheCoolMenu;
        public bool SwagInitialized;
        public bool ScheduledEnterText;
        public MenuButton[] Buttons = new MenuButton[3];
        public bool RevealedButtons;

        public FunnyTextScreen(TextDisplayType displayType)
        {
            DisplayType = displayType;
        }

        protected override void Update()
        {
            base.Update();

            UpdateChecks();
            UpdateMenuSongVolume();

            if (!IsQuirkyIntroFinished || DisplayType == TextDisplayType.Exit)
                UpdateTextDisplay();

            if (DisplayType == TextDisplayType.Exit)
                return;

            if (HaveWeActuallyEnteredTheCoolMenu)
            {
                DoCoolMenuStuff();
                return;
            }

            if (!IsEnterPromptInitialized && IsQuirkyIntroFinished)
                InitializeEnterState();
        }

        protected void UpdateChecks()
        {
            if (DisplayType == TextDisplayType.Exit)
                return;

            WasIntroFinishedThisCycle = false;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (PressedEnter && TimeOnEntering == TimeSpan.Zero.Milliseconds)
                TimeOnEntering = Clock.CurrentTime;

            if (PressedEnter /*&& ScreenFlashBang?.Alpha >= 1f*/)
            {
                // this.Push(new SimpleKeyScreen(RootTrack.GetTrackFromFile("Json/Songs/bopeebo/bopeebo.json").Song));

                // Music.Stop();
                HaveWeActuallyEnteredTheCoolMenu = true;
            }
        }

        protected void DoCoolMenuStuff()
        {
            if (SwagInitialized)
                return;

            SwagInitialized = true;

            const float scale = 0.8f;

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

                    Buttons[i].ButtonGraphic.Show();
                    Buttons[i].Delay(delay, sprite =>
                    {
                        sprite.MoveTo(gotoPosition, 500D, Easing.OutBounce);
                        return new TransformSequence<MenuButton>(sprite);
                    });
                    Buttons[i].ButtonGraphic.Delay(delay, sprite =>
                    {
                        sprite.ScaleTo(1f, 500D);
                        return new TransformSequence<Sprite>(sprite);
                    });

                    delay += delayIncrease;
                }
            };
        }

        protected void UpdateMenuSongVolume()
        {
            TimeSpan time = TimeSpan.FromMilliseconds(Clock.CurrentTime - LastUpdatedTime);

            if (!(DisplayType == TextDisplayType.Intro && Music.Volume.Value < 1D ||
                  DisplayType == TextDisplayType.Exit && Music.Volume.Value < 0D) || time.Milliseconds < 100D)
                return;

            LastUpdatedTime = Clock.CurrentTime;
            Music.Volume.Value += DisplayType == TextDisplayType.Intro ? 0.0025D : -0.0025D;
        }

        protected void SetText(string text, int index)
        {
            Text[index].Text = text;
            Text[index].Position = new Vector2(0f, -80f + index * 40f);
        }

        protected void Clear()
        {
            foreach (SpriteText text in Text)
                text.Text = "";
        }

        protected void UpdateTextDisplay()
        {
            switch (DisplayType)
            {
                case TextDisplayType.Intro:
                    switch (CurrentBeat)
                    {
                        case 1D:
                            SetText("TOMAT", 0);
                            break;

                        case 3D:
                            SetText("PRESENTS", 1);
                            break;

                        case 4D:
                            Clear();
                            break;

                        case 5D:
                            SetText("UNASSOCIATED WITH", 0);
                            break;

                        case 7D:
                            SetText("NEWGROUNDS", 1);
                            break;

                        case 8D:
                            Clear();
                            break;

                        case 9D:
                            SetText(FunkinGame.FunnyText[0].ToUpper(), 0);
                            break;

                        case 11D:
                            SetText(FunkinGame.FunnyText[1].ToUpper(), 1);
                            break;

                        case 12D:
                            Clear();
                            break;

                        case 13D:
                            SetText("FRIDAY", 0);
                            break;

                        case 14D:
                            SetText("NIGHT", 1);
                            break;

                        case 15D:
                            SetText("FUNKIN'", 2);
                            break;

                        case 16D:
                            Clear();
                            IsQuirkyIntroFinished = true;
                            WasIntroFinishedThisCycle = true;
                            break;
                    }

                    break;

                case TextDisplayType.Exit:
                    switch (CurrentBeat)
                    {
                        case 1D:
                            SetText("THANKS FOR", 0);
                            break;

                        case 3D:
                            SetText("PLAYING", 1);
                            break;

                        case 4D:
                            Clear();
                            break;

                        case 5D:
                            SetText("SEE YOU", 0);
                            break;

                        case 7D:
                            SetText("LATER", 1);
                            break;

                        case 8D:
                            Clear();
                            break;

                        case 9D:
                            SetText(FunkinGame.FunnyText[0].ToUpper(), 0);
                            break;

                        case 11D:
                            SetText(FunkinGame.FunnyText[1].ToUpper(), 1);
                            break;

                        case 12D:
                            Clear();
                            break;

                        case 13D:
                            Game.Exit();
                            break;
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(DisplayType));
            }
        }

        public void InitializeEnterState()
        {
            IsEnterPromptInitialized = true;

            void Fade(Drawable drawable)
            {
                if (PressedEnter && !ScheduledEnterText)
                {
                    ScheduledEnterText = true;

                    // I am aware that this is a crime again humanity
                    // unfortunately, I live in a world where god is dead
                    drawable.FadeOut().Delay(150D).FadeIn().Delay(150D).FadeOut().Delay(150D).FadeIn().Delay(150D)
                        .FadeOut().Delay(150D).FadeIn().Delay(150D).FadeOut().Delay(150D).FadeIn().Delay(150D)
                        .FadeOut();
                }

                if (PressedEnter)
                    return;

                if (!HasBangCycled)
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
                if (!HasBangCycled)
                    drawable.Alpha = 0f;
                else
                    drawable.Alpha = 1f / 3f;

                double rotOffset = 0D;
                if (PressedEnter)
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
                if (HasBangCycled)
                    return;

                switch (drawable.Alpha)
                {
                    case >= 1f /*when !PressedEnter*/:
                        drawable.FadeOutFromOne(1500D);
                        HasBangCycled = true;
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

            switch (DisplayType)
            {
                case TextDisplayType.Intro:
                    switch (IsQuirkyIntroFinished)
                    {
                        case false:
                            Clear();
                            PressedEnter = false;
                            IsQuirkyIntroFinished = true;
                            WasIntroFinishedThisCycle = true;
                            return;

                        case true when !WasIntroFinishedThisCycle && !DoNotPlayTheSoundAgainAaa:
                            if (ScreenFlashBang != null && ScreenFlashBang.Alpha != 0f)
                                return;

                            DoNotPlayTheSoundAgainAaa = PressedEnter = true;
                            ConfirmSample.Play();
                            break;
                    }

                    break;

                case TextDisplayType.Exit:
                    Game.Exit();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(action));
            }
        }

        protected override void BeatHit()
        {
            base.BeatHit();

            if (DisplayType == TextDisplayType.Exit)
                return;

            GirlfriendAnimation.SwapAnimation();
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio, TextureStore textures, FunkinGame game, Storage storage)
        {
            Music = new DrawableTrack(audio.Tracks.Get(@"Main/FreakyMenu.ogg"));
            Music.Stop();
            Music.Looping = true;
            Music.Start();

            Music.VolumeTo(DisplayType == TextDisplayType.Exit ? 1D : 0D);

            ConfirmSample = new DrawableSample(audio.Samples.Get("Main/ConfirmEnter.ogg"));

            if (DisplayType == TextDisplayType.Exit)
                return;

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

            AddInternal(Text[0] = new SpriteText
            {
                Anchor = Anchor.Centre,
                RelativeAnchorPosition = Size / 2f,
                Font = FunkinFont.Funkin.With(size: 40f),
                Origin = Anchor.Centre
            });

            AddInternal(Text[1] = new SpriteText
            {
                Anchor = Anchor.Centre,
                RelativeAnchorPosition = Size / 2f,
                Font = FunkinFont.Funkin.With(size: 40f),
                Origin = Anchor.Centre
            });

            AddInternal(Text[2] = new SpriteText
            {
                Anchor = Anchor.Centre,
                RelativeAnchorPosition = Size / 2f,
                Font = FunkinFont.Funkin.With(size: 40f),
                Origin = Anchor.Centre
            });

            for (int i = 0; i < Buttons.Length; i++)
            {
                string request = i switch
                {
                    0 => "Title/story mode white",
                    1 => "Title/freeplay white",
                    2 => "Title/options white",
                    _ => ""
                };

                Action action = i switch
                {
                    0 => () =>
                    {
                        this.Push(BaseKeyPlayScreen.GetPlayScreen("Json/Songs/Bopeebo/bopeebo-normal.json",
                            "Songs/Bopeebo/Inst.ogg", "Songs/Bopeebo/Voices.ogg"));
                    },
                    2 => game.Settings.Show,
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
    }
}