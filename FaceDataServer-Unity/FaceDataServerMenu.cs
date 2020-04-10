using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using System.Linq;
using Cjbc.FaceDataServer.Unity.Exceptions;

namespace Cjbc.FaceDataServer.Unity {

    public class FaceDataServerMenu : MonoBehaviour
    {
        static AnimatorControllerLayer layer = null;
        public static string FDS_LayerName = "FDS_faceRotation";
        public static string XRotationParameterName = "FDS_X_Rotation";
        public static string YRotationParameterName = "FDS_Y_Rotation";
        public static string ZRotationParameterName = "FDS_Z_Rotation";

        /// <summary>Inject layer and Animation Parameters required by FDS</summary>
        [MenuItem("FaceDataServer/Inject Required Layer & Parameters")]
        static void InjectRequiredLayerAndParameter() {
            IEnumerable<AnimatorController> controllers = Selection.assetGUIDs
                                                .Select(id => AssetDatabase.GUIDToAssetPath(id))
                                                .Select(path => (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController)));

            foreach(var controller in controllers) {
                InjectLayer(controller);
                InjectParameters(controller);
                EditorUtility.SetDirty(controller);
            }

            AssetDatabase.SaveAssets();
        }

        [MenuItem("FaceDataServer/Inject Required Layer & Parameters", true)]
        static bool ValidateInjectRequiredLayerAndParameter() {
            return Selection.assetGUIDs
                      .Select(id => AssetDatabase.GUIDToAssetPath(id))
                      .Select(path => AssetDatabase.GetMainAssetTypeAtPath(path))
                      .Any(t => t == typeof(AnimatorController));
        }



        /// <summary>Inject FDS layer to given controller, if it doesn't have one</summary>
        static void InjectLayer(AnimatorController c) {
            if(c.layers.Any(l => l.name == FDS_LayerName)) {
                Debug.Log($"The AnimatorController '{c.ToString()}' already have layer '{FDS_LayerName}'. Stop Injecting to this");
                return;
            }

            // Use memorized one
            if(layer != null) {
                c.AddLayer(layer);
                return;
            }

            // Creating Layer {{{
            // BlendTree configuration {{{2
            BlendTree xRotationTree = CreateChild("xRotationTree", "FDS_LookUp"  , "FDS_LookDown", XRotationParameterName);
            BlendTree yRotationTree = CreateChild("yRotationTree", "FDS_LookLeft", "FDS_LookRight", YRotationParameterName);
            BlendTree zRotationTree = CreateChild("zRotationTree", "FDS_TiltLeft", "FDS_TiltRight", ZRotationParameterName);

            BlendTree rootTree = new BlendTree();
            rootTree.name = "faceRotationRootTree";
            rootTree.blendType = BlendTreeType.Direct;
            rootTree.AddChild(xRotationTree);
            rootTree.AddChild(yRotationTree);
            rootTree.AddChild(zRotationTree);
            // }}}

            // Default State configuration {{{2
            AnimatorState defState = new AnimatorState();
            defState.name = "faceRotationDefaultState";
            defState.motion = rootTree;
            // TODO: Should we turn on 'WriteDefaultValues'?
            // }}}

            // State Machine configuration {{{2
            AnimatorStateMachine stateMachine = new AnimatorStateMachine();
            stateMachine.name = "faceRotationStateMachine";
            stateMachine.AddState(defState, new Vector3(0, 0, 0));
            // }}}

            // Layer configuration {{{2
            AnimatorControllerLayer faceRotationLayer = new AnimatorControllerLayer();
            faceRotationLayer.avatarMask    = (AvatarMask)LoadFDSAsset<AvatarMask>("FDS_HeadRotationMask");
            faceRotationLayer.blendingMode  = AnimatorLayerBlendingMode.Override;
            faceRotationLayer.defaultWeight = 1.0f;
            faceRotationLayer.iKPass        = false;
            faceRotationLayer.name          = FDS_LayerName;
            faceRotationLayer.stateMachine  = stateMachine;
            // }}}
            // }}}

            c.AddLayer(faceRotationLayer);
            layer = faceRotationLayer;

            // Save asset
            // Each stuff I created should be 'Add'ed with 'AddObjectToAsset'
            // otherwise they'll be gone forever.
            // Reference:
            //   - https://forum.unity.com/threads/modify-animatorcontroller-through-script-and-save-it.612844/#post-5271999
            var controllerAssetPath = AssetDatabase.GetAssetPath(c);
            AssetDatabase.AddObjectToAsset(stateMachine, controllerAssetPath);
            AssetDatabase.AddObjectToAsset(defState, controllerAssetPath);
            AssetDatabase.AddObjectToAsset(rootTree, controllerAssetPath);
            AssetDatabase.AddObjectToAsset(xRotationTree, controllerAssetPath);
            AssetDatabase.AddObjectToAsset(yRotationTree, controllerAssetPath);
            AssetDatabase.AddObjectToAsset(zRotationTree, controllerAssetPath);
        }


        /// <summary>Inject FDS animation parameters if it doesn't have</summary>
        static void InjectParameters(AnimatorController c) {
            AddParameterIfNeeded(c, XRotationParameterName);
            AddParameterIfNeeded(c, YRotationParameterName);
            AddParameterIfNeeded(c, ZRotationParameterName);
        }


        static BlendTree CreateChild(string name, string FirstMotion, string SecondMotion, string parameterName) {
            BlendTree result = new BlendTree();
            result.name = name;
            result.blendType = BlendTreeType.Simple1D;
            result.AddChild((Motion)LoadFDSAsset<Motion>(FirstMotion));
            result.AddChild((Motion)LoadFDSAsset<Motion>(SecondMotion));
            result.blendParameter = parameterName;
            return result;
        }


        /// <summary>Execute 'AddParameter' only if given controller doesn't have it</summary>
        static private void AddParameterIfNeeded(AnimatorController c, string name) {
            AnimatorControllerParameter[] parameters = c.parameters;
            if(parameters.Any(p => p.name == name)) {
                Debug.Log($"The AnimatorController '{c.ToString()}' already have parameter '{name}'. Stop Injecting to this");
                return;
            }

            c.AddParameter(name, AnimatorControllerParameterType.Float);
        }


        /// <summary>
        ///     wrapper of <c>LoadAssetAtPath</c> for this unitypackage
        ///     If asset is not found, throw exception
        /// </summary>
        /// <exception cref="MissingAssetException">When Asset of given <c>name</c> is not found</exception>
        static private UnityEngine.Object LoadFDSAsset<T>(string name) {
            string[] guids = AssetDatabase.FindAssets(name);
            if(guids is null || guids.Length == 0) throw new MissingAssetException(name);

            string guid = guids[0];
            string assetpath = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath(assetpath, typeof(T));
        }
    }
}
