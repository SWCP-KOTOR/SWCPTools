namespace SWCP.Core;

[DefOf]
public class SWCPDefOf
{
    [MayRequire("Rick.SWCP.Legion")]
    public static FactionDef SWCP_Faction_Caesars_Legion;
        
    [MayRequire("Rick.SWCP.NCR")]
    public static FactionDef SWCP_Faction_NCR;
    
    public static PawnGroupKindDef SWCP_PawnGroupKind_TaxCollector;
    public static LetterDef SWCP_Letter_AcceptStoryteller;
    public static JobDef SWCP_AICastAbilityAtTarget;
    
    public static JobDef SWCP_VATS_AttackHybrid;
    public static ThingDef SWCP_VATS_Zoomer;
    public static StatCategoryDef SWCP_LegendaryEffectStats;
    public static EffecterDef SWCP_VATSLegendaryEffect_Explosive_Explosion;

    public static LegendaryEffectDef SWCP_VATSLegendaryEffect_Rapid;
    
    static SWCPDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(SWCPDefOf));
    }
}