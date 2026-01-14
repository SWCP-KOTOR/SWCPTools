using LudeonTK;
using RimWorld;
using SWCP.Core.ThingComps;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SWCP.Core.SectionLayer
{
    public class SectionLayer_ShipRoofBuildingsSimple : Verse.SectionLayer
    {
        public SectionLayer_ShipRoofBuildingsSimple(Section section)
            : base(section)
        {
            relevantChangeTypes = MapMeshFlagDefOf.Buildings;
        }

        public override void DrawLayer()
        {
            if (SWCP_Settings.showShipRoof)
            {
                base.DrawLayer();
            }
        }

        public override void Regenerate()
        {
            ClearSubMeshes(MeshParts.All);

            foreach (IntVec3 cell in section.CellRect)
            {
                List<Thing> thingList = base.Map.thingGrid.ThingsListAt(cell);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Thing thing = thingList[i];
                    if (thing is Building building)
                    {
                        CompHideShipRoof comp = building.GetComp<CompHideShipRoof>();
                        if (comp != null && comp.HideParent)
                        {
                            PrintForOverlay(this, building, comp);
                        }
                    }
                }
            }

            FinalizeMesh(MeshParts.All);
        }

        [TweakValue("SWCPShipLayer", -450f, 450)]
        public static float Layer = 0f;

        [TweakValue("SWCPShipLayer", 0f, 40)]
        public static float AltLevel = 0f;

        private void PrintForOverlay(Verse.SectionLayer layer, Thing parent, CompHideShipRoof comp)
        {
            foreach (IntVec3 cell in parent.OccupiedRect())
            {
                Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.Terrain) +
                               new Vector3(0f, -0.35f, 0f);

                Material material = GetRoofMaterialForBuilding(parent, cell);
                material.renderQueue = 389;

                Printer_Plane.PrintPlane(layer, center, parent.DrawSize, material, parent.Rotation.AsAngle);
            }
        }

        private Material GetRoofMaterialForBuilding(Thing building, IntVec3 cell)
        {
            return building.Graphic?.MatAt(building.Rotation, building) ?? BaseContent.BadMat;
        }
    }
}