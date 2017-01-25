using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRRealTime.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string msg)
        {
            //GlobalHost.ConnectionManager.GetHubContext<ChatHub>.Clients.send
            Clients.All.addNewMessageToPage(name, msg);
        }
    }
}