using System;
using System.Collections.Generic;
using Funkin.NET.Common.KeyBinds.SelectionKey;
using Funkin.NET.Content.Elements.Composites;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osuTK;

namespace Funkin.NET.Content.Screens
{
    public class IntroScreen : MusicScreen, IBackgroundDependencyLoadable, IKeyBindingHandler<SelectionKeyAction>
    {
        public override double ExpectedBpm => 102D;

        private double _lastUpdatedTime;
        private readonly List<string> _addedText = new();
        private bool _quirkyIntroFinished;
        private bool _initializedEnter;
        private GirlfriendDanceTitle _girlfriend;
        private LogoTitle _logo;
        private Box _flashBang;
        private bool _bangCycled;
        private bool _entering;
        private double _enteringRecord = TimeSpan.Zero.Milliseconds;
        private bool _introFinishedThisCycle;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            InitializeInternals();
        }

        protected override void Update()
        {
            base.Update();

            _introFinishedThisCycle = false;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_entering && _enteringRecord == TimeSpan.Zero.Milliseconds)
                _enteringRecord = Clock.CurrentTime;

            if (_entering && _flashBang?.Alpha >= 1f)
            {
                FunkinGame.RunningGame.ScreenStack.Push(new SimpleKeyScreen()); // temp introscreen: todo, put new screen
                Music.Volume.Value = 0;
            }

            UpdateSongVolume();

            if (!_quirkyIntroFinished)
                UpdateTextDisplay();

            if (!_initializedEnter && _quirkyIntroFinished)
                InitializeEnterState();
        }

        public void InitializeInternals()
        {
            AddInternal(new SelectionKeyBindingContainer(FunkinGame.RunningGame));
        }

        public void UpdateSongVolume()
        {
            TimeSpan time = TimeSpan.FromMilliseconds(Clock.CurrentTime - _lastUpdatedTime);

            if (!(Music.Volume.Value < 1D) || time.Milliseconds < 100)
                return;

            _lastUpdatedTime = Clock.CurrentTime;
            Music.Volume.Value += 0.0025D;
        }

        public void UpdateTextDisplay()
        {
            void AddText(string text, float positionOffset)
            {
                if (_addedText.Contains(text))
                    return;

                _addedText.Add(text);

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

            void Clear()
            {
                _addedText.Clear();
                ClearInternal();
                InitializeInternals();
            }

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
                    AddText(FunkinGame.RunningGame.FunnyText[0].ToUpper(), -80f);
                    break;

                case 11D:
                    AddText(FunkinGame.RunningGame.FunnyText[1].ToUpper(), -40f);
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
                    _quirkyIntroFinished = true;
                    _introFinishedThisCycle = true;
                    break;

                /*case 17D:
                    AddText("PRETEND THE", -80f);
                    break;

                case 18D:
                    AddText("GAME STARTS", -40F);
                    break;

                case 19D:
                    AddText("NOW!!", 0f);
                    break;*/
            }

            /*ClearInternal();

            AddInternal(new SpriteText
            {
                Text = CurrentBeat.ToString(CultureInfo.InvariantCulture),
                Colour = Colour4.White,
                Anchor = Anchor.Centre,
                Font = new FontUsage("VCR-Regular", 40f)
            });

            AddInternal(new SpriteText
            {
                Text = Clock.CurrentTime.ToString(CultureInfo.InvariantCulture),
                Colour = Colour4.White,
                Anchor = Anchor.Centre,
                Position = new Vector2(0f, -50f),
                Font = new FontUsage("VCR-Regular", 40f)
            });*/
        }

        public void InitializeEnterState()
        {
            _initializedEnter = true;

            void Fade(Drawable drawable)
            {
                if (!_bangCycled)
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
                if (!_bangCycled)
                    drawable.Alpha = 0f;
                else
                    drawable.Alpha = 1f / 3f;

                double rotOffset = 0D;
                if (_entering)
                    rotOffset = (Clock.CurrentTime - _enteringRecord) / 100D * 25D;

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
                if (_flashBang.Alpha >= 1f)
                    drawable.Alpha = 1f; // basically activates 'em
            }

            SpriteText GetEnterText() => new()
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

            SpriteText enterText = GetEnterText();
            SpriteText enterRed = GetEnterText();
            SpriteText enterBlue = GetEnterText();
            SpriteText enterGreen = GetEnterText();

            enterRed.Colour = Colour4.Red;
            enterBlue.Colour = Colour4.Blue;
            enterGreen.Colour = Colour4.Green;
            enterRed.Blending = BlendingParameters.Additive;
            enterBlue.Blending = BlendingParameters.Additive;
            enterGreen.Blending = BlendingParameters.Additive;
            enterText.OnUpdate += Fade;
            enterRed.OnUpdate += CircularOffset;
            enterBlue.OnUpdate += CircularOffset;
            enterGreen.OnUpdate += CircularOffset;
            enterText.OnUpdate += MagicallyAppear;
            enterRed.OnUpdate += MagicallyAppear;
            enterBlue.OnUpdate += MagicallyAppear;
            enterGreen.OnUpdate += MagicallyAppear;
            _girlfriend.OnUpdate += MagicallyAppear;
            _logo.OnUpdate += MagicallyAppear;

            AddInternal(enterText);
            AddInternal(enterRed);
            AddInternal(enterBlue);
            AddInternal(enterGreen);
            AddInternal(_girlfriend);
            AddInternal(_logo);

            _flashBang = new Box
            {
                Colour = Colour4.White,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0f,
                AlwaysPresent = true
            };

            _flashBang.OnUpdate += drawable =>
            {
                if (_bangCycled && !_entering)
                    return;

                switch (drawable.Alpha)
                {
                    case >= 1f when !_entering:
                        drawable.FadeOutFromOne(4000D);
                        _bangCycled = true;
                        break;

                    case >= 1f when _entering:
                        drawable.FadeOutFromOne(8000D);
                        break;

                    case <= 0f:
                        drawable.FadeInFromZero(1500D);
                        break;
                }
            };

            AddInternal(_flashBang);
        }

        public bool OnPressed(SelectionKeyAction action) => false;

        public void OnReleased(SelectionKeyAction action)
        {
            switch (_quirkyIntroFinished)
            {
                case false:
                    ClearInternal();
                    _entering = false;
                    _quirkyIntroFinished = true;
                    _introFinishedThisCycle = true;
                    return;

                case true when !_introFinishedThisCycle:
                    _entering = true;
                    break;
            }
        }

        protected override void BeatHit()
        {
            base.BeatHit();

            _girlfriend.SwapAnimation();
        }

        #region BackgroundDependencyLoader

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            Music = new DrawableTrack(AudioManager.Tracks.Get(@"Main/FreakyMenu.ogg"));
            Music.Stop();
            Music.Looping = true;
            Music.Start();
            Music.VolumeTo(0D);

            _girlfriend = new GirlfriendDanceTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(80f, 220f),
                Origin = Anchor.Centre,
                Scale = new Vector2(1.5f),
                AlwaysPresent = true,
                Alpha = 0f
            };

            _logo = new LogoTitle
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(-780f, 190f),
                Origin = Anchor.Centre,
                Scale = new Vector2(2f),
                AlwaysPresent = true,
                Alpha = 0f
            };
        }

        #endregion
    }
}