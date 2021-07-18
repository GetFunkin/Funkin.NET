using System;
using System.Collections.Generic;
using Funkin.NET.Common.KeyBinds.SelectionKey;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
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

        protected override void LoadComplete()
        {
            base.LoadComplete();

            InitializeInternals();
        }

        protected override void Update()
        {
            base.Update();

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

            static void Fade(Drawable drawable)
            {
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

            SpriteText enterText = new()
            {
                Anchor = Anchor.Centre,
                RelativeAnchorPosition = Size / 2f,
                Text = "Press Enter to Begin",
                Position = new Vector2(0f, 180f),
                Origin = Anchor.Centre,
                Font = new FontUsage("VCR", 40f),
                AlwaysPresent = true
            };

            enterText.OnUpdate += Fade;

            AddInternal(enterText);
        }

        public bool OnPressed(SelectionKeyAction action) => false;

        public void OnReleased(SelectionKeyAction action)
        {
            if (!_quirkyIntroFinished)
            {
                ClearInternal();
                _quirkyIntroFinished = true;
            }

            if (_quirkyIntroFinished)
                ; // TODO: game
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
        }

        #endregion
    }
}