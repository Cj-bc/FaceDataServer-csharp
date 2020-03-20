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
        /// <summary>
        ///     face x rotation in radian.
        ///     Should be:
        ///     <code>
        ///         Math.Clamp(FaceXRadian, -1/Math.PI, 1/Math.PI) == FaceXRadian
        ///     </code>
        /// </summary>
        public readonly float FaceXRadian;

        /// <summary>
        ///     face y rotation in radian.
        ///     Should be:
        ///     <code>
        ///         Math.Clamp(FaceYRadian, -1/Math.PI, 1/Math.PI) == FaceYRadian
        ///     </code>
        /// </summary>
        public readonly float FaceYRadian;

        /// <summary>
        ///     face z rotation in radian.
        ///     Should be:
        ///     <code>
        ///         Math.Clamp(FaceZRadian, -1/Math.PI, 1/Math.PI) == FaceZRadian
        ///     </code>
        /// </summary>
        public readonly float FaceZRadian;

        /// <summary>how much percent current mouth height is comparing to the default.</summary>
        public readonly int   MouthHeightPercent;

        /// <summary>how much percent current mouth width is comparing to the default.</summary>
        public readonly int   MouthWidthPercent;

        /// <summary>how much percent current left eye height is comparing to the default.</summary>
        public readonly int   LeftEyePercent;
        /// <summary>how much percent current right eye height is comparing to the default.</summary>
        public readonly int   RightEyePercent;
    }
}
