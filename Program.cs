using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Threading;
using System.Text.RegularExpressions;

using WebSocket4Net;

/* 
 * Essentially, an example in C# showing how to retrieve messages from the Trollbox using WebSocket.
 * Author: Simran Singh
 * 
 * This is only an EXAMPLE and will be ported to a GUI app for Windows, Linux, and Mac OSx(If possible)!
 * Not to mention a Trollbox application for Android and if someone gets me a Mac I can make one for iOS!
 * 
 */

namespace TBChatExample
{
    class Program
    {

		static WebSocket websocket = new WebSocket ("wss://ws.pusherapp.com/app/4e0ebd7a8b66fa3554a4?protocol=6&client=js&version=2.0.0&flash=false");
		//^ Declare WebSocket with PusherApp API URL for Trollbox

        static void Main(string[] args)
        {
			Functions.InitColors ();

			Connect();
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }

		public static void Connect()
        {
            try
            {
				websocket.Opened += new EventHandler(websocket_Opened);
				websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);

				websocket.Open();

				Thread.Sleep(1200); //Wait for a sec, so server starts and ready to accept connection..

				Send(websocket, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
		}

		private static void Send(WebSocket websocket, string message)
		{
			while (websocket.State == WebSocketState.Open)
            {
				string stringtoSend = message;
                websocket.Send(stringtoSend);
                Thread.Sleep(1000);
            }
		}
		

		private static void websocket_Opened(object sender, EventArgs e)
		{
			//Send HandShake
			websocket.Send ("{\"event\":\"pusher:subscribe\",\"data\":{\"channel\":\"chat_en\"}}");
		}
		
		private static void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			//Let's filter out any errors that are irrelevant.
			if (e.Message.Equals ("{\"event\":\"pusher:error\",\"data\":{\"code\":null,\"message\":\"Received invalid JSON\"}}")) 
			{ } 
			else 
			{
				filterMsg (e.Message);
			}
		}

		private static void filterMsg(string Msg)
		{
			try
			{
				Msg.Replace("\\", "").Replace("\":\"\"", "\":").Replace("}\"\"", "}");

				var JSS = new JavaScriptSerializer();
				var JSON = JSS.Deserialize<Dictionary<string,dynamic>>(Msg);

				string login = Regex.Match(JSON["data"], @"""login\\"":\\""(.*?)\\""").Groups[1].Value;
				string msg = Regex.Match(JSON["data"], @"msg\\"":\\""(.*?)\\""").Groups[1].Value;

				if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(msg)) { }
				else
				{
					Functions.log(login, Functions.RandomNum());
					Console.WriteLine(HttpUtility.HtmlDecode(msg));
				}
			}
			catch { }
		}
    }
}
