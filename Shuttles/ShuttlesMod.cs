using HarmonyLib;
using Verse;

namespace SWCP_Shuttles
{
    public class SWCP_ShuttlesMod : Mod
    {
        public SWCP_ShuttlesMod(ModContentPack pack) : base(pack)
        {
            new Harmony("SWCP_ShuttlesMod").PatchAll();
        }
    }
}