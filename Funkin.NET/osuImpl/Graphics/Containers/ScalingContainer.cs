using Funkin.NET.Common.Configuration;
using Funkin.NET.Common.Screens.Background;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Funkin.NET.osuImpl.Graphics.Containers
{
    /// <summary>
    ///     See: osu!'s ScalingContainer. <br />
    ///     Handles user-defined scaling, allowing application at multiple levels defined by <see cref="FunkinConfigManager.ScalingMode"/>
    /// </summary>
    public class ScalingContainer : Container
    {
        public Bindable<float> SizeX;
        public Bindable<float> SizeY;
        public Bindable<float> PosX;
        public Bindable<float> PosY;
        public readonly FunkinConfigManager.ScalingMode? TargetMode;
        public Bindable<FunkinConfigManager.ScalingMode> ScalingMode;

        private readonly Container _content;
        private readonly Container _sizableContainer;
        private BackgroundScreenStack _backgroundStack;
        private bool allowScaling = true;

        /// <summary>
        /// Whether user scaling preferences should be applied. Enabled by default.
        /// </summary>
        public bool AllowScaling
        {
            get => allowScaling;
            set
            {
                if (value == allowScaling)
                    return;

                allowScaling = value;

                if (IsLoaded)
                    UpdateSize();
            }
        }

        public ScalingContainer(FunkinConfigManager.ScalingMode? targetMode = null)
        {
            TargetMode = targetMode;
            RelativeSizeAxes = Axes.Both;

            InternalChild = _sizableContainer = new AlwaysInputContainer
            {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                CornerRadius = 10f,
                Child = _content = new ScalingDrawSizePreservingFillContainer(true)
            };
        }

        protected override Container<Drawable> Content => _content;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        [BackgroundDependencyLoader]
        private void Load(FunkinConfigManager config)
        {
            ScalingMode =
                config.GetBindable<FunkinConfigManager.ScalingMode>(FunkinConfigManager.FunkinSetting.Scaling);
            ScalingMode.ValueChanged += _ => UpdateSize();

            SizeX = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.ScalingSizeX);
            SizeX.ValueChanged += _ => UpdateSize();

            SizeY = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.ScalingSizeY);
            SizeY.ValueChanged += _ => UpdateSize();

            PosX = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.ScalingPositionX);
            PosX.ValueChanged += _ => UpdateSize();

            PosY = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.ScalingPositionY);
            PosY.ValueChanged += _ => UpdateSize();
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            UpdateSize();
            _sizableContainer.FinishTransforms();
        }

        private bool RequiresBackgroundVisible =>
            ScalingMode.Value is FunkinConfigManager.ScalingMode.On && (SizeX.Value != 1 || SizeY.Value != 1);

        private void UpdateSize()
        {
            const float fadeTime = 500f;

            if (TargetMode == FunkinConfigManager.ScalingMode.On)
            {
                // the top level scaling container manages the background to be displayed while scaling.
                if (RequiresBackgroundVisible)
                {
                    if (_backgroundStack == null)
                    {
                        AddInternal(_backgroundStack = new BackgroundScreenStack
                        {
                            Colour = new Colour4(0.1f, 0.1f, 0.1f, 1f),
                            Alpha = 0,
                            Depth = float.MaxValue
                        });

                        _backgroundStack.Push(new ScalingBackgroundScreen());
                    }

                    _backgroundStack.FadeIn(fadeTime);
                }
                else
                    _backgroundStack?.FadeOut(fadeTime);
            }

            bool scaling = AllowScaling && (TargetMode == null || ScalingMode.Value == TargetMode);

            Vector2 targetSize = scaling ? new Vector2(SizeX.Value, SizeY.Value) : Vector2.One;
            Vector2 targetPosition =
                scaling ? new Vector2(PosX.Value, PosY.Value) * (Vector2.One - targetSize) : Vector2.Zero;
            bool requiresMasking = scaling && targetSize != Vector2.One;

            if (requiresMasking)
                _sizableContainer.Masking = true;

            _sizableContainer.MoveTo(targetPosition, 500D, Easing.OutQuart);
            _sizableContainer.ResizeTo(targetSize, 500D, Easing.OutQuart).OnComplete(_ =>
            {
                _sizableContainer.Masking = requiresMasking;
            });
        }

        public class ScalingDrawSizePreservingFillContainer : DrawSizePreservingFillContainer
        {
            private readonly bool _applyUIScale;
            private Bindable<float> _uiScale;

            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

            public ScalingDrawSizePreservingFillContainer(bool applyUIScale)
            {
                _applyUIScale = applyUIScale;
            }

            [BackgroundDependencyLoader]
            private void Load(FunkinConfigManager config)
            {
                if (!_applyUIScale)
                    return;

                _uiScale = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.UIScale);
                _uiScale.BindValueChanged(ScaleChanged, true);
            }

            private void ScaleChanged(ValueChangedEvent<float> args)
            {
                this.ScaleTo(new Vector2(args.NewValue), 500D, Easing.Out);
                this.ResizeTo(new Vector2(1f / args.NewValue), 500D, Easing.Out);
            }
        }

        public class ScalingBackgroundScreen : BackgroundScreenDefault
        {
            public override void OnEntering(IScreen last)
            {
                this.FadeInFromZero(4000D, Easing.OutQuint);
            }
        }

        public class AlwaysInputContainer : Container
        {
            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

            public AlwaysInputContainer()
            {
                RelativeSizeAxes = Axes.Both;
            }
        }
    }
}