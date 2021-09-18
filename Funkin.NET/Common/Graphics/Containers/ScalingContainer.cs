using Funkin.NET.Common.Configuration;
using Funkin.NET.Common.Screens.Backgrounds;
using Funkin.NET.Intermediary.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Funkin.NET.Common.Graphics.Containers
{
    public class ScalingContainer : Container
    {
        public Bindable<float> SizeX;
        public Bindable<float> SizeY;
        public Bindable<float> PosX;
        public Bindable<float> PosY;
        public BackgroundScreenStack BackgroundStack;
        public readonly FunkinConfigManager.ScalingMode? TargetMode;
        public Bindable<FunkinConfigManager.ScalingMode> ScalingMode;

        protected readonly Container ProtectedContent;
        protected readonly Container SizableContainer;
        protected bool OriginalAllowScaling = true;

        /// <summary>
        /// Whether user scaling preferences should be applied. Enabled by default.
        /// </summary>
        public bool AllowScaling
        {
            get => OriginalAllowScaling;

            set
            {
                if (value == OriginalAllowScaling)
                    return;

                OriginalAllowScaling = value;

                if (IsLoaded)
                    UpdateSize();
            }
        }

        public ScalingContainer(FunkinConfigManager.ScalingMode? targetMode = null)
        {
            TargetMode = targetMode;
            RelativeSizeAxes = Axes.Both;

            InternalChild = SizableContainer = new AlwaysInputContainer
            {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                CornerRadius = 10f,
                Child = ProtectedContent = new ScalingDrawSizePreservingFillContainer(true)
            };
        }

        protected override Container<Drawable> Content => ProtectedContent;

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
            SizableContainer.FinishTransforms();
        }

        private bool RequiresBackgroundVisible => ScalingMode.Value is FunkinConfigManager.ScalingMode.On
                                                  && (SizeX.Value != 1 || SizeY.Value != 1);

        private void UpdateSize()
        {
            const float fadeTime = 500f;

            if (TargetMode == FunkinConfigManager.ScalingMode.On)
            {
                // the top level scaling container manages the background to be displayed while scaling.
                if (RequiresBackgroundVisible)
                {
                    if (BackgroundStack == null)
                    {
                        AddInternal(BackgroundStack = new BackgroundScreenStack
                        {
                            Colour = new Colour4(0.1f, 0.1f, 0.1f, 1f),
                            Alpha = 0,
                            Depth = float.MaxValue
                        });

                        // why does this die
                        BackgroundStack.Push(new ScalingDefaultBackgroundScreen(DefaultBackgroundType.Purple));
                    }

                    BackgroundStack.FadeIn(fadeTime);
                }
                else
                    BackgroundStack?.FadeOut(fadeTime);
            }

            bool scaling = AllowScaling && (TargetMode == null || ScalingMode.Value == TargetMode);

            Vector2 targetSize = scaling ? new Vector2(SizeX.Value, SizeY.Value) : Vector2.One;
            Vector2 targetPosition = scaling 
                ? new Vector2(PosX.Value, PosY.Value) * (Vector2.One - targetSize) 
                : Vector2.Zero;
            bool requiresMasking = scaling && targetSize != Vector2.One;

            if (requiresMasking)
                SizableContainer.Masking = true;

            SizableContainer.MoveTo(targetPosition, 500D, Easing.OutQuart);
            SizableContainer.ResizeTo(targetSize, 500D, Easing.OutQuart).OnComplete(_ =>
            {
                SizableContainer.Masking = requiresMasking;
            });
        }

        public class ScalingDrawSizePreservingFillContainer : DrawSizePreservingFillContainer
        {
            protected readonly bool ApplyUIScale;
            protected Bindable<float> UIScale;

            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

            public ScalingDrawSizePreservingFillContainer(bool applyUIScale)
            {
                ApplyUIScale = applyUIScale;
            }

            [BackgroundDependencyLoader]
            private void Load(FunkinConfigManager config)
            {
                if (!ApplyUIScale)
                    return;

                UIScale = config.GetBindable<float>(FunkinConfigManager.FunkinSetting.UIScale);
                UIScale.BindValueChanged(ScaleChanged, true);
            }

            private void ScaleChanged(ValueChangedEvent<float> args)
            {
                this.ScaleTo(new Vector2(args.NewValue), 500D, Easing.Out);
                this.ResizeTo(new Vector2(1f / args.NewValue), 500D, Easing.Out);
            }
        }

        public class ScalingDefaultBackgroundScreen : DefaultBackgroundScreen
        {
            public override void OnEntering(IScreen last)
            {
                this.FadeInFromZero(4000D, Easing.OutQuint);
            }

            public ScalingDefaultBackgroundScreen(DefaultBackgroundType backgroundType) : base(backgroundType)
            {
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