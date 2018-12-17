
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* from vs */

namespace IcmAlgoStreamserver
{
    class Order
    {
        private decimal referansid;

        public decimal Referansid
        {
            get { return referansid; }
            set { referansid = value; }
        }

        private int sirano;

        public int Sirano
        {
            get { return sirano; }
            set { sirano = value; }
        }

        private string hesapno;

        public string Hesapno
        {
            get { return hesapno; }
            set { hesapno = value; }
        }
        private string custid;

        public string Custid
        {
            get { return custid; }
            set { custid = value; }
        }

        private string menkul;

        public string Menkul
        {
            get { return menkul; }
            set { menkul = value; }
        }
        private string fin_inst_id;

        public string Fin_inst_id
        {
            get { return fin_inst_id; }
            set { fin_inst_id = value; }
        }
        private string emirtipi;

        public string Emirtipi
        {
            get { return emirtipi; }
            set { emirtipi = value; }
        }
        private DateTime islemtarihi;

        public DateTime Islemtarihi
        {
            get { return islemtarihi; }
            set { islemtarihi = value; }
        }
        private DateTime takastarihi;

        public DateTime Takastarihi
        {
            get { return takastarihi; }
            set { takastarihi = value; }
        }
        private string alsat;

        public string Alsat
        {
            get { return alsat; }
            set { alsat = value; }
        }
        private string accid;

        public string Accid
        {
            get { return accid; }
            set { accid = value; }
        }
        private decimal lot;

        public decimal Lot
        {
            get { return lot; }
            set { lot = value; }
        }
        private decimal maxlot;

        public decimal Maxlot
        {
            get { return maxlot; }
            set { maxlot = value; }
        }
        private int marjyuzde;

        public int Marjyuzde
        {
            get { return marjyuzde; }
            set { marjyuzde = value; }
        }
        private decimal sonfiyat;

        public decimal Sonfiyat
        {
            get { return sonfiyat; }
            set { sonfiyat = value; }
        }
        private decimal hesaplananemirfiyati;

        public decimal Hesaplananemirfiyati
        {
            get { return hesaplananemirfiyati; }
            set { hesaplananemirfiyati = value; }
        }
        private int tetiksaat;

        public int Tetiksaat
        {
            get { return tetiksaat; }
            set { tetiksaat = value; }
        }
        private int tetikdakika;

        public int Tetikdakika
        {
            get { return tetikdakika; }
            set { tetikdakika = value; }
        }
        private int tetiksaniye;

        public int Tetiksaniye
        {
            get { return tetiksaniye; }
            set { tetiksaniye = value; }
        }
        private string statu;

        public string Statu
        {
            get { return statu; }
            set { statu = value; }
        }
        private string userinf;

        public string Userinf
        {
            get { return userinf; }
            set { userinf = value; }
        }

        private int gib_yuzde1;

        public int Gib_yuzde1
        {
            get { return gib_yuzde1; }
            set { gib_yuzde1 = value; }
        }

        private int gib_yuzde2;

        public int Gib_yuzde2
        {
            get { return gib_yuzde2; }
            set { gib_yuzde2 = value; }
        }

        private int gib_baszmn1;

        public int Gib_baszmn1
        {
            get { return gib_baszmn1; }
            set { gib_baszmn1 = value; }
        }

        private int gib_baszmn2;

        public int Gib_baszmn2
        {
            get { return gib_baszmn2; }
            set { gib_baszmn2 = value; }
        }

        private int gib_baszmn3;

        public int Gib_baszmn3
        {
            get { return gib_baszmn3; }
            set { gib_baszmn3 = value; }
        }

        private int gib_bitzmn1;

        public int Gib_bitzmn1
        {
            get { return gib_bitzmn1; }
            set { gib_bitzmn1 = value; }
        }

        private int gib_bitzmn2;

        public int Gib_bitzmn2
        {
            get { return gib_bitzmn2; }
            set { gib_bitzmn2 = value; }
        }

        private int gib_bitzmn3;

        public int Gib_bitzmn3
        {
            get { return gib_bitzmn3; }
            set { gib_bitzmn3 = value; }
        }

        private int gib_parcasayisi;

        public int Gib_parcasayisi
        {
            get { return gib_parcasayisi; }
            set { gib_parcasayisi = value; }
        }

         private int gib_enson_aktifOlanParca;

       public int Gib_enson_aktifOlanParca
       {
           get { return gib_enson_aktifOlanParca; }
           set { gib_enson_aktifOlanParca = value; }
       }


       private string aktiflesme_sekli;

       public string Aktiflesme_sekli
       {
           get { return aktiflesme_sekli; }
           set { aktiflesme_sekli = value; }
       }


       private decimal hesaplananagirlikliortalamafiyati;

       public decimal Hesaplananagirlikliortalamafiyati
       {
           get { return hesaplananagirlikliortalamafiyati; }
           set { hesaplananagirlikliortalamafiyati = value; }
       }

       private string borsadurum;

       public string Borsadurum
       {
           get { return borsadurum; }
           set { borsadurum = value; }
       }


       private string initialmarketsessionsel;

       public string Initialmarketsessionsel
       {
           get { return initialmarketsessionsel; }
           set { initialmarketsessionsel = value; }
       }




       private string endingmarketsessionsel;

       public string Endingmarketsessionsel
       {
           get { return endingmarketsessionsel; }
           set { endingmarketsessionsel = value; }
       }

   

       private string tip;

       public string Tip
       {
           get { return tip; }
           set { tip = value; }
       }


       private string gecerlilik;

       public string Gecerlilik
       {
           get { return gecerlilik; }
           set { gecerlilik = value; }
       }

       private string lak;

       public string Lak
       {
           get { return lak; }
           set { lak = value; }
       }

   

       private string transactionid;

       public string Transactionid
       {
           get { return transactionid; }
           set { transactionid = value; }
       }

   

    }
}

