using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
    public class User
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string ID { get; set; }
    }

    public class Talk
    {
        public string From { get; set; }

        public string Content { get; set; }

        public string Host { get; set; }
    }
    public class WebChatServer : BaseServer
    {

        

        [Command("talk")]
        public void OnTalk(Beetle.Express.IChannel channel, string id, string command, JToken token)
        {
            string content = token["content"].ToString();
            Talk talk = new Talk();
            talk.Content = content;
            talk.From = channel.Name;
            talk.Host = channel.EndPoint.ToString();
            SendAll(id, command, talk);
        }

        [Command("list")]
        public void OnList(Beetle.Express.IChannel channel, string id, string command, JToken token)
        {
            List<User> users = new List<User>();
            Server.GetOnlines(Onlines);
            for (int i = 0; i < Onlines.Count; i++)
            {
                Beetle.Express.IChannel client = Onlines.Channels[i];
                users.Add(new User { Host = client.EndPoint.ToString(), ID = client.ID, Name = client.Name });
            }
            SendAll(id, command, users);
        }
        [Command("login")]
        public void OnLogin(Beetle.Express.IChannel channel, string id, string command, JToken token)
        {
            channel.Name = token["name"].ToString();
            User user = new User();
            user.Name = channel.Name;
            user.ID = channel.ID;
            user.Host = channel.EndPoint.ToString();
            SendAll(id, command, user);
        }   
        public override void Disposed(Beetle.Express.IServer server, Beetle.Express.ChannelEventArgs e)
        {         
            base.Disposed(server, e);
            User user = new User();
            user.Name = e.Channel.Name;
            user.ID = e.Channel.ID;
            user.Host = e.Channel.EndPoint.ToString();
            SendAll(null, "exit", user);
        }
       
        protected override void StatReport(Beetle.Express.WebSockets.WebSocketServer.Report report)
        {
            base.StatReport(report);
        }
    }
}
