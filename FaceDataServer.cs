using System.Net.Sockets;
using System.Net;
using Cjbc.FaceDataServer.Type;

namespace Cjbc.FaceDataServer {

    public class FaceDataServer {
        /// <summary>
        /// local port to use.
        /// This could be random.
        /// </summary>
        int local_port = 202003;

        /// <summary><c>System.Net.Sockets.UdpClient</c> to connect </summary>
        static UdpClient cl;

        /// <summary>
        /// Default multicast group address
        /// </summary>
        /// <remarks>
        /// Default multicast address is defined in http://github.com/Cj-bc/FDS-protos
        /// </remarks>
        static IPAddress default_multicast_address = new IPAddress(new byte[] {0xe2, 0x46, 0x44, 0x53});
        IPEndPoint endpoint;

        /// <summary>Store latest FaceData</summary>
        public FaceData current;

        public FaceDataServer() {
            cl = new UdpClient(local_port); // Need to bind to port in order to join multicast group
            cl.JoinMulticastGroup(default_multicast_address);
        }

        ~FaceDataServer() {
            cl.DropMulticastGroup(default_multicast_address);
            cl.Dispose();
        }
    }

}
