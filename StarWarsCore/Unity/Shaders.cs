using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace SWCP.Core.Unity
{
    [StaticConstructorOnStartup]
    public static class Shaders
    {
        private static readonly Dictionary<string, Shader> _lookupShaders = new Dictionary<string, Shader>();
        public static readonly Shader ZoomShader;

        static Shaders()
        {
            ZoomShader = LoadShader(Path.Combine("Assets", "Shaders", "ZoomShader.shader"));
        }

        public static Shader LoadShader(string shaderName)
        {
            if (!_lookupShaders.TryGetValue(shaderName, out var shader))
            {
                if (SWCPCoreMod.mod?.MainBundle == null)
                {
                    SWCPLog.Warning($"MainBundle is not initialized, cannot load shader: {shaderName}");
                    return ShaderDatabase.DefaultShader;
                }

                shader = SWCPCoreMod.mod.MainBundle.LoadAsset<Shader>(shaderName);
                if (shader != null)
                {
                    _lookupShaders[shaderName] = shader;
                }
            }

            return shader ?? ShaderDatabase.DefaultShader;
        }
    }
}