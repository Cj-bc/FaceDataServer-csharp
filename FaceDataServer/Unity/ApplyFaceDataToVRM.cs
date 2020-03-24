using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cjbc.FaceDataServer.Unity;
using Cjbc.FaceDataServer.Type;

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
        FaceData latest;
        // Start is called before the first frame update
        void Start()
        {
            source = (FaceDataServerComponent)FindObjectOfType(typeof(FaceDataServerComponent));
            transform = GetComponent<Transform>();
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
            Quaternion latestRot = Quaternion.Euler( ((float)latest.FaceXRadian) * Mathf.Rad2Deg
                                                   , ((float)latest.FaceYRadian) * Mathf.Rad2Deg
                                                   , ((float)latest.FaceZRadian) * Mathf.Rad2Deg
                                                   );
            head.localRotation = Quaternion.Lerp(head.localRotation, latestRot, Mathf.Clamp(Time.time * 0.03f, 0.0f, 1.0f));


        }
    }
}
