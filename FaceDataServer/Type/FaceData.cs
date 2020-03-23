using System.Net;
using System;

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

        public static FaceData Default() {
            return new FaceData(0, 0, 0, 100, 100, 100, 100);
        }

        /// <summary>Parse Raw Bytes and create <c>FaceData</c> from that if possible</summary>
        /// <param name="raw">
        ///     raw binary. This should NOT contain version number.
        ///     This SHOULD be big endian(Internet byte code).
        /// </param>
        public static FaceData FromBinary(byte[] raw) {
            byte[] xByte  = new byte[8];
            byte[] yByte  = new byte[8];
            byte[] zByte  = new byte[8];

            // TODO: Is this heigh cost manipulation?
            System.Array.Copy(raw,  0, xByte, 0, 8);
            System.Array.Copy(raw,  8, yByte, 0, 8);
            System.Array.Copy(raw, 16, zByte, 0, 8);
            if (BitConverter.IsLittleEndian) {
                System.Array.Reverse(xByte);
                System.Array.Reverse(yByte);
                System.Array.Reverse(zByte);
            }

            byte MouthHeigh = raw[24];
            byte MouthWidth = raw[25];
            byte LeftEye = raw[26];
            byte RightEye = raw[27];

            // BitConverter.To* reads given byte as system-local endian binary.
            // (It's mostly little endian)
            double FaceX = BitConverter.ToDouble(xByte, 0);
            double FaceY = BitConverter.ToDouble(yByte, 0);
            double FaceZ = BitConverter.ToDouble(zByte, 0);

            FaceData ret = new FaceData(FaceX, FaceY, FaceZ, MouthHeigh, MouthWidth, LeftEye, RightEye);

            return ret;
        }

        /// <summary>
        ///     Convert to binary format.
        ///     This will generate 'big endian' binary
        /// </summary>
        public byte[] ToBinary() {
            byte[] bin = new byte[28];
            byte[] faceX = BitConverter.GetBytes(this.FaceXRadian);
            byte[] faceY = BitConverter.GetBytes(this.FaceYRadian);
            byte[] faceZ = BitConverter.GetBytes(this.FaceZRadian);

            // Make sure binaries are big endian
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(faceX);
                Array.Reverse(faceY);
                Array.Reverse(faceZ);
            }

            Array.Copy(faceX, 0, bin,  0, 8);
            Array.Copy(faceY, 0, bin,  8, 8);
            Array.Copy(faceZ, 0, bin, 16, 8);
            bin[24] = this.MouthHeightPercent;
            bin[25] = this.MouthWidthPercent;
            bin[26] = this.LeftEyePercent;
            bin[27] = this.RightEyePercent;

            return bin;
        }
    }
}
