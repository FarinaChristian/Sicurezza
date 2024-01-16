using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace esame
{
    class Program
    {
        public static bool IsConnected;
        public static NetworkStream Writer;

        public static void SendCommand(string Command)
        {
            //Try to send

            try
            {
                //Creates a packet to hold the command
                byte[] Packet = Encoding.ASCII.GetBytes(Command);

                //Send the command over the network
                Writer.Write(Packet, 0, Packet.Length);

                //Flush out any extra data that didn't send in the start.
                Writer.Flush();

            }

            catch
            {
                //Couldn't send, so we aren't connected anymore!
                IsConnected = false;

                Console.WriteLine("Disconnessione dal server");
                Console.ReadKey();

                //Close the connection.
                Writer.Close();

            }
        }
        static void Main(string[] args)
        {

            //This is the TcpClient; we will use this for the connection.
            TcpClient Connector = new TcpClient();

        //If you can't connect, it takes you back here to try again.
        GetConnection:

            //Get the user to enter the IP of the server.
            Console.WriteLine("Scrivi l'indirizzo IP :");
            string IP = Console.ReadLine();

            //provo a connettermi
            try
            {
                //Connect to the specified IP on port 2000 (the port the trojan server uses!)
                Connector.Connect(IP, 2000);

                //So the program continues to receive commands.
                IsConnected = true;

                //Make Writer the stream coming from / going to Connector.
                Writer = Connector.GetStream();

                //We connected!
            }

            catch
            {
                //We couldn't connect :-(
                Console.WriteLine("Errore di connessione, premi un tasto per continuare");
                Console.ReadKey();

                //Go back and start again!
                Console.Clear();
                goto GetConnection;
            }

            //Let user know they connected and that if they type HELP they'll get a list of commands to use.
            Console.WriteLine("Connessione stabilita a " + IP + ".");
            Console.WriteLine("Digita H per vedere la lista di comandi");

            //While you're connected to the server
            while (IsConnected)
            {
                Console.WriteLine("Enter command : ");
                string CMD = Console.ReadLine();

                //If they type HELP
                if (CMD == "H")
                {
                    Console.WriteLine("COMANDI");
                    Console.WriteLine("MESSAGE!!!---<tuo comando>");
                }

                //They entered a real command, so lets send it!
                else
                {
                    //Send the command using our function above
                    SendCommand(CMD);
                }

            }
        }
    }
}
