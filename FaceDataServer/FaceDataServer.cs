using System.Net.Sockets;
using System.Net;
using Cjbc.FaceDataServer.Type;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cjbc.FaceDataServer {

    /// <summary>
    /// A Base class for sender and receiver.
    /// </summary>
    public class FaceDataServer {
        /// <summary>currently used protocol's major version</summary>
        /// <see>https://github.com/Cj-bc/FDS-protos</see>
        public static readonly byte protocolMajor = 1;

        /// <summary>currently used protocol's major version</summary>
        /// <see>https://github.com/Cj-bc/FDS-protos</see>
        public static readonly byte protocolMinor = 0;

        /// <summary>
        /// local port to use.
        /// This could be random.
        /// </summary>
        int local_port = 20203;

        /// <summary><c>System.Net.Sockets.UdpClient</c> to connect </summary>
        protected static UdpClient cl;

        /// <summary>
        /// Default multicast group address
        /// </summary>
        /// <remarks>
        /// Default multicast address is defined in http://github.com/Cj-bc/FDS-protos
        /// </remarks>
        static IPEndPoint DefaultEndPoint = new IPEndPoint(IPAddress.Parse("226.70.68.83"), 5032);

        public FaceDataServer() {
            cl = new UdpClient(DefaultEndPoint.Port); // Need to bind to port in order to join multicast group
            cl.JoinMulticastGroup(DefaultEndPoint.Address);
        }

        ~FaceDataServer() {
            cl.DropMulticastGroup(DefaultEndPoint.Address);
            cl.Dispose();
        }

        /// <summary>
        ///     Validate if given byte's protocol version is supported
        ///     Return <c>true</c> when:
        ///     <list>
        ///         <item>
        ///           <description>Major version number is the same as the library</description>
        ///         </item>
        ///         <item>
        ///           <description>Minor version number is the same or grater than the library</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="row">Raw data to validate.</param>
        protected static bool ValidateProtocolVersion(byte[] raw) {
            // first 4 bit represents 'Major version', and
            // next  4 bit represents 'Minor version'.
            // see: https://github.com/Cj-bc/FDS-protos/blob/develop/en/communication.md#description-for-each-section
            byte versionByte = raw[0];
            byte datasMajorV = (byte) (versionByte >> 4);
            byte datasMinorV = (byte) (versionByte & 0b00001111);

            return (datasMajorV == protocolMajor) && (datasMinorV >= protocolMinor);
        }
    }


    /// <summary>
    ///   Receiver for FaceDataServer
    /// </summary>
    public class FaceDataServerReceiver : FaceDataServer {
        /// <summary>Store latest FaceData received from peer</summary>
        public FaceData latest = FaceData.Default();

        CancellationTokenSource cts;
        Task thread;

        IPEndPoint peer = null;

        /// <summary>Start receiving and storing data</summary>
        public void StartReceive() {

            cts = new CancellationTokenSource();

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 5032);
            cl.BeginReceive(new AsyncCallback(onFDSReceived), null);


        }

        void onFDSReceived(IAsyncResult result) {
            byte[] Received = cl.EndReceive(result, ref peer);
            byte[] Version  = new byte[1];
            byte[] Contents = new byte[Received.Length - 1];
            Array.Copy(Received, Version, 1);
            Array.Copy(Received, 1, Contents, 0, Contents.Length);

            // TODO: Throw exception if Version is not supported
            if (ValidateProtocolVersion(Version))
                latest = FaceData.FromBinary(Contents);

            if (!cts.IsCancellationRequested)
                cl.BeginReceive(new AsyncCallback(onFDSReceived), null);
        }


        /// <summary>Stop receiving</summary>
        public void StopReceive() {
            cts.Cancel();
        }
    }
}
