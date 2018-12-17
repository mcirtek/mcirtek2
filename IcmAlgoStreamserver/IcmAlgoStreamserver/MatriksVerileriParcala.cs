using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IcmAlgoStreamserver
{
    class MatriksVerileriParcala
    {
        public string satirtut;
        public string hisse;

        public MatriksVerileriParcala(string satirtut, string hisse)
        {
            this.satirtut = satirtut;
            this.hisse = hisse;
        }


        //**************************************************************************************************************************

        public void islem()
        {
            int pos2;
            string islemno, fiyat, miktar, zaman;

            try
            {
                pos2 = satirtut.IndexOf("ÿ");
                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                islemno = satirtut.Substring(0, pos2);

                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                fiyat = satirtut.Substring(0, pos2);

                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                miktar = satirtut.Substring(0, pos2);

                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");

                satirtut = (satirtut.Substring(pos2 + 1, satirtut.Length - (pos2 + 1))).Trim();
                pos2 = satirtut.IndexOf("ÿ");
                zaman = satirtut.Substring(0, pos2);
                zaman = zaman.Replace(":", "");
                Utilities utl = new Utilities();
                utl.Matriksten_GerceklesenHisseyeAitVeriyiSakla(islemno, hisse, fiyat, miktar, zaman);
                utl = null;
            }
            catch (Exception ex11) { }

        }
        //**************************************************************************************************************************

    }
}
