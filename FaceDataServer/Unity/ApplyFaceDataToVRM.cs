using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using Cjbc.FaceDataServer.Type;
using Cjbc.FaceDataServer.Unity.Exceptions;
using VRM;

namespace Cjbc.FaceDataServer.Unity {

    /// <summary>
    ///     Apply FaceData to VRM model
    ///     Please Attach this component to root object of VRM.
    ///     <c>FaceDataServerComponent</c> should not be on the same object.
    /// </summary>
    public class ApplyFaceDataToVRM : MonoBehaviour
    {
        FaceDataServerComponent source;
        Transform transform;
        Transform head;
        VRMBlendShapeProxy blenderShapeProxy;
        FaceData latest;
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            source = (FaceDataServerComponent)FindObjectOfType(typeof(FaceDataServerComponent));
            transform = GetComponent<Transform>();
            blenderShapeProxy = GetComponent<VRMBlendShapeProxy>();
            head  = transform.Find("Root")
                        .Find("J_Bip_C_Hips")
                        .Find("J_Bip_C_Spine")
                        .Find("J_Bip_C_Chest")
                        .Find("J_Bip_C_UpperChest")
                        .Find("J_Bip_C_Neck")
                        .Find("J_Bip_C_Head")
                        ;
            animator = GetComponent<Animator>();

            InjectAnimationLayer();

            animator.SetFloat("Blend", 1.0f);
            animator.SetFloat("X_Rotation", 0.5f);
            animator.SetFloat("Y_Rotation", 0.5f);
            animator.SetFloat("Z_Rotation", 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            latest = source.latest();

            // ----- Set Face Rotation -----
            // Model can rotate -40~40 degree.
            // So firstly, I'll clamp
            float x = Mathf.Clamp(-((float)latest.FaceXRadian) * Mathf.Rad2Deg, -40.0f, 40.0f);
            float y = Mathf.Clamp( ((float)latest.FaceYRadian) * Mathf.Rad2Deg, -40.0f, 40.0f);
            float z = Mathf.Clamp( ((float)latest.FaceZRadian) * Mathf.Rad2Deg, -40.0f, 40.0f);
            animator.SetFloat("X_Rotation", (x + 40f) / 80f);
            animator.SetFloat("Y_Rotation", (y + 40f) / 80f);
            animator.SetFloat("Z_Rotation", (z + 40f) / 80f);


            // ----- Set Facial Expression -----
            Dictionary<BlendShapeKey, float> face = new Dictionary<BlendShapeKey, float> {};

            // FDS define '0' to 'closing eye', '1' to 'opened eye', but UniVRM is opposit way.
            // I should convert it.
            //
            // I defined 'EyePercent > 100' represents 'surprising'.
            if (latest.LeftEyePercent > 100 || latest.RightEyePercent > 100) {
                face.Add(new BlendShapeKey(BlendShapePreset.Blink_L), 0.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.Blink_R), 0.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.A), 0.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.U), 0.0f);
                // Surprised parameter is affected by bigger one
                face.Add(new BlendShapeKey("Surprised")
                        , 2.0f * (Mathf.Max(latest.RightEyePercent, latest.LeftEyePercent) - 100.0f) / 100.0f);
            } else {
                face.Add(new BlendShapeKey("Surprised"), 0.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.Blink_L), 1.0f - latest.LeftEyePercent / 100.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.Blink_R), 1.0f - latest.RightEyePercent / 100.0f);
                // TODO: Apply MouthWidthPercent too
                face.Add(new BlendShapeKey(BlendShapePreset.A)
                        , Mathf.Clamp(latest.MouthHeightPercent, 0.0f, 100.0f) / 100.0f);
                face.Add(new BlendShapeKey(BlendShapePreset.U)
                        , 1.0f - Mathf.Clamp(latest.MouthWidthPercent, 0.0f, 100.0f) / 100.0f);
            }

            blenderShapeProxy.SetValues(face);
        }

        private BlendTree CreateChild(string name, string FirstMotion, string SecondMotion, string parameterName) {
            BlendTree result = new BlendTree();
            result.name = name;
            result.blendType = BlendTreeType.Simple1D;
            result.AddChild((Motion)LoadFDSAsset<Motion>(FirstMotion));
            result.AddChild((Motion)LoadFDSAsset<Motion>(SecondMotion));
            result.blendParameter = parameterName;
            return result;
        }


        /// <summary>
        ///     Inject new Animation layer which perform face rotation
        ///     into currently attached model's animatorController.
        /// </summary>
        private void InjectAnimationLayer() {
            // 1. Get currently used animatorController and check if Injection is needed
            // 2. Create new layer that contains faceRotation blend tree
            // 3. Add Animation Parameters
            // 4. Add layer to the controller
            // 5. Save it

            // 1.
            AnimatorController original_c = (AnimatorController)animator.runtimeAnimatorController;
            if((System.Array.Find(original_c.layers, l => l.name == "faceRotation")) != null)
                return;

            string XRotationParameterName = "X_Rotation";
            string YRotationParameterName = "Y_Rotation";
            string ZRotationParameterName = "Z_Rotation";

            // 2. {{{
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
            defState.motion = rootTree;
            // TODO: Should we turn on 'WriteDefaultValues'?
            // }}}

            // State Machine configuration {{{2
            AnimatorStateMachine stateMachine = new AnimatorStateMachine();
            stateMachine.name = "faceRotationState";
            stateMachine.AddState(defState, new Vector3(0, 0, 0));
            // }}}

            // Layer configuration {{{2
            AnimatorControllerLayer faceRotationLayer = new AnimatorControllerLayer();
            faceRotationLayer.avatarMask    = (AvatarMask)LoadFDSAsset<AvatarMask>("FDS_HeadRotationMask");
            faceRotationLayer.blendingMode  = AnimatorLayerBlendingMode.Override;
            faceRotationLayer.defaultWeight = 1.0f;
            faceRotationLayer.iKPass        = false;
            faceRotationLayer.name          = "faceRotation";
            faceRotationLayer.stateMachine  = stateMachine;
            // }}}
            // }}}

            // 3
            original_c.AddParameter(XRotationParameterName, AnimatorControllerParameterType.Float);
            original_c.AddParameter(YRotationParameterName, AnimatorControllerParameterType.Float);
            original_c.AddParameter(ZRotationParameterName, AnimatorControllerParameterType.Float);

            // 4, 5
            if (original_c is null) Debug.LogError("Model's AnimatorController is missing. Please attach it.");

            original_c.AddLayer(faceRotationLayer);

            animator.runtimeAnimatorController = (RuntimeAnimatorController)original_c;
        }

        /// <summary>
        ///     wrapper of <c>LoadAssetAtPath</c> for this unitypackage
        ///     If asset is not found, throw exception
        /// </summary>
        /// <exception cref="MissingAssetException">When Asset of given <c>name</c> is not found</exception>
        private Object LoadFDSAsset<T>(string name) {
            string[] guids = AssetDatabase.FindAssets(name);
            if(guids is null || guids.Length == 0) throw new MissingAssetException(name);

            string guid = guids[0];
            string assetpath = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath(assetpath, typeof(T));
        }
    }
}
