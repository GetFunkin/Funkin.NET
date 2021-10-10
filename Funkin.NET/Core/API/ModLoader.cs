using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Funkin.NET.Core.ManiaContent;
using osu.Framework.Platform;

namespace Funkin.NET.Core.API
{
    public static class ModLoader
    {
        public delegate void ModLoadStatus(string status);

        public static ReadOnlyCollection<IMod> Mods => LoadedMods.AsReadOnly();

        internal static List<IMod> LoadedMods = new();

        public static bool FinishedLoading { get; private set; }

        internal static void LoadMods(ModLoadStatus status, Storage probe)
        {
            status.Invoke("Loading Default (Mania) Mod");
            LoadedMods.Add(new ManiaMod());

            status.Invoke("Locating other mods to load...");
            Directory.CreateDirectory(probe.GetFullPath("Mods"));
            List<string> dirs = probe.GetDirectories("Mods").ToList();
            List<string> loadedMods = new();

            if (dirs.Count > 0) 
                status.Invoke("Found directories...");
            else
            {
                status.Invoke("Found no directories.");
                FinishedLoading = true;
                return;
            }

            status.Invoke("Searching directories for loadable assemblies...");
            foreach (string dir in dirs)
            {
                if (probe.GetFiles(dir).Contains(dir + ".dll"))
                    loadedMods.Add(Path.Combine(dir, dir + ".dll"));
                else
                    status.Invoke("No loadable dll found in: " + dir);
            }

            status.Invoke("Finished searching directories for loadable assemblies.");
            status.Invoke($"\nFound {loadedMods.Count} mods in {dirs.Count} folders.");
            status.Invoke("\nProceeding to load assemblies.");

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
                        status.Invoke("Could not load mod because there are no IMod types: " + modAssembly.FullName);
                        continue;

                    case > 1:
                        status.Invoke("Could not load mod because there is more than one IMod type: " + modAssembly.FullName);
                        continue;
                }

                IMod loadedMod = (IMod) Activator.CreateInstance(modTypes[0]);

                if (loadedMod is null)
                {
                    status.Invoke("Could not load mod because loaded mod was null: " + modAssembly.FullName);
                    continue;
                }

                loadedMod.Assembly = modAssembly;

                status.Invoke("Loaded mod: " + modAssembly.FullName);
                LoadedMods.Add(loadedMod);
            }

            FinishedLoading = true;
        }

        // TODO
        internal static void LoadContent(ModLoadStatus status)
        {
        }
    }
}