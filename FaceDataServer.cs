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
        protected static UdpClient cl;

        /// <summary>
        /// Default multicast group address
        /// </summary>
        /// <remarks>
        /// Default multicast address is defined in http://github.com/Cj-bc/FDS-protos
        /// </remarks>
        static IPEndPoint DefaultEndPoint = new IPEndPoint(IPAddress.Parse("226.70.68.83"), 0);

        public FaceDataServer() {
            cl = new UdpClient(local_port); // Need to bind to port in order to join multicast group
            cl.JoinMulticastGroup(DefaultEndPoint.Address);
        }

        ~FaceDataServer() {
            cl.DropMulticastGroup(DefaultEndPoint.Address);
            cl.Dispose();
        }
    }

}
