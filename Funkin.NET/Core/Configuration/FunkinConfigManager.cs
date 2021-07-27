using osu.Framework.Configuration;
using osu.Framework.Configuration.Tracking;
using osu.Framework.Extensions;
using osu.Framework.Platform;

namespace Funkin.NET.Core.Configuration
{
    public class FunkinConfigManager : IniConfigManager<FunkinConfigManager.FunkinSetting>
    {
        public FunkinConfigManager(Storage storage) : base(storage)
        {
        }

        protected override void InitialiseDefaults()
        {
            SetDefault(FunkinSetting.MenuParallax, true);
            SetDefault(FunkinSetting.ShowFpsDisplay, false);
            SetDefault(FunkinSetting.Scaling, ScalingMode.Off);
            SetDefault(FunkinSetting.ScalingSizeX, 0.8f, 0.2f, 1f);
            SetDefault(FunkinSetting.ScalingSizeY, 0.8f, 0.2f, 1f);
            SetDefault(FunkinSetting.ScalingPositionX, 0.5f, 0f, 1f);
            SetDefault(FunkinSetting.ScalingPositionY, 0.5f, 0f, 1f);
            SetDefault(FunkinSetting.UIScale, 1f, 0.8f, 1.6f, 0.01f);
            SetDefault(FunkinSetting.GameplayDisableWinKey, true);
            SetDefault(FunkinSetting.DiscordRichPresence, true);
            SetDefault(FunkinSetting.CursorSize, 1.0f, 0.5f, 2f, 0.01f);
        }

        public override TrackedSettings CreateTrackedSettings() => new()
        {
            new TrackedSetting<ScalingMode>(FunkinSetting.Scaling,
                x => new SettingDescription(x, "scaling", x.GetDescription()))
        };

        public enum FunkinSetting
        {
            MenuParallax,
            ShowFpsDisplay,
            Scaling,
            ScalingPositionX,
            ScalingPositionY,
            ScalingSizeX,
            ScalingSizeY,
            UIScale,
            GameplayDisableWinKey,
            DiscordRichPresence,
            CursorSize
        }

        public enum ScalingMode
        {
            Off,
            Everything,
            ExcludeOverlays
        }
    }
}