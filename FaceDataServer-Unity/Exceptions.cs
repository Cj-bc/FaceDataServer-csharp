using System;

namespace Cjbc.FaceDataServer.Unity.Exceptions {
    /// <summary>Indicate the given path or Asset name is not exist.</summary>
    /// This class is written according to <a>https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions</a>
    public class MissingAssetException: Exception {

        public MissingAssetException(): base() {}
        public MissingAssetException(string message): base("Asset: " + message + " is missing") {}
        public MissingAssetException(string message, Exception inner): base(message, inner) {}
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected MissingAssetException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
