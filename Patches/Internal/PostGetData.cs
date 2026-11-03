using GorillaNetworking;
using GorillaNetworking.Store;
using HarmonyLib;
using static externallol.Menu.Main;

namespace externallol.Patches.Internal
{
    [HarmonyPatch(typeof(BundleManager), nameof(BundleManager.CheckIfBundlesOwned))]
    public class PostGetData
    {
        public static bool CosmeticsInitialized;
        private static void Postfix()
        {
            CosmeticsInitialized = true;
            CosmeticsOwned = CosmeticsController.instance.concatStringCosmeticsAllowed;
        }
    }
}
