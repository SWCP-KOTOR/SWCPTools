using SWCP.Core.PawnRenderNodes;
using System.Text;
using UnityEngine;

namespace SWCP.Core.ThingComps
{
    public class Comp_ModularArmor : ThingComp
    {
        public int selectedModularArmorIndex = 0;
        public string selectedModularArmorPart;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref selectedModularArmorPart, "selectedModularArmorPart");
            Scribe_Values.Look(ref selectedModularArmorIndex, "selectedModularArmorIndex");
        }

        public List<ModularArmorSet> GetModularArmorSets()
        {
            if (parent.def.apparel?.RenderNodeProperties != null)
            {
                foreach (var properties in parent.def.apparel.RenderNodeProperties)
                {
                    if (properties is PawnRenderNodeProperties_Modular renderNodeProps &&
                        renderNodeProps.modulararmorparts != null)
                    {
                        return renderNodeProps.modulararmorparts;
                    }
                    if (properties is PawnRenderNodeProperties_ModularHead renderNodePropsHead &&
                        renderNodePropsHead.modulararmorparts != null)
                    {
                        return renderNodePropsHead.modulararmorparts;
                    }
                }
            }
            return new List<ModularArmorSet>();
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            var armorSets = GetModularArmorSets();
            if (armorSets.Count > 1)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Select Armor Part",
                    defaultDesc = "Choose an armor part from the modular options.",
                    icon =  BaseContent.BadTex,
                    action = () => Find.WindowStack.Add(new Dialog_ModularArmorSelector(this))
                };
            }
        }

        public override float GetStatFactor(StatDef stat)
        {
            return 1f;
        }

        public override float GetStatOffset(StatDef stat)
        {
            float offset = 0f;
            var armorSets = GetModularArmorSets();

            if (selectedModularArmorIndex >= 0 && selectedModularArmorIndex < armorSets.Count)
            {
                var selectedSet = armorSets[selectedModularArmorIndex];
                if (selectedSet.equippedStatOffsets != null)
                {
                    foreach (var statMod in selectedSet.equippedStatOffsets)
                    {
                        if (statMod.stat == stat)
                        {
                            offset = statMod.value;
                            Log.Message("Increasing Stat");
                        }
                    }
                }
            }
            return offset;
        }

        public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
        {
            var armorSets = GetModularArmorSets();
            if (selectedModularArmorIndex >= 0 && selectedModularArmorIndex < armorSets.Count)
            {
                var selectedSet = armorSets[selectedModularArmorIndex];
                if (selectedSet.equippedStatOffsets != null)
                {
                    bool hasStat = false;

                    foreach (var statMod in selectedSet.equippedStatOffsets)
                    {
                        if (statMod.stat == stat)
                        {
                            if (!hasStat)
                            {
                                sb.AppendLine(whitespace + "Modular Armor (" + selectedSet.armorName + "):");
                                hasStat = true;
                            }
                            sb.AppendLine(whitespace + "  - " + statMod.stat.LabelCap + ": " +
                                statMod.value.ToStringByStyle(statMod.stat.toStringStyle, statMod.stat.toStringNumberSense));
                        }
                    }
                }
            }
        }
    }


    public class CompProperties_ModularArmor : CompProperties
    {
        public CompProperties_ModularArmor()
        {
            this.compClass = typeof(Comp_ModularArmor);
        }
    }

    public class ModularArmorSet
    {
        public string armorName;
        public string uiIcon = BaseContent.BadTexPath;
        public string wornGraphicPath = "";
        public bool useWornGraphicMask;
        public List<StatModifier> equippedStatOffsets;
        [NoTranslate]
        public List<string> tags = new List<string>();
    }

    public class Dialog_ModularArmorSelector : Window
    {
        private Comp_ModularArmor compModularArmor;
        private List<ModularArmorSet> armorSets;
        private Vector2 scrollPosition;
        private float scrollViewHeight;

        public override Vector2 InitialSize => new Vector2(600f, 500f);

        public Dialog_ModularArmorSelector(Comp_ModularArmor comp)
        {
            this.compModularArmor = comp;
            this.armorSets = comp.GetModularArmorSets();
            this.scrollViewHeight = armorSets.Count * 45f;
            doCloseButton = true;
            closeOnClickedOutside = true;
            scrollPosition = Vector2.zero;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float lineHeight = Text.LineHeight;
            float buttonHeight = 40f;
            float textureSize = 250f;

            // Title
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(0f, 0f, inRect.width, lineHeight), "Select Armor Part");
            Text.Font = GameFont.Small;

            // Preview area
            Rect texturePreviewRect = new Rect(
                (inRect.width - textureSize) / 2,
                (inRect.height - textureSize) / 2 - 40f,
                textureSize, textureSize
            );

            // Show current selection
            if (compModularArmor.selectedModularArmorIndex >= 0 &&
                compModularArmor.selectedModularArmorIndex < armorSets.Count)
            {
                var selectedSet = armorSets[compModularArmor.selectedModularArmorIndex];
                Texture2D previewTexture = ContentFinder<Texture2D>.Get(selectedSet.uiIcon);

                if (previewTexture != null)
                {
                    Widgets.DrawTextureFitted(texturePreviewRect, previewTexture, 1.0f);
                }
                else
                {
                    Widgets.Label(texturePreviewRect, "No Preview");
                }
            }
            else
            {
                Widgets.Label(texturePreviewRect, "No Armor Selected");
            }

            // Default button
            Rect defaultButtonRect = new Rect(
                (inRect.width - textureSize) / 2,
                texturePreviewRect.yMin - buttonHeight - 10f,
                textureSize,
                buttonHeight
            );

            if (Widgets.ButtonText(defaultButtonRect, "Default Armor"))
            {
                compModularArmor.selectedModularArmorIndex = 0;
                compModularArmor.selectedModularArmorPart = armorSets[0].armorName;
                compModularArmor.parent.Notify_ColorChanged();
            }

            // Selection list
            float scrollAreaHeight = inRect.height - texturePreviewRect.yMax - 30f;
            Rect scrollOutRect = new Rect(0f, texturePreviewRect.yMax + 10f, inRect.width, scrollAreaHeight);
            Rect scrollViewRect = new Rect(0f, 0f, inRect.width - 16f, scrollViewHeight);

            Widgets.BeginScrollView(scrollOutRect, ref scrollPosition, scrollViewRect);

            for (int i = 0; i < armorSets.Count; i++)
            {
                var set = armorSets[i];
                Rect buttonRect = new Rect(0f, i * (buttonHeight + 5f), inRect.width - 20f, buttonHeight);

                if (set.uiIcon != null)
                {
                    Rect iconRect = new Rect(buttonRect.x + 5f, buttonRect.y + 5f, 30f, 30f);
                    GUI.DrawTexture(iconRect, ContentFinder<Texture2D>.Get(set.uiIcon));
                }

                if (Widgets.ButtonText(buttonRect, set.armorName))
                {
                    compModularArmor.selectedModularArmorIndex = i;
                    compModularArmor.selectedModularArmorPart = set.armorName;
                    compModularArmor.parent.Notify_ColorChanged();
                }
            }

            Widgets.EndScrollView();
        }
    }
}