using System.Collections.Generic;
using System;
using Xunit;
using Cjbc.FaceDataServer.Type;

namespace Cjbc.FaceDataServer.UnitTests.Type {
    public class FaceData_FromBinaryShould {
        private FaceData correctResult = new FaceData(-3.0f, 0.0f, 3.0f
                                                     , 0, 10, 100, 150);
        private byte[] testBinary = new byte[] { 0xc0, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceXRadian
                                               , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceYRadian
                                               , 0x40, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceZRadian
                                               , 0x00, 0x0a // mouth_height, mouth_width
                                               , 0x64, 0x96 // left_eye, right_eye
                                               };
        private FaceDataComparer comparer = new FaceDataComparer();

        /// <summary>Generate Randomized FaceData for Xunit</summary>
        public static IEnumerable<object[]> RandomFaceData() {
            var RandGen = new System.Random();
            Func<System.Random, double> genRad  = (g) => (g.NextDouble() * 2 * Math.PI) - Math.PI;
            Func<System.Random, byte>   genPerc = (g) => (byte)g.Next(0, 150);

            FaceData v = new FaceData(genRad(RandGen)
                                     , genRad(RandGen)
                                     , genRad(RandGen)
                                     , genPerc(RandGen)
                                     , genPerc(RandGen)
                                     , genPerc(RandGen)
                                     , genPerc(RandGen)
                                     );
            yield return new object[] {v};
        }

        public static IEnumerable<object[]> RandomBinaryFaceData() {
            var RandGen = new System.Random();
            Func<System.Random, double> genRad  = (g) => (g.NextDouble() * 2 * Math.PI) - Math.PI;
            Func<System.Random, byte>   genPerc = (g) => (byte)g.Next(0, 150);

            byte[] a = BitConverter.GetBytes(genRad(RandGen));
            byte[] b = BitConverter.GetBytes(genRad(RandGen));
            byte[] c = BitConverter.GetBytes(genRad(RandGen));
            Array.Reverse(a);
            Array.Reverse(b);
            Array.Reverse(c);
            byte[] ret = new byte[28];
            Array.Copy(a, 0, ret,  0, 8);
            Array.Copy(b, 0, ret,  8, 8);
            Array.Copy(c, 0, ret, 16, 8);
            ret[25] = genPerc(RandGen);
            ret[26] = genPerc(RandGen);
            ret[27] = genPerc(RandGen);


            yield return new object[] {ret};
        }


        [Fact]
        public void FromBinary_CorrectInput_ReturnFaceData() {
            FaceData result = FaceData.FromBinary(testBinary);

            Assert.Equal(correctResult, result, comparer);
        }

        [Theory]
        [MemberData(nameof(RandomFaceData))]
        public void FromBinaryToBinary_ReturnSameAsInput(FaceData inData) {
            FaceData ret = FaceData.FromBinary(inData.ToBinary());

            Assert.Equal(inData, ret, comparer);
        }

        [Theory]
        [MemberData(nameof(RandomBinaryFaceData))]
        public void ToBinaryFromBinary_ReturnSameAsInput(byte[] inData) {
            byte[] ret = FaceData.FromBinary(inData).ToBinary();

            Assert.Equal(inData, ret);
        }

    }



    /// <summary>
    ///     Comparer for FaceData.
    ///     Used by <c>Assert.Equal()</c> to determine 'equality'
    ///     <see>https://thinkami.hatenablog.com/entry/2015/05/09/071648</see>
    ///     <see>https://github.com/xunit/assert.xunit/blob/master/EqualityAsserts.cs#L35</see>
    /// </summary>
    class FaceDataComparer : System.Collections.Generic.IEqualityComparer<FaceData> {
        public bool Equals(FaceData a, FaceData b) {
            return a.FaceXRadian.Equals(b.FaceXRadian)
                   && a.FaceYRadian.Equals(b.FaceYRadian)
                   && a.FaceZRadian.Equals(b.FaceZRadian)
                   && a.MouthHeightPercent.Equals(b.MouthHeightPercent)
                   && a.MouthWidthPercent.Equals(b.MouthWidthPercent)
                   && a.LeftEyePercent.Equals(b.LeftEyePercent)
                   && a.RightEyePercent.Equals(b.RightEyePercent)
                   ;
        }

        public int GetHashCode(FaceData a) {
          return a.FaceXRadian.GetHashCode()
                 ^ a.FaceYRadian.GetHashCode()
                 ^ a.FaceZRadian.GetHashCode()
                 ^ a.MouthHeightPercent.GetHashCode()
                 ^ a.MouthWidthPercent.GetHashCode()
                 ^ a.LeftEyePercent.GetHashCode()
                 ^ a.RightEyePercent.GetHashCode()
                 ;
        }
    
    }

}
