using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Data.Odbc;
using System.Net;
using Gtp.Framework;
using Gtp.Framework.ControlLibrary;

namespace IcmAlgoStreamserver
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        Socket client;
        Socket server;
        
        public static List<string> MNK;
        public static List<string> MNK_VWAP;

        public static List<Socket> AllClientSockets;
        byte[] buffer;
        byte[] buffer2;

        //public static double BASZAMAN_SEANS1=093500 , BITZAMAN_SEANS1=123500 , BASZAMAN_SEANS2=133000, BITZAMAN_SEANS2=173500;
        public static decimal BASZAMAN_SEANS1, BITZAMAN_SEANS1, BASZAMAN_SEANS2, BITZAMAN_SEANS2;

        public static dizi[] dizisira;
        public static ListBox lst = new ListBox();
        public static System.Windows.Forms.ListView LVIEW  = new System.Windows.Forms.ListView();

        public struct dizi
        {
            public string[,] ORD;  //{EmirTipi, IslemTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, SonFiyat, HesaplananEmirFiyati, Tetiksaat, Tetikdakika, Tetiksaniye, Statü, userinf,  gib_yuzde1, gib_yuzde2, gib_baszmn1, gib_baszmn2, gib_baszmn3, gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, gib_parcasayisi, gib_enson_aktifolanparca, aktsek }
            public string HISSE;
            public int TOPSATIR_ALTDIZI;

            public void initialize()
            {
                ORD = new string[20, 31];
                HISSE = "";
                TOPSATIR_ALTDIZI = 0;
            }
        }

        public static Object factLock_dizisira = new Object();
        public static Object factLock_lview    = new Object();
        public static Object factLock_mnk      = new Object();

        //****************************************************************************************************************


        public Form1()
        {
            InitializeComponent();
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // CLIENT SOCKET - Matriksten dinler.
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // SERVER SOCKET - Gtp den dinler.
            AllClientSockets = new List<Socket>();

            System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer(); //Initialize a new Timer of name timer1
            timer2.Tick += new EventHandler(timer2_Tick); //Link the Tick event with timer1_Tick
            timer2.Start(); //Start the timer

        }

        //****************************************************************************************************************

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            //lst   = listBox1;              //* listBox1 componentini static değişkende tutalım , threadden erişeceğiz.
            LVIEW = listView1;
            // tedt  = textEdit1;

            MNK = new List<string>();
            MNK_VWAP = new List<string>();

            dizisira = new dizi[600];
            for (int j = 0; j < 600; j++)
                dizisira[j].initialize();

                Biriken_Emirleri_IslemeKoy();
                Kapanista_BekleyenEmirleri_Aktive_Et();

                IPEndPoint poin = new System.Net.IPEndPoint(IPAddress.Any, 8055);   // SERVER SOCKET START - GTP' den geleni alır.
                server.Bind(poin);
                server.Listen(Int32.MaxValue);
                server.BeginAccept(new AsyncCallback(ServerDinlemeIslemi), server);
  
             
            //**Eski yapı Kapadım..   client.BeginConnect("128.10.254.244", 4005, new AsyncCallback(ClientServerBaglandi), null);  // CLIENT SOCKET START - Matriksten dinler.
              MatriksVerileri obj = new MatriksVerileri();
              Thread thread = new Thread(new ThreadStart(obj.Initialize));
              thread.Start();

        }

        //****************************************************************************************************************

        private void ServerDinlemeIslemi(IAsyncResult result)
        {
                    buffer2 = new byte[20000];
                    Socket yeniclient = (Socket)result.AsyncState;
                    yeniclient = yeniclient.EndAccept(result);
                    yeniclient.BeginReceive(buffer2, 0, buffer2.Length, SocketFlags.None, new AsyncCallback(clientMesajDinle), yeniclient);

                    AllClientSockets.Add(yeniclient);

                    System.Threading.Thread.Sleep(100);  //* Bekle.
                    server.BeginAccept(new AsyncCallback(ServerDinlemeIslemi), server);
                    System.Threading.Thread.Sleep(100);  //* Bekle.
        }


        //****************************************************************************************************************

        private void clientMesajDinle(IAsyncResult res)
        {
            Socket scr = (Socket)res.AsyncState;
            SocketError errm;
            int bytesRead2 = scr.EndReceive(res, out errm);
            bool dosyayayaz = true;

            if (SocketError.Success == errm)
            {
                      string gelen = Encoding.Default.GetString(buffer2, 0, bytesRead2);
                      Array.Clear(buffer2, 0, buffer2.Length);
                      System.Threading.Thread.Sleep(200); //* Bekle.
                      string str_tut = "";
                  int pos = gelen.IndexOf("%");

                      while (pos > 0)
                      {
                        str_tut = gelen.Substring(0, pos);
                        if (str_tut == "KAP")   
                        {
                            try
                            {
                                AllClientSockets.Remove(scr);
                                break;
                            }
                            catch (NullReferenceException nexp) { }
                        }
                        else
                        {
                            if (str_tut != "HBT")     
                            {
                                ThreadOrdino thr = new ThreadOrdino(str_tut, dosyayayaz);
                                Thread thread = new Thread(new ThreadStart(thr.islem));
                                thread.Start();
                                gelen = gelen.Substring(pos + 1, gelen.Length - (pos + 1)).Trim();
                                pos = gelen.IndexOf("%");
                                System.Threading.Thread.Sleep(150); //* Bekle.
                            }
                            else
                            { pos = -1;   }
                        }
                      }
            }
            
            try
            {  
                scr.BeginReceive(buffer2, 0, buffer2.Length, SocketFlags.None, new AsyncCallback(clientMesajDinle), scr);  
            }  catch (SocketException sexp) 
            {
                int dummy = 1;
            }
        }


        //****************************************************************************************************************   
   

        public void Biriken_Emirleri_IslemeKoy()
        {
            bool dosyayayaz = false;
            string str_tut="" , s2 = "" , s1="";
            string IslemTipi = "1"; //*bekleyenemir

            Utilities utl = new Utilities();
            utl.BorsaZamanlari(ref BASZAMAN_SEANS1, ref BITZAMAN_SEANS1, ref BASZAMAN_SEANS2, ref BITZAMAN_SEANS2); 
                      
            OrderList beklist = utl.BekleyenEmirler_Al();
            List<Order> lst = beklist.Resultlist;

            foreach (Order a in lst)
            {
                s1  = a.Userinf;

                s2  = a.Emirtipi + "&" + Convert.ToString(a.Islemtarihi) + "&" + Convert.ToString(a.Takastarihi) + "&" + Convert.ToString(a.Referansid) + "&";
                s2  += a.Menkul + "&" + a.Fin_inst_id + "&" + a.Alsat + "&" + a.Hesapno + "&" + a.Custid + "&" + a.Accid + "&";
                s2  += Convert.ToString(a.Lot) + "&" + Convert.ToString(a.Maxlot) + "&" + Convert.ToString(a.Marjyuzde) + "&";
                s2  += Convert.ToString(a.Tetiksaat) + "&" + Convert.ToString(a.Tetikdakika) + "&" + Convert.ToString(a.Tetiksaniye) + "&" + IslemTipi + "&";
                s2 += Convert.ToString(a.Gib_yuzde1) + "&" + Convert.ToString(a.Gib_yuzde2) + "&" + Convert.ToString(a.Gib_baszmn1) + "&" + Convert.ToString(a.Gib_baszmn2) + "&" + Convert.ToString(a.Gib_baszmn3) + "&" + Convert.ToString(a.Gib_bitzmn1) + "&" + Convert.ToString(a.Gib_bitzmn2) + "&" + Convert.ToString(a.Gib_bitzmn3) + "&" + Convert.ToString(a.Gib_parcasayisi) + "&" + Convert.ToString(a.Gib_enson_aktifOlanParca) + "&" + Convert.ToString(a.Aktiflesme_sekli) + "&" + Convert.ToString(a.Hesaplananagirlikliortalamafiyati);

                str_tut = s1 + "|" + s2 + "%";

                ThreadOrdino thr = new ThreadOrdino(str_tut, dosyayayaz);
                Thread thread = new Thread(new ThreadStart(thr.islem));
                thread.Start();
               // System.Threading.Thread.Sleep(200); //* Bekle.
            }


        }

        //****************************************************************************************************************   

        public void Kapanista_BekleyenEmirleri_Aktive_Et()   //*17:29:00 da bekleyen tüm emirleri gönder.
        {
            ThreadOrdino thr = new ThreadOrdino();
            Thread thread = new Thread(new ThreadStart(thr.islem2));
            thread.Start();
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
            // double time;
            decimal time ;

            Socket scr = (Socket)res.AsyncState;
            SocketError errm;
            int bytesRead = scr.EndReceive(res, out errm);
            if (SocketError.Success == errm)
            {
                string gelen = Encoding.Default.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer, 0, buffer.Length);

                saat   = Convert.ToString(System.DateTime.Now.Hour);
                dakika = Convert.ToString(System.DateTime.Now.Minute);
                saniye = Convert.ToString(System.DateTime.Now.Second);
                //time   = Convert.ToDouble(SifirKoy(saat, 2) + SifirKoy(dakika, 2) + SifirKoy(saniye, 2));
                time = Convert.ToDecimal(SifirKoy(saat, 2) + SifirKoy(dakika, 2) + SifirKoy(saniye, 2));

                if (((time >= BASZAMAN_SEANS1) && (time <= BITZAMAN_SEANS1)) || ((time >= BASZAMAN_SEANS2) && (time <= BITZAMAN_SEANS2)))
                {
                    ThreadCalis obj = new ThreadCalis(gelen);
                    Thread thread = new Thread(new ThreadStart(obj.islem));
                    thread.Start();
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



        private void timer2_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 21 && DateTime.Now.Minute >= 40 && DateTime.Now.Second >= 10) 
            {
                Utilities utl = new Utilities();
                utl.GunSonu_MatriksVerileriSil();
                Application.Exit(); 
            }
        }

        //****************************************************************************************************************   

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
