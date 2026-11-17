using HarmonyLib;

namespace externallol.Patches.Internal
{
	public class RigPatches
	{
		public class OnDisable
		{
			public static bool Prefix(VRRig __instance)
			{
				return __instance != null && !__instance.isLocal;
			}
		}
		public class Awake
		{
			public static bool Prefix(VRRig __instance)
			{
				return __instance?.gameObject != null &&
					   __instance.gameObject.name != "Local Gorilla Player(Clone)";
			}
		}
		public class PostTick
		{
			public static bool Prefix(VRRig __instance)
			{
				return __instance != null &&
					   (!__instance.isLocal || __instance.enabled);
			}
		}
	}
}
