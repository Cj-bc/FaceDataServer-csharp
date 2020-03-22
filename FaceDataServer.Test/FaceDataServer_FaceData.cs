using Xunit;
using Cjbc.FaceDataServer.Type;

namespace Cjbc.FaceDataServer.UnitTests.Type {
    public class FaceData_FromBinaryShould {
        private FaceData correctResult = new FaceData(-3.0f, 0.0f, 3.0f
                                                     , 0, 10, 100, 150);
        private byte[] testBinary = new byte[] {0b00010000 // Version number (1.0.0)
                                               , 0xc0, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceXRadian
                                               , 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceYRadian
                                               , 0x40, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 // faceZRadian
                                               , 0x00, 0x0a // mouth_height, mouth_width
                                               , 0x64, 0x96 // left_eye, right_eye
                                               };
        private FaceDataComparer comparer = new FaceDataComparer();

        [Fact]
        public void FromBinary_supportedVersionInput_ReturnFaceData() {
            FaceData result = FaceData.FromBinary(testBinary);

            Assert.Equal(correctResult, result, comparer);
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
