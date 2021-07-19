using Funkin.NET.Content.Configuration;
using Funkin.NET.Content.osu.Configuration;
using Funkin.NET.Content.osu.Screens;
using Funkin.NET.Content.osu.Screens.Backgrounds;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Funkin.NET.Content.osu.Graphics.Containers
{
    /// <summary>
    ///     osu!'s ScalingContainer. <br />
    ///     Handles user-defined scaling, allowing application at multiple levels defined by <see cref="osu.Configuration.ScalingMode"/>.
    /// </summary>
    public class ScalingContainer : Container, IBackgroundDependencyLoadable
    {
        public Bindable<float> SizeX;
        public Bindable<float> SizeY;
        public Bindable<float> PosX;
        public Bindable<float> PosY;
        public readonly ScalingMode? TargetMode;
        public Bindable<ScalingMode> ScalingMode;
        public readonly Container SizableContainer;
        public BackgroundScreenStack BackgroundStack;

        private bool _allowScaling = true;
        private readonly Container _content;

        protected override Container<Drawable> Content => _content;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        /// <summary>
        /// Whether user scaling preferences should be applied. Enabled by default.
        /// </summary>
        public bool AllowScaling
        {
            get => _allowScaling;
            set
            {
                if (value == _allowScaling)
                    return;

                _allowScaling = value;
                if (IsLoaded) UpdateSize();
            }
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="targetMode">The mode which this container should be handling. Handles all modes if null.</param>
        public ScalingContainer(ScalingMode? targetMode = null)
        {
            TargetMode = targetMode;
            // ReSharper disable once VirtualMemberCallInConstructor
            RelativeSizeAxes = Axes.Both;

            InternalChild = SizableContainer = new AlwaysInputContainer
            {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                CornerRadius = 10,
                Child = _content =
                    new ScalingDrawSizePreservingFillContainer(targetMode != Configuration.ScalingMode.Gameplay)
            };
        }

        private class ScalingDrawSizePreservingFillContainer : DrawSizePreservingFillContainer,
            IBackgroundDependencyLoadable
        {
            private readonly bool _applyUIScale;
            private Bindable<float> _uiScale;

            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

            public ScalingDrawSizePreservingFillContainer(bool applyUIScale)
            {
                _applyUIScale = applyUIScale;
            }

            [BackgroundDependencyLoader]
            void IBackgroundDependencyLoadable.BackgroundDependencyLoad(FunkinConfigManager config)
            {
                if (!_applyUIScale)
                    return;

                _uiScale = config.GetBindable<float>(FunkinSetting.UIScale);
                _uiScale.BindValueChanged(ScaleChanged, true);
            }

            private void ScaleChanged(ValueChangedEvent<float> args)
            {
                this.ScaleTo(new Vector2(args.NewValue), 500, Easing.Out);
                this.ResizeTo(new Vector2(1 / args.NewValue), 500, Easing.Out);
            }
        }

        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad(FunkinConfigManager config)
        {
            ScalingMode = config.GetBindable<ScalingMode>(FunkinSetting.Scaling);
            ScalingMode.ValueChanged += _ => UpdateSize();

            SizeX = config.GetBindable<float>(FunkinSetting.ScalingSizeX);
            SizeX.ValueChanged += _ => UpdateSize();

            SizeY = config.GetBindable<float>(FunkinSetting.ScalingSizeY);
            SizeY.ValueChanged += _ => UpdateSize();

            PosX = config.GetBindable<float>(FunkinSetting.ScalingPositionX);
            PosX.ValueChanged += _ => UpdateSize();

            PosY = config.GetBindable<float>(FunkinSetting.ScalingPositionY);
            PosY.ValueChanged += _ => UpdateSize();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            UpdateSize();
            SizableContainer.FinishTransforms();
        }

        public bool RequiresBackgroundVisible =>
            ScalingMode.Value is Configuration.ScalingMode.Everything or Configuration.ScalingMode.ExcludeOverlays &&
            (SizeX.Value != 1 || SizeY.Value != 1);

        private void UpdateSize()
        {
            const float fadeTime = 500;

            if (TargetMode == Configuration.ScalingMode.Everything)
            {
                // the top level scaling container manages the background to be displayed while scaling.
                if (RequiresBackgroundVisible)
                {
                    if (BackgroundStack == null)
                    {
                        AddInternal(BackgroundStack = new BackgroundScreenStack
                        {
                            Colour = OsuColour.Gray(0.1f),
                            Alpha = 0,
                            Depth = float.MaxValue
                        });

                        BackgroundStack.Push(new ScalingBackgroundScreen());
                    }

                    BackgroundStack.FadeIn(fadeTime);
                }
                else
                    BackgroundStack?.FadeOut(fadeTime);
            }

            bool scaling = AllowScaling && (TargetMode == null || ScalingMode.Value == TargetMode);

            Vector2 targetSize = scaling ? new Vector2(SizeX.Value, SizeY.Value) : Vector2.One;
            Vector2 targetPosition =
                scaling ? new Vector2(PosX.Value, PosY.Value) * (Vector2.One - targetSize) : Vector2.Zero;
            bool requiresMasking = scaling && targetSize != Vector2.One;

            if (requiresMasking)
                SizableContainer.Masking = true;

            SizableContainer.MoveTo(targetPosition, 500, Easing.OutQuart);
            SizableContainer.ResizeTo(targetSize, 500, Easing.OutQuart).OnComplete(_ =>
            {
                SizableContainer.Masking = requiresMasking;
            });
        }

        private class ScalingBackgroundScreen : BackgroundScreenDefault
        {
            protected override bool AllowStoryboardBackground => false;

            public override void OnEntering(IScreen last)
            {
                this.FadeInFromZero(4000, Easing.OutQuint);
            }
        }

        private class AlwaysInputContainer : Container
        {
            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

            public AlwaysInputContainer()
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                RelativeSizeAxes = Axes.Both;
            }
        }
    }
}