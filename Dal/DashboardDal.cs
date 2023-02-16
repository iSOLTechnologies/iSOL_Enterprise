using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace iSOL_Enterprise.Dal
{
    public class DashboardDal
    {
        public HMIDataModels getData()
        {


            string GetQuery = "  select top 1 * From [dbo].[Line_One_ProductionData] order by id desc";

            //            string Query = @"select
            //(select cast(R_2002 as int)
            //-
            //( select cast(R_2002 as int)    from [dbo].[HMI_ProductionDetail]
            // where cast(Timestamp as time) = (select top 1 DATEADD(MINUTE, -1, cast(Timestamp as time))  from [dbo].[HMI_ProductionDetail] order by id desc ))
            //    from [dbo].[HMI_ProductionDetail]
            // where cast(Timestamp as time) = (select top 1 cast(Timestamp as time)  from [dbo].[HMI_ProductionDetail] order by id desc ))
            // as Average,(select top 1 R_2002 from[dbo].[HMI_ProductionDetail] order by id desc ) as Total ,
            //(select top 1 R_2004 from[dbo].[HMI_ProductionDetail] order by id desc ) as Good 
            // ,(select top 1 R_2006 from[dbo].[HMI_ProductionDetail] order by id desc ) as Waste
            //from[dbo].[HMI_ProductionDetail] where cast(Timestamp as time) = (select max(cast(Timestamp as time)) from[dbo].[HMI_ProductionDetail])
            //order by Timestamp";

            string Query = @"select
(select cast(R_2002 as int)
-
( select cast(R_2002 as int)    from [dbo].[HMI_ProductionDetail]
 where cast(Timestamp as date)=cast(GETDATE() as date) and cast(Timestamp as time) = (select top 1 DATEADD(MINUTE, -1, cast(Timestamp as time))  from [dbo].[HMI_ProductionDetail] 
 where cast(Timestamp as date)=cast(GETDATE() as date) order by id desc ))
    from [dbo].[HMI_ProductionDetail]
 where cast(Timestamp as date)=cast(GETDATE() as date) and cast(Timestamp as time) = (select top 1 cast(Timestamp as time)  from [dbo].[HMI_ProductionDetail] order by id desc ))
 as Average,(select top 1 R_2002 from[dbo].[HMI_ProductionDetail] order by id desc ) as Total ,
(select top 1 R_2004 from[dbo].[HMI_ProductionDetail] order by id desc ) as Good 
 ,(select top 1 R_2006 from[dbo].[HMI_ProductionDetail] order by id desc ) as Waste
from[dbo].[HMI_ProductionDetail] where
--cast(Timestamp as date)=cast(GETDATE() as date) and 
cast(Timestamp as datetime) = (select max(cast(Timestamp as datetime)) from[dbo].[HMI_ProductionDetail])
order by Timestamp";




            HMIDataModels Nowmodels = new HMIDataModels();
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.default2ndDB, CommandType.Text, GetQuery))
            {
                while (rdr.Read())
                {
                    Nowmodels.Average = rdr["MachineCycleRate"].ToDecimal();
                    Nowmodels.Total = rdr["Total"].ToDecimal();
                    Nowmodels.Good = rdr["Good"].ToDecimal();
                    Nowmodels.Waste = rdr["Waste"].ToDecimal();
                }
            }


            //  insertData(Nowmodels);
            /// recordProduction(Nowmodels);
            return Nowmodels;
        }

        public bool insertData(HMIDataModels input)
        {

            string query = @"insert into Line_One_ProductionData(MachineCycleRate,Total,Good,Waste,TimeStamp,WindowsService)
values(@MachineCycleRate,@Total,@Good,@Waste,@TimeStamp,@WindowsService)";
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@MachineCycleRate",input.Average),
                new SqlParameter("@Total",input.Total),
                new SqlParameter("@Good",input.Good),
                new SqlParameter("@Waste",input.Waste),
                new SqlParameter("@TimeStamp",DateTime.Now),
                new SqlParameter("@WindowsService",false)
            };

            SqlHelper.ExecuteNonQuery(SqlHelper.default2ndDB, CommandType.Text, query, param.ToArray()).ToInt();
            return true;
        }

   

        public DataTable GetDowntimeBarChart(DateTime ShiftDate, string ShiftCode)
        {
            string datetime = DateTime.Now.ToString("HH:mm:ss");
            if (datetime == "15:48:20")
            {

            }

            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@ShiftDate",ShiftDate),
                new SqlParameter("@ShiftCode",ShiftCode)
            };
            DataTable dt = SqlHelper.ExecuteStoreProcedure(SqlHelper.default2ndDB, "GetShiftWiseDowntime", param.ToArray());
            return dt;
        }

        public DataTable GetLineChart()
        {
            DataTable dt = SqlHelper.ExecuteStoreProcedure(SqlHelper.default2ndDB, "WeeklyProductionAnalysis");
            return dt;
        }
        //public DataTable GetDynamicChartData(FLoatChartModels input)
        //{
        //    List<SqlParameter> param = new List<SqlParameter>
        //    {

        //        new SqlParameter("@To",input.ToDate),
        //        new SqlParameter("@From",input.FromDate),
        //    };
        //    DataTable dt = SqlHelper.ExecuteStoreProcedure(SqlHelper.default2ndDB, "Speedometer", param.ToArray());
        //    return dt;
        //}


        //public DataTable ProductionDataOnParticularDatetime(FLoatChartModels input)
        //{
        //    List<SqlParameter> param = new List<SqlParameter>
        //    {

        //              new SqlParameter("@ToDatetime",input.ToDate),
        //        new SqlParameter("@FromDatetime",input.FromDate),
        //    };
        //    DataTable dt = SqlHelper.ExecuteStoreProcedure(SqlHelper.default2ndDB, "ProductionDataOnParticularDatetime", param.ToArray());
        //    return dt;
        //}


        public DataTable GetDowntme()
        {
            //string Query = @"select  top 1 * from HMI_ShiftDowntime order by id desc";
            string Query = @"select
 MAX(Shift_A_Hours_2104) Shift_A_Hours,MAX(Shift_A_Min_2102)Shift_A_Min,MAX(Shift_A_Sec_2100)Shift_A_Sec,
MAX(Shift_B_Hours_2112)Shift_B_Hours,MAX(Shift_B_Min_2110)Shift_B_Min,MAX(Shift_B_Sec_2108)Shift_B_Sec,
MAX(Shift_C_Hours_2118)Shift_C_Hours,MAX(Shift_C_Min_2116)Shift_C_Min,MAX(Shift_C_Sec_2114)Shift_C_Sec
,format(cast(timestamp as date), 'MMM/dd') date
from HMI_ShiftDowntime 
group by cast(Timestamp as date)";

            DataTable dt = SqlHelper.GetData(SqlHelper.default2ndDB, Query);
            return dt;
        }
        public DataTable GetCurrentDowntme()
        {
            DataTable dt = SqlHelper.ExecuteStoreProcedure(SqlHelper.default2ndDB, "GetCurrentDowntime");
            return dt;
        }
       
        public DashboardDataModel DashboardData(int? UserId)
        {
            string RatingQuery = @"select isnull(avg(Rating),0) from Tickets_Master t 
inner join Users u on u.Id=t.AssignTo
where AssignTo=@UserId";
          
            DashboardDataModel model = new DashboardDataModel();
            model.TotalTickets = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Tickets_Master where UserId=@UserId", new SqlParameter("@UserId", UserId)).ToInt();
            model.TotalInProgressTickets = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Tickets_Master where UserId=@UserId and StatusCode='ING'", new SqlParameter("@UserId", UserId)).ToInt();
            model.TotalPendingTickets = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Tickets_Master where UserId=@UserId and StatusCode='PND'", new SqlParameter("@UserId", UserId)).ToInt();
            model.TotalCompletedTickets = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Tickets_Master where UserId=@UserId and StatusCode='COM'", new SqlParameter("@UserId", UserId)).ToInt();
            model.TotalRejectedTickets = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, "select count(*) from Tickets_Master where UserId=@UserId and StatusCode='REJ'", new SqlParameter("@UserId", UserId)).ToInt();
            model.StarRating = SqlHelper.ExecuteScalar(SqlHelper.defaultDB, CommandType.Text, RatingQuery, new SqlParameter("@UserId", UserId)).ToDecimal();

            return model;
        }
     
            public List<CountModel> GetDocCounts()
            {
                 string countQuery;
                 List<CountModel> list = new List<CountModel>();
                 string[] docTables = { "QUT1", "RDR1" , "DLN1" , "INV1" , "PQT1" , "POR1" , "PDN1" , "PCH1" };
                        
                        foreach (var table in docTables)
                        {
                                if (table == "QUT1" || table == "PQT1")
                                 countQuery = @"select COUNT(*) as Count from ( select distinct Id  from "+table+")a";                
                                else
                                 countQuery = @"select COUNT(*) as Count from ( select distinct Id  from "+table+" where BaseEntry is null )a";

               
                            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, countQuery))
                            {
                                while (rdr.Read())
                                {

                                    list.Add(
                                        new CountModel()
                                        {
                                            Count = rdr["Count"].ToInt()
                                          
                                        });

                                }
                            }

                        }

                    return list;


            }

        public class CountModel
        {
            public int? Count { get;set; }
        }

        public class DashboardDataModel
        {
            public int? TotalTickets { get; set; }
            public int? TotalInProgressTickets { get; set; }
            public int? TotalPendingTickets { get; set; }
            public int? TotalCompletedTickets { get; set; }
            public int? TotalRejectedTickets { get; set; }
            public decimal? StarRating { get; set; }
        }


        public class DowntimeSummaryModel
        {
            public DateTime? Date { get; set; }
            public string Shift_A { get; set; }
            public string Shift_B { get; set; }
            public string Shift_C { get; set; }
            public string Total { get; set; }
        }
        public class SantexShiftTCimings
        {
            public int Id { get; set; }
            public string ShiftCode { get; set; }
            public string ShiftName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
        }
        public class DailyShiftWiseProduction
        {
            public decimal ShiftCode { get; set; }
            public decimal Total { get; set; }
            public decimal Good { get; set; }
            public decimal Waste { get; set; }
            public DateTime Date { get; set; }
            public DateTime Timestamp { get; set; }
        }
        public class HMIDataModels
        {
            public decimal Average { get; set; }
            public decimal Total { get; set; }
            public decimal Good { get; set; }
            public decimal Waste { get; set; }
        }

        public class Line_One_ProductionDataModel
        {
            public int Id { get; set; }
            public decimal MachineCycleRate { get; set; }
            public decimal Total { get; set; }
            public decimal Good { get; set; }
            public decimal Waste { get; set; }
            public DateTime? TimeStamp { get; set; }
        }
        public class chartData
        {
            public string y { get; set; }
            public int a { get; set; }
        }

        public class BackData_LineOneProductionData
        {
            public DateTime? TimeStamp { get; set; }
            public string SapProductionOrderNo { get; set; }
            public string ItemCode { get; set; }
            public decimal? TargetQuantity { get; set; }
            public decimal? Shift_A_Total { get; set; }
            public decimal? Shift_B_Total { get; set; }
            public decimal? Shift_C_Total { get; set; }

            public decimal? Shift_A_Good { get; set; }
            public decimal? Shift_B_Good { get; set; }
            public decimal? Shift_C_Good { get; set; }

            public decimal? Shift_A_Waste { get; set; }
            public decimal? Shift_B_Waste { get; set; }
            public decimal? Shift_C_Waste { get; set; }
            public bool WindowsService { get; set; }
        }
    }
}
