using UnityEngine;

namespace SWCP.Core.Unity;

[StaticConstructorOnStartup]
public static class Shaders
{
    private static Dictionary<string, Shader> _lookupShaders;
    
    public static readonly Shader ZoomShader = LoadShader(Path.Combine("Assets", "Shaders", "ZoomShader.shader"));
    
    public static Shader LoadShader(string shaderName)
    {
        _lookupShaders ??= new Dictionary<string, Shader>();
        if (!_lookupShaders.ContainsKey(shaderName))
        {
            _lookupShaders[shaderName] = SWCPCoreMod.mod.MainBundle.LoadAsset<Shader>(shaderName);
        }
        
        Shader shader = _lookupShaders[shaderName];
        if (shader != null) 
            return shader;
        
        SWCPLog.Warning($"Could not load shader: {shaderName}");
        return ShaderDatabase.DefaultShader;
    }
}