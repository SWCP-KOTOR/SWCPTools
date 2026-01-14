using LudeonTK;
using SWCP.Core.SectionLayer;
using UnityEngine;

namespace SWCP.Core.ThingComps
{
    internal class CompHideShipRoof : ThingComp
    {
        public CompProperties_HideShipRoof Props => (CompProperties_HideShipRoof)props;

        public bool HideParent => SWCP_Settings.showShipRoof;

        public override bool DontDrawParent()
        {
            return HideParent;
        }

        public override void ReceiveCompSignal(string signal)
        {
            if (signal == "VisibilityChanged")
            {
                parent.DirtyMapMesh(parent.Map);
                parent.Notify_ColorChanged();
                base.parent.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_ShipRoofBuildingsSimple));
                base.parent.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_Terrain));
            }
        }
    }
}
