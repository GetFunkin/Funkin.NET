using System;
using Funkin.NET.Common.KeyBinds.ArrowKeys;
using Funkin.NET.Core.BackgroundDependencyLoading;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace Funkin.NET.Content.Elements.Composites
{
    public class ArrowKeyDrawable : CompositeDrawable, IBackgroundDependencyLoadable
    {
        public ArrowKeyDrawable(ArrowKeyAction arrowKey)
        {
            ArrowKey = arrowKey;
            ArrowSprite = new Sprite();
        }

        [Resolved] private TextureStore Textures { get; set; }
        public ArrowKeyAction ArrowKey { get; }
        public Sprite ArrowSprite { get; }
        
        [BackgroundDependencyLoader]
        void IBackgroundDependencyLoadable.BackgroundDependencyLoad()
        {
            // Get the arrow texture
            string arrowName = Enum.GetName(ArrowKey)?.ToUpperInvariant() ?? throw new ArgumentOutOfRangeException(nameof(ArrowKey));
            string textureName = "Arrow/arrow" + arrowName;
            ArrowSprite.Texture = Textures.Get(textureName);
            
            // Add the texture
            AddInternal(ArrowSprite);
        }
    }
}