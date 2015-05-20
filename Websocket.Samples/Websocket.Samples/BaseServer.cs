using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
   public class BaseServer:Beetle.Express.WebSockets.WebSocketJsonServer
    {
        public override void Connect(Beetle.Express.IServer server, Beetle.Express.ChannelConnectEventArgs e)
        {
            base.Connect(server, e);
            Console.WriteLine("{1}:{0} connect", e.Channel.EndPoint, GetType().Name);
        }
        protected override void ReceivePacket(Beetle.Express.IChannel channel, Beetle.Express.WebSockets.IPacketData data)
        {
            base.ReceivePacket(channel, data);
            Console.WriteLine(data.Data.Decoding());
        }
        public override void Disposed(Beetle.Express.IServer server, Beetle.Express.ChannelEventArgs e)
        {
            Console.WriteLine("{1}:{0} disconnect", e.Channel.EndPoint, GetType().Name);
            base.Disposed(server, e);
           
        }
        public override void Error(Beetle.Express.IServer server, Beetle.Express.ErrorEventArgs e)
        {
            if (e.Channel != null)
            {
                Console.WriteLine("{3}:{0}[{2}] channel error {1}", e.Channel.EndPoint, e.Error.Message, e.Channel.Name,GetType().Name);
            }
            else
            {
                Console.WriteLine("{1}: error {0}", e.Error.Message, GetType().Name);
            }

            base.Error(server, e);
        }
    }
}
