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

                //Flush out any extra data that didn't send in the start.
                Writer.Flush();
            }

            catch
            {
                IsConnected = false;

                Console.WriteLine("Disconnessione dal server");
                Console.ReadKey();

                //chiudo la connessione.
                Writer.Close();
            }
        }
        static void Main(string[] args)
        {

            //è TcpClient; si usa per la connessione
            TcpClient Connector = new TcpClient();

        //etichetta su cui in fondo si fa il salto
        GetConnection:

            //passo come parametro l'ip della vittima datomi dal server in python
            Console.WriteLine("Scrivi l'indirizzo IP :");
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
                //connession fallita
                Console.WriteLine("Errore di connessione, premi un tasto per continuare");
                Console.ReadKey();

                //riprovo
                Console.Clear();
                goto GetConnection;
            }

            Console.WriteLine("Connessione stabilita a " + IP + ".");
            Console.WriteLine("Digita H per vedere la lista di comandi");

            //sono connesso al server
            while (IsConnected)
            {
                Console.WriteLine("Enter command : ");
                string CMD = Console.ReadLine();

                //comando help
                if (CMD == "H")
                {
                    Console.WriteLine("COMANDI");
                    Console.WriteLine("MESSAGE>>>tuoMessaggio");
                }
                else//altrimenti mando il comando scritto
                {
                    SendCommand(CMD);
                }

            }
        }
    }
}
