using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Funkin.NET.Core.API.Loaders;
using Funkin.NET.Core.API.ModTypes;
using Funkin.NET.Core.ManiaContent;
using osu.Framework.Platform;

namespace Funkin.NET.Core.API
{
    public static class ModLoader
    {
        public delegate void ModLoadStatus(string status);

        public static ReadOnlyCollection<IMod> Mods => LoadedMods.AsReadOnly();

        internal static List<IMod> LoadedMods = new();

        public static List<IModTypeLoader> Loaders = new();

        public static bool FinishedLoadingMods { get; private set; }

        public static bool FinishedLoadingContent { get; private set; }

        internal static void LoadMods(ModLoadStatus status, Storage probe)
        {
            status?.Invoke("Loading Default (Mania) Mod");
            LoadedMods.Add(new ManiaMod());

            status?.Invoke("Locating other mods to load...");
            Directory.CreateDirectory(probe.GetFullPath("Mods"));
            List<string> dirs = probe.GetDirectories("Mods").ToList();
            List<string> loadedMods = new();

            if (dirs.Count > 0) 
                status?.Invoke("Found directories...");
            else
            {
                status?.Invoke("Found no directories.");
                FinishedLoadingMods = true;
                return;
            }

            status?.Invoke("Searching directories for loadable assemblies...");
            foreach (string dir in dirs)
            {
                IEnumerable<string> files = probe.GetFiles(dir);
                string expected = Path.Combine(dir, dir.Split(Path.DirectorySeparatorChar)[1] + ".dll");

                if (files.Contains(expected))
                    loadedMods.Add(expected);
                else
                    status?.Invoke("No loadable dll found in: " + dir);
            }

            status?.Invoke("Finished searching directories for loadable assemblies.");
            status?.Invoke($"\nFound {loadedMods.Count} mods in {dirs.Count} folders.");
            status?.Invoke("\nProceeding to load assemblies.");

            foreach (string mod in loadedMods)
            {
                Assembly modAssembly = Assembly.LoadFile(probe.GetFullPath(mod));
                Type[] modTypes = modAssembly
                    .GetTypes()
                    .Where(x => typeof(IMod).IsAssignableFrom(x) &&
                                !x.IsAbstract &&
                                x.GetConstructor(Array.Empty<Type>()) != null)
                    .ToArray();

                switch (modTypes.Length)
                {
                    case 0:
                        status?.Invoke("No IMod types: " + modAssembly.GetName().Name);
                        continue;

                    case > 1:
                        status?.Invoke("More than one IMod type: " + modAssembly.GetName().Name);
                        continue;
                }

                IMod loadedMod = (IMod) Activator.CreateInstance(modTypes[0]);

                if (loadedMod is null)
                {
                    status?.Invoke("Loaded mod was null: " + modAssembly.GetName().Name);
                    continue;
                }

                loadedMod.Assembly = modAssembly;

                status?.Invoke("Loaded mod: " + modAssembly.GetName().Name);
                LoadedMods.Add(loadedMod);
            }

            status?.Invoke("Mod loading complete!");

            FinishedLoadingMods = true;

            LoadContent(status);
        }

        // TODO
        internal static void LoadContent(ModLoadStatus status)
        {
            foreach (IMod mod in LoadedMods)
            {
                status?.Invoke("Caching content in mod: " + mod.Identity.Name);

                foreach (Type type in mod.GetLoadableContent())
                {
                    status?.Invoke("Found loadable:" + type.Name);

                    ILoadable loadable = (ILoadable)Activator.CreateInstance(type);
                    mod.AddContent(loadable);
                }
            }

            status?.Invoke("Loading content in mods.");

            foreach (IMod mod in LoadedMods)
            {
                status?.Invoke("Loading content in mod: " + mod.Identity.Name);

                foreach (IIdentifiable identifiable in mod.Content.Values)
                {
                    if (identifiable is ILoadable loadable)
                        loadable.Load();    

                    if (identifiable is not IModType modType)
                        continue;

                    modType.Mod = mod;

                    IModTypeLoader loader = Loaders.FirstOrDefault(x => x.CanLoadType(modType.GetType()));

                    if (loader is null)
                        throw new Exception("Cannot finder loader for type: " + modType.GetType().FullName);

                    loader.DoLoad(mod, modType);
                }
            }

            FinishedLoadingContent = true;
        }
    }
}