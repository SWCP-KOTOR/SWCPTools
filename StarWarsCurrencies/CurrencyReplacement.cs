

// ReSharper disable ClassNeverInstantiated.Global

namespace SWCP.Currencies;

public class CurrencyReplacement : DefModExtension
{
    public ThingDef currency;

    public static CurrencyReplacement Get(Def def)
    {
        return def.GetModExtension<CurrencyReplacement>();
    }
}