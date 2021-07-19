using Funkin.NET.Content.osu.Configuration;
using osu.Framework.Configuration;
using osu.Framework.Configuration.Tracking;
using osu.Framework.Extensions;
using osu.Framework.Platform;

namespace Funkin.NET.Content.Configuration
{
    public class FunkinConfigManager : IniConfigManager<FunkinSetting>
    {
        public FunkinConfigManager(Storage storage) : base(storage)
        {
        }

        protected override void InitialiseDefaults()
        {
            SetDefault(FunkinSetting.DefaultWindowType, WindowMode.Fullscreen);

            SetDefault(FunkinSetting.Scaling, ScalingMode.Off);

            SetDefault(FunkinSetting.ScalingSizeX, 0.8f, 0.2f, 1f);
            SetDefault(FunkinSetting.ScalingSizeY, 0.8f, 0.2f, 1f);

            SetDefault(FunkinSetting.ScalingPositionX, 0.5f, 0f, 1f);
            SetDefault(FunkinSetting.ScalingPositionY, 0.5f, 0f, 1f);

            SetDefault(FunkinSetting.UIScale, 1f, 0.8f, 1.6f, 0.01f);

            SetDefault(FunkinSetting.MenuParallax, true);

        }

        public override TrackedSettings CreateTrackedSettings()
        {
            return new()
            {
                new TrackedSetting<ScalingMode>(FunkinSetting.Scaling,
                    x => new SettingDescription(x, "scaling", x.GetDescription()))
            };
        }
    }
}