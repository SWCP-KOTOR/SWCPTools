using SWCP.Core.ThingComps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWCP.Core.PawnRenderNodes
{
    internal class PawnRenderNode_ModularHeadgear : PawnRenderNode_Apparel
    {
        public new PawnRenderNodeProperties_ModularHead Props => (PawnRenderNodeProperties_ModularHead)props;


        public PawnRenderNode_ModularHeadgear(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
             : base(pawn, props, tree, null)
        {
        }

        protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
        {
            foreach (var armorSet in Props.modulararmorparts)
            {
                string graphicPath = !string.IsNullOrEmpty(armorSet.wornGraphicPath) ?
                    armorSet.wornGraphicPath :
                    armorSet.armorName;

                var graphic = GraphicDatabase.Get<Graphic_Multi>(
                    graphicPath,
                    GetShader(),
                    Props.drawSize,
                    GetColor());

                yield return graphic;
            }
        }

        private Shader GetShader()
        {
            Shader shader = ShaderDatabase.Cutout;
            if (apparel.StyleDef?.graphicData.shaderType != null)
            {
                shader = apparel.StyleDef.graphicData.shaderType.Shader;
            }
            else if ((apparel.StyleDef is null && apparel.def.apparel.useWornGraphicMask) || (apparel.StyleDef is not null && apparel.StyleDef.UseWornGraphicMask))
            {
                shader = ShaderDatabase.CutoutComplex;
            }
            return shader;
        }
        private Color GetColor()
        {
            return apparel.DrawColor;
        }
    }

    public class PawnRenderNodeProperties_ModularHead : PawnRenderNodeProperties
    {
        public List<ModularArmorSet> modulararmorparts = new List<ModularArmorSet>();
        public PawnRenderNodeProperties_ModularHead()
        {

        }
    }

    public class PawnRenderNodeWorker_ModularHead : PawnRenderNodeWorker_Apparel_Head
    {
        protected override Graphic GetGraphic(PawnRenderNode node, PawnDrawParms parms)
        {
            Apparel wornApparel = node.apparel;
            var compModularArmor = wornApparel?.TryGetComp<Comp_ModularArmor>();

            if (compModularArmor != null)
            {
                if (compModularArmor.selectedModularArmorIndex == 0)
                {
                    return node.Graphics[compModularArmor.selectedModularArmorIndex];
                }
                else
                {
                    if (compModularArmor.selectedModularArmorIndex >= 0 && compModularArmor.selectedModularArmorIndex < node.Graphics.Count)
                    {
                        return node.Graphics[compModularArmor.selectedModularArmorIndex];
                    }
                }
            }
            return (Graphic)node.Graphics;
        }

    }
}
