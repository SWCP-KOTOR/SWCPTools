using UnityEngine;

namespace SWCP.Core.Unity;

[StaticConstructorOnStartup]
public static class Materials
{
    private static Dictionary<string, Material> _lookupMaterials;
    
    public static readonly Material ZoomMat = LoadMaterial(Path.Combine("Assets", "Shaders", "Unlit_ZoomShader.mat"));

    public static Material LoadMaterial(string materialName)
    {
        _lookupMaterials ??= new Dictionary<string, Material>();
        if (!_lookupMaterials.ContainsKey(materialName))
        {
            _lookupMaterials[materialName] = SWCPCoreMod.mod.MainBundle.LoadAsset<Material>(materialName);
        }

        Material mat = _lookupMaterials[materialName];
        if (mat != null) 
            return mat;
        
        SWCPLog.Warning($"Could not load material: {materialName}");
        return null;
    }
}