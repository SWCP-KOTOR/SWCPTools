using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace SWCP.Core.Unity
{
    [StaticConstructorOnStartup]
    public static class Materials
    {
        private static Dictionary<string, Material> _lookupMaterials;
        public static readonly Material ZoomMat;

        static Materials()
        {
            _lookupMaterials = new Dictionary<string, Material>();
            try
            {
                ZoomMat = LoadMaterial(Path.Combine("Assets", "Shaders", "Unlit_ZoomShader.mat"));
            }
            catch (System.Exception ex)
            {
                SWCPLog.Error($"Failed to initialize materials: {ex}");
                ZoomMat = null;
            }
        }

        public static Material LoadMaterial(string materialName)
        {
            if (string.IsNullOrEmpty(materialName))
            {
                SWCPLog.Warning("Material name is null or empty");
                return null;
            }

            if (_lookupMaterials.TryGetValue(materialName, out var existingMat))
            {
                return existingMat;
            }

            if (SWCPCoreMod.mod?.MainBundle == null)
            {
                return null;
            }

            var material = SWCPCoreMod.mod.MainBundle.LoadAsset<Material>(materialName);
            if (material == null)
            {
                SWCPLog.Warning($"Could not load material: {materialName}");
                return null;
            }

            _lookupMaterials[materialName] = material;
            return material;
        }
    }
}