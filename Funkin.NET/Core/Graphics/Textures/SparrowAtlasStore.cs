using System.Collections.Generic;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuTK.Graphics.ES30;

namespace Funkin.NET.Core.Graphics.Textures
{
    /// <summary>
    ///     TextureStore designed for caching Textures loaded from a SparrowAtlas. Bypasses TextureStore-style atlas functionality.
    /// </summary>
    public class SparrowAtlasStore : ResourceStore<TextureUpload>
    {
        protected readonly Dictionary<string, Texture> TextureCache = new();
        protected readonly All FilteringMode;
        protected readonly bool ManualMipmaps;
        protected readonly object ReferenceCountLock = new();

        public SparrowAtlasStore(IResourceStore<TextureUpload> store = null, All filteringMode = All.Linear,
            bool manualMipmaps = false) : base(store)
        {
            FilteringMode = filteringMode;
            ManualMipmaps = manualMipmaps;

            // don't append png since we wanna read xml
            // we can read png files through the xml attributes
            AddExtension("xml");
        }
    }
}