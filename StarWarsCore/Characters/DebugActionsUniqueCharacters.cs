using HarmonyLib;
using LudeonTK;

namespace SWCP.Core;

public static class DebugActionsUniqueCharacters
{
    public const string CategoryName = "SWCP: Characters";
    
    [DebugAction(CategoryName, "Spawn Character", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
    private static void GenerateCharacter()
    {
        var charOptions = new List<DebugMenuOption>();

        foreach (CharacterDef def in DefDatabase<CharacterDef>.AllDefsListForReading)
        {
            charOptions.Add(new DebugMenuOption(def.defName, DebugMenuOptionMode.Tool, delegate
            {
                IntVec3 cell = UI.MouseCell();
                Pawn pawn = UniqueCharactersTracker.Instance.GetOrGenPawn(def);
                GenSpawn.Spawn(pawn, cell, Find.CurrentMap);
            }));
        }
        
        Find.WindowStack.Add(new Dialog_DebugOptionListLister(charOptions, "Characters"));
    }
    
    [DebugAction(CategoryName, "Log Characters", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing)]
    private static void LogCharacters()
    {
        var characterDefs = DefDatabase<CharacterDef>.AllDefsListForReading;

        SWCPLog.Message("Logging Characters: ");
        foreach (CharacterDef charDef in characterDefs)
        {
            SWCPLog.Message($"{charDef.defName} with {charDef.definitions.Count} definitions and {charDef.roles.Count} roles");
            SWCPLog.Message($"{charDef.defName} definitions: ");
            foreach (CharacterBaseDefinition definition in charDef.definitions)
            {
                SWCPLog.Message($"- {definition.GetType().Name}");
            }
            SWCPLog.Message($"{charDef.defName} roles: ");
            foreach (CharacterRole role in charDef.roles)
            {
                SWCPLog.Message($"- {role.GetType().Name}");
            }
        }
    }
    
    [DebugAction(CategoryName, "Log Roles", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.Playing)]
    private static void LogRoles()
    {
        SWCPLog.Message("Logging Roles: ");
        foreach (Type roleType in CharacterRoleUtils.GetAllRoleTypes())
        {
            var charactersWithRole = DefDatabase<CharacterDef>.AllDefsListForReading
                .Where(x => x.roles.Any(role => role.GetType() == roleType)).ToList();
            
            SWCPLog.Message($"Role Type: {roleType.Name} ({charactersWithRole.Count}):\nChars: {charactersWithRole.Join()}");
        }
    }
}