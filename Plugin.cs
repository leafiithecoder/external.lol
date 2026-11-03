using System.ComponentModel;
using BepInEx;
using externallol.Classes;
using externallol.Managers;
using externallol.Patches;

namespace externallol;

[Description(PluginInfo.Description)]
[BepInPlugin(
        PluginInfo.GUID,
        PluginInfo.Name,
        PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Preferences.Load();
        
        gameObject.AddComponent<CoroutineManager>();

        GorillaTagger.OnPlayerSpawned(
                OnPlayerSpawned);
    }

    private void OnApplicationQuit() { Tools.Utilities.Shutdown(); }

    private void OnPlayerSpawned()
    {
        PatchHandler.PatchAll();

        Preferences.ApplyButtonStates();
    }
}