using LiteNetLib;
using LiteNetLib.Utils;

EventBasedNetListener listener = new EventBasedNetListener();
NetManager server = new NetManager(listener);
server.Start(9050);
System.Console.WriteLine("[Server] Server started on :9050");

listener.ConnectionRequestEvent += request => {
  System.Console.WriteLine("[Server] Incoming connection request");
  if (server.ConnectedPeersCount < 10) {
    System.Console.WriteLine("[Server] Accepted connection request");
    request.AcceptIfKey("SomeConnectionKey");
  } else {
    System.Console.WriteLine("[Server] Rejected connection request");
    request.Reject();
  }
};

listener.PeerConnectedEvent += peer => {
  Console.WriteLine("[Server] Client connection from: {0}", peer.EndPoint.Address);
  NetDataWriter writer = new NetDataWriter();
  writer.Put("Hello client!");
  peer.Send(writer, DeliveryMethod.ReliableOrdered);
};

while (!Console.KeyAvailable) {
  server.PollEvents();
  Thread.Sleep(15);
}
server.Stop();