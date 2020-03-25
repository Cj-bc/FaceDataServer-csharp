using System.Collections.Generic;
using UnityEngine;
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
        }

        // Update is called once per frame
        void Update()
        {
            latest = source.latest();
            Quaternion latestRot = Quaternion.Euler( -((float)latest.FaceXRadian) * Mathf.Rad2Deg
                                                   , ((float)latest.FaceYRadian) * Mathf.Rad2Deg
                                                   , ((float)latest.FaceZRadian) * Mathf.Rad2Deg
                                                   );
            head.localRotation = Quaternion.Lerp(head.localRotation, latestRot, Mathf.Clamp(Time.time * 0.03f, 0.0f, 1.0f));

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
