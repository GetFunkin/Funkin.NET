using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Core.Graphics.UserInterface
{
    /// <summary>
    ///     See: osu!'s OsuTextBox.
    /// </summary>
    public class FunkinTextBox : BasicTextBox
    {
        private readonly Sample[] _textAddedSamples = new Sample[4];
        private Sample _capsTextAddedSample;
        private Sample _textRemovedSample;
        private Sample _textCommittedSample;
        private Sample _caretMovedSample;

        /// <summary>
        ///     Whether to allow playing a different samples based on the type of character. <br />
        ///     If set to false, the same sample will be used for all characters.
        /// </summary>
        protected virtual bool AllowUniqueCharacterSamples => true;

        protected override float LeftRightPadding => 10;

        protected override float CaretWidth => 3;

        protected override SpriteText CreatePlaceholder() => new()
        {
            Font = new FontUsage("Torus-Regular"),
            Colour = new Color4(180, 180, 180, 255),
            Margin = new MarginPadding {Left = 2},
        };

        public FunkinTextBox()
        {
            Height = 40;
            TextContainer.Height = 0.5f;
            CornerRadius = 5;
            LengthLimit = 1000;

            Current.DisabledChanged += disabled => { Alpha = disabled ? 0.3f : 1; };
        }

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            BackgroundUnfocused = Color4.Black.Opacity(0.5f);
            BackgroundFocused = new Color4(0.3f, 0.3f, 0.3f, 0.8f);
            BackgroundCommit = BorderColour = Color4Extensions.FromHex(@"ffcc22");

            for (int i = 0; i < _textAddedSamples.Length; i++)
                _textAddedSamples[i] = audio.Samples.Get($@"Keyboard/key-press-{1 + i}");

            _capsTextAddedSample = audio.Samples.Get(@"Keyboard/key-caps");
            _textRemovedSample = audio.Samples.Get(@"Keyboard/key-delete");
            _textCommittedSample = audio.Samples.Get(@"Keyboard/key-confirm");
            _caretMovedSample = audio.Samples.Get(@"Keyboard/key-movement");
        }

        protected override Color4 SelectionColour => new(249f, 90f, 255f, 255f);

        protected override void OnUserTextAdded(string added)
        {
            base.OnUserTextAdded(added);

            if (added.Any(char.IsUpper) && AllowUniqueCharacterSamples)
                _capsTextAddedSample?.Play();
            else
                _textAddedSamples[RNG.Next(0, 3)]?.Play();
        }

        protected override void OnUserTextRemoved(string removed)
        {
            base.OnUserTextRemoved(removed);

            _textRemovedSample?.Play();
        }

        protected override void OnTextCommitted(bool textChanged)
        {
            base.OnTextCommitted(textChanged);

            _textCommittedSample?.Play();
        }

        protected override void OnCaretMoved(bool selecting)
        {
            base.OnCaretMoved(selecting);

            _caretMovedSample?.Play();
        }

        protected override void OnFocus(FocusEvent e)
        {
            BorderThickness = 3;
            base.OnFocus(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            BorderThickness = 0;

            base.OnFocusLost(e);
        }

        protected override Drawable GetDrawableCharacter(char c) => new FallingDownContainer
        {
            AutoSizeAxes = Axes.Both,
            Child = new SpriteText {Text = c.ToString(), Font = FunkinFont.GetFont(size: CalculatedTextSize)},
        };

        protected override Caret CreateCaret() => new FunkinCaret
        {
            CaretWidth = CaretWidth,
            SelectionColor = SelectionColour,
        };

        public class FunkinCaret : Caret
        {
            public const float CaretMoveTime = 60;

            private readonly CaretContainer _beatSync;

            public FunkinCaret()
            {
                RelativeSizeAxes = Axes.Y;
                Size = new Vector2(1, 0.9f);

                Colour = Color4.Transparent;
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;

                Masking = true;
                CornerRadius = 1;
                InternalChild = _beatSync = new CaretContainer
                {
                    RelativeSizeAxes = Axes.Both,
                };
            }

            public override void Hide() => this.FadeOut(200);

            public float CaretWidth { get; set; }

            public Color4 SelectionColor { get; set; }

            public override void DisplayAt(Vector2 position, float? selectionWidth)
            {
                _beatSync.HasSelection = selectionWidth != null;

                if (selectionWidth != null)
                {
                    this.MoveTo(new Vector2(position.X, position.Y), 60, Easing.Out);
                    this.ResizeWidthTo(selectionWidth.Value + CaretWidth / 2, CaretMoveTime, Easing.Out);
                    this.FadeColour(SelectionColor, 200, Easing.Out);
                }
                else
                {
                    this.MoveTo(new Vector2(position.X - CaretWidth / 2, position.Y), 60, Easing.Out);
                    this.ResizeWidthTo(CaretWidth, CaretMoveTime, Easing.Out);
                    this.FadeColour(Color4.White, 200, Easing.Out);
                }
            }

            public class CaretContainer : Container
            {
                private bool _hasSelection;

                public bool HasSelection
                {
                    set
                    {
                        _hasSelection = value;
                        if (value)

                            this.FadeTo(0.5f, 200, Easing.Out);
                    }
                }

                public CaretContainer()
                {
                    InternalChild = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White,
                    };
                }
            }
        }
    }
}