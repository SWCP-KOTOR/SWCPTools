using HarmonyLib;

namespace SWCP.Core
{
    [HarmonyPatch(typeof(ThingDef), nameof(ThingDef.PlayerAcquirable), MethodType.Getter)]
    public static class ThingDef_PlayerAcquirable_Patch
    {
        public static void Postfix(ThingDef __instance, ref bool __result)
        {
            if (__instance.IsUniqueItemAndCreatedAlready())
            {
                __result = false;
            }
        }
    }
}
