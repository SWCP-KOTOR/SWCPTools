// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace SWCP.Core;

public class GroupMakerWithTraderKind : PawnGroupMaker
{
    public List<TraderKindDef> traderKinds = [];
    public List<CharacterDef> characterDefs = [];
    public float characterChance = 1f;
}
