namespace SWCP.Core;

public class CompProperties_LabelColored : CompProperties
{
    public Rarity rarity = Rarity.Common;

    public CompProperties_LabelColored()
    {
        compClass = typeof(CompLabelColored);
    }
}