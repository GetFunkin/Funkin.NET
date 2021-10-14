using Funkin.NET.Backgrounds;
using Funkin.NET.Intermediary.Screens;
using Funkin.NET.Resources;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Transforms;
using osuTK;

namespace Funkin.NET.Tests.Visual.Backgrounds
{
    public class RenderDesaturatedBackgroundTestScene : FunkinTestScene
    {
        private DefaultScreenStack Stack;
        private Track SampleTrack;

        [BackgroundDependencyLoader]
        private void Load(AudioManager audio)
        {
            SampleTrack = audio.GetTrackStore().Get(Tracks.Menu.FreakyMenu);
        }

        [Test]
        public void PushBackgrounds()
        {
            AddStep("push desaturated background", () =>
            {
                //if (Game.ScreenStack is not DefaultScreenStack defStack)
                //    throw new Exception("Malformed ScreenStack type.");

                //defStack.PushBackground(new SimpleBackgroundScreen(Textures.Backgrounds.DesaturatedMenu));
                
                if (Stack is null) 
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(new SimpleBackgroundScreen(Textures.Backgrounds.DesaturatedMenu));
            });

            AddStep("push gradient background", () =>
            {
                SimpleBackgroundScreen screen = new(Textures.Backgrounds.DesaturatedMenu);

                screen.OnUpdate += drawable =>
                {
                    drawable.Colour = ColourInfo.GradientHorizontal(Colour4.Red, Colour4.Yellow);
                };

                if (Stack is null)
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(screen);
            });

            Bindable<float> amp = new BindableFloat();

            AddStep("push flowing gradient background", () =>
            {
                SampleTrack.Restart();
                SampleTrack.Start();

                SimpleBackgroundScreen screen = new(Textures.Backgrounds.DesaturatedMenu);

                screen.OnUpdate += drawable =>
                {
                    if (amp.Value < SampleTrack.CurrentAmplitudes.Average)
                        amp.Value += 0.01f;
                    else
                        amp.Value -= 0.01f;

                    ColourInfo gradient = ColourInfo.GradientVertical(Colour4.Yellow, Colour4.Red);
                    gradient.TopRight = gradient.TopLeft = gradient.Interpolate(new Vector2(amp.Value * 0.5f - 0.25f)) * gradient.TopRight;

                    drawable.Colour = gradient;
                };

                if (Stack is null)
                    LoadComponentAsync(Stack = new DefaultScreenStack(), Add);

                Stack.PushBackground(screen);
            });
        }
    }
}