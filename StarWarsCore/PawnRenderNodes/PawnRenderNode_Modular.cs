using SWCP.Core.ThingComps;
using System.Collections.Generic;
using UnityEngine;

namespace SWCP.Core.PawnRenderNodes
{
    internal class PawnRenderNode_Modular : PawnRenderNode_Apparel
    {
        public new PawnRenderNodeProperties_Modular Props => (PawnRenderNodeProperties_Modular)props;

        public PawnRenderNode_Modular(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
             : base(pawn, props, tree, null) { }

        public PawnRenderNode_Modular(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel)
            : base(pawn, props, tree, apparel) { }

        public PawnRenderNode_Modular(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel, bool useHeadMesh)
            : base(pawn, props, tree, apparel, useHeadMesh) { }

        protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
        {
            foreach (var armorSet in Props.modulararmorparts)
            {
                string graphicPath = !string.IsNullOrEmpty(armorSet.wornGraphicPath) ?
                    armorSet.wornGraphicPath :
                    armorSet.armorName;

                // Append body type if needed
                string fullGraphicPath = graphicPath;
                if (pawn != null && !string.IsNullOrEmpty(pawn.story?.bodyType?.defName))
                {
                    fullGraphicPath += "_" + pawn.story.bodyType.defName;
                }

                var graphic = GraphicDatabase.Get<Graphic_Multi>(
                    fullGraphicPath,
                    GetShader(armorSet.useWornGraphicMask),
                    Props.drawSize,
                    GetColor());

                yield return graphic;
            }
        }

        private Shader GetShader(bool useWornGraphicMask)
        {
            if (apparel.StyleDef?.graphicData.shaderType != null)
            {
                return apparel.StyleDef.graphicData.shaderType.Shader;
            }

            if ((apparel.StyleDef == null && apparel.def.apparel.useWornGraphicMask) ||
                (apparel.StyleDef != null && apparel.StyleDef.UseWornGraphicMask))
            {
                return ShaderDatabase.CutoutComplex;
            }

            return useWornGraphicMask ?
                ShaderDatabase.CutoutComplex :
                ShaderDatabase.Cutout;
        }

        private Color GetColor()
        {
            return apparel.DrawColor;
        }
    }

    public class PawnRenderNodeProperties_Modular : PawnRenderNodeProperties
    {
        public List<ModularArmorSet> modulararmorparts = new List<ModularArmorSet>();
        public PawnRenderNodeProperties_Modular() { }
    }

    public class PawnRenderNodeWorker_Modular : PawnRenderNodeWorker_Apparel_Body
    {
        protected override Graphic GetGraphic(PawnRenderNode node, PawnDrawParms parms)
        {
            Apparel wornApparel = node.apparel;
            var compModularArmor = wornApparel?.TryGetComp<Comp_ModularArmor>();

            if (compModularArmor != null &&
                compModularArmor.selectedModularArmorIndex >= 0 &&
                compModularArmor.selectedModularArmorIndex < node.Graphics.Count)
            {
                return node.Graphics[compModularArmor.selectedModularArmorIndex];
            }

            return node.Graphics.FirstOrDefault();
        }
    }
}