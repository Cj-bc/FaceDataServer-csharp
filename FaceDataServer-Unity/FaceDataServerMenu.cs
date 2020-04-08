using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using System.Linq;

namespace Cjbc.FaceDataServer.Unity {

    public class FaceDataServerMenu : MonoBehaviour
    {

        [MenuItem("FaceDataServer/Inject Required Layer & Parameters")]
        static void InjectRequiredLayerAndParameter() {
            
        }

        [MenuItem("FaceDataServer/Inject Required Layer & Parameters", true)]
        static bool ValidateInjectRequiredLayerAndParameter() {
            return Selection.assetGUIDs
                      .Select(id => AssetDatabase.GUIDToAssetPath(id))
                      .Select(path => AssetDatabase.GetMainAssetTypeAtPath(path))
                      .Any(t => t == typeof(AnimatorController));
        }

    }
}
