using Xunit;
using Cjbc.FaceDataServer.Type;
using System;

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

        [Fact]
        public void FromBinary_supportedVersionInput_ReturnFaceData() {
            FaceData result = FaceData.FromBinary(testBinary);

            Assert.Equal(result, correctResult);
        }
    }

}
