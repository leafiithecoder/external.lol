using System.IO;
using BepInEx;

namespace externallol.Managers
{
    public static class FolderManager
    {
        public static readonly string BaseDirectory =
            Path.Combine(Directory.GetParent(Paths.PluginPath).Parent.FullName, "externallol");

        public static readonly string Sounds =
            Path.Combine(BaseDirectory, "Sounds");

        private static bool initialized;

        public static void Initialize()
        {
            if (initialized)
                return;

            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(Sounds);

            initialized = true;
        }
    }
}