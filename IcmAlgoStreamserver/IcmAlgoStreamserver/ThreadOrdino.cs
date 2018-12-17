using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace IcmAlgoStreamserver
{
    class ThreadOrdino
    {
        public string gelen;
        public bool dosyayayaz;
        public bool kalanlotgonderildi;

        /*
        static Object _factLock = new Object();
        static Object _factLock2 = new Object();
        static Object _factLock3 = new Object();
        static Object _factLock6 = new Object();
        */

        public ThreadOrdino()
        {
        }

        public ThreadOrdino(string gelen,bool dosyayayaz)
        {
            this.gelen      = gelen;
            this.dosyayayaz = dosyayayaz;
        }

        //****************************************************************************************************************************************

        public void islem()
        {
                string hardbit = gelen.Substring(0, 3);
                if (hardbit == "HBT")
                   return;

                string[] gelenveri = (gelen).Split('|');
                string userinf = gelenveri[0];
                string emirinf = gelenveri[1];

                string[] emir   = emirinf.Split('&');
                string EmirTipi = emir[0];

                    if (EmirTipi == "Z.A")
                        Zaman_Aktivasyonlu_Emir();
                    else if (EmirTipi == "G.I.B")
                        GunIciBolmeli_Emir();
        }

        //****************************************************************************************************************************************

        public void islem2()
        {
            Kapanista_BekleyenEmirleri_Aktive_Et();
        }

        //****************************************************************************************************************************************

        public void Zaman_Aktivasyonlu_Emir()
        {

            string[] gelenveri = (gelen).Split('|');
            string userinf     = gelenveri[0];
            string emirinf     = gelenveri[1];

            string[] emir      = emirinf.Split('&');
            string EmirTipi    = emir[0];
            string BugunTarihi = emir[1];
            string TakasTarihi = emir[2];
            string RefId       = emir[3];
            string Menkul      = emir[4];
            string Fin_Inst_Id = emir[5];
            string Alsat       = emir[6];
            string HesapNo     = emir[7];
            string CustId      = emir[8];
            string AccId       = emir[9];
            string Lot         = emir[10];
            string Maxlot      = emir[11];
            string MarjYuzde   = emir[12];
            string Zmn1        = emir[13];
            string Zmn2        = emir[14];
            string Zmn3        = emir[15];
            string IslemTipi   = emir[16];
            string gib_aktsek  = emir[27];
          

            if (IslemTipi == "1")        //*Yeni emir
            {
                ZAkt_DiziyeYaz_DosyayaYaz_EmiriTetikle(EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, Alsat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, Zmn1, Zmn2, Zmn3, userinf,dosyayayaz);
            }
            else if (IslemTipi == "3")  //*Silme işlemi
            {
                ZAkt_EmirIptalEt(RefId,Menkul);
            }
            else if (IslemTipi == "2")  //*Kalanı Aktive Et
            {
                ZAkt_EmirKalaniAktiveEt(RefId, gib_aktsek, "Kullanici.Aktv- ");
            }


        }

        //****************************************************************************************************************************************


        public static void ZAkt_DiziyeYaz_DosyayaYaz_EmiriTetikle(String EmirTipi, String BugunTarihi, String TakasTarihi, String RefId, String Menkul, String Fin_Inst_Id, String AlSat, String HesapNo, String CustId, String AccId, String Lot, String Maxlot, String MarjYuzde, String Zmn1, String Zmn2, String Zmn3, String userinf,bool dosyayayaz)
        {
              Utilities utl = new Utilities();
              int interval = utl.TimeDifference(Convert.ToInt32(Zmn1), Convert.ToInt32(Zmn2), Convert.ToInt32(Zmn3), 0);
              int ind=-1, ind2=-1;

              if (interval > 0)    //*There is no Start method; the timer starts running as soon as the instance is created.
              {
                  ind = Indis_Bul(Menkul," ");

                try{
                    lock (Form1.factLock_dizisira)
                    {
                        //if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                        //{
                        Form1.dizisira[ind].HISSE = Menkul;
                        ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;
                        Form1.dizisira[ind].ORD[ind2, 0] = EmirTipi;
                        Form1.dizisira[ind].ORD[ind2, 1] = BugunTarihi;
                        Form1.dizisira[ind].ORD[ind2, 2] = TakasTarihi;
                        Form1.dizisira[ind].ORD[ind2, 3] = RefId;
                        Form1.dizisira[ind].ORD[ind2, 4] = Menkul;
                        Form1.dizisira[ind].ORD[ind2, 5] = Fin_Inst_Id;
                        Form1.dizisira[ind].ORD[ind2, 6] = AlSat;
                        Form1.dizisira[ind].ORD[ind2, 7] = HesapNo;
                        Form1.dizisira[ind].ORD[ind2, 8] = CustId;
                        Form1.dizisira[ind].ORD[ind2, 9] = AccId;
                        Form1.dizisira[ind].ORD[ind2, 10] = Lot;
                        Form1.dizisira[ind].ORD[ind2, 11] = Maxlot;
                        Form1.dizisira[ind].ORD[ind2, 12] = MarjYuzde;
                        Form1.dizisira[ind].ORD[ind2, 13] = "";           //* SonFiyat               - daha sonra gönderirken oluşacak
                        Form1.dizisira[ind].ORD[ind2, 14] = "";           //* HesaplananEmirFiyati   - daha sonra gönderirken oluşacak
                        Form1.dizisira[ind].ORD[ind2, 15] = Zmn1;
                        Form1.dizisira[ind].ORD[ind2, 16] = Zmn2;
                        Form1.dizisira[ind].ORD[ind2, 17] = Zmn3;
                        Form1.dizisira[ind].ORD[ind2, 18] = "1";          //* Bekliyor.
                        Form1.dizisira[ind].ORD[ind2, 19] = userinf;
                        Form1.dizisira[ind].TOPSATIR_ALTDIZI = Form1.dizisira[ind].TOPSATIR_ALTDIZI + 1;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lock Wait timeout 1");
                }
               // finally
               // {
               //     Monitor.Exit(Form1.factLock_dizisira);
               // }

                  string statu = "Bekliyor";  
                  string zaman = Zmn1 + ":" + Zmn2 + ":" + Zmn3;
                  ZAkt_SG_Doldur(RefId, EmirTipi, Menkul, AlSat, Lot, "-", "-", MarjYuzde, zaman, statu);

                  if (dosyayayaz == true) //*Socketten geliyorsa dosyada yoktur yazsın. Program ilk çalıştı file'dan okuyarak geliyor ise dosyada vardır yeniden yazma...
                      utl.DosyayaYaz(EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, Zmn1, Zmn2, Zmn3, userinf, "0", "0", "0", "0", "0", "0", "0", "0", "0","0", " ");


                  Veri StateObj = new Veri();   //* Start Timer Trigger on Order...
                  StateObj.ind = ind;
                  StateObj.ind2 = ind2;
                  System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Timer_AlgoEmirEkle_ZamanAkt);

                  interval = utl.TimeDifference(Convert.ToInt32(Zmn1), Convert.ToInt32(Zmn2), Convert.ToInt32(Zmn3), 0); //*interval yeniden alalım, çünkü yukardaki işlemlerle zaman kaybettik.
                  if (interval > 0)
                  {
                      System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, interval, interval);
                      StateObj.TimerReference = TimerItem;  // Save a reference for Dispose.
                  }
              }

        }


        //****************************************************************************************************************

        public static void ZAkt_SG_Doldur(string RefId, string EmirTipi, string Menkul, string Alsat, string Lot, string SonFiyat, string HesaplananFiyat, string MarjYuzde, string zaman, string statu)
        {
            ListViewItem view = new ListViewItem();
            view.Text = RefId;
            view.SubItems.Add(EmirTipi);
            view.SubItems.Add(Menkul);
            view.SubItems.Add(Alsat);
            view.SubItems.Add(Lot);
            view.SubItems.Add(SonFiyat);
            view.SubItems.Add(HesaplananFiyat);
            view.SubItems.Add(MarjYuzde);
            view.SubItems.Add(zaman);
            view.SubItems.Add(statu);
            view.SubItems.Add("-");  //* gib_yuzde1 
            view.SubItems.Add("-");  //* gib_yuzde2 
            view.SubItems.Add("-");  //* gib_bşl.zamanı 
            view.SubItems.Add("-");  //* gib_bit.zamanı
            view.SubItems.Add("-");  //* gib_parça sayısı
            view.SubItems.Add("-");  //* gib_enson_aktifolanparca
            view.SubItems.Add("-");  //* aktsek

            try{
                lock (Form1.factLock_lview)
                {
                   // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                   // {
                        Form1.LVIEW.Items.Add(view);
                   // }
                }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Lock Wait timeout 2");
               }
               //finally
               //{
               //     Monitor.Exit(Form1.factLock_lview);
               //}

        }

       //****************************************************************************************************************

        public static void Timer_AlgoEmirEkle_ZamanAkt(object StateObj)
        {
                Veri State = (Veri)StateObj;
                State.TimerReference.Dispose();   //*Threadi 2. bir interval için çalıştırmasın diye yokedelim.

                int ind = State.ind;
                int ind2 = State.ind2;
                string statu = Form1.dizisira[State.ind].ORD[State.ind2, 18];

                if (statu == "1")   //*Bekliyor ise
                {
                    //** İşlemler....
                    string EmirTipi    = Form1.dizisira[ind].ORD[ind2, 0];
                    string BugunTarihi = Form1.dizisira[ind].ORD[ind2, 1];
                    string TakasTarihi = Form1.dizisira[ind].ORD[ind2, 2];
                    string RefId       = Form1.dizisira[ind].ORD[ind2, 3];
                    string Menkul      = Form1.dizisira[ind].ORD[ind2, 4];
                    string Fin_Inst_Id = Form1.dizisira[ind].ORD[ind2, 5];
                    string AlSat       = Form1.dizisira[ind].ORD[ind2, 6];
                    string HesapNo     = Form1.dizisira[ind].ORD[ind2, 7];
                    string CustId      = Form1.dizisira[ind].ORD[ind2, 8];
                    string AccId       = Form1.dizisira[ind].ORD[ind2, 9];
                    string Lot         = Form1.dizisira[ind].ORD[ind2, 10];
                    string Maxlot      = Form1.dizisira[ind].ORD[ind2, 11];
                    string MarjYuzde   = Form1.dizisira[ind].ORD[ind2, 12];
                    string SonFiyat    = Form1.dizisira[ind].ORD[ind2, 13];
                    string HesaplananEmirFiyati = Form1.dizisira[ind].ORD[ind2, 14];
                    string Zmn1       = Form1.dizisira[ind].ORD[ind2, 15];
                    string Zmn2       = Form1.dizisira[ind].ORD[ind2, 16];
                    string Zmn3       = Form1.dizisira[ind].ORD[ind2, 17];
                    string Statu      = Form1.dizisira[ind].ORD[ind2, 18];
                    string userinf    = Form1.dizisira[ind].ORD[ind2, 19];

                    string logdizi = "";
                    Utilities utl = new Utilities();
                    //*** utl.YeniFiyat_Hesapla(Menkul, AlSat, MarjYuzde, ref SonFiyat, ref HesaplananEmirFiyati);
                    utl.YeniFiyat_Hesapla2(Menkul, AlSat, MarjYuzde, ref SonFiyat, ref HesaplananEmirFiyati, ref logdizi);
                    string transaction_id = utl.Save_Equity_Order(EmirTipi, BugunTarihi, TakasTarihi, RefId, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, HesaplananEmirFiyati, userinf);

                    ZAkt_Dizi_Dosya_SG_Guncelle(ind, ind2, RefId, SonFiyat, HesaplananEmirFiyati, transaction_id, logdizi);
                }
          
        }


        //****************************************************************************************************************

        public static void ZAkt_Dizi_Dosya_SG_Guncelle(int ind, int ind2, string refid, string sonfiyat, string hesaplananfiyat, string transaction_id, string logdizi)
        {
            string statu = "2"; //*İşleme konuldu.

            try{
                lock (Form1.factLock_dizisira)
                {
                  //  if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                  //  {
                        Form1.dizisira[ind].ORD[ind2, 18] = statu;
                        Form1.dizisira[ind].ORD[ind2, 13] = sonfiyat;           //*Son Fiyat
                        Form1.dizisira[ind].ORD[ind2, 14] = hesaplananfiyat;    //*Marj % si n göre Hesaplanan Emir Fiyatı.
                  //  }
                }
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Lock Wait timeout 3");
               }
               //finally
               //{
               //     Monitor.Exit(Form1.factLock_dizisira);
               //}




            foreach (ListViewItem lst in Form1.LVIEW.Items)
            {
                    if ((refid == Convert.ToString(lst.SubItems[0].Text)))
                    {
                        if (Convert.ToString(lst.SubItems[9].Text) == "Bekliyor")
                        {

                         try{
                             lock (Form1.factLock_lview)
                             {
                                // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                // {
                                     lst.BackColor = System.Drawing.Color.Green;
                                     lst.ForeColor = System.Drawing.Color.White;
                                     lst.SubItems[5].Text = sonfiyat;
                                     lst.SubItems[6].Text = hesaplananfiyat;
                                     lst.SubItems[9].Text = "Emir İletildi";
                                // }
                             }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lock Wait timeout 4");
                            }
                            //finally
                            //{
                            //   Monitor.Exit(Form1.factLock_lview);
                            //}

                            Utilities utl = new Utilities();
                            //utl.DosyaGuncelle("Z.A", refid, sonfiyat, hesaplananfiyat, 0, statu, transaction_id, " ");   //* Database tablosunu güncelle statüyü koşul sağlandı yap.
                            utl.DosyaGuncelle("Z.A", refid, sonfiyat, hesaplananfiyat, 0, statu, transaction_id, logdizi);   //* Database tablosunu güncelle statüyü koşul sağlandı yap.
                            break;
                        }
                    }
            }
          
        }

        //****************************************************************************************************************
        
        public void ZAkt_EmirIptalEt(string RefId, string Menkul)
        {
            int ind;
            ind = Indis_Bul(Menkul," ");


            try{
                lock (Form1.factLock_dizisira)
                {
                   // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                   // {
                        Form1.dizisira[ind].HISSE = Menkul;
                   // }
                }
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Lock Wait timeout 5");
               }
               //finally
               // {
               //     Monitor.Exit(Form1.factLock_dizisira);
               // }
            

            int ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;
            for (int i = 0; i < ind2; i++)
            {
                    if (Form1.dizisira[ind].ORD[i, 3] == Convert.ToString(RefId))
                    {
                        if (Form1.dizisira[ind].ORD[i, 18] == "1")     //* Bekleyen emir ise Silme yapılabilir.
                        {

                            try{
                                lock (Form1.factLock_dizisira)
                                {
                                   // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                                   // {
                                        Form1.dizisira[ind].ORD[i, 18] = "3";
                                   // }
                                }
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Lock Wait timeout 6");
                               }
                               //finally
                               //{
                               //     Monitor.Exit(Form1.factLock_dizisira);
                               //}

                            Utilities utl = new Utilities();
                            utl.DosyaGuncelle("Z.A", RefId, "0", "0", 0, "3", " ", " ");   //* Database tablosundan iptal et.
                            SG_Sil(Convert.ToString(RefId));
                            break;
                        }

                    }
            }
         
        }

        //****************************************************************************************************************




        public void Gib_EmirIptalEt(string RefId, string Menkul)
        {
            int ind;
            ind = Indis_Bul(Menkul," ");

            try{
                lock (Form1.factLock_dizisira)
                {
                    //if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                    //{
                        Form1.dizisira[ind].HISSE = Menkul;
                    //}
                }
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Lock Wait timeout 7");
               }
               //finally
               //{
               //     Monitor.Exit(Form1.factLock_dizisira);
               //}

     
                int ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;
                for (int i = 0; i < ind2; i++)
                {
                    if (Form1.dizisira[ind].ORD[i, 3] == Convert.ToString(RefId))
                    {
                        if (Form1.dizisira[ind].ORD[i, 18] == "1")     //* Bekleyen emir ise Silme yapılabilir.
                        {

                            try{
                                lock (Form1.factLock_dizisira)
                                {
                                    //if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                                    //{
                                        Form1.dizisira[ind].ORD[i, 18] = "3";
                                    //}
                                }
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Lock Wait timeout 8");
                               }
                               //finally
                               //{
                               //     Monitor.Exit(Form1.factLock_dizisira);
                               //}

                            Utilities utl = new Utilities();
                            int gib_enson_aktifolanparca = Convert.ToInt32(Form1.dizisira[ind].ORD[i, 29]);
                            utl.DosyaGuncelle("G.I.B", RefId, "0", "0", gib_enson_aktifolanparca, "3", " ", " ");
                            utl.Gib_DetayDosyaSil(RefId);
                            SG_Sil(Convert.ToString(RefId));
                            break;
                        }
                    }
                }
           
        }

        //****************************************************************************************************************

        public void SG_Sil(string RefId)
        {

            foreach (ListViewItem lst in Form1.LVIEW.Items)
            {
                if ((RefId == Convert.ToString(lst.SubItems[0].Text)))
                {
                    try{
                        lock (Form1.factLock_lview)
                        {
                           // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                           // {
                                lst.BackColor = System.Drawing.Color.Yellow;
                                lst.ForeColor = System.Drawing.Color.Black;
                                lst.SubItems[9].Text = Convert.ToString("Silindi");
                           // }
                        }
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show("Lock Wait timeout 9");
                       }
                       //finally
                       //{
                       //     Monitor.Exit(Form1.factLock_lview);
                       //}

                    break;
                }
            }
            
        }

        //****************************************************************************************************************************************

        public static int Indis_Bul(string menkul,string aktsek)
        {
            int ind = -1;
            try
            {
                lock (Form1.factLock_mnk)
                {
                    //if (Monitor.TryEnter(Form1.factLock_mnk, 2000))
                    //{
                    ind = Form1.MNK.IndexOf(menkul);
                    if (ind < 0)  //* dizide yoksa ekler.
                    {
                        Form1.MNK.Add(menkul);
                        ind = Form1.MNK.IndexOf(menkul);
                        if (aktsek == "VWAP_IKIISLEMORT")  //*matriks veri okuma kriterini MNK_VWAP dan yapalım.
                            Form1.MNK_VWAP.Add(menkul);
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lock Wait timeout 10");
            }
            //finally
            //{
            //    Monitor.Exit(Form1.factLock_mnk);
           // }


            return ind;
        }

        //****************************************************************************************************************************************


        public void GunIciBolmeli_Emir()
        {
            string[] gelenveri = (gelen).Split('|');
            string userinf = gelenveri[0];
            string emirinf = gelenveri[1];

            string[] emir      = emirinf.Split('&');
            string EmirTipi    = emir[0];
            string BugunTarihi = emir[1];
            string TakasTarihi = emir[2];
            string RefId       = emir[3];
            string Menkul      = emir[4];
            string Fin_Inst_Id = emir[5];
            string Alsat       = emir[6];
            string HesapNo     = emir[7];
            string CustId      = emir[8];
            string AccId       = emir[9];
            string Lot         = emir[10];
            string Maxlot      = emir[11];
            string MarjYuzde   = emir[12];
            string Zmn1        = emir[13];
            string Zmn2        = emir[14];
            string Zmn3        = emir[15];
            string IslemTipi   = emir[16];

            string gib_yuzde1  = emir[17];      
            string gib_yuzde2  = emir[18];      
            string gib_baszmn1 = emir[19];     
            string gib_baszmn2 = emir[20];     
            string gib_baszmn3 = emir[21];    
            string gib_bitzmn1 = emir[22];    
            string gib_bitzmn2 = emir[23];     
            string gib_bitzmn3 = emir[24];      
            string gib_parcasayisi          = emir[25];
            string gib_enson_aktifolanparca = emir[26] ;
            string gib_aktsek = emir[27];

                if (IslemTipi == "1")        //*Yeni emir
                {
                    Gib_DiziyeYaz_DosyayaYaz_EmiriTetikle(EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, Alsat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, Zmn1, Zmn2, Zmn3, gib_yuzde1, gib_yuzde2, gib_baszmn1, gib_baszmn2, gib_baszmn3, gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, gib_parcasayisi, gib_enson_aktifolanparca, gib_aktsek, userinf, dosyayayaz);
                }
                else if (IslemTipi == "3")  //*Silme işlemi
                {
                    Gib_EmirIptalEt(RefId, Menkul);
                }
                else if (IslemTipi == "2")  //*Kalanı Aktive Et
                {
                    Gib_EmirKalaniAktiveEt(RefId, gib_aktsek, "Kullanici.Aktv- ");
                }                                             


        }

        //****************************************************************************************************************************************



        public void Gib_DiziyeYaz_DosyayaYaz_EmiriTetikle(String EmirTipi, String BugunTarihi, String TakasTarihi, String RefId, String Menkul, String Fin_Inst_Id, String AlSat, String HesapNo, String CustId, String AccId, String Lot, String Maxlot, String MarjYuzde, String Zmn1, String Zmn2, String Zmn3,String gib_yuzde1, String gib_yuzde2, String gib_baszmn1, String gib_baszmn2, String gib_baszmn3, String gib_bitzmn1, String gib_bitzmn2 , String gib_bitzmn3, String gib_parcasayisi, String gib_enson_aktifolanparca, String gib_aktsek, String userinf, bool dosyayayaz)
        {
            int seans1_interval = 0, seans2_interval = 0 , seans1_parca1_kadar_gecikme_interval = 0, seans2_parca1_kadar_gecikme_interval = 0;
            int kalanlot = 0, emrlot = 0, sns1_parcasay = 0, sns2_parcasay = 0;
            kalanlotgonderildi = false;
            int ind=-1, ind2=-1;

            ind = Indis_Bul(Menkul, gib_aktsek);

            try{
                lock (Form1.factLock_dizisira)
                {
                   // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                   // {
                        Form1.dizisira[ind].HISSE = Menkul;
                        ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;

                        Form1.dizisira[ind].ORD[ind2, 0] = EmirTipi;
                        Form1.dizisira[ind].ORD[ind2, 1] = BugunTarihi;
                        Form1.dizisira[ind].ORD[ind2, 2] = TakasTarihi;
                        Form1.dizisira[ind].ORD[ind2, 3] = RefId;
                        Form1.dizisira[ind].ORD[ind2, 4] = Menkul;
                        Form1.dizisira[ind].ORD[ind2, 5] = Fin_Inst_Id;
                        Form1.dizisira[ind].ORD[ind2, 6] = AlSat;
                        Form1.dizisira[ind].ORD[ind2, 7] = HesapNo;
                        Form1.dizisira[ind].ORD[ind2, 8] = CustId;
                        Form1.dizisira[ind].ORD[ind2, 9] = AccId;
                        Form1.dizisira[ind].ORD[ind2, 10] = Lot;
                        Form1.dizisira[ind].ORD[ind2, 11] = Maxlot;
                        Form1.dizisira[ind].ORD[ind2, 12] = "";       //*Zakt  MarjYuzde
                        Form1.dizisira[ind].ORD[ind2, 13] = "";       //*ZAkt  Sonfiyat   
                        Form1.dizisira[ind].ORD[ind2, 14] = "";       //*ZAkt  HesaplananEmirFiyati  
                        Form1.dizisira[ind].ORD[ind2, 15] = "";       //*Zakt  Zmn1 
                        Form1.dizisira[ind].ORD[ind2, 16] = "";       //*Zakt  Zmn2 
                        Form1.dizisira[ind].ORD[ind2, 17] = "";       //*Zakt  Zmn3 
                        Form1.dizisira[ind].ORD[ind2, 18] = "1";          //* Bekliyor.
                        Form1.dizisira[ind].ORD[ind2, 19] = userinf;

                        Form1.dizisira[ind].ORD[ind2, 20] = gib_yuzde1;
                        Form1.dizisira[ind].ORD[ind2, 21] = gib_yuzde2;
                        Form1.dizisira[ind].ORD[ind2, 22] = gib_baszmn1;
                        Form1.dizisira[ind].ORD[ind2, 23] = gib_baszmn2;
                        Form1.dizisira[ind].ORD[ind2, 24] = gib_baszmn3;
                        Form1.dizisira[ind].ORD[ind2, 25] = gib_bitzmn1;
                        Form1.dizisira[ind].ORD[ind2, 26] = gib_bitzmn2;
                        Form1.dizisira[ind].ORD[ind2, 27] = gib_bitzmn3;
                        Form1.dizisira[ind].ORD[ind2, 28] = gib_parcasayisi;
                        Form1.dizisira[ind].ORD[ind2, 29] = gib_enson_aktifolanparca;
                        Form1.dizisira[ind].ORD[ind2, 30] = gib_aktsek;
                        Form1.dizisira[ind].TOPSATIR_ALTDIZI = Form1.dizisira[ind].TOPSATIR_ALTDIZI + 1;

                   // }
                }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Lock Wait timeout 11");
               }
              // finally
              // {
              //      Monitor.Exit(Form1.factLock_dizisira);
              // }




            string statu = "Bekliyor";
            string bslzaman = gib_baszmn1 + ":" + gib_baszmn2 + ":" + gib_baszmn3;
            string bitzaman = gib_bitzmn1 + ":" + gib_bitzmn2 + ":" + gib_bitzmn3;

            ZGib_SG_Doldur(RefId, EmirTipi, Menkul, AlSat, Lot, statu, gib_yuzde1, gib_yuzde2, bslzaman, bitzaman, gib_parcasayisi, gib_enson_aktifolanparca, gib_aktsek);

            Utilities utl = new Utilities();

            if (dosyayayaz == true) //*Socketten geliyorsa dosyada yoktur yazsın. Program ilk çalıştı file'dan okuyarak geliyor ise dosyada vardır yeniden yazma...
            {
                utl.DosyayaYaz(EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, "0", "0", "0", "0", userinf, gib_yuzde1, gib_yuzde2, gib_baszmn1, gib_baszmn2, gib_baszmn3, gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, gib_parcasayisi, "0", gib_aktsek);

                utl.Calculate_TimeIntervals(ref seans1_interval, ref seans2_interval, ref seans1_parca1_kadar_gecikme_interval, ref seans2_parca1_kadar_gecikme_interval, Convert.ToInt32(gib_yuzde1), Convert.ToInt32(gib_yuzde2), Convert.ToInt32(gib_baszmn1), Convert.ToInt32(gib_baszmn2), Convert.ToInt32(gib_baszmn3), Convert.ToInt32(gib_bitzmn1), Convert.ToInt32(gib_bitzmn2), Convert.ToInt32(gib_bitzmn3), 0);
                utl.Calculate_GecikmeZamaniParcaSayisi(Lot, gib_parcasayisi, seans1_interval, seans2_interval, gib_yuzde1, ref sns1_parcasay, ref sns2_parcasay, ref emrlot, ref kalanlot);

                int sirano = 0;
                if (seans1_interval > 0)
                    Gib_Emirleri_Olustur(ind, ind2, seans1_interval, seans1_parca1_kadar_gecikme_interval, emrlot, kalanlot, sns1_parcasay, EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, Zmn1, Zmn2, Zmn3, gib_yuzde1, gib_yuzde2, gib_baszmn1, gib_baszmn2, gib_baszmn3, gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, gib_parcasayisi, userinf, gib_aktsek ,dosyayayaz, ref sirano);

                if (seans2_interval > 0)
                    Gib_Emirleri_Olustur(ind, ind2, seans2_interval, seans2_parca1_kadar_gecikme_interval, emrlot, kalanlot, sns2_parcasay, EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, MarjYuzde, Zmn1, Zmn2, Zmn3, gib_yuzde1, gib_yuzde2, gib_baszmn1, gib_baszmn2, gib_baszmn3, gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, gib_parcasayisi, userinf, gib_aktsek, dosyayayaz, ref sirano);
            }
            else  //*Eğer Program kapatılıp açıldı ise ("dosyayayaz=false ise"), GİB de kalan her bir işlemin threadini tekrar başlatmak gerekir.
            {
                Gib_BekleyenDetayEmirleri_Gonder(ind, ind2, RefId);
            }



        }


        //****************************************************************************************************************


        private void Gib_Emirleri_Olustur(int ind, int ind2 ,int interval, int ilkgecikme_interval, int emrlot, int kalanlot, int parcasay, String EmirTipi, String BugunTarihi, String TakasTarihi, String RefId, String Menkul, String Fin_Inst_Id, String AlSat, String HesapNo, String CustId, String AccId, String Lot, String Maxlot, String MarjYuzde, String Zmn1, String Zmn2, String Zmn3, String gib_yuzde1, String gib_yuzde2, String gib_baszmn1, String gib_baszmn2, String gib_baszmn3, String gib_bitzmn1, String gib_bitzmn2, String gib_bitzmn3, String gib_parcasayisi, String userinf, String gib_aktsek, bool dosyayayaz, ref int sirano)
        {
            string fiyat = "0" , statu= "0"; //*bekliyor
            
            int yeniemrlot= emrlot ;
            int yeniinterval;
            int tekbirparcainterval;

            if(parcasay>1)
                tekbirparcainterval = interval / (parcasay - 1);
            else
                tekbirparcainterval = interval / (parcasay);

            if ((kalanlot > 0) && (kalanlotgonderildi == false))
            {
                yeniemrlot = emrlot + kalanlot;
                kalanlotgonderildi = true;
            }


            Utilities utl = new Utilities();

            yeniinterval    = ilkgecikme_interval ;
            DateTime date2  = utl.GetServerDate();
            date2 = date2.AddMilliseconds(yeniinterval + 1000);  //* +1000ms lik gecikme ekliyorum.

          
            for (int i = 0; i < parcasay; i++)
            {
                  sirano++;
                  VeriDetay StateObj = new VeriDetay();   //* Start Timer Trigger on Order...
                  StateObj.ind    = ind;
                  StateObj.ind2   = ind2;
                  StateObj.lot    = yeniemrlot; //* lot değerini sakla. 
                  StateObj.sirano = sirano;

                  System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Timer_AlgoEmirEkle_Gib);

                  System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, yeniinterval, yeniinterval);
                  StateObj.TimerReference = TimerItem;  // Save a reference for Dispose.
                
                  if (dosyayayaz == true) //*Socketten geliyorsa dosyada yoktur yazsın. Program ilk çalıştı file'dan okuyarak geliyor ise dosyada vardır yeniden yazma...
                      utl.GibDetay_DosyasinaYaz(EmirTipi, BugunTarihi, TakasTarihi, RefId, Menkul, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, yeniemrlot, Maxlot, date2, sirano, gib_aktsek,  userinf);

                  date2      = date2.AddMilliseconds(tekbirparcainterval);
                  yeniemrlot = emrlot;
                  yeniinterval += tekbirparcainterval;
            }
         
         }

        //****************************************************************************************************************


        public static void Timer_AlgoEmirEkle_Gib(object StateObj)
        {
         
                VeriDetay State = (VeriDetay)StateObj;
                State.TimerReference.Dispose();   //*Threadi 2. bir interval için çalıştırmasın diye yokedelim.

                int ind = State.ind;
                int ind2 = State.ind2;
                decimal lot = State.lot;     //*sakladığımız lotu geri alalım. Bu lot değerini göndereceğiz. 
                int sirano = State.sirano;   //*sakladığımız sıranosunu alalım.

                string statu = Form1.dizisira[State.ind].ORD[State.ind2, 18];

                if (statu == "1")   //*Bekliyor ise
                {
                    //** İşlemler....
                    string EmirTipi      = Form1.dizisira[ind].ORD[ind2, 0];
                    string BugunTarihi   = Form1.dizisira[ind].ORD[ind2, 1];
                    string TakasTarihi   = Form1.dizisira[ind].ORD[ind2, 2];
                    string RefId         = Form1.dizisira[ind].ORD[ind2, 3];
                    string Menkul        = Form1.dizisira[ind].ORD[ind2, 4];
                    string Fin_Inst_Id   = Form1.dizisira[ind].ORD[ind2, 5];
                    string AlSat         = Form1.dizisira[ind].ORD[ind2, 6];
                    string HesapNo       = Form1.dizisira[ind].ORD[ind2, 7];
                    string CustId        = Form1.dizisira[ind].ORD[ind2, 8];
                    string AccId         = Form1.dizisira[ind].ORD[ind2, 9];
                    string Lot           = Convert.ToString(lot);
                    string Maxlot        = Form1.dizisira[ind].ORD[ind2, 11];
                    string MarjYuzde     = Form1.dizisira[ind].ORD[ind2, 12];
                    string SonFiyat      = Form1.dizisira[ind].ORD[ind2, 13];
                    string HesaplananEmirFiyati = Form1.dizisira[ind].ORD[ind2, 14];
                    string Zmn1          = Form1.dizisira[ind].ORD[ind2, 15];
                    string Zmn2          = Form1.dizisira[ind].ORD[ind2, 16];
                    string Zmn3          = Form1.dizisira[ind].ORD[ind2, 17];
                    string Statu         = Form1.dizisira[ind].ORD[ind2, 18];
                    string userinf       = Form1.dizisira[ind].ORD[ind2, 19];

                    string gib_yuzde1  = Form1.dizisira[ind].ORD[ind2, 20];
                    string gib_yuzde2  = Form1.dizisira[ind].ORD[ind2, 21];
                    string gib_baszmn1 = Form1.dizisira[ind].ORD[ind2, 22];
                    string gib_baszmn2 = Form1.dizisira[ind].ORD[ind2, 23];
                    string gib_baszmn3 = Form1.dizisira[ind].ORD[ind2, 24];
                    string gib_bitzmn1 = Form1.dizisira[ind].ORD[ind2, 25];
                    string gib_bitzmn2 = Form1.dizisira[ind].ORD[ind2, 26];
                    string gib_bitzmn3 = Form1.dizisira[ind].ORD[ind2, 27];
                    string gib_parcasayisi = Form1.dizisira[ind].ORD[ind2, 28];
                    string gib_enson_aktifOlanParca = Form1.dizisira[ind].ORD[ind2, 29];
                    string gib_aktsek = Form1.dizisira[ind].ORD[ind2, 30];


                    string EmirFiyati = "0";
                    string HesaplananAgirlikliOrtalamaFiyati = "0";
                    Utilities utl = new Utilities();
                    gib_aktsek = utl.Gib_SonIkiEmir_BorsadaGerceklestimi(RefId, sirano, gib_aktsek);
                    HesaplananEmirFiyati = utl.Gib_EmirFiyatiBul(Menkul, AlSat, gib_aktsek, ref HesaplananAgirlikliOrtalamaFiyati);

                    string transaction_id = utl.Save_Equity_Order(EmirTipi, BugunTarihi, TakasTarihi, RefId, Fin_Inst_Id, AlSat, HesapNo, CustId, AccId, Lot, Maxlot, HesaplananEmirFiyati, userinf);

                    Gib_Dizi_Dosya_SG_Guncelle(ind, ind2, RefId, sirano, gib_parcasayisi, gib_enson_aktifOlanParca, transaction_id, HesaplananEmirFiyati, gib_aktsek, HesaplananAgirlikliOrtalamaFiyati);
                }
           
        }


        //****************************************************************************************************************

        public static void Gib_Dizi_Dosya_SG_Guncelle(int ind, int ind2, string refid, int sirano, string gib_parcasayisi, string gib_enson_aktifOlanParca, string transaction_id, string HesaplananEmirFiyati, string gib_aksek, string HesaplananAgirlikliOrtalamaFiyati)
        {
            string statu = "2";    //*İşleme konuldu yada "Emir İletildi"

            try{
                lock (Form1.factLock_dizisira)
                {
                   // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                   // {
                        Form1.dizisira[ind].ORD[ind2, 29] = Convert.ToString(sirano);       //* Emir parçalarından En son Aktif olan parça. "gib_enson_aktifOlanParca"  
                   // }
                }
               }
               catch (Exception ex)
               {
                    MessageBox.Show("Lock Wait timeout 12");
               }
               //finally
              // {
              //      Monitor.Exit(Form1.factLock_dizisira);
              // }


            Utilities utl = new Utilities();
            foreach (ListViewItem lst in Form1.LVIEW.Items)
            {
                    if ((refid == Convert.ToString(lst.SubItems[0].Text)) && (Convert.ToString(lst.SubItems[9].Text) == "Bekliyor"))
                    {
                        if (Convert.ToInt32(sirano) == Convert.ToInt32(gib_parcasayisi))    //* Tüm emir parçaları gönderildiyse Ana emirde Statu="Emir İletildi" olsun.
                        {

                            lock (Form1.factLock_dizisira) { Form1.dizisira[ind].ORD[ind2, 18] = statu; }

                            try{
                                lock (Form1.factLock_lview)
                                {
                                  //  if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                  //  {
                                        lst.BackColor = System.Drawing.Color.Green;
                                        lst.ForeColor = System.Drawing.Color.White;
                                        lst.SubItems[9].Text = "Emir İletildi";
                                        lst.SubItems[15].Text = Convert.ToString(sirano);
                                  //  }
                                }
                               }
                               catch (Exception ex)
                               {
                                    MessageBox.Show("Lock Wait timeout 13");
                               }
                               //finally
                               //{
                               //     Monitor.Exit(Form1.factLock_lview);
                               //}


                            utl.DosyaGuncelle("G.I.B", refid, "0", "0", sirano, statu, " ", " ");   //* Database tablosunu güncelle statu=2 , koşul sağlandı yap.
                        }
                        else
                        {

                            try{
                                lock (Form1.factLock_lview)
                                {
                                    //if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                    // {
                                        lst.SubItems[15].Text = Convert.ToString(sirano);
                                    // }
                                }
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Lock Wait timeout 14");
                               }
                               //finally
                               //{
                               //     Monitor.Exit(Form1.factLock_lview);
                               //}


                            utl.DosyaGuncelle("G.I.B", refid, "0", "0", sirano, "1", " ", " ");   //* Ana Emrin tablosuna "Gib_Enson_AktifOlanParca <== sirano" ataması yap. statu=1 de , "Bekliyor" kalsın. Emrin tamamı bitmedi çünkü
                        }
                        break;
                    }
            }

            utl.Gib_DetayDosyaGuncelle(refid, sirano, transaction_id, statu, HesaplananEmirFiyati, gib_aksek, HesaplananAgirlikliOrtalamaFiyati, " ");  //* Parça Emirlerin tutulduğu detay tablosunda Statu="2" , "Emir İletildi" yap.

        }

        //****************************************************************************************************************




        public static void ZGib_SG_Doldur(string RefId, string EmirTipi, string Menkul, string Alsat, string Lot, string statu, string gib_yuzde1, string gib_yuzde2, string bslzaman, string bitzaman, string gib_parcasayisi, string gib_enson_aktifolanparca, String gib_aktsek)
        {
            ListViewItem view = new ListViewItem();
            view.Text = RefId;
            view.SubItems.Add(EmirTipi);
            view.SubItems.Add(Menkul);
            view.SubItems.Add(Alsat);
            view.SubItems.Add(Lot);
            view.SubItems.Add("-");
            view.SubItems.Add("-");
            view.SubItems.Add("-");
            view.SubItems.Add("-");
            view.SubItems.Add(statu);
            view.SubItems.Add(gib_yuzde1);  
            view.SubItems.Add(gib_yuzde2);  
            view.SubItems.Add(bslzaman);  
            view.SubItems.Add(bitzaman); 
            view.SubItems.Add(gib_parcasayisi);
            view.SubItems.Add(gib_enson_aktifolanparca);
            view.SubItems.Add(gib_aktsek);


            try{
                lock (Form1.factLock_lview)
                {
                   // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                   // {
                        Form1.LVIEW.Items.Add(view);
                   // }
                }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Lock Wait timeout 15");
               }
               //finally
               //{
               //     Monitor.Exit(Form1.factLock_lview);
               //}



        }


        //****************************************************************************************************************


        private void Gib_BekleyenDetayEmirleri_Gonder(int ind,int ind2,string RefId)  //*Server kapatılıp açıldı. Bekleyen GİB detaylarını devreye alır.
        {
            string tetikzaman;
            int interval;

            Utilities utl = new Utilities();
            OrderList beklist = utl.GibDetay_BekleyenEmirler_Al(RefId);
            List<Order> lst = beklist.Resultlist;

            foreach (Order a in lst)
            {
                tetikzaman = utl.CreateDate(a.Tetiksaat, a.Tetikdakika, a.Tetiksaniye, 0);
                interval   = utl.GetInterval("GETDATE()", tetikzaman);
                if (interval > 0)
                {
                    VeriDetay StateObj = new VeriDetay();   //* Start Timer Trigger on Order...
                    StateObj.ind    = ind;
                    StateObj.ind2   = ind2;
                    StateObj.lot    = a.Lot;
                    StateObj.sirano = a.Sirano;

                    System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Timer_AlgoEmirEkle_Gib);
                    System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, interval, interval);
                    StateObj.TimerReference = TimerItem;  // Save a reference for Dispose.
                }
            }
        }


        //****************************************************************************************************************

        public static void ZAkt_EmirKalaniAktiveEt(string RefId, string gib_aktsek, string ManuelAktivasyonAciklama)
        {
            string refid, TransactionId, hesapno, menkul, debitcredit, fininstid, customerid, accountid, valuedate, initialmarketdate, tip, gecerlilik, lak, userinf, borsadurum, gib_parcasayisi, ManuelAktAciklama = "", gibaktsekli="";
            Int32 initialMarketSessionSel, EndingMarketSessionSel, orderMaxlot, statu, sirano = 0;
            decimal units, price;

                Utilities utl = new Utilities();
                OrderList beklist = utl.Zakt_Emir_Getir(RefId, gib_aktsek);
                List<Order> lst = beklist.Resultlist;
                foreach (Order a in lst)
                {

                    statu       = Convert.ToInt32(a.Statu);
                    borsadurum  = a.Borsadurum;
                    refid       = Convert.ToString(a.Referansid);                 //* localde detay dosyasında Aktifleşme Şeklini güncelle. 
                    debitcredit = a.Alsat;
                    fininstid   = a.Fin_inst_id;
                    units       = a.Lot;
                    accountid   = a.Accid;
                    customerid  = a.Custid;
                    hesapno     = a.Hesapno;
                    menkul      = a.Menkul;
                    price       = a.Hesaplananemirfiyati;
                    orderMaxlot = Convert.ToInt32(a.Maxlot);
                    tip         = a.Tip;
                    valuedate   = Convert.ToString(a.Takastarihi);
                    initialmarketdate = Convert.ToString(a.Islemtarihi);
                    userinf     = a.Userinf;

                    if (gib_aktsek == "AKTIFE_VER")        gibaktsekli = "AKT.VER";
                    else if (gib_aktsek == "PASIFE_VER")   gibaktsekli = "PAS.VER";
                    else if (gib_aktsek == "TAHTAYI_BOYA") gibaktsekli = "TAH.BOYA";


                    ManuelAktAciklama = ManuelAktivasyonAciklama + "(" + gibaktsekli + " - " + DateTime.Now.ToLongTimeString() + ")";

                    if ((statu == 2) && (borsadurum != "GERÇEKLEŞTİ") && (borsadurum != "İptal Edildi"))  //* Emir İletildi ve Borsada Tahtada Bekliyor 
                    {                                                                                     //* borsadaki emri iyileştir.
                        TransactionId = a.Transactionid;
                        initialMarketSessionSel = Convert.ToInt32(a.Initialmarketsessionsel);
                        EndingMarketSessionSel  = Convert.ToInt32(a.Endingmarketsessionsel);
                        gecerlilik = a.Gecerlilik;
                        lak = a.Lak;

                        bool OrdinoBorsayaIletildimi = utl.EmirOrdinoBorsayaIletildimi(TransactionId);  //* Sistemde bekliyor , yada borsada...
                        if (OrdinoBorsayaIletildimi == false)
                            utl.Sistemden_Iyilestir(TransactionId, debitcredit, fininstid, units, price, customerid, accountid, valuedate, initialmarketdate, initialMarketSessionSel, EndingMarketSessionSel, orderMaxlot, tip, gecerlilik, lak, userinf);
                        else
                            utl.Borsadan_Iyilestir(TransactionId, price, units, units, gecerlilik, initialmarketdate, userinf);

                        utl.ManuelAktivasyonAciklama_Guncelle1(refid, ManuelAktAciklama);
                    }
                    else if ((statu == 1) && (borsadurum != "GERÇEKLEŞTİ") && (borsadurum != "İptal Edildi"))   //Bekliyor , borsaya gönderilmedi henüz.  
                    {

                        ZAkt_AktifThread_CalisanIsi_Bitir(refid, menkul, Convert.ToString(price));
                        TransactionId = utl.Save_Equity_Order(tip, initialmarketdate, valuedate, refid, fininstid, debitcredit, hesapno, customerid, accountid, Convert.ToString(units), Convert.ToString(orderMaxlot), Convert.ToString(price), userinf);
                        ZAkt_KalanAktiveEt_Guncelle(refid, Convert.ToString(price), TransactionId, ManuelAktAciklama);
                    }
                }

        }

        //****************************************************************************************************************


        public static void ZAkt_AktifThread_CalisanIsi_Bitir(string refid, string menkul, string hesaplananfiyat)
        {
            string statu = "2";    //*İşleme konuldu yada "Emir İletildi"
            int ind,ind2;
            string reftut;

            ind = Indis_Bul(menkul," ");

            ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;
            for (int i = 0; i < ind2; i++)
            {
                    reftut = Form1.dizisira[ind].ORD[i, 3];
                    if (refid == reftut)
                    {

                        try{
                            lock (Form1.factLock_dizisira)
                            {
                               // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                               // {
                                    Form1.dizisira[ind].ORD[i, 18] = statu;
                                    Form1.dizisira[ind].ORD[i, 14] = hesaplananfiyat;
                               // }
                            }
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show("Lock Wait timeout 16");
                           }
                           //finally
                           // {
                           //      Monitor.Exit(Form1.factLock_dizisira);
                           // }

                        break;
                    }
            }
        }

        //****************************************************************************************************************

        public static void ZAkt_KalanAktiveEt_Guncelle(string refid, string hesaplananfiyat, string transaction_id, string ManuelAktivasyonAciklama)
        {
            string statu = "2"; //*İşleme konuldu.

            foreach (ListViewItem lst in Form1.LVIEW.Items)
            {
                    if ((refid == Convert.ToString(lst.SubItems[0].Text)))
                    {
                        if (Convert.ToString(lst.SubItems[9].Text) == "Bekliyor")
                        {

                            try{
                                lock (Form1.factLock_lview)
                                {
                                  //  if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                  //  {
                                        lst.BackColor = System.Drawing.Color.Green;
                                        lst.ForeColor = System.Drawing.Color.White;
                                        lst.SubItems[6].Text = hesaplananfiyat;
                                        lst.SubItems[9].Text = "Emir İletildi";
                                  //  }
                                }
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Lock Wait timeout 17");
                               }
                              // finally
                              // {
                              //      Monitor.Exit(Form1.factLock_lview);
                              // }


                            Utilities utl = new Utilities();
                            utl.DosyaGuncelle("Z.A", refid, "0", hesaplananfiyat, 0, statu, transaction_id, ManuelAktivasyonAciklama);   //* Database tablosunu güncelle statüyü koşul sağlandı yap.
                            break;
                        }
                    }
            }
        }



        //****************************************************************************************************************


        public static void Gib_EmirKalaniAktiveEt(string RefId, string gib_aktsek, string ManuelAktivasyonAciklama)
        {
            string refid, TransactionId, hesapno, menkul, debitcredit, fininstid, customerid, accountid, valuedate, initialmarketdate, tip, gecerlilik, lak, userinf, borsadurum, gib_parcasayisi, gibaktsekli="", ManuelAktAciklama="";
            Int32 initialMarketSessionSel, EndingMarketSessionSel, orderMaxlot, statu,sirano=0;
            decimal units, price;

                Utilities utl = new Utilities();
                OrderList beklist = utl.GibDetay_Emirler_Getir(RefId, gib_aktsek);
                List<Order> lst = beklist.Resultlist;
                foreach (Order a in lst)
                {

                    statu         = Convert.ToInt32(a.Statu);
                    borsadurum    = a.Borsadurum;
                    refid         = Convert.ToString(a.Referansid);                 //* localde detay dosyasında Aktifleşme Şeklini güncelle. 
                    sirano        = a.Sirano;
                    debitcredit   = a.Alsat;
                    fininstid     = a.Fin_inst_id;
                    units         = a.Lot;
                    accountid     = a.Accid;
                    customerid    = a.Custid;
                    hesapno       = a.Hesapno;
                    menkul        = a.Menkul;
                    price         = a.Hesaplananemirfiyati;
                    orderMaxlot   = Convert.ToInt32(a.Maxlot);
                    tip           = a.Tip;
                    valuedate         = Convert.ToString(a.Takastarihi);
                    initialmarketdate = Convert.ToString(a.Islemtarihi);
                    userinf = a.Userinf;

                    if (gib_aktsek == "AKTIFE_VER")        gibaktsekli = "AKT.VER"; 
                    else if (gib_aktsek == "PASIFE_VER")   gibaktsekli = "PAS.VER";
                    else if (gib_aktsek == "TAHTAYI_BOYA") gibaktsekli = "TAH.BOYA";

                    ManuelAktAciklama = ManuelAktivasyonAciklama + "(" + gibaktsekli + " - " + DateTime.Now.ToLongTimeString() + ")";


                    if ((statu == 2) && (borsadurum != "GERÇEKLEŞTİ") && (borsadurum != "İptal Edildi"))  //* Emir İletildi ve Borsada Tahtada Bekliyor 
                    {                                                                                     //* borsadaki emri iyileştir.
                        TransactionId           = a.Transactionid;
                        initialMarketSessionSel = Convert.ToInt32(a.Initialmarketsessionsel);
                        EndingMarketSessionSel  = Convert.ToInt32(a.Endingmarketsessionsel);
                        gecerlilik              = a.Gecerlilik;
                        lak                     = a.Lak;

                        bool OrdinoBorsayaIletildimi = utl.EmirOrdinoBorsayaIletildimi(TransactionId);  //* Sistemde bekliyor , yada borsada...
                        if (OrdinoBorsayaIletildimi == false)
                            utl.Sistemden_Iyilestir(TransactionId, debitcredit, fininstid, units, price, customerid, accountid, valuedate, initialmarketdate, initialMarketSessionSel, EndingMarketSessionSel, orderMaxlot, tip, gecerlilik, lak, userinf);
                        else
                            utl.Borsadan_Iyilestir(TransactionId, price, units, units, gecerlilik, initialmarketdate, userinf);

                        utl.Gib_DetayDosya_AktiflesmeSeklini_Guncelle(refid, sirano, gib_aktsek, ManuelAktAciklama);      //* localde detay dosyasında Aktifleşme Şeklini güncelle. 
                        utl.ManuelAktivasyonAciklama_Guncelle1(refid, ManuelAktAciklama);

                    }
                    else if ((statu == 1) && (borsadurum != "GERÇEKLEŞTİ") && (borsadurum != "İptal Edildi"))   //Bekliyor , borsaya gönderilmedi henüz.  
                    {
                        string HesaplananAgirlikliOrtalamaFiyati = "0";
                        gib_parcasayisi = Convert.ToString(a.Gib_parcasayisi);

                        Gib_AktifThread_CalisanIsi_Bitir(refid, menkul, sirano);
                        TransactionId = utl.Save_Equity_Order(tip, initialmarketdate, valuedate, refid, fininstid, debitcredit, hesapno, customerid, accountid, Convert.ToString(units), Convert.ToString(orderMaxlot), Convert.ToString(price), userinf);
                        Gib_KalanAktiveEt_Guncelle(refid, sirano, gib_parcasayisi, TransactionId, Convert.ToString(price), gib_aktsek, HesaplananAgirlikliOrtalamaFiyati, ManuelAktAciklama);

                    }
                }

        }

        //****************************************************************************************************************

        public static void Gib_AktifThread_CalisanIsi_Bitir(string refid, string menkul, int sirano)
        {
            string statu = "2";    //*İşleme konuldu yada "Emir İletildi"
            string reftut;
            int ind, ind2;

            ind = Indis_Bul(menkul," ");

            ind2 = Form1.dizisira[ind].TOPSATIR_ALTDIZI;
            for (int i = 0; i < ind2; i++)
            {
                    reftut = Form1.dizisira[ind].ORD[i, 3];
                    if (refid == reftut)
                    {

                        try{
                            lock (Form1.factLock_dizisira)
                            {
                               // if (Monitor.TryEnter(Form1.factLock_dizisira, 2000))
                               // {
                                    Form1.dizisira[ind].ORD[i, 29] = Convert.ToString(sirano);
                                    Form1.dizisira[ind].ORD[i, 18] = statu;
                               // }
                            }
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show("Lock Wait timeout 18");
                           }
                           //finally
                           //{
                           //     Monitor.Exit(Form1.factLock_dizisira);
                           //}

                        break;
                    }

            }
        }


        //****************************************************************************************************************

        public static void Gib_KalanAktiveEt_Guncelle(string refid, int sirano, string gib_parcasayisi, string transaction_id, string HesaplananEmirFiyati, string gib_aksek, string HesaplananAgirlikliOrtalamaFiyati, string ManuelAktivasyonAciklama)
        {
            string statu = "2";    //*İşleme konuldu yada "Emir İletildi"

            Utilities utl = new Utilities();
            foreach (ListViewItem lst in Form1.LVIEW.Items)
            {
                    if ((refid == Convert.ToString(lst.SubItems[0].Text)) && (Convert.ToString(lst.SubItems[9].Text) == "Bekliyor"))
                    {
                        if (Convert.ToInt32(sirano) == Convert.ToInt32(gib_parcasayisi))    //* Tüm emir parçaları gönderildiyse Ana emirde Statu="Emir İletildi" olsun.
                        {

                            try{
                                lock (Form1.factLock_lview)
                                {
                                   // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                   // {
                                        lst.BackColor = System.Drawing.Color.Green;
                                        lst.ForeColor = System.Drawing.Color.White;
                                        lst.SubItems[9].Text = "Emir İletildi";
                                        lst.SubItems[15].Text = Convert.ToString(sirano);
                                   // }
                                }
                               }
                               catch (Exception ex)
                               {
                                    MessageBox.Show("Lock Wait timeout 19");
                               }
                               // finally
                               // {
                               //     Monitor.Exit(Form1.factLock_lview);
                               // }


                            utl.DosyaGuncelle("G.I.B", refid, "0", "0", sirano, statu, " ", ManuelAktivasyonAciklama);   //* Database tablosunu güncelle statu=2 , koşul sağlandı yap.
                        }
                        else
                        {

                            try{
                                lock (Form1.factLock_lview)
                                {
                                   // if (Monitor.TryEnter(Form1.factLock_lview, 2000))
                                   // {
                                        lst.SubItems[15].Text = Convert.ToString(sirano);
                                   // }
                                }
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show("Lock Wait timeout 20");
                               }
                               //finally
                               //{
                               //     Monitor.Exit(Form1.factLock_lview);
                               //}

                            utl.DosyaGuncelle("G.I.B", refid, "0", "0", sirano, "1", " ", ManuelAktivasyonAciklama);   //* Ana Emrin tablosuna "Gib_Enson_AktifOlanParca <== sirano" ataması yap. statu=1 de , "Bekliyor" kalsın. Emrin tamamı bitmedi çünkü
                        }
                        break;
                    }
            }

            utl.Gib_DetayDosyaGuncelle(refid, sirano, transaction_id, statu, HesaplananEmirFiyati, gib_aksek, HesaplananAgirlikliOrtalamaFiyati, ManuelAktivasyonAciklama);  //* Parça Emirlerin tutulduğu detay tablosunda Statu="2" , "Emir İletildi" yap.
        }


        //****************************************************************************************************************

        public void Kapanista_BekleyenEmirleri_Aktive_Et()   
        {
            string Zmn1="17" , Zmn2="29" , Zmn3="00";       //** Sadece G.İ.B emirleri aktive edilecektir.
             
            Utilities utl = new Utilities();
            Veri StateObj = new Veri();   
            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(Timer_KapanistaBekleyenEmirler);

            int interval = utl.TimeDifference(Convert.ToInt32(Zmn1), Convert.ToInt32(Zmn2), Convert.ToInt32(Zmn3), 0); //*interval yeniden alalım, çünkü yukardaki işlemlerle zaman kaybettik.
            if (interval > 0)
            {
                System.Threading.Timer TimerItem = new System.Threading.Timer(TimerDelegate, StateObj, interval, interval);
                StateObj.TimerReference = TimerItem;  // Save a reference for Dispose.
            }

        }


        public static void Timer_KapanistaBekleyenEmirler(object StateObj)
        {
                string EmirTipi = "", RefId = "";
                Veri State = (Veri)StateObj;
                State.TimerReference.Dispose();   //*Threadi 2. bir interval için çalıştırmasın diye yokedelim.

                Utilities utl = new Utilities();
                OrderList beklist = utl.BugunkuTumEmirleri_Al();

                List<Order> lst = beklist.Resultlist;
                foreach (Order a in lst)
                {
                    RefId = Convert.ToString(a.Referansid);
                    EmirTipi = Convert.ToString(a.Emirtipi);
                    if (EmirTipi == "G.I.B")      //** Sadece G.İ.B emirleri aktive edilecektir.    
                        Gib_EmirKalaniAktiveEt(RefId, "TAHTAYI_BOYA", "Kapanis.Aktv- ");
                }

        }


        //****************************************************************************************************************



    }


   //****************************************************************************************************************
   //****************************************************************************************************************

    public class Veri
    {
        // Used to hold parameters for calls to TimerTask.
        public int ind;
        public int ind2;
        public System.Threading.Timer TimerReference;
    }

    public class VeriDetay
    {
        // Used to hold parameters for calls to TimerTask.
        public int ind;
        public int ind2;
        public decimal lot;
        public int sirano;
        public System.Threading.Timer TimerReference;
    }


}
