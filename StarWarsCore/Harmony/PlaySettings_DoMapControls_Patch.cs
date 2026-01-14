using HarmonyLib;
using SWCP.Core.SectionLayer;
using SWCP.Core.ThingComps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWCP.Core.Harmony
{
    internal class ShowShipRoofPatch
    {
        [HarmonyPatch(typeof(PlaySettings), "DoMapControls")]
        public static class PlaySettings_DoMapControls_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(WidgetRow row)
            {
                bool customToggle = SWCP_Settings.showShipRoof;
                Texture2D icon = ContentFinder<Texture2D>.Get("UI/ship_overlay_toggle") ?? BaseContent.BadTex; 
                row.ToggleableIcon(ref customToggle, icon, "SWCP_ShowShipRoof".Translate(), SoundDefOf.Mouseover_ButtonToggle);

                if (customToggle != SWCP_Settings.showShipRoof)
                {
                    SWCP_Settings.showShipRoof = customToggle;
                    NotifyAllComps();
                    
                }
            }

            private static void NotifyAllComps()
            {
                foreach (Map map in Find.Maps)
                {
                    foreach (Thing thing in map.listerThings.AllThings)
                    {
                        var comp = thing.TryGetComp<CompHideShipRoof>();
                        if (comp != null)
                        {
                            comp.ReceiveCompSignal("VisibilityChanged");
                        }
                    }
                }
            }
        }


    }
}
