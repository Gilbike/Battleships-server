using LiteNetLib;
using LiteNetLib.Utils;

EventBasedNetListener listener = new EventBasedNetListener();
NetManager server = new NetManager(listener);
server.Start(9050);

listener.ConnectionRequestEvent += request => {
  if (server.ConnectedPeersCount < 10)
    request.AcceptIfKey("SomeConnectionKey");
  else
    request.Reject();
};

listener.PeerConnectedEvent += peer => {
  Console.WriteLine("We got connection: {0}", peer.EndPoint);
  NetDataWriter writer = new NetDataWriter();
  writer.Put("Hello client!");
  peer.Send(writer, DeliveryMethod.ReliableOrdered);
};

while (!Console.KeyAvailable) {
  server.PollEvents();
  Thread.Sleep(15);
}
server.Stop();