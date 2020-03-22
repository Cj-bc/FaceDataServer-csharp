using System.Net.Sockets;
using System.Net;

namespace Cjbc.FaceDataServer.Type {
    /// <summary>
    /// FaceData Data type.
    /// This represents one FaceData defined in FDS-protos
    /// </summary>
    /// <remarks>
    /// reference at: https://github.com/Cj-bc/FDS-protos
    /// </remarks>
    public class FaceData {
        /// <summary>face x rotation in radian.</summary>
        /// <remarks>This should be range of (-<c>Math.PI</c>, <c>Math.PI</c>)</remarks>
        public readonly double FaceXRadian;

        /// <summary>face y rotation in radian.</summary>
        /// <remarks>This should be range of (-<c>Math.PI</c>, <c>Math.PI</c>)</remarks>
        public readonly double FaceYRadian;

        /// <summary>face z rotation in radian.</summary>
        /// <remarks>This should be range of (-<c>Math.PI</c>, <c>Math.PI</c>)</remarks>
        public readonly double FaceZRadian;

        /// <summary>how much percent current mouth height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly byte   MouthHeightPercent;

        /// <summary>how much percent current mouth width is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly byte   MouthWidthPercent;

        /// <summary>how much percent current left eye height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly byte   LeftEyePercent;
        /// <summary>how much percent current right eye height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly byte   RightEyePercent;

        public FaceData(double x, double y, double z, byte mh, byte mw, byte le, byte re) {
            FaceXRadian = x;
            FaceYRadian = y;
            FaceZRadian = z;
            MouthHeightPercent = mh;
            MouthWidthPercent  = mw;
            LeftEyePercent     = le;
            RightEyePercent    = re;
        }

        /// <summary>Parse Raw Bytes and create <c>FaceData</c> from that if possible</summary>
        /// <param name="raw">raw binary. Make sure protocol version is supported before passing this function</param>
        public static FaceData FromBinary(byte[] raw) {
        }
    }
}
