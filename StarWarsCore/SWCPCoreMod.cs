using System.Runtime.InteropServices;
using HarmonyLib;
using UnityEngine;

namespace SWCP.Core;

public class SWCPCoreMod : Mod
{
    public static SWCPCoreMod mod;
    public static SWCP_Settings Settings;
    public static HarmonyLib.Harmony harmony;
    public AssetBundle bundleInt;
    
    public SWCPCoreMod(ModContentPack content) : base(content)
    {
        harmony = new HarmonyLib.Harmony("SWCP.Core.Patches"); // PatchesUwU ~ Steve
        mod = this;
        Settings = GetSettings<SWCP_Settings>();
        SWCPLog.Warning("Beta version: bugs likely, if not guaranteed! " +
                       "Report bugs on steam workshop page or on discord: 3HEXN3Qbn4");
    }

    public static void PatchAll()
    {
        AccessTools.GetTypesFromAssembly(typeof(SWCPCoreMod).Assembly).Do(delegate (Type type)
        {
            try
            {
                harmony.CreateClassProcessor(type).Patch();
            }
            catch (Exception e)
            {
                Log.Error("Error patching " + type + " - " + e.ToString());
            }
        });
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        Settings.DoWindowContents(inRect);
    }
    
    public override string SettingsCategory()
    {
        return "SWCP_Settings_Category".Translate();
    }

    public AssetBundle MainBundle
    {
        get
        {
            if(bundleInt != null) return bundleInt;

            string text = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                text = "StandaloneOSX";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                text = "StandaloneWindows64";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                text = "StandaloneLinux64";
            }

            string bundlePath = Path.Combine(Content.RootDir, $@"SWCP-UnityAssets\Materials\{text}\SWCPshaders");
            SWCPLog.Message("Bundle Path: " + bundlePath);

            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

            if (bundle == null)
            {
                SWCPLog.Message("Failed to load bundle at path: " + bundlePath);
            }

            bundleInt = bundle;

            return bundleInt;
        }
    }
}