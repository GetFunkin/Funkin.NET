using System;
using Funkin.NET.Common.KeyBinds.ArrowKeys;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Content.Elements.Composites
{
    public class ArrowKeyDrawable : CompositeDrawable, IBackgroundDependencyLoadable
    {
        // Notes:
        // "x press" is for when you incorrectly press a key, or continue holding a key after key ends
        // "x confirm" is for when you correctly press a key, and continue holding while the key hasn't ended
        
        public ArrowKeyDrawable(ArrowKeyAction arrowKey)
        {
            ArrowKey = arrowKey;
            ArrowIdleSprite = new Sprite
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre
            };
            ArrowPressAnim = new TextureAnimation
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                IsPlaying = false,
                Loop = false 
            };
            ArrowConfirmAnim = new TextureAnimation
            {
                Origin = Anchor.Centre,
                Anchor = Anchor.Centre,
                IsPlaying = false,
                Loop = false
            };
        }

        [Resolved] private TextureStore Textures { get; set; }
        public ArrowKeyAction ArrowKey { get; }
        public Sprite ArrowIdleSprite { get; }
        public TextureAnimation ArrowPressAnim { get; }
        public TextureAnimation ArrowConfirmAnim { get; }
        
        
        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            string keyName = Enum.GetName(ArrowKey)?.ToUpperInvariant() ?? throw new ArgumentOutOfRangeException(nameof(ArrowKey));
            
            // Get the arrow texture
            string arrowName = keyName;
            string textureName = "Arrow/arrow" + arrowName;
            ArrowIdleSprite.Texture = Textures.Get(textureName);

            keyName = keyName.ToLowerInvariant();
            for (int i = 0; i < 4; i++)
            {
                ArrowPressAnim.AddFrame(Textures.Get($"Arrow/{keyName} press{i}"));
                ArrowConfirmAnim.AddFrame(Textures.Get($"Arrow/{keyName} confirm{i}"));
            }
            
            // Add the textures
            AddInternal(ArrowIdleSprite);
            AddInternal(ArrowPressAnim);
            AddInternal(ArrowConfirmAnim);
            
            ArrowPressAnim.Hide();
            ArrowConfirmAnim.Hide();
        }
    }
}