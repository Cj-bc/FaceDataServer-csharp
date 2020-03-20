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
        public readonly float FaceXRadian;

        /// <summary>face y rotation in radian.</summary>
        /// <remarks>This should be range of (-<c>Math.PI</c>, <c>Math.PI</c>)</remarks>
        public readonly float FaceYRadian;

        /// <summary>face z rotation in radian.</summary>
        /// <remarks>This should be range of (-<c>Math.PI</c>, <c>Math.PI</c>)</remarks>
        public readonly float FaceZRadian;

        /// <summary>how much percent current mouth height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly int   MouthHeightPercent;

        /// <summary>how much percent current mouth width is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly int   MouthWidthPercent;

        /// <summary>how much percent current left eye height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly int   LeftEyePercent;
        /// <summary>how much percent current right eye height is comparing to the default.</summary>
        /// <remarks>This should be range of (0, 150)</remarks>
        public readonly int   RightEyePercent;
    }
}
