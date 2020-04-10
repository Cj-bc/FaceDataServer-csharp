using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using Cjbc.FaceDataServer.Type;
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

            animator.SetFloat("Blend", 1.0f);
            animator.SetFloat(FaceDataServerMenu.XRotationParameterName, 0.5f);
            animator.SetFloat(FaceDataServerMenu.YRotationParameterName, 0.5f);
            animator.SetFloat(FaceDataServerMenu.ZRotationParameterName, 0.5f);
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
            animator.SetFloat(FaceDataServerMenu.XRotationParameterName, (x + 40f) / 80f);
            animator.SetFloat(FaceDataServerMenu.YRotationParameterName, (y + 40f) / 80f);
            animator.SetFloat(FaceDataServerMenu.ZRotationParameterName, (z + 40f) / 80f);


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
    }
}
