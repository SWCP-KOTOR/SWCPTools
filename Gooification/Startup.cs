using UnityEngine;
using Verse;
using System.Text;
using Verse.AI;
using HarmonyLib;

namespace SWCP.Core
{
    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            new Harmony("SWCP.CoreGooification").PatchAll();
        }
    }
}

