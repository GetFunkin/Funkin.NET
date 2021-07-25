using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Funkin.NET.Graphics;
using Funkin.NET.Graphics.Sprites;
using Funkin.NET.Graphics.UserInterface;
using Funkin.NET.Input.Bindings;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;
// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.Overlays.KeyBindings
{
    public class KeyBindingRow : Container, IFilterable
    {
        private readonly object _action;
        private IEnumerable<IKeyBinding> _bindings;

        public const float TransitionTime = 150;
        public const float ConstantHeight = 20;
        public const float ConstantPadding = 5;

        private bool _matchingFilter;

        public bool MatchingFilter
        {
            get => _matchingFilter;
            set
            {
                _matchingFilter = value;
                this.FadeTo(!_matchingFilter ? 0 : 1);
            }
        }

        private Container _content;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) =>
            _content.ReceivePositionalInputAt(screenSpacePos);

        public bool FilteringActive { get; set; }

        private FunkinSpriteText _text;
        private FillFlowContainer _cancelAndClearButtons;
        private FillFlowContainer<KeyButton> _buttons;

        private Bindable<bool> IsDefault { get; } = new BindableBool(true);

        public IEnumerable<string> FilterTerms =>
            _bindings.Select(b => b.KeyCombination.ReadableString()).Prepend(_text.Text.ToString());

        [Resolved] private UniversalActionContainer ActionContainer { get; set; }

        public KeyBindingRow(object action, IEnumerable<IKeyBinding> bindings)
        {
            _action = action;
            _bindings = bindings;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding {Horizontal = SettingsPanel.ContentMargins};

            InternalChildren = new Drawable[]
            {
                _content = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Masking = true,
                    CornerRadius = ConstantPadding,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Radius = 2,
                        Colour = Color4Extensions.FromHex("eeaa00").Opacity(0),
                        Type = EdgeEffectType.Shadow,
                        Hollow = true,
                    },

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                            Alpha = 0.6f,
                        },

                        _text = new FunkinSpriteText
                        {
                            Text = _action.GetDescription(),
                            Font = FunkinFont.GetFont(size: 15f),
                            Margin = new MarginPadding(ConstantPadding),
                        },

                        _buttons = new FillFlowContainer<KeyButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight
                        },

                        _cancelAndClearButtons = new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Padding = new MarginPadding(ConstantPadding) {Top = ConstantHeight + ConstantPadding * 2},
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Alpha = 0,
                            Spacing = new Vector2(5),
                            Children = new Drawable[]
                            {
                                new CancelButton {Action = DoFinalize},
                                new ClearButton {Action = DoClear}
                            },
                        }
                    }
                }
            };

            foreach (IKeyBinding b in _bindings)
                _buttons.Add(new KeyButton(b));

            UpdateIsDefaultValue();
        }

        protected override bool OnHover(HoverEvent e)
        {
            _content.FadeEdgeEffectTo(1, TransitionTime, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            _content.FadeEdgeEffectTo(0, TransitionTime, Easing.OutQuint);

            base.OnHoverLost(e);
        }

        public override bool AcceptsFocus => _bindTarget == null;

        private KeyButton _bindTarget;

        public bool AllowMainMouseButtons;

        public IEnumerable<KeyCombination> Defaults;

        private static bool IsModifier(Key k) => k < Key.F1;

        protected override bool OnClick(ClickEvent e) => true;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (!HasFocus || !_bindTarget.IsHovered)
                return base.OnMouseDown(e);

            if (!AllowMainMouseButtons)
            {
                switch (e.Button)
                {
                    case MouseButton.Left:
                    case MouseButton.Right:
                        return true;
                }
            }

            _bindTarget.UpdateKeyCombination(KeyCombination.FromInputState(e.CurrentState), ActionContainer);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            // don't do anything until the last button is released.
            if (!HasFocus || e.HasAnyButtonPressed)
            {
                base.OnMouseUp(e);
                return;
            }

            if (_bindTarget.IsHovered)
                DoFinalize();
            // prevent updating bind target before clear button's action
            else if (!_cancelAndClearButtons.Any(b => b.IsHovered))
                UpdateBindTarget();
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            if (!HasFocus || !_bindTarget.IsHovered)
                return base.OnScroll(e);

            _bindTarget.UpdateKeyCombination(KeyCombination.FromInputState(e.CurrentState, e.ScrollDelta),
                ActionContainer);
            DoFinalize();
            return true;

        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (!HasFocus)
                return false;

            _bindTarget.UpdateKeyCombination(KeyCombination.FromInputState(e.CurrentState), ActionContainer);

            if (!IsModifier(e.Key))
                DoFinalize();

            return true;
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (!HasFocus)
            {
                base.OnKeyUp(e);
                return;
            }

            DoFinalize();
        }

        protected override bool OnJoystickPress(JoystickPressEvent e)
        {
            if (!HasFocus)
                return false;

            _bindTarget.UpdateKeyCombination(KeyCombination.FromInputState(e.CurrentState), ActionContainer);
            DoFinalize();

            return true;
        }

        protected override void OnJoystickRelease(JoystickReleaseEvent e)
        {
            if (!HasFocus)
            {
                base.OnJoystickRelease(e);
                return;
            }

            DoFinalize();
        }

        protected override bool OnMidiDown(MidiDownEvent e)
        {
            if (!HasFocus)
                return false;

            _bindTarget.UpdateKeyCombination(KeyCombination.FromInputState(e.CurrentState), ActionContainer);
            DoFinalize();

            return true;
        }

        protected override void OnMidiUp(MidiUpEvent e)
        {
            if (!HasFocus)
            {
                base.OnMidiUp(e);
                return;
            }

            DoFinalize();
        }

        private void DoClear()
        {
            if (_bindTarget == null)
                return;

            _bindTarget.UpdateKeyCombination(InputKey.None, ActionContainer);
            DoFinalize();
        }

        private void DoFinalize()
        {
            if (_bindTarget != null)
            {
                UpdateIsDefaultValue();

                _bindTarget.IsBinding = false;
                Schedule(() =>
                {
                    // schedule to ensure we don't instantly get focus back on next OnMouseClick (see AcceptFocus impl.)
                    _bindTarget = null;
                });
            }

            if (HasFocus)
                GetContainingInputManager().ChangeFocus(null);

            _cancelAndClearButtons.FadeOut(300, Easing.OutQuint);
            _cancelAndClearButtons.BypassAutoSizeAxes |= Axes.Y;
        }

        protected override void OnFocus(FocusEvent e)
        {
            _content.AutoSizeDuration = 500;
            _content.AutoSizeEasing = Easing.OutQuint;

            _cancelAndClearButtons.FadeIn(300, Easing.OutQuint);
            _cancelAndClearButtons.BypassAutoSizeAxes &= ~Axes.Y;

            UpdateBindTarget();
            base.OnFocus(e);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            DoFinalize();
            base.OnFocusLost(e);
        }

        /// <summary>
        /// Updates the bind target to the currently hovered key button or the first if clicked anywhere else.
        /// </summary>
        private void UpdateBindTarget()
        {
            if (_bindTarget != null)
                _bindTarget.IsBinding = false;

            _bindTarget = _buttons.FirstOrDefault(b => b.IsHovered) ?? _buttons.FirstOrDefault();

            if (_bindTarget != null)
                _bindTarget.IsBinding = true;
        }

        private void UpdateIsDefaultValue()
        {
            IsDefault.Value = _bindings.Select(b => b.KeyCombination).SequenceEqual(Defaults);
        }

        public class CancelButton : FunkinButton
        {
            public CancelButton()
            {
                Text = "Cancel";
                Size = new Vector2(80, 20);
            }
        }

        public class ClearButton : FunkinButton
        {
            public ClearButton()
            {
                Text = "Clear";
                Size = new Vector2(80, 20);
            }
        }

        public class KeyButton : Container
        {
            public readonly IKeyBinding WrappedBinding;

            private readonly Box _box;
            public readonly FunkinSpriteText Text;

            private Color4 _hoverColor;

            private bool _isBinding;

            public bool IsBinding
            {
                get => _isBinding;
                set
                {
                    if (value == _isBinding) return;

                    _isBinding = value;

                    UpdateHoverState();
                }
            }

            public KeyButton(IKeyBinding wrappedBinding)
            {
                WrappedBinding = wrappedBinding;

                Margin = new MarginPadding(ConstantPadding);

                Masking = true;
                CornerRadius = ConstantPadding;

                Height = ConstantHeight;
                AutoSizeAxes = Axes.X;

                Children = new Drawable[]
                {
                    new Container
                    {
                        AlwaysPresent = true,
                        Width = 80,
                        Height = ConstantHeight,
                    },
                    _box = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Black
                    },
                    Text = new FunkinSpriteText
                    {
                        Font = FunkinFont.GetFont(size: 10f),
                        Margin = new MarginPadding(5),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = wrappedBinding.KeyCombination.ReadableString(),
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void Load()
            {
                _hoverColor = Color4Extensions.FromHex(@"eeaa00");
            }

            protected override bool OnHover(HoverEvent e)
            {
                UpdateHoverState();
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                UpdateHoverState();
                base.OnHoverLost(e);
            }

            private void UpdateHoverState()
            {
                if (_isBinding)
                {
                    _box.FadeColour(Color4.White, TransitionTime, Easing.OutQuint);
                    Text.FadeColour(Color4.Black, TransitionTime, Easing.OutQuint);
                }
                else
                {
                    _box.FadeColour(IsHovered ? _hoverColor : Color4.Black, TransitionTime, Easing.OutQuint);
                    Text.FadeColour(IsHovered ? Color4.Black : Color4.White, TransitionTime, Easing.OutQuint);
                }
            }

            public void UpdateKeyCombination(KeyCombination newCombination, UniversalActionContainer actionContainer)
            {
                static bool DoTheseTwoFuckingCollectionsMatch(ImmutableArray<InputKey> key1, ImmutableArray<InputKey> key2)
                {
                    List<InputKey> list1 = key1.ToList();
                    List<InputKey> list2 = key2.ToList();

                    if (list1.Count != list2.Count)
                        return false;

                    bool youFailedIHateYouSoMuch = false;
                    foreach (InputKey key in list1.TakeWhile(_ => !youFailedIHateYouSoMuch))
                    {
                        if (list2.Contains(key))
                            list2.Remove(key);
                        else
                            youFailedIHateYouSoMuch = true;
                    }

                    return !youFailedIHateYouSoMuch;
                }

                List<IKeyBinding> current = actionContainer.DefaultKeyBindings.ToList();
                current.RemoveAll(x => x.Action.ToString() == WrappedBinding.Action.ToString()
                                       && DoTheseTwoFuckingCollectionsMatch(x.KeyCombination.Keys,
                                           WrappedBinding.KeyCombination.Keys));

                WrappedBinding.KeyCombination = newCombination;
                Text.Text = WrappedBinding.KeyCombination.ReadableString();

                if (!current.Any(x => x.Action.ToString() == WrappedBinding.Action.ToString()
                                      && DoTheseTwoFuckingCollectionsMatch(x.KeyCombination.Keys, newCombination.Keys)))
                    current.Add(WrappedBinding);

                actionContainer.UpdateKeyBindings(current);
            }
        }
    }
}