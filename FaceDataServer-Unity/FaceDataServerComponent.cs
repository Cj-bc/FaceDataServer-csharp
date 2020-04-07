using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cjbc.FaceDataServer;
using Cjbc.FaceDataServer.Type;

namespace Cjbc.FaceDataServer.Unity {
    /// <summary>
    ///     FaceDataServer for Unity Component
    ///     Currently, this only holds <c>FaceDataServerReceiver</c>
    /// </summary>
    public class FaceDataServerComponent : MonoBehaviour {
        private FaceDataServerReceiver server;

        /// <summary>Return Latest received FaceData</summary>
        public FaceData latest() { return server.latest; }

        void Start() {
            server = new FaceDataServerReceiver();
            server.StartReceive();
        }

        void OnApplicationQuit() {
            server.StopReceive();
        }
    }
}
