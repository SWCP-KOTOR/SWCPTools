using HarmonyLib;
using Verse;

namespace SWCP.Core
{
    [HarmonyPatch(typeof(ThingStuffPair), nameof(ThingStuffPair.Commonality), MethodType.Getter)]
    public static class ThingStuffPair_Commonality_Patch
    {
        public static void Postfix(ThingStuffPair __instance, ref float __result)
        {
            if (__instance.thing.HasModExtension<UniqueThingExtension>())
            {
                __result = 0f;
            }
        }
    }
}
