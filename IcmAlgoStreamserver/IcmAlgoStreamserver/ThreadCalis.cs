using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IcmAlgoStreamserver
{
    class ThreadCalis
    {
        public string gelen;

        public ThreadCalis(string gelen)
        {
            this.gelen = gelen;
        }


      //**************************************************************************************************************************

        public void islem()
        {
            int pos , tutx , xx;
            string str_tut = "", satirtut = "", func = "" ;

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

                              if (Form1.MNK.IndexOf(hisse) >= 0)
                              {
                                  try
                                  {
                                      string islemno, fiyat, miktar, zaman;
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
                          }


                          gelen = gelen.Substring(pos + 1, gelen.Length - (pos + 1));
                          pos = gelen.ToString().IndexOf("\n");
                      }
                
            }
            catch (Exception ex) { }

           
        }
        //**************************************************************************************************************************


    }
}
