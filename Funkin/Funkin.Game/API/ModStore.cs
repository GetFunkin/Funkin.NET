using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace Funkin.Game.API
{
    /// <summary>
    ///     A class that handles collecting and loading mods.
    /// </summary>
    public sealed class ModStore : IDisposable
    {
        private readonly Dictionary<Assembly, Type> loadedAssemblies = new();

        public IEnumerable<IMod> AvailableMods => availableMods;

        private readonly List<IMod> availableMods = new();

        public ModStore(Storage? storage = null)
        {
            if (RuntimeInfo.StartupDirectory is not null)
                loadFromDisk();

            AppDomain.CurrentDomain.AssemblyResolve += resolveModDependencyAssembly;

            Storage? modStorage = storage?.GetStorageForDirectory("mods");

            if (modStorage is not null)
                loadUserMods(modStorage);

            availableMods.AddRange(
                loadedAssemblies.Values
                                .Select(x => Activator.CreateInstance(x) as IMod)
                                .Where(x => x is not null)
                                .Select(x => x.AsNonNull())
            );

            foreach (IMod mod in availableMods)
                mod.Load(); // initial load
        }

        private void loadFromDisk()
        {
            try
            {
                string[] files = Directory.GetFiles(RuntimeInfo.StartupDirectory, $"{Constants.MOD_LIBRARY_PREFIX}.*.dll");

                foreach (string file in files.Where(x => !Path.GetFileName(x).Contains("Tests")))
                    loadModFromFile(file);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Could not load file from directory {RuntimeInfo.StartupDirectory}.");
            }
        }

        private void loadUserMods(Storage modStorage)
        {
            IEnumerable<string> mods = modStorage.GetFiles(".", $"{Constants.MOD_LIBRARY_PREFIX}.*.dll");

            foreach (string mod in mods.Where(x => !x.Contains("Tests")))
                loadModFromFile(modStorage.GetFullPath(mod));
        }

        private void loadModFromFile(string file)
        {
            string? name = Path.GetFileNameWithoutExtension(file);

            if (loadedAssemblies.Values.Any(x => Path.GetFileNameWithoutExtension(x.Assembly.Location) == name))
                return;

            try
            {
                addMod(Assembly.LoadFrom(file));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed to load ruleset {name}.");
            }
        }

        private void addMod(Assembly assembly)
        {
            if (loadedAssemblies.ContainsKey(assembly))
                return;

            if (loadedAssemblies.Any(x => x.Key.FullName == assembly.FullName))
                return;

            try
            {
                loadedAssemblies[assembly] = assembly.GetTypes().First(x => x.IsPublic && !x.IsAbstract && x.IsAssignableTo(typeof(IMod)));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed to add mod {assembly}.");
            }
        }

        private Assembly? resolveModDependencyAssembly(object? sender, ResolveEventArgs args)
        {
            Assembly? domainAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(x =>
                                                {
                                                    string? name = x.GetName().Name;

                                                    return name is not null && args.Name.Contains(name, StringComparison.Ordinal);
                                                })
                                                .OrderByDescending(x => x.GetName().Version)
                                                .FirstOrDefault();

            return domainAssembly ?? loadedAssemblies.Keys.FirstOrDefault(x => x.FullName == new AssemblyName(args.Name).FullName);
        }

        public void Dispose() => AppDomain.CurrentDomain.AssemblyResolve -= resolveModDependencyAssembly;
    }
}
