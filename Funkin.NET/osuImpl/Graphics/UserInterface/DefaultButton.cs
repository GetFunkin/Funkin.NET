﻿using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK.Graphics;

// ReSharper disable VirtualMemberCallInConstructor

namespace Funkin.NET.osuImpl.Graphics.UserInterface
{
    /// <summary>
    ///     See: osu!'s OsuButton. <br />
    ///     A button with added default sound effects. (lel no)
    /// </summary>
    public class DefaultButton : Button
    {
        public LocalisableString Text
        {
            get => SpriteText?.Text ?? default;
            set
            {
                if (SpriteText != null)
                    SpriteText.Text = value;
            }
        }

        private Color4? NullableBackgroundColor;

        public Color4 BackgroundColor
        {
            set
            {
                NullableBackgroundColor = value;
                Background.FadeColour(value);
            }
        }

        public virtual IEnumerable<string> FilterTerms => new[] {Text.ToString()};

        public bool MatchingFilter
        {
            set => this.FadeTo(value ? 1 : 0);
        }

        public bool FilteringActive { get; set; }

        protected override Container<Drawable> Content { get; }

        protected Box Hover;
        protected Box Background;
        protected SpriteText SpriteText;

        public DefaultButton()
        {
            Height = 40;

            AddInternal(Content = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                CornerRadius = 5,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    Background = new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                    Hover = new Box
                    {
                        Alpha = 0,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White.Opacity(.1f),
                        Blending = BlendingParameters.Additive,
                        Depth = float.MinValue
                    },
                    SpriteText = CreateText(),
                }
            });

            //if (hoverSounds.HasValue)
            //    AddInternal(new HoverClickSounds(hoverSounds.Value));

            Enabled.BindValueChanged(EnabledChanged, true);
        }

        [BackgroundDependencyLoader]
        private void Load()
        {
            if (NullableBackgroundColor == null)
                BackgroundColor = Color4Extensions.FromHex(@"44aadd");

            Enabled.ValueChanged += EnabledChanged;
            Enabled.TriggerChange();
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (Enabled.Value)
            {
                Debug.Assert(NullableBackgroundColor != null);
                Background.FlashColour(NullableBackgroundColor.Value, 200);
            }

            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Enabled.Value)
                Hover.FadeIn(200, Easing.OutQuint);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            Hover.FadeOut(300);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Content.ScaleTo(0.9f, 4000, Easing.OutQuint);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            Content.ScaleTo(1, 1000, Easing.OutElastic);
            base.OnMouseUp(e);
        }

        protected virtual SpriteText CreateText() => new()
        {
            Depth = -1,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            Font = new FontUsage("Torus-Bold")
        };

        private void EnabledChanged(ValueChangedEvent<bool> e)
        {
            this.FadeColour(e.NewValue ? Color4.White : Color4.Gray, 200, Easing.OutQuint);
        }
    }
}