using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace IcmAlgoStreamserver
{
    class MatriksVerileri
    {
        Socket client;
        byte[] buffer;
        byte[] buffer2;
        public static decimal BASZAMAN_SEANS1, BITZAMAN_SEANS1, BASZAMAN_SEANS2, BITZAMAN_SEANS2;


        public MatriksVerileri()
        {
             client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // CLIENT SOCKET - Matriksten dinler.
        }

        public void Initialize()
        {
            client.BeginConnect("128.10.254.244", 4005, new AsyncCallback(ClientServerBaglandi), null);  // CLIENT SOCKET START - Matriksten dinler.
            Utilities utl = new Utilities();
            utl.BorsaZamanlari(ref BASZAMAN_SEANS1, ref BITZAMAN_SEANS1, ref BASZAMAN_SEANS2, ref BITZAMAN_SEANS2); 
        }


        //**********************************************************************************************************************************************************************   
        //****** CLIENT SOCKET - Matriksden gelen Veriyi Alır ******************************************************************************************************************

        private void ClientServerBaglandi(IAsyncResult re)
        {
            buffer = new byte[20000];
            client.EndConnect(re);

            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Serverdanmesajgeldi), client);
            SendToServer();
        }


        private void Serverdanmesajgeldi(IAsyncResult res)
        {
            string saat, dakika, saniye;
            decimal time;

            int pos, ind = -1, ind2 = -1;
            string str_tut = "", satirtut = "", func = "", aktsek = "";

            Socket scr = (Socket)res.AsyncState;
            SocketError errm;
            int bytesRead = scr.EndReceive(res, out errm);
            if (SocketError.Success == errm)
            {
                string gelen = Encoding.Default.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer, 0, buffer.Length);

                saat = Convert.ToString(System.DateTime.Now.Hour);
                dakika = Convert.ToString(System.DateTime.Now.Minute);
                saniye = Convert.ToString(System.DateTime.Now.Second);
                time = Convert.ToDecimal(SifirKoy(saat, 2) + SifirKoy(dakika, 2) + SifirKoy(saniye, 2));

                if (((time >= BASZAMAN_SEANS1) && (time <= BITZAMAN_SEANS1)) || ((time >= BASZAMAN_SEANS2) && (time <= BITZAMAN_SEANS2)))
                {
                    try
                    {
                        pos = gelen.ToString().IndexOf("\n");
                        while (pos > 0)
                        {
                            str_tut = gelen.Substring(0, pos - 1);
                            satirtut = str_tut;
                            func = str_tut.Substring(0, 3);
                            if (func == "067")
                            {
                                int pos2 = satirtut.IndexOf("ÿ");
                                string hisse = satirtut.Substring(3, pos2 - 3);

                                lock (Form1.factLock_mnk) { ind = Form1.MNK_VWAP.IndexOf(hisse); }

                                if (ind >= 0)
                                {
                                    MatriksVerileriParcala obj = new MatriksVerileriParcala(satirtut, hisse);
                                    Thread thread = new Thread(new ThreadStart(obj.islem));
                                    thread.Start();
                                    // break;
                                }
                            }

                            gelen = gelen.Substring(pos + 1, gelen.Length - (pos + 1));
                            pos = gelen.ToString().IndexOf("\n");
                        }

                    }
                    catch (Exception ex) { }

                }


            }
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Serverdanmesajgeldi), client);

        }

        private void SendToServer()
        {
            client.Send(Encoding.Default.GetBytes("UserName:MATRIKS160;Password:MATRIKS;Tip:3;vb\r\n"));
        }


        //****** CLIENT SOCKET - Matriksden gelen Veriyi Alır ******************************************************************************************************************
        //**********************************************************************************************************************************************************************   

        private string SifirKoy(string sayi, int uzunluk)
        {
            string rc;
            int len = sayi.Trim().Length;
            rc = sayi;
            for (int i = len; i < uzunluk; i++)
                rc = "0" + rc;
            return rc.Trim();
        }


    }
}
