using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtp.Framework;
using Gtp.Framework.ControlLibrary;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace IcmAlgoStreamserver
{
    class Utilities
    {
        public Utilities()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }

        //********************************************************************************************************************

        public string Save_Equity_Order(string EmirTipi, string BugunTarihi, string TakasTarihi, string RefId, string Fin_Inst_Id, string AlSat, string HesapNo, string CustId, string AccId, string Lot, string Maxlot, string HesaplananEmirFiyati, string userinf)
        {
            string transaction_id="";
            string Error;
            int OrderEntryStatus;
            string EquityTransactionTypeId = "0000-000001-ETT";  //* defaultu LOT olsun.
            string OrderType = "0000-000001-ETT";
            string TimeInForce = "0"; //Gün
            Int32 InitialMarketSessionSel = 1, EndingMarketSessionSel = 2;

            decimal fiyat = Convert.ToDecimal(HesaplananEmirFiyati);

            string hangiseanstayiz = HangiSeanstayiz();        //* sql server üzerinden zamanı alalım.
            if (hangiseanstayiz == "1")                        //* Eğer 1. seanstaysak 
            {
                InitialMarketSessionSel = 1;
                EndingMarketSessionSel = 2;
            }
            else if (hangiseanstayiz == "2")                   //* 2. seanstaysak
            {
                InitialMarketSessionSel = 1;
                EndingMarketSessionSel = 1;
            }


            DataTable Tablo1;
            var g = new GtpXml("SAVE_EQUITY_ORDER", "1.0");

            g.AddParameter("orderDate", "System.DateTime", Convert.ToDateTime(BugunTarihi));
            g.AddParameter("transactionExtId", "System.String", " ");
            g.AddParameter("transactionStatus", "System.String", "ACTIVE");
            g.AddParameter("customerId", "System.String", CustId);
            g.AddParameter("accountId", "System.String", AccId);
            g.AddParameter("equityTransactionTypeId", "System.String", EquityTransactionTypeId);
            g.AddParameter("debitCredit", "System.String", AlSat);
            g.AddParameter("finInstId", "System.String", Fin_Inst_Id);
            g.AddParameter("units", "System.Decimal", Convert.ToDecimal(Lot));
            g.AddParameter("orderPrice", "System.Decimal", fiyat);
            g.AddParameter("orderMaxLot", "System.Int32", Convert.ToInt32(Maxlot));
            g.AddParameter("intLotCount", "System.Int32", Convert.ToInt32(1));
            g.AddParameter("SettlementDate", "System.DateTime", Convert.ToDateTime(TakasTarihi));
            g.AddParameter("initialMarketSessionDate", "System.DateTime", Convert.ToDateTime(BugunTarihi));
            g.AddParameter("initialMarketSessionSel", "System.Int32", InitialMarketSessionSel);
            g.AddParameter("endingMarketSessionDate", "System.DateTime", Convert.ToDateTime(BugunTarihi));
            g.AddParameter("endingMarketSessionSel", "System.Int32", EndingMarketSessionSel);
            g.AddParameter("holdType", "System.String", "NONE");
            g.AddParameter("orderType", "System.String", OrderType);
            g.AddParameter("timeInForce", "System.String", TimeInForce);
            g.AddParameter("limitControl", "System.String", "N");
            g.AddParameter("routeTypeId", "System.String", "0000-000001-TRT");
            g.AddParameter("routeIntSysId", "System.String", " ");
            g.AddParameter("routeCollPosId", "System.String", "0000-000002-POS");
            g.AddParameter("destReconPosId", "System.String", " ");
            g.AddParameter("left", "System.Decimal", Convert.ToDecimal(Lot));
            g.AddParameter("shortfall", "System.Int32", Convert.ToDecimal(0));
            g.AddParameter("oldeAmount", "System.Decimal", Convert.ToDecimal(0));
            g.AddParameter("marketType", "System.String", " ");
            g.AddParameter("verbalOrder", "System.String", "0");
            g.AddParameter("SMSFillNotification", "bool", Convert.ToBoolean("False"));
            g.AddParameter("EmailFillNotification", "bool", Convert.ToBoolean("False"));
            g.AddParameter("equityTransfer", "System.String", "0");

            var userInfo = new UserInfoRec();
            userInfo.Reset();
            string[] usr = (userinf).Split(';');
            userInfo.ApplicationName = usr[0];
            userInfo.ChannelId = usr[1];
            userInfo.CustomSectionName = usr[2];
            userInfo.DivisionId = usr[3];
            userInfo.EmployeeId = usr[4];
            userInfo.HostName = usr[5];
            userInfo.OrganizationGroupId = usr[6];
            userInfo.OrganizationId = usr[7];
            userInfo.PartyId = usr[8];
            userInfo.PositionId = usr[9];
            userInfo.SessionId = usr[10];

            GtpXml res = ExecCustomSQL2(userInfo, g);
            try
            { Error = res.GetNodeContent("request-broker-message\\response\\error"); }
            catch
            { Error = null; }

            if (Error == null) Error = res.GetResponseOutput("Error");
            if (Error != null) Error += ":" + res.GetResponseOutput("RealError");

            if (Error == null)
            {
                decimal OrderAmount = Convert.ToDecimal(res.GetResponseOutput("OrderAmount"));

                var x = res.GetResponseOutputDataSet("orders");
                Tablo1 = x.Tables[0];
                
                for (int i = 0; i < Tablo1.Rows.Count; i++)
                {
                    transaction_id = Tablo1.Rows[i]["TRANSACTION_ID"].ToString();
                }
                OrderEntryStatus = Convert.ToInt32(res.GetResponseOutput("OrderEntryStatus"));

            }
            else
                Error = "HATA : " + Error;

            if (Error == null) Error = "";

            return transaction_id;
        }


        //********************************************************************************************************************

        public string Update_Equity_Order(string TransactionId, string debitcredit, string fininstid, decimal units, decimal price, string customerid, string accountid, string valuedate, string initialmarketdate, int initialMarketSessionSel, int EndingMarketSessionSel, int orderMaxlot, string tip, string gecerlilik, string lak, string userinf)
        {
            string Error;
            string orderId = "", orderType, newTIF = "0", newExpireDate;
            bool result = false;

            orderType = lak;
            newTIF = "0";  //TimeinForce

            DateTime dt = Convert.ToDateTime(initialmarketdate);
            newExpireDate = dt.ToString("yyyyMMdd");

            GtpXml g = new GtpXml("UPDATE_EQUITY_ORDER", "1.0");

            g.AddParameter("CheckLimits", "System.Boolean", Convert.ToBoolean("True"));
            g.AddParameter("IsPastDateOrder", "System.Boolean", Convert.ToBoolean("False"));
            g.AddParameter("CanUpdatePriceUnit", "System.String", "0");
            g.AddParameter("TransactionId", "string", TransactionId);
            g.AddParameter("LimitControl", "System.String", "N");
            g.AddParameter("DebitCredit", "string", debitcredit);
            g.AddParameter("FinInstId", "System.String", fininstid);
            g.AddParameter("Units", "System.Decimal", units);
            g.AddParameter("Price", "System.Decimal", price);
            g.AddParameter("BrokerPasor", "string", "PASOR");
            g.AddParameter("PasorId", "string", "0000-000002-POS");
            g.AddParameter("BrokerId", "string", " ");
            g.AddParameter("CustomerId", "string", customerid);
            g.AddParameter("AccountId", "string", accountid);
            g.AddParameter("valueDate", "System.DateTime", valuedate);

            g.AddParameter("InitialMarketDate", "System.DateTime", initialmarketdate);

            g.AddParameter("InitialMarketSessionSel", "System.String", initialMarketSessionSel);
            g.AddParameter("EndingMarketSessionSel", "System.String", EndingMarketSessionSel);
            g.AddParameter("orderMaxLot", "System.Int32", orderMaxlot);
            g.AddParameter("OrderType", "System.String", orderType);
            g.AddParameter("newTIF", "System.String", newTIF);
            g.AddParameter("newExpireDate", "System.String", newExpireDate);


            var userInfo = new UserInfoRec();
            userInfo.Reset();
            string[] usr = (userinf).Split(';');
            userInfo.ApplicationName = usr[0];
            userInfo.ChannelId = usr[1];
            userInfo.CustomSectionName = usr[2];
            userInfo.DivisionId = usr[3];
            userInfo.EmployeeId = usr[4];
            userInfo.HostName = usr[5];
            userInfo.OrganizationGroupId = usr[6];
            userInfo.OrganizationId = usr[7];
            userInfo.PartyId = usr[8];
            userInfo.PositionId = usr[9];
            userInfo.SessionId = usr[10];

            GtpXml res = ExecCustomSQL2(userInfo, g);

            try
            { Error = res.GetNodeContent("request-broker-message\\response\\error"); }
            catch
            {
                Error = null;
            }


            if (Error == null) Error = res.GetResponseOutput("Error");
            if (Error != null) Error += ":" + res.GetResponseOutput("RealError");

            if (Error == null)
            {
                orderId = Convert.ToString(res.GetResponseOutput("orderId"));
                result = Convert.ToBoolean(res.GetResponseOutput("Result"));
            }
            else
            {
                Error = "HATA : " + Error;
            }

            if (Error == null) Error = "";


            return Error;
        }


        //********************************************************************************************************************

        public string Save_Improve_Order(string TransactionId, decimal improveprice, decimal improveunits, decimal oldunits, string gecerlilik, string initialmarketdate, string userinf)
        {
            string Error;
            string orderId = "", oldTIF = "0", newTIF = "0", newExpireDate;
            bool result = false;

            if (gecerlilik == "Gün")
                newTIF = "0";
            else if (gecerlilik == "KİE")
                newTIF = "3";

           //  newExpireDate = initialmarketdate;
            DateTime dt = Convert.ToDateTime(initialmarketdate);
            newExpireDate = dt.ToString("yyyyMMdd");


            GtpXml g = new GtpXml("SAVE_IMPROVE_ORDER", "1.0");

            g.AddParameter("orderId", "System.String", TransactionId);
            g.AddParameter("improvePrice", "System.Decimal", improveprice);
            g.AddParameter("improveUnits", "System.Decimal", improveunits);
            g.AddParameter("oldUnits", "System.Decimal", oldunits);
            g.AddParameter("oldTIF", "System.String", oldTIF);
            g.AddParameter("newTIF", "System.String", newTIF);
            g.AddParameter("newExpireDate", "System.String", newExpireDate);
            g.AddParameter("RejectIfOrderChanged", "bool", true);

            var userInfo = new UserInfoRec();
            userInfo.Reset();
            string[] usr = (userinf).Split(';');
            userInfo.ApplicationName = usr[0];
            userInfo.ChannelId = usr[1];
            userInfo.CustomSectionName = usr[2];
            userInfo.DivisionId = usr[3];
            userInfo.EmployeeId = usr[4];
            userInfo.HostName = usr[5];
            userInfo.OrganizationGroupId = usr[6];
            userInfo.OrganizationId = usr[7];
            userInfo.PartyId = usr[8];
            userInfo.PositionId = usr[9];
            userInfo.SessionId = usr[10];

            GtpXml res = ExecCustomSQL2(userInfo, g);

            try
            { Error = res.GetNodeContent("request-broker-message\\response\\error"); }
            catch
            {
                Error = null;
            }

            if (Error == null) Error = res.GetResponseOutput("Error");
            if (Error != null) Error += ":" + res.GetResponseOutput("RealError");

            if (Error == null)
            {
                orderId = Convert.ToString(res.GetResponseOutput("orderId"));
            }
            else
            {
                Error = "HATA : " + Error;
            }

            if (Error == null) Error = "";


            return Error;
        }


        //********************************************************************************************************************



     
        public int TimeDifference(int hours, int minutes, int seconds, int milliseconds)
        {
            DateTime dateTime = DateTime.Now;
            int interval=0;

            Random rnd = new Random();
            int milsecond = rnd.Next(1, 999);

            DateTime ilktar = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milsecond,
                dateTime.Kind);

            string tut = Convert.ToDateTime(ilktar).ToString("yyyy-MM-dd HH:mm:ss.fff"); 

            string str = "SELECT DATEDIFF(ms,GETDATE(),'" + tut + "') AS Fark";
            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                interval = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Fark"]));
            }

            return interval;
        }


        //********************************************************************************************************************

        public void Calculate_TimeIntervals(ref int seans1_interval, ref int seans2_interval, ref int seans1_parca1_kadar_gecikme_interval, ref int seans2_parca1_kadar_gecikme_interval, int gib_yuzde1, int gib_yuzde2, int gib_baszmn1, int gib_baszmn2, int gib_baszmn3, int gib_bitzmn1, int gib_bitzmn2, int gib_bitzmn3, int milliseconds)
        {
            string seans1_baszmn, seans1_bitzmn, seans2_baszmn, seans2_bitzmn;

            int Bitsaat_Seans1=12, Bitdakika_Seans1=30; 
            int Bassaat_Seans2=13, Basdakika_Seans2=30;
            int Bitsaat_Seans2=17, Bitdakika_Seans2=30;  //** default değerler. Aşağıda BorsaSaatleri ile dosyadan alarak bunları eziyoruz.

            BorsaSaatleri(ref Bitsaat_Seans1, ref Bitdakika_Seans1, ref Bassaat_Seans2, ref Basdakika_Seans2, ref Bitsaat_Seans2, ref Bitdakika_Seans2);

            string seans = HangiSeanstayiz();
            if (seans == "1")
            {
                if (gib_yuzde1 > 0)
                {
                    if (gib_yuzde2 > 0)
                    {
                        if (gib_baszmn1 > 0)
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans1_baszmn = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans1_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans1_baszmn);   //* İlk emir parçasına kadar bir gecikme süresi var bunuda hesaba katacağız.  
                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                            else
                            {
                                seans1_baszmn = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans1_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans1_baszmn);   //* İlk emir parçasına kadar bir gecikme süresi var bunuda hesaba katacağız.  
                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                        }
                        else
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans1_baszmn = "GETDATE()";
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans1_parca1_kadar_gecikme_interval = 0;
                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                            else
                            {
                                seans1_baszmn = "GETDATE()";
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans1_parca1_kadar_gecikme_interval = 0;
                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                        }

                    }
                    else
                    {
                        if (gib_baszmn1 > 0)
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans1_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans1_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = 0;
                                seans1_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans1_baszmn);
                                seans2_parca1_kadar_gecikme_interval = 0;
                            }
                            else
                            {
                                seans1_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = 0;
                                seans1_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans1_baszmn);
                                seans2_parca1_kadar_gecikme_interval = 0;
                            }
                        }
                        else
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans1_baszmn   = "GETDATE()";
                                seans1_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = 0;
                                seans1_parca1_kadar_gecikme_interval = 0;
                                seans2_parca1_kadar_gecikme_interval = 0;
                            }
                            else
                            {
                                seans1_baszmn = "GETDATE()";
                                seans1_bitzmn = CreateDate(Bitsaat_Seans1, Bitdakika_Seans1, 0, milliseconds);
                                seans1_interval = GetInterval(seans1_baszmn, seans1_bitzmn);
                                seans2_interval = 0;
                                seans1_parca1_kadar_gecikme_interval = 0;
                                seans2_parca1_kadar_gecikme_interval = 0;
                            }
                        }
                    }


                }
                else
                {
                        seans1_interval = 0;
                        seans1_parca1_kadar_gecikme_interval = 0;

                        if (gib_baszmn1 > 0)
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans2_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans2_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                            else
                            {
                                seans2_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                                seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                        }
                        else
                        {
                            if (gib_bitzmn1 > 0)
                            {
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                            else
                            {
                                seans2_baszmn = CreateDate(Bassaat_Seans2, Basdakika_Seans2, 0, milliseconds);
                                seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                                seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                                seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                            }
                        }
                }
                  

            }  //*2.Seans 
            else
            {
                if (gib_baszmn1 > 0)
                {
                    if (gib_bitzmn1 > 0)
                    {
                        seans1_interval = 0;
                        seans2_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                        seans2_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                        seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                        seans1_parca1_kadar_gecikme_interval = 0;
                        seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                    }
                    else
                    {
                        seans1_interval = 0;
                        seans2_baszmn   = CreateDate(gib_baszmn1, gib_baszmn2, gib_baszmn3, milliseconds);
                        seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                        seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                        seans1_parca1_kadar_gecikme_interval = 0;
                        seans2_parca1_kadar_gecikme_interval = GetInterval("GETDATE()", seans2_baszmn);
                    }
                }
                else
                {
                    if (gib_bitzmn1 > 0)
                    {
                        seans1_interval = 0;
                        seans2_baszmn   = "GETDATE()";
                        seans2_bitzmn   = CreateDate(gib_bitzmn1, gib_bitzmn2, gib_bitzmn3, milliseconds);
                        seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                        seans1_parca1_kadar_gecikme_interval = 0;
                        seans2_parca1_kadar_gecikme_interval = 0;
                    }
                    else
                    {
                        seans1_interval = 0;
                        seans2_baszmn   = "GETDATE()";
                        seans2_bitzmn = CreateDate(Bitsaat_Seans2, Bitdakika_Seans2, 0, milliseconds);
                        seans2_interval = GetInterval(seans2_baszmn, seans2_bitzmn);

                        seans1_parca1_kadar_gecikme_interval = 0;
                        seans2_parca1_kadar_gecikme_interval = 0;
                    }

                }

            }


      
        }

        //********************************************************************************************************************
    
        public void Calculate_GecikmeZamaniParcaSayisi(string Lot, string gib_parcasayisi, int seans1_interval, int seans2_interval, string gib_yuzde1, ref int sns1_parcasay, ref int sns2_parcasay, ref int emrlot, ref int kalanlot)
        {
            int toplamlot = Convert.ToInt32(Lot);
            int toplamparcasayisi = Convert.ToInt32(gib_parcasayisi);

             kalanlot = toplamlot % toplamparcasayisi;
             emrlot   = (toplamlot - kalanlot) / toplamparcasayisi;

            if (seans1_interval > 0)
            {
                decimal tut= Convert.ToDecimal(gib_yuzde1)/100;
                int sns1_toplamlot = Convert.ToInt32( (toplamlot-kalanlot) * tut);
                sns1_parcasay = sns1_toplamlot / emrlot;
                toplamparcasayisi = toplamparcasayisi - sns1_parcasay;
            }


            if (seans2_interval > 0)
            {
                 sns2_parcasay = toplamparcasayisi;
            }
        }

       //********************************************************************************************************************


        public string CreateDate(int hours,int minutes,int seconds,int milliseconds)
        {
            DateTime tar = DateTime.Now;

            int interval = 0;
            DateTime ilktar = new DateTime(
                tar.Year,
                tar.Month,
                tar.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                tar.Kind);

            string tarihzaman = Convert.ToDateTime(ilktar).ToString("yyyy-MM-dd HH:mm:ss");

            return tarihzaman;
        }

        //********************************************************************************************************************

        public int GetInterval(string tar1 , string tar2 )
        {
            string str   = "";
            int interval = 0;
           
            if(tar1=="GETDATE()")
                  str = "SELECT DATEDIFF(ms," + tar1 + ",'" + tar2 + "') AS Fark , GETDATE() AS IslemTarihi";
            else
                str = "SELECT DATEDIFF(ms,'" + tar1 + "','" + tar2 + "') AS Fark , GETDATE() AS IslemTarihi";

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                interval     = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Fark"]));
            }

            return interval ; 
        }

        //********************************************************************************************************************

        public DateTime GetServerDate()
        {
            DateTime serverdate=DateTime.Now;

            string str = "SELECT GETDATE() as ServerDate";
            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                serverdate = Convert.ToDateTime(Convert.ToString(Tablo1.Rows[0]["ServerDate"]));
            }

            return serverdate;
        }

        //********************************************************************************************************************



        public string HangiSeanstayiz()
        {
            string seans;
            decimal zaman = 0;
            string str = "SELECT CONVERT(VARCHAR(8),GETDATE(),108) AS Zaman";
            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                zaman = Convert.ToDecimal(Convert.ToString(Tablo1.Rows[0]["Zaman"]).Replace(":", ""));
            }

            if (zaman < 130000)  //* 1.Seans bitiş zamanı
                seans = "1";
            else
                seans = "2";

            return seans;
        }


        //********************************************************************************************************************

        public void BorsaZamanlari(ref decimal Baszaman_Seans1, ref decimal Bitzaman_Seans1, ref decimal Baszaman_Seans2, ref decimal Bitzaman_Seans2)
        {
            string str = "SELECT * FROM NETMESAJ.dbo.IcmBorsaSeansSaatleri ";

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                Baszaman_Seans1 = Convert.ToDecimal(Convert.ToString(Tablo1.Rows[0]["Baszaman_Seans1"]));
                Bitzaman_Seans1 = Convert.ToDecimal(Convert.ToString(Tablo1.Rows[0]["Bitzaman_Seans1"]));
                Baszaman_Seans2 = Convert.ToDecimal(Convert.ToString(Tablo1.Rows[0]["Baszaman_Seans2"]));
                Bitzaman_Seans2 = Convert.ToDecimal(Convert.ToString(Tablo1.Rows[0]["Bitzaman_Seans2"]));
            }

            return;
        }


        //********************************************************************************************************************

        public void BorsaSaatleri(ref int Bitsaat_Seans1, ref int Bitdakika_Seans1, ref int Bassaat_Seans2, ref int Basdakika_Seans2, ref int Bitsaat_Seans2, ref int Bitdakika_Seans2)
        {
          
            string str = "SELECT * FROM NETMESAJ.dbo.IcmBorsaSeansSaatleri ";

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
            {
                Bitsaat_Seans1   = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Bitsaat_Seans1"]));
                Bitdakika_Seans1 = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Bitdakika_Seans1"]));

                Bassaat_Seans2   = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Bassaat_Seans2"]));
                Basdakika_Seans2 = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Basdakika_Seans2"]));

                Bitsaat_Seans2   = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Bitsaat_Seans2"]));
                Bitdakika_Seans2 = Convert.ToInt32(Convert.ToString(Tablo1.Rows[0]["Bitdakika_Seans2"]));
            }

            return;
        }


        //********************************************************************************************************************


        public void DosyayaYaz(string EmirTipi, string IslemTarihi, string TakasTarihi, string RefId, string Menkul, string Fin_Inst_Id, string AlSat, string HesapNo, string CustId, string AccId, string Lot, string Maxlot, string MarjYuzde, string Zmn1, string Zmn2, string Zmn3, string userinf, string gib_yuzde1, string gib_yuzde2, string gib_baszmn1, string gib_baszmn2, string gib_baszmn3, string gib_bitzmn1, string gib_bitzmn2, string gib_bitzmn3, string gib_parcasayisi, string gib_enson_aktifolanparca, string gib_aktsek)
        {
            string str = "";
            string donus="";
            string SonFiyat = "0" , HesaplananEmirFiyati = "0" , Statu="1" ;
                str += " INSERT INTO NETMESAJ.dbo.IcmAlgoEmirler (ReferansId, HesapNo, CustId, Menkul, Fin_Inst_Id, EmirTipi, IslemTarihi, TakasTarihi, AlSat, AccId, Lot, Maxlot, MarjYuzde, SonFiyat, HesaplananEmirFiyati, TetikSaat, TetikDakika, TetikSaniye, Statu, Userinf, Gib_Yuzde1, Gib_Yuzde2, Gib_BasZmn1, Gib_BasZmn2, Gib_BasZmn3, Gib_BitZmn1, Gib_BitZmn2, Gib_BitZmn3, Gib_ParcaSayisi, Gib_Enson_AktifOlanParca, AktiflesmeSekli) ";
                str += " VALUES(" + RefId + ",'" + HesapNo + "','" + CustId + "','" + Menkul + "','" + Fin_Inst_Id + "','" + EmirTipi + "','";
                str += IslemTarihi + "','" + TakasTarihi + "','" + AlSat + "','" + AccId + "'," + Lot + "," + Maxlot + "," + MarjYuzde + "," + SonFiyat + "," + HesaplananEmirFiyati + ",";
                str += Zmn1 + "," + Zmn2 + "," + Zmn3 + ",'" + Statu + "','" + userinf + "'," + gib_yuzde1 + "," + gib_yuzde2 + "," + gib_baszmn1 + "," + gib_baszmn2 + "," + gib_baszmn3 + "," + gib_bitzmn1 + "," + gib_bitzmn2 + "," + gib_bitzmn3 + "," + gib_parcasayisi + "," + gib_enson_aktifolanparca + ",'" + gib_aktsek + "'";
                str += ") ";

                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************

        public void GibDetay_DosyasinaYaz(string EmirTipi, string IslemTarihi, string TakasTarihi, string RefId, string Menkul, string Fin_Inst_Id, string AlSat, string HesapNo, string CustId, string AccId, int Lot, string Maxlot, DateTime Date2, int Sira, string aktsek, string userinf)
        {
            string str = "";
            string donus = "";
            string Fiyat = "0", HesaplananAgirlikliOrtalamaFiyati="0" , Statu = "1"; //Bekliyor
            string TetikSaat   = Convert.ToString(Date2.Hour);
            string TetikDakika = Convert.ToString(Date2.Minute);
            string TetikSaniye = Convert.ToString(Date2.Second);
            string Sirano      = Convert.ToString(Sira);

                str += " INSERT INTO NETMESAJ.dbo.IcmAlgoEmirler_GibEmirDetay (ReferansId, Sira, HesapNo, CustId, Menkul, Fin_Inst_Id, EmirTipi, IslemTarihi, TakasTarihi, AlSat, AccId, Lot, Maxlot, Fiyat, HesaplananAgirlikliOrtalamaFiyati, TetikSaat, TetikDakika, TetikSaniye, Statu, Userinf, AktiflesmeSekli) ";
                str += " VALUES(" + RefId + "," + Sirano  +  ",'" + HesapNo + "','" + CustId + "','" + Menkul + "','" + Fin_Inst_Id + "','" + EmirTipi + "','";
                str += IslemTarihi + "','" + TakasTarihi + "','" + AlSat + "','" + AccId + "'," + Convert.ToString(Lot) + "," + Maxlot + "," + Fiyat + "," + HesaplananAgirlikliOrtalamaFiyati + ",";
                str += TetikSaat + "," + TetikDakika + "," + TetikSaniye + ",'" + Statu + "','" + userinf + "','" + aktsek + "'";
                str += ") ";

                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************

        public void DosyaGuncelle(string emrtip, string refid, string sonfiyat, string hesaplananfiyat, int sirano, string statu, string transaction_id, string ManuelAktivasyonAciklama)
        {
            string str = "";
            string donus = "";
                if (emrtip == "Z.A")
                {
                    str += " UPDATE NETMESAJ.dbo.IcmAlgoEmirler SET Statu='" + statu + "', SonFiyat=" + sonfiyat + ", HesaplananEmirFiyati=" + hesaplananfiyat ;
                    if (statu == "2")
                        str += ", TransactionId='" + transaction_id + "', ManuelAktivasyonAciklama='" + ManuelAktivasyonAciklama + "'";
                    str += " WHERE ReferansId=" + refid;
                }
                else if (emrtip == "G.I.B")
                {
                    str += " UPDATE NETMESAJ.dbo.IcmAlgoEmirler SET Statu='" + statu + "', Gib_Enson_AktifOlanParca=" + Convert.ToString(sirano) + ", ManuelAktivasyonAciklama='" + ManuelAktivasyonAciklama + "'";
                    str += " WHERE ReferansId=" + refid ;
                }

                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************

        public void ManuelAktivasyonAciklama_Guncelle1(string refid, string ManuelAktivasyonAciklama)
        {
            string str = "";
            string donus = "";
                    str = " UPDATE NETMESAJ.dbo.IcmAlgoEmirler SET ManuelAktivasyonAciklama='" + ManuelAktivasyonAciklama + "'";
                    str += " WHERE ReferansId=" + refid;

                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************
       


        public void Gib_DetayDosyaSil(string refid)
        {
            string str = "";
            string donus = "";
                str += "UPDATE NETMESAJ.dbo.IcmAlgoEmirler_GibEmirDetay SET Statu='3'  WHERE Statu='1' AND ReferansId=" + refid;
                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************
       

        public void Gib_DetayDosyaGuncelle(string refid, int sirano, string transaction_id, string statu, string HesaplananEmirFiyati, string gib_aksek, string HesaplananAgirlikliOrtalamaFiyati, string ManuelAktivasyonAciklama)
        {
            string str = "";
            string donus = "";
                str += " UPDATE NETMESAJ.dbo.IcmAlgoEmirler_GibEmirDetay SET Statu='" + statu + "', TransactionId='" + transaction_id + "', Fiyat=" + HesaplananEmirFiyati + ", AktiflesmeSekli='" + gib_aksek + "', HesaplananAgirlikliOrtalamaFiyati=" + HesaplananAgirlikliOrtalamaFiyati ;
                str += ", ManuelAktivasyonAciklama='" + ManuelAktivasyonAciklama + "'";
                str += " WHERE ReferansId=" + refid + " AND Sira=" + Convert.ToString(sirano);
         
                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************

        public void Gib_DetayDosya_AktiflesmeSeklini_Guncelle(string refid, int sirano,string gib_aksek, string ManuelAktivasyonAciklama)
        {
            string str = "";
            string donus = "";
                str += " UPDATE NETMESAJ.dbo.IcmAlgoEmirler_GibEmirDetay SET AktiflesmeSekli='" + gib_aksek + "'";
                str += ", ManuelAktivasyonAciklama='" + ManuelAktivasyonAciklama + "'";
                str += " WHERE ReferansId=" + refid + " AND Sira=" + Convert.ToString(sirano);

                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************

        public void GunSonu_MatriksVerileriSil()
        {
            string str = "";
            string donus = "";
                str += "DELETE FROM NETMESAJ.dbo.IcmBistHacim";
                donus = ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }

        //********************************************************************************************************************


        public void Matriksten_GerceklesenHisseyeAitVeriyiSakla(string islemno, string hisse, string fiyat, string miktar, string zaman)
        {
            string str = "";
            string donus = "";
                // EXEC NETMESAJ.dbo.sp_IcmGetMatriksHisseHacim @ZAMAN, @ISLEMNO , @MENKUL , @FIYAT, @MIKTAR
                str = "EXEC NETMESAJ.dbo.sp_IcmGetMatriksHisseHacim " + zaman + "," +  islemno + ",'" +  hisse + "'," +  fiyat + "," + miktar ;
                ExecCustomSQL(str).GetResponseOutput("Result");

            return;
        }


        //********************************************************************************************************************


        public DataTable FiyatKademeListesi_Al(string menkul)
        {
            DataTable Tablo1;

            string str = "SELECT V.*  , I.NAME , I.DESCRIPTION , C.MAX_LOT ";
            str += "FROM GTPFSI_FI_VALUES V ";
            str += "Left Join GTPFSI_FINANCIAL_INSTRUMENTS I on I.FIN_INST_ID=V.FIN_INST_ID ";
            str += "Left Join GTPFSI_EQ_CHTS C on I.FIN_INST_ID=C.FIN_INST_ID ";
            str += "WHERE VALUE_DATE = cast(floor(cast(GETDATE() as float)) as datetime)  ";   //*serverdan tarih kontrolü
            //* str += "YEAR(VALUE_DATE)='" + yil + "' and MONTH(VALUE_DATE)='" + ay + "' and DAY(VALUE_DATE)='" + gun + "'";
            str += "AND I.NAME = '" + menkul + "' ";
            str += "ORDER BY SESSION_NO desc";         // en son seans numarasını alsın. Bu select çift record döndürüyor 1. ve 2. seans için row lar.

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            return Tablo1;
        }


        //********************************************************************************************************************

        public decimal Vwap_IkiIslemOrt_Al(string menkul)
        {
            DataTable Tablo1;
            Decimal snsagrortfiyat=0;

            string str = "EXEC NETMESAJ.dbo.sp_IcmGetMatriksSeansOrtalamasi '" + menkul + "'";
            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)   //* sadece ilk recordu alalım. En son seansa aitttir.
            {
                snsagrortfiyat = Convert.ToDecimal(Tablo1.Rows[0]["SEANSORTALAMASI"]);
            }


            return snsagrortfiyat;
        }


        //********************************************************************************************************************



        public OrderList BekleyenEmirler_Al()
        {
            OrderList lst = new OrderList();
            Order ord;
            DataTable Tablo1;

            string str  = "SELECT * ";
            str += "FROM NETMESAJ.dbo.IcmAlgoEmirler ";
            str += "WHERE IslemTarihi = cast(floor(cast(GETDATE() as float)) as datetime)  ";
            str += " AND Statu='1'";
            str += "AND HesapNo='112542-103'";   //* ICM için bu satırı kapatalım.....

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ord = new Order();
                ord.Referansid  = Convert.ToDecimal(Tablo1.Rows[i]["ReferansId"]);
                ord.Hesapno     = Convert.ToString(Tablo1.Rows[i]["HesapNo"]);
                ord.Custid      = Convert.ToString(Tablo1.Rows[i]["CustId"]);
                ord.Menkul      = Convert.ToString(Tablo1.Rows[i]["Menkul"]);
                ord.Fin_inst_id = Convert.ToString(Tablo1.Rows[i]["Fin_Inst_Id"]);
                ord.Emirtipi    = Convert.ToString(Tablo1.Rows[i]["EmirTipi"]);
                ord.Islemtarihi = Convert.ToDateTime(Tablo1.Rows[i]["IslemTarihi"]);
                ord.Takastarihi = Convert.ToDateTime(Tablo1.Rows[i]["TakasTarihi"]);
                ord.Alsat       = Convert.ToString(Tablo1.Rows[i]["AlSat"]);
                ord.Accid       = Convert.ToString(Tablo1.Rows[i]["AccId"]);
                ord.Lot         = Convert.ToDecimal(Tablo1.Rows[i]["Lot"]);
                ord.Maxlot      = Convert.ToDecimal(Tablo1.Rows[i]["Maxlot"]);
                ord.Marjyuzde   = Convert.ToInt32(Tablo1.Rows[i]["MarjYuzde"]);
                ord.Sonfiyat    = Convert.ToDecimal(Tablo1.Rows[i]["SonFiyat"]);
                ord.Hesaplananemirfiyati = Convert.ToDecimal(Tablo1.Rows[i]["HesaplananEmirFiyati"]);
                ord.Tetiksaat   = Convert.ToInt32(Tablo1.Rows[i]["TetikSaat"]);
                ord.Tetikdakika = Convert.ToInt32(Tablo1.Rows[i]["TetikDakika"]);
                ord.Tetiksaniye = Convert.ToInt32(Tablo1.Rows[i]["TetikSaniye"]);
                ord.Statu       = Convert.ToString(Tablo1.Rows[i]["Statu"]);
                ord.Userinf     = Convert.ToString(Tablo1.Rows[i]["Userinf"]);
                ord.Gib_yuzde1  = Convert.ToInt32(Tablo1.Rows[i]["Gib_Yuzde1"]);
                ord.Gib_yuzde2  = Convert.ToInt32(Tablo1.Rows[i]["Gib_Yuzde2"]);
                ord.Gib_baszmn1 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BasZmn1"]);
                ord.Gib_baszmn2 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BasZmn2"]);
                ord.Gib_baszmn3 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BasZmn3"]);
                ord.Gib_bitzmn1 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BitZmn1"]);
                ord.Gib_bitzmn2 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BitZmn2"]);
                ord.Gib_bitzmn3 = Convert.ToInt32(Tablo1.Rows[i]["Gib_BitZmn3"]);
                ord.Gib_parcasayisi = Convert.ToInt32(Tablo1.Rows[i]["Gib_ParcaSayisi"]);
                ord.Gib_enson_aktifOlanParca =  Convert.ToInt32(Tablo1.Rows[i]["Gib_Enson_AktifOlanParca"]);
                ord.Aktiflesme_sekli = Convert.ToString(Tablo1.Rows[i]["AktiflesmeSekli"]);
               

                lst.Resultlist.Add(ord);
            }


            return lst;
        }


        //********************************************************************************************************************


        public OrderList GibDetay_BekleyenEmirler_Al(string RefId)
        {
            OrderList lst = new OrderList();
            Order ord;
            DataTable Tablo1;

            string str = "SELECT * ";
            str += "FROM NETMESAJ.dbo. IcmAlgoEmirler_GibEmirDetay ";
            str += "WHERE IslemTarihi = cast(floor(cast(GETDATE() as float)) as datetime)  ";
            str += " AND ReferansId=" + RefId; 
            str += " AND Statu='1'";
            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ord = new Order();
                ord.Referansid  = Convert.ToDecimal(Tablo1.Rows[i]["ReferansId"]);
                ord.Sirano      = Convert.ToInt32(Tablo1.Rows[i]["Sira"]);
                ord.Hesapno     = Convert.ToString(Tablo1.Rows[i]["HesapNo"]);
                ord.Custid      = Convert.ToString(Tablo1.Rows[i]["CustId"]);
                ord.Menkul      = Convert.ToString(Tablo1.Rows[i]["Menkul"]);
                ord.Fin_inst_id = Convert.ToString(Tablo1.Rows[i]["Fin_Inst_Id"]);
                ord.Emirtipi    = Convert.ToString(Tablo1.Rows[i]["EmirTipi"]);
                ord.Islemtarihi = Convert.ToDateTime(Tablo1.Rows[i]["IslemTarihi"]);
                ord.Takastarihi = Convert.ToDateTime(Tablo1.Rows[i]["TakasTarihi"]);
                ord.Alsat       = Convert.ToString(Tablo1.Rows[i]["AlSat"]);
                ord.Accid       = Convert.ToString(Tablo1.Rows[i]["AccId"]);
                ord.Lot         = Convert.ToDecimal(Tablo1.Rows[i]["Lot"]);
                ord.Maxlot      = Convert.ToDecimal(Tablo1.Rows[i]["Maxlot"]);
                ord.Sonfiyat    = Convert.ToDecimal(Tablo1.Rows[i]["Fiyat"]);
                ord.Hesaplananagirlikliortalamafiyati = Convert.ToDecimal(Tablo1.Rows[i]["HesaplananAgirlikliOrtalamaFiyati"]);
                ord.Tetiksaat   = Convert.ToInt32(Tablo1.Rows[i]["TetikSaat"]);
                ord.Tetikdakika = Convert.ToInt32(Tablo1.Rows[i]["TetikDakika"]);
                ord.Tetiksaniye = Convert.ToInt32(Tablo1.Rows[i]["TetikSaniye"]);
                ord.Aktiflesme_sekli = Convert.ToString(Tablo1.Rows[i]["AktiflesmeSekli"]); 
                ord.Statu       = Convert.ToString(Tablo1.Rows[i]["Statu"]);
                ord.Userinf     = Convert.ToString(Tablo1.Rows[i]["Userinf"]);

                lst.Resultlist.Add(ord);
            }


            return lst;
        }



        //********************************************************************************************************************

        public OrderList BugunkuTumEmirleri_Al()
        {
            OrderList lst = new OrderList();
            Order ord;
            DataTable Tablo1;

            string str = "SELECT * ";
            str += "FROM NETMESAJ.dbo.IcmAlgoEmirler ";
            str += "WHERE IslemTarihi = cast(floor(cast(GETDATE() as float)) as datetime)  ";
            str += "AND HesapNo='112542-103'";   //* ICM için bu satırı kapatalım.....

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ord = new Order();
                ord.Referansid = Convert.ToDecimal(Tablo1.Rows[i]["ReferansId"]);
                ord.Emirtipi   = Convert.ToString(Tablo1.Rows[i]["EmirTipi"]);
                lst.Resultlist.Add(ord);
            }

            return lst;
        }

        //********************************************************************************************************************


        public OrderList Zakt_Emir_Getir(string RefId, string gib_aktsek)
        {
            OrderList lst = new OrderList();
            Order ord;
            DataTable Tablo1;

            string str = "DECLARE @Tarih datetime = cast(floor(cast(GETDATE() as float)) as datetime)  ";
            str += "SELECT d.*, ";
            str += "Case  When (d.TransactionId is Null) Then '-'  ";
            str += "When (t.UNITS-isNull(t.REALIZED_UNITS,0)>0) And ( t.TRANSACTION_STATUS_ID in ( '0000-000001-ESD',  '0000-000002-ESD',  '0000-00000F-ESD',  '0000-000005-ESD',  '0000-000006-ESD')) ";
            str += "Then 'Borsaya İletildi'  ";
            str += "When (t.UNITS-isNull(t.REALIZED_UNITS,0)=0) then 'GERÇEKLEŞTİ'  Else 'İptal Edildi' End BorsaDurum, ";
            str += "t.INITIAL_MARKET_SESSION_SEL  InitialMarketSessionSel, ";
            str += "t.ENDING_MARKET_SESSION_SEL  EndingMarketSessionSel ";
            str += "FROM [NETMESAJ].[dbo].IcmAlgoEmirler d  ";
            str += "Left Join [gtpbrdb].[dbo].GTPBR_EQ_TRANS t on d.TransactionId collate Latin1_General_CI_AS=t.TRANSACTION_ID  AND (t.INITIAL_MARKET_SESSION_DATE= @Tarih ) ";
            //***str += "Left Join [NETMESAJ].[dbo].IcmAlgoEmirler i on d.ReferansId=i.ReferansId ";
            str += " WHERE (d.IslemTarihi= @Tarih ) AND (d.ReferansId=" + RefId + ") ";


            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ord = new Order();
                ord.Referansid = Convert.ToDecimal(Tablo1.Rows[i]["ReferansId"].ToString());
                ord.Hesapno    = Tablo1.Rows[i]["HesapNo"].ToString();
                ord.Statu      = Tablo1.Rows[i]["Statu"].ToString();
                ord.Borsadurum = Tablo1.Rows[i]["BorsaDurum"].ToString();
                ord.Menkul     = Tablo1.Rows[i]["Menkul"].ToString(); ;
                ord.Transactionid = Tablo1.Rows[i]["TransactionId"].ToString();
                ord.Alsat         = Tablo1.Rows[i]["AlSat"].ToString();
                ord.Fin_inst_id   = Tablo1.Rows[i]["Fin_Inst_Id"].ToString();
                ord.Lot           = Convert.ToDecimal(Tablo1.Rows[i]["Lot"].ToString());
                ord.Accid         = Tablo1.Rows[i]["AccId"].ToString();
                string HesaplananEmirFiyati = "0";
                string HesaplananAgirlikliOrtalamaFiyati = "0";
                HesaplananEmirFiyati = Gib_EmirFiyatiBul(ord.Menkul, ord.Alsat, gib_aktsek, ref HesaplananAgirlikliOrtalamaFiyati);

                ord.Hesaplananemirfiyati = Convert.ToDecimal(HesaplananEmirFiyati);
                ord.Custid               = Tablo1.Rows[i]["CustId"].ToString(); ;
                ord.Takastarihi          = Convert.ToDateTime(Tablo1.Rows[i]["TakasTarihi"].ToString());
                ord.Islemtarihi          = Convert.ToDateTime(Tablo1.Rows[i]["IslemTarihi"].ToString());
                ord.Initialmarketsessionsel = Tablo1.Rows[i]["InitialMarketSessionSel"].ToString();
                ord.Endingmarketsessionsel  = Tablo1.Rows[i]["EndingMarketSessionSel"].ToString();
                ord.Maxlot               = Convert.ToInt32(Tablo1.Rows[i]["Maxlot"].ToString());
                ord.Marjyuzde            = Convert.ToInt32(Tablo1.Rows[i]["MarjYuzde"].ToString());

                ord.Tip        = "Limit";
                ord.Gecerlilik = "Gün";
                ord.Lak        = "LOT";
                ord.Userinf    = Tablo1.Rows[i]["Userinf"].ToString();

                lst.Resultlist.Add(ord);
            }

            return lst;
        }



        //********************************************************************************************************************



        public OrderList GibDetay_Emirler_Getir(string RefId, string gib_aktsek)
        {
            OrderList lst = new OrderList();
            Order ord;
            DataTable Tablo1;

            string str="DECLARE @Tarih datetime = cast(floor(cast(GETDATE() as float)) as datetime)  ";
            str += "SELECT d.*, i.Gib_ParcaSayisi, ";
                  str+="Case  When (d.TransactionId is Null) Then '-'  ";
                  str+="When (t.UNITS-isNull(t.REALIZED_UNITS,0)>0) And ( t.TRANSACTION_STATUS_ID in ( '0000-000001-ESD',  '0000-000002-ESD',  '0000-00000F-ESD',  '0000-000005-ESD',  '0000-000006-ESD')) ";
                  str+="Then 'Borsaya İletildi'  ";
                  str+="When (t.UNITS-isNull(t.REALIZED_UNITS,0)=0) then 'GERÇEKLEŞTİ'  Else 'İptal Edildi' End BorsaDurum, ";
                  str+="t.INITIAL_MARKET_SESSION_SEL  InitialMarketSessionSel, ";
                  str+="t.ENDING_MARKET_SESSION_SEL  EndingMarketSessionSel ";
                  str+="FROM [NETMESAJ].[dbo].IcmAlgoEmirler_GibEmirDetay d  ";
                  str+="Left Join [gtpbrdb].[dbo].GTPBR_EQ_TRANS t on d.TransactionId collate Latin1_General_CI_AS=t.TRANSACTION_ID  AND (t.INITIAL_MARKET_SESSION_DATE= @Tarih ) ";
                  str+="Left Join [NETMESAJ].[dbo].IcmAlgoEmirler i on d.ReferansId=i.ReferansId ";
                  str += " WHERE (d.IslemTarihi= @Tarih ) AND (d.ReferansId=" + RefId + ")  ORDER BY Id";


            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            Tablo1 = x.Tables[0];

            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ord = new Order();
                ord.Referansid   = Convert.ToDecimal(Tablo1.Rows[i]["ReferansId"].ToString());
                ord.Sirano       = Convert.ToInt32(Tablo1.Rows[i]["Sira"].ToString());
                ord.Hesapno      = Tablo1.Rows[i]["HesapNo"].ToString();
                ord.Statu        = Tablo1.Rows[i]["Statu"].ToString();
                ord.Borsadurum   = Tablo1.Rows[i]["BorsaDurum"].ToString();
                ord.Menkul       = Tablo1.Rows[i]["Menkul"].ToString(); ;
                ord.Transactionid= Tablo1.Rows[i]["TransactionId"].ToString();
                ord.Alsat        = Tablo1.Rows[i]["AlSat"].ToString();
                ord.Fin_inst_id  = Tablo1.Rows[i]["Fin_Inst_Id"].ToString();
                ord.Lot          = Convert.ToDecimal(Tablo1.Rows[i]["Lot"].ToString());
                ord.Accid        = Tablo1.Rows[i]["AccId"].ToString();
                string HesaplananEmirFiyati              = "0";
                string HesaplananAgirlikliOrtalamaFiyati = "0";
                HesaplananEmirFiyati  =  Gib_EmirFiyatiBul(ord.Menkul, ord.Alsat, gib_aktsek, ref HesaplananAgirlikliOrtalamaFiyati);

                ord.Hesaplananemirfiyati              = Convert.ToDecimal(HesaplananEmirFiyati);
                ord.Hesaplananagirlikliortalamafiyati = Convert.ToDecimal(HesaplananAgirlikliOrtalamaFiyati);
                ord.Custid                            = Tablo1.Rows[i]["CustId"].ToString(); ;
                ord.Takastarihi                       = Convert.ToDateTime(Tablo1.Rows[i]["TakasTarihi"].ToString());
                ord.Islemtarihi                       = Convert.ToDateTime(Tablo1.Rows[i]["IslemTarihi"].ToString());
                ord.Initialmarketsessionsel           = Tablo1.Rows[i]["InitialMarketSessionSel"].ToString();
                ord.Endingmarketsessionsel            = Tablo1.Rows[i]["EndingMarketSessionSel"].ToString();
                ord.Maxlot                            = Convert.ToInt32(Tablo1.Rows[i]["Maxlot"].ToString());
                ord.Gib_parcasayisi                   = Convert.ToInt32(Tablo1.Rows[i]["Gib_ParcaSayisi"].ToString());

                ord.Tip        = "Limit";
                ord.Gecerlilik = "Gün";
                ord.Lak        = "LOT";
                ord.Userinf    = Tablo1.Rows[i]["Userinf"].ToString();

                lst.Resultlist.Add(ord);
            }

            return lst;
        }


       

        //********************************************************************************************************************

        public void Sistemden_Iyilestir(string TransactionId, string debitcredit, string fininstid, decimal units, decimal price, string customerid, string accountid, string valuedate, string initialmarketdate, int initialMarketSessionSel, int EndingMarketSessionSel, int orderMaxlot, string tip, string gecerlilik, string lak, string userinf)
        {
                string donus = Update_Equity_Order(TransactionId, debitcredit, fininstid, units, price, customerid, accountid, valuedate, initialmarketdate, initialMarketSessionSel, EndingMarketSessionSel, orderMaxlot, tip, gecerlilik, lak, userinf);
        }

        //********************************************************************************************************************

        public void Borsadan_Iyilestir(string TransactionId, decimal improveprice, decimal improveunits, decimal oldunits, string gecerlilik, string initialmarketdate, string userinf)
        {
                string donus = Save_Improve_Order(TransactionId, improveprice, improveunits, oldunits, gecerlilik, initialmarketdate, userinf);
        }


        //********************************************************************************************************************


        private decimal Kademe_Bul(decimal sonfiy)
        {
            decimal kademe = new decimal(0.01);

            if ((sonfiy > new decimal(0.01)) && (sonfiy <= new decimal(19.99)))
                kademe = new decimal(0.01);
            else if ((sonfiy >= new decimal(20)) && (sonfiy <= new decimal(49.98)))
                kademe = new decimal(0.02);
            else if ((sonfiy >= new decimal(50)) && (sonfiy <= new decimal(99.95)))
                kademe = new decimal(0.05);
            else if (sonfiy >= new decimal(100))
                kademe = new decimal(0.10);

            return kademe;
        }


        //********************************************************************************************************************


        private string SifirKoy(string sayi, int uzunluk)
        {
            string rc;
            int len = sayi.Trim().Length;
            rc = sayi;
            for (int i = len; i < uzunluk; i++)
                rc = "0" + rc;
            return rc.Trim();
        }

        //********************************************************************************************************************


        public void YeniFiyat_Hesapla(string menkul, string AlSat, string MarjYuzde, ref string SonFiyat, ref string HesaplananEmirFiyati)
        {
            //** Bunu iptal ettim , aşağıdaki YeniFiyat_Hesapla2 fonksiyonunu kullanıyorum.

            DataTable Tablo1;
            Decimal sonfiy, taban, tavan, kademe, degfiyat, yuzde, yzd1 = 0, yenfyt = 0;

            yuzde = Convert.ToDecimal(MarjYuzde);

            Tablo1 = FiyatKademeListesi_Al(menkul);
            if (Tablo1.Rows.Count > 0)   //* sadece ilk recordu alalım. En son seansa aitttir.
            {
                sonfiy = Convert.ToDecimal(Tablo1.Rows[0]["VALUE1"]);
                taban  = Convert.ToDecimal(Tablo1.Rows[0]["VALUE10"]);
                tavan  = Convert.ToDecimal(Tablo1.Rows[0]["VALUE11"]);
                kademe = Kademe_Bul(sonfiy);

                if (AlSat == "CREDIT")  //*Alış
                    yzd1 = sonfiy + (sonfiy * yuzde) / 100;
                else if (AlSat == "DEBIT")
                    yzd1 = sonfiy - (sonfiy * yuzde) / 100;


                decimal kalan = taban % kademe;   //* Açıklama : Taban fiyatı bazen kademe değerinin katları şeklinde olmuyor.
                degfiyat      = taban - kalan;    //*            bu yüzden en yakın katına indirge. 

                while (degfiyat <= tavan)
                {
                    if (degfiyat <= yzd1)
                        yenfyt = degfiyat;
                    else break;

                    degfiyat += kademe;
                }

                                         //* kapanış seansında sonfiyatın +-3% değerinin dışına çıkarsa marj dışına çıkmış oluyor.
                                         //* yani sonfiyat'ın +-3% değeri yeni taban-tavan gibi düşünülebilir.
                if (AlSat == "DEBIT")    //* bu yüzden , satışta bir kademe daha eklemek gerekiyor, yoksa tabanın altına gelebiliyor.
                    yenfyt = yenfyt + kademe;

                HesaplananEmirFiyati = Convert.ToString(yenfyt);
                SonFiyat = Convert.ToString(sonfiy);
            }
        }

        //********************************************************************************************************************    

        public void YeniFiyat_Hesapla2(string menkul, string AlSat, string MarjYuzde, ref string SonFiyat, ref string HesaplananEmirFiyati, ref string logdizi)
        {
            DataTable Tablo1;
            decimal sonfiy=0, taban=0, tavan=0, kademe=0, degfiyat=0, yuzde, yzd1 = 0;
            decimal tbn_kademesayisi = 0, tvn_kademesayisi = 0, tabanfiyattakikademe = 0, tavanfiyattakikademe=0, olusanfiyat = 0, yenitaban = 0, yenitavan = 0;
            string logstr = "";

            yuzde = Convert.ToDecimal(MarjYuzde);
            decimal tabankatsayisi = 1 - yuzde/100 ;
            decimal tavankatsayisi = 1 + yuzde/100;             //* +-%marjyuzde ekledik,çıkardık


            Tablo1 = FiyatKademeListesi_Al(menkul);
            if (Tablo1.Rows.Count > 0)                          //* sadece ilk recordu alalım. En son seansa aitttir.
            {
                sonfiy = Convert.ToDecimal(Tablo1.Rows[0]["VALUE1"]);

                taban  = sonfiy * tabankatsayisi;
                tavan  = sonfiy * tavankatsayisi;
                
                tabanfiyattakikademe = Kademe_Bul(taban);  //* taban ve tavan için ayrı ayrı kademe bulmak lazım. fiyat kademe geçişinde arada ise gerekli. 
                tavanfiyattakikademe = Kademe_Bul(tavan);  //* ÖRNEK: BIMAS ta taban fiyat 50 den küçükken kademe 0.02 dir. tavan fiyat 50 den büyükken kademe 0.05 dir. 


                tbn_kademesayisi = Math.Floor(taban / tabanfiyattakikademe);  //* aşağı integer değerine yuvarlar. 
                yenitaban = (tbn_kademesayisi + 1) * tabanfiyattakikademe;   //* bir kademe fazla almak lazım aralık içinde kalması için.

                tvn_kademesayisi = Math.Floor(tavan / tavanfiyattakikademe);
                yenitavan = tvn_kademesayisi * tavanfiyattakikademe;


                if (AlSat == "CREDIT")
                    olusanfiyat = yenitavan;    //*Alışta tavandan 
                else if (AlSat == "DEBIT")
                    olusanfiyat = yenitaban;    //*Satışta tabandan

                logstr  = "yuzde= " + Convert.ToString(yuzde) + " tabankatsayisi= " + Convert.ToString(tabankatsayisi) + " tavankatsayisi= " + Convert.ToString(tavankatsayisi);
                logstr += " taban= "+ Convert.ToString(taban) + " tavan= " + Convert.ToString(tavan) + " kademe= "  + Convert.ToString(kademe) ;
                logstr += " tbn_kademesayisi= " + Convert.ToString(tbn_kademesayisi) + " yenitaban= " + Convert.ToString(yenitaban);
                logstr += " tvn_kademesayisi= " + Convert.ToString(tvn_kademesayisi) + " yenitavan= " + Convert.ToString(yenitavan);
                logstr += " AlSat= " + AlSat + " SonFiyat= " + Convert.ToString(sonfiy) + " HesaplananEmirFiyati= " + Convert.ToString(olusanfiyat); 
                logdizi = logstr;

                HesaplananEmirFiyati = Convert.ToString(olusanfiyat);
                SonFiyat = Convert.ToString(sonfiy);
            }
        }



        //********************************************************************************************************************    

        public string Gib_EmirFiyatiBul(string menkul, string AlSat, string aktsek, ref string HesaplananAgirlikliOrtalamaFiyati )
        {
            DataTable Tablo1;
            Decimal sonfiy=0, taban=0, tavan=0, alis=0, satis=0, seansagrort=0, kademe=0, fiyat=0, degfiyat=0, yenfyt = 0;

            Tablo1 = FiyatKademeListesi_Al(menkul);
            if (Tablo1.Rows.Count > 0)   //* sadece ilk recordu alalım. En son seansa aitttir.
            {
                taban       = Convert.ToDecimal(Tablo1.Rows[0]["VALUE10"]);
                tavan       = Convert.ToDecimal(Tablo1.Rows[0]["VALUE11"]);
                sonfiy      = Convert.ToDecimal(Tablo1.Rows[0]["VALUE1"]);
                alis        = Convert.ToDecimal(Tablo1.Rows[0]["VALUE3"]);
                satis       = Convert.ToDecimal(Tablo1.Rows[0]["VALUE2"]);
                seansagrort = Convert.ToDecimal(Tablo1.Rows[0]["VALUE7"]);
            }

            if (aktsek == "VWAP_IKIISLEMORT")
            {
                fiyat = Vwap_IkiIslemOrt_Al(menkul);
                if (fiyat==0)  
                    fiyat = seansagrort;
                HesaplananAgirlikliOrtalamaFiyati = Convert.ToString(fiyat);
            }
            else
            {
                if (aktsek == "AKTIFE_VER")
                    if (AlSat == "CREDIT") fiyat = satis; else fiyat = alis;
                else if (aktsek == "PASIFE_VER")
                    if (AlSat == "CREDIT") fiyat = alis;  else fiyat = satis;
                else if (aktsek == "TAHTAYI_BOYA")
                    if (AlSat == "CREDIT") fiyat = tavan; else fiyat = taban;
                else if (aktsek == "SONISLEM_FIYATI")
                    fiyat = sonfiy;
                else if (aktsek == "VWAP_SEANSORT")
                {
                    fiyat = seansagrort;
                    HesaplananAgirlikliOrtalamaFiyati = Convert.ToString(fiyat);
                }
            }

            fiyat = Math.Round(fiyat, 2, MidpointRounding.AwayFromZero);  //*En yakın 2 decimalli değere yuvarlar.  
            kademe = Kademe_Bul(fiyat);

            decimal kalan = taban % kademe;   //* Açıklama : Taban fiyatı bazen kademe değerinin katları şeklinde olmuyor.
            degfiyat = taban - kalan;    //*            bu yüzden en yakın katına indirge. 

            while (degfiyat <= tavan)
            {
                if (degfiyat <= fiyat)
                    yenfyt = degfiyat;
                else break;

                degfiyat += kademe;
            }

            return Convert.ToString(yenfyt);
        }

        //********************************************************************************************************************    

        public string Gib_SonIkiEmir_BorsadaGerceklestimi(string refid, int sirano, string gib_aktsek)
        {
            DataTable Tablo1;
            string  str;
            int sayac=0;
       
                if (sirano > 2)
                {
                    str  = " select COUNT(t.TRANSACTION_ID) as BekleyenEmirSayisi ";
                    str += " from [gtpbrdb].[dbo].GTPBR_EQ_TRANS t ";
                    str += " Left Join [NETMESAJ].[dbo].IcmAlgoEmirler_GibEmirDetay d on d.TransactionId collate Latin1_General_CI_AS=t.TRANSACTION_ID ";
                    str += " where  (t.UNITS-isNull(t.REALIZED_UNITS,0)>0) "; // kalan emir lotu var demektir. Emrin tamamı gerçekleştiyse 0 olmalı.
                    str += " AND t.INITIAL_MARKET_SESSION_DATE>= cast(floor(cast(GETDATE() as float)) as datetime) ";
                    str += " AND d.ReferansId=" + refid;
                    //* str += " AND d.Sira>=" + Convert.ToString(sirano - 2) + " AND d.Sira<=" + Convert.ToString(sirano - 1);
                    str += " AND d.Sira<=" + Convert.ToString(sirano - 1);

                    var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
                    Tablo1 = x.Tables[0];

                    for (int i = 0; i < Tablo1.Rows.Count; i++)
                        sayac = Convert.ToInt32(Tablo1.Rows[i]["BekleyenEmirSayisi"]);

                   //* if (sayac == 2) //* Bu emirden 2 önceki emir gerçekleşmemiş. yada kısmi gerçekleşmiş. O zaman bu emiri Aktif Fiyattan gönder !!!
                    if (sayac >= 2)    //* Bu emirden önceki emirlerde en az 2 emir pasifte ise yeni emri Aktif Fiyattan gönder !!!
                       gib_aktsek = "AKTIFE_VER";
                }
      

            return gib_aktsek;
        }


        //********************************************************************************************************************    


        public bool EmirOrdinoBorsayaIletildimi(string transactionid)
        {
            string brokername = "";
            bool donus = false;
     

            string str = "select t.TRANSACTION_ID, ISNULL(BRK.NAME,'') BROKER_NAME  from GTPBR_EQ_TRANS t ";
            str += " LEFT JOIN GTPBS_POSITIONS BRK (NOLOCK) ON BRK.POSTN_ID = t.DEST_RECON_POS_ID ";
            str += " Left Join GTPFSI_FINANCIAL_INSTRUMENTS h on h.FIN_INST_ID=t.FIN_INST_ID  ";
            str += " WHERE t.TRANSACTION_ID='" + transactionid + "'";

            var x = ExecCustomSQL(str).GetResponseOutputDataSet("Result");
            DataTable Tablo1 = x.Tables[0];

            if (Tablo1.Rows.Count > 0)
                brokername = Convert.ToString(Tablo1.Rows[0]["BROKER_NAME"]);

            if (brokername.Trim().Length > 0)
                donus = true;
            else
                donus = false;


            return donus;
        }




        //********************************************************************************************************************    

        GtpXml ExecCustomSQL(string SQLText)
        {
            var userInfo = new UserInfoRec(); //*Boş açsın 
            // userInfo.
            string requestBrokerUri = "tcp://128.10.250.132:8080/RequestBroker";
            var invoker = new Invoker(requestBrokerUri, userInfo);
            var G = new GtpXml("GLB_CRM_EXEC_CUSTOM_SQL", "1.0");
            G.AddParameter("SQLText", SQLText);
            return invoker.RbmInvoke(G);
        }

        //********************************************************************************************************************


        GtpXml ExecCustomSQL2(UserInfoRec userInfo, GtpXml g)
        {
            string requestBrokerUri = "tcp://128.10.250.132:8080/RequestBroker";
            var invoker = new Invoker(requestBrokerUri, userInfo);

            g.UserInfo.ApplicationName     = userInfo.ApplicationName;
            g.UserInfo.ChannelId           = userInfo.ChannelId;
            g.UserInfo.CustomSectionName   = userInfo.CustomSectionName;
            g.UserInfo.DivisionId          = userInfo.DivisionId;
            g.UserInfo.EmployeeId          = userInfo.EmployeeId;
            g.UserInfo.HostName            = userInfo.HostName;
            g.UserInfo.OrganizationGroupId = userInfo.OrganizationGroupId;
            g.UserInfo.OrganizationId      = userInfo.OrganizationId;
            g.UserInfo.PartyId             = userInfo.PartyId;
            g.UserInfo.PositionId          = userInfo.PositionId;
            g.UserInfo.SessionId           = userInfo.SessionId;
            g = invoker.RbmInvoke(g);
            return g;
        }

    }
}
