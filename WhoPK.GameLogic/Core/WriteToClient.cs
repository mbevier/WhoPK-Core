﻿using System;

using WhoPK.GameLogic.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WhoPK.GameLogic.Core
{
    public class WriteToClient : IClientMessenger
    {
        private readonly IHubContext<GameHub> _hubContext;
      

        public WriteToClient(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

    

        public async void WriteLine(string message, string id)
        {
          
            try
            {
                
                await _hubContext.Clients.Client(id).SendAsync("SendMessage", message + "<br />", "");
            }
            catch (Exception ex)
            {
                
            }
        }

        public async void WriteLine(string message)
        {

            try
            {

                await _hubContext.Clients.All.SendAsync("SendMessage", message, "");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
    
 