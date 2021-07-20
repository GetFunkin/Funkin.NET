using System;
using System.Collections.Generic;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Input.Bindings.SelectionKey;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Screens;
using osuTK;

namespace Funkin.NET.Screens
{
    public class FunnyTextScreen : MusicScreen, IKeyBindingHandler<SelectionKeyAction>
    {
        public enum TextDisplayType
        {
            Intro,
            Exit
        }

        public override double ExpectedBpm => 102D;

        public virtual TextDisplayType DisplayType { get; }

        public double LastUpdatedTime;
        public readonly List<string> AddedText = new();
        public bool IsQuirkyIntroFinished;
        public bool IsEnterPromptInitialized;
        public GirlfriendDanceTitle GirlfriendAnimation;
        public LogoTitle LogoAnimation;
        public Box ScreenFlashBang;
        public bool HasBangCycled;
        public bool IsEntering;
        public double TimeOnEntering = TimeSpan.Zero.Milliseconds;
        public bool WasIntroFinishedThisCycle;

        public FunnyTextScreen(TextDisplayType displayType)
        {
            DisplayType = displayType;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            InitializeInternals();
        }

        public void InitializeInternals()
        {
            AddInternal(new SelectionKeyBindingContainer(Game));
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

            if (!IsEnterPromptInitialized && IsQuirkyIntroFinished)
                InitializeEnterState();
        }

        protected void UpdateChecks()
        {
            if (DisplayType == TextDisplayType.Exit)
                return;

            WasIntroFinishedThisCycle = false;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (IsEntering && TimeOnEntering == TimeSpan.Zero.Milliseconds)
                TimeOnEntering = Clock.CurrentTime;

            if (IsEntering && ScreenFlashBang?.Alpha >= 1f)
            {
                // TODO: fade-out music
                this.Push(new SimpleKeyScreen());

                Music.Stop();
            }
        }

        protected void UpdateMenuSongVolume()
        {
            TimeSpan time = TimeSpan.FromMilliseconds(Clock.CurrentTime - LastUpdatedTime);

            if (!(Music.Volume.Value < 1D) || time.Milliseconds < 100D)
                return;

            LastUpdatedTime = Clock.CurrentTime;
            Music.Volume.Value += 0.0025D;
        }

        protected void AddText(string text, float positionOffset)
        {
            if (AddedText.Contains(text))
                return;

            AddedText.Add(text);

            FontUsage fontUsage = new("Funkin", 40f);

            SpriteText spriteText = new()
            {
                Anchor = Anchor.Centre,
                RelativeAnchorPosition = Size / 2f,
                Text = text,
                Font = fontUsage,
                Position = new Vector2(0f, positionOffset),
                Origin = Anchor.Centre
            };

            AddInternal(spriteText);
        }

        protected void Clear()
        {
            AddedText.Clear();
            ClearInternal();
            InitializeInternals();
        }

        protected void UpdateTextDisplay()
        {
            switch (DisplayType)
            {
                case TextDisplayType.Intro:
                    switch (CurrentBeat)
                    {
                        case 1D:
                            AddText("TOMAT", -80f);
                            break;

                        case 3D:
                            AddText("PRESENTS", -40f);
                            break;

                        case 4D:
                            Clear();
                            break;

                        case 5D:
                            AddText("UNASSOCIATED WITH", -80f);
                            break;

                        case 7D:
                            AddText("NEWGROUNDS", -40f);
                            break;

                        case 8D:
                            Clear();
                            break;

                        case 9D:
                            AddText(FunkinGame.FunnyText[0].ToUpper(), -80f);
                            break;

                        case 11D:
                            AddText(FunkinGame.FunnyText[1].ToUpper(), -40f);
                            break;

                        case 12D:
                            Clear();
                            break;

                        case 13D:
                            AddText("FRIDAY", -80f);
                            break;

                        case 14D:
                            AddText("NIGHT", -40f);
                            break;

                        case 15D:
                            AddText("FUNKIN'", 0f);
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
                            AddText("THANKS FOR", -80f);
                            break;

                        case 3D:
                            AddText("PLAYING", -40f);
                            break;

                        case 4D:
                            Clear();
                            break;

                        case 5D:
                            AddText("SEE YOU", -80f);
                            break;

                        case 7D:
                            AddText("LATER", -40f);
                            break;

                        case 8D:
                            Clear();
                            break;

                        case 9D:
                            AddText(FunkinGame.FunnyText[0].ToUpper(), -80f);
                            break;

                        case 11D:
                            AddText(FunkinGame.FunnyText[1].ToUpper(), -40f);
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
                if (IsEntering)
                    rotOffset = (Clock.CurrentTime - TimeOnEntering) / 100D * 25D;

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
                if (HasBangCycled && !IsEntering)
                    return;

                switch (drawable.Alpha)
                {
                    case >= 1f when !IsEntering:
                        drawable.FadeOutFromOne(4000D);
                        HasBangCycled = true;
                        break;

                    case >= 1f when IsEntering:
                        drawable.FadeOutFromOne(8000D);
                        break;

                    case <= 0f:
                        drawable.FadeInFromZero(2000D);
                        break;
                }
            };

            AddInternal(ScreenFlashBang);
        }

        public bool OnPressed(SelectionKeyAction action) => false;

        public void OnReleased(SelectionKeyAction action)
        {
            switch (DisplayType)
            {
                case TextDisplayType.Intro:
                    switch (IsQuirkyIntroFinished)
                    {
                        case false:
                            ClearInternal();
                            IsEntering = false;
                            IsQuirkyIntroFinished = true;
                            WasIntroFinishedThisCycle = true;
                            return;

                        case true when !WasIntroFinishedThisCycle:
                            IsEntering = true;
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
        [UsedImplicitly]
        private void Load(AudioManager audio)
        {
            Music = new DrawableTrack(audio.Tracks.Get(@"Main/FreakyMenu.ogg"));
            Music.Stop();
            Music.Looping = true;
            Music.Start();
            Music.VolumeTo(0D);

            if (DisplayType == TextDisplayType.Exit)
                return;

            GirlfriendAnimation = new GirlfriendDanceTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(80f, 220f),
                Origin = Anchor.Centre,
                Scale = new Vector2(1.5f),
                AlwaysPresent = true,
                Alpha = 0f
            };

            LogoAnimation = new LogoTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(-680f, 190f),
                Origin = Anchor.Centre,
                Scale = new Vector2(1.5f),
                AlwaysPresent = true,
                Alpha = 0f
            };
        }
    }
}