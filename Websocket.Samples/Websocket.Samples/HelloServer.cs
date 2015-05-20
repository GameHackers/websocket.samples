using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
    public class HelloServer:BaseServer
    {
        [Command("hello")]
        public void OnList(Beetle.Express.IChannel channel, string id, string command, JToken token)
        {
            string result = "hello " +token.ToString();
            Send(channel,id,command,result);
        }
    }
}
