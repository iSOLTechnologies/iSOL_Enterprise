using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ReportManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ReportManager.Dal
{
    public class ReportDal
    {
        private string DBId = ""; 
        private string DBPassword = ""; 
        public ReportDal() 
        {
            DBId = ConfigurationManager.AppSettings["UserId"];
            DBPassword = ConfigurationManager.AppSettings["Password"];
        }
        public ResponseModels GenerateAccesoriesConsumptionReport(Document_MasterModel input)
        {


            //         input.CardCode = Convert_HexvalueToStringvalue(input.CardCode, System.Text.Encoding.Unicode);
            //          input.DocEntry = Convert.ToInt32(Convert_HexvalueToStringvalue(input.DocEntry.ToString(), System.Text.Encoding.Unicode));
            ResponseModels models = new ResponseModels();
            try
            {

                var FilePath = "\\Assets\\Reports\\Accessories Consumption Report.rpt";
                input.Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);

                string savepath = System.Web.HttpContext.Current.Server.MapPath("\\Assets\\Reports\\ReportLogs");
                if (!Directory.Exists(savepath)) { Directory.CreateDirectory(savepath); }
                //var foldername = "Customer Ledgers-" + DateTime.Now.ToString("yyyy-MM-dd");



                // Weaving Sales Pending Contract

                ReportDocument cryRpt = new ReportDocument();

                cryRpt.Close();
                cryRpt.Load(input.Path);

                cryRpt.SetDatabaseLogon(DBId, DBPassword);

                ParameterFieldDefinitions Param = cryRpt.DataDefinition.ParameterFields;

                ParameterFieldDefinition pfd1;
                ParameterValues pv1 = new ParameterValues();
                ParameterDiscreteValue pdv1 = new ParameterDiscreteValue();

                pdv1.Value = input.DocNum;

                pfd1 = Param["Sales Order"];
                pv1 = pfd1.CurrentValues;
                pv1.Clear();
                pv1.Add(pdv1);
                pfd1.ApplyCurrentValues(pv1);

                ParameterFieldDefinition pfd2;
                ParameterValues pv2 = new ParameterValues();
                ParameterDiscreteValue pdv2 = new ParameterDiscreteValue();



                string FileName = "";
                string CombinePath = "";
                if (input.DocType == "PDF")
                {
                    FileName = "\\ACR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".pdf";
                    CombinePath = savepath + FileName;
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, CombinePath);
                }
                else if (input.DocType == "Xlsx")
                {
                    // Declare variables and get the export options.
                    ExportOptions exportOpts = new ExportOptions();
                    //   ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                    DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    exportOpts = cryRpt.ExportOptions;

                    // Set the excel format options.
                    //excelFormatOpts.ExcelAreaType = AreaSectionKind.GroupHeader;
                    //excelFormatOpts.ExcelUseConstantColumnWidth = false;
                    //excelFormatOpts.ExcelTabHasColumnHeadings = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;

                    //  exportOpts.ExportFormatType = ExportFormatType.ExcelWorkbook;
                    //           exportOpts.FormatOptions = excelFormatOpts;
                    // Set the disk file options and export.
                    exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    FileName = "\\ACR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xls";
                    CombinePath = savepath + FileName;
                    diskOpts.DiskFileName = CombinePath;
                    exportOpts.DestinationOptions = diskOpts;

                    cryRpt.Export();



                    // cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, CombinePath);
                }


                models.Data = "\\Assets\\Reports\\ReportLogs" + FileName;
                models.isSuccess = true;
                models.isError = false;

            }
            catch (Exception ex)
            {
                models.isSuccess = false;
                models.isError = true;
                return models;
            }

            return models;
        }
        public ResponseModels GeneratePurchaseTrackingReportV1(Document_MasterModel input)
        {


            //         input.CardCode = Convert_HexvalueToStringvalue(input.CardCode, System.Text.Encoding.Unicode);
            //          input.DocEntry = Convert.ToInt32(Convert_HexvalueToStringvalue(input.DocEntry.ToString(), System.Text.Encoding.Unicode));
            ResponseModels models = new ResponseModels();
            try
            {

                var FilePath = "\\Assets\\Reports\\Purchase Tracking Report V1.rpt";
                input.Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);

                string savepath = System.Web.HttpContext.Current.Server.MapPath("\\Assets\\Reports\\ReportLogs");
                if (!Directory.Exists(savepath)) { Directory.CreateDirectory(savepath); }
                //var foldername = "Customer Ledgers-" + DateTime.Now.ToString("yyyy-MM-dd");



                // Weaving Sales Pending Contract

                ReportDocument cryRpt = new ReportDocument();

                cryRpt.Close();
                cryRpt.Load(input.Path);

                cryRpt.SetDatabaseLogon(DBId, DBPassword);

                ParameterFieldDefinitions Param = cryRpt.DataDefinition.ParameterFields;

                ParameterFieldDefinition pfd1;
                ParameterValues pv1 = new ParameterValues();
                ParameterDiscreteValue pdv1 = new ParameterDiscreteValue();

                pdv1.Value = input.DocNum;

                pfd1 = Param["So"];
                pv1 = pfd1.CurrentValues;
                pv1.Clear();
                pv1.Add(pdv1);
                pfd1.ApplyCurrentValues(pv1);

                ParameterFieldDefinition pfd2;
                ParameterValues pv2 = new ParameterValues();
                ParameterDiscreteValue pdv2 = new ParameterDiscreteValue();



                string FileName = "";
                string CombinePath = "";
                if (input.DocType == "PDF")
                {
                    FileName = "\\PTRV1_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".pdf";
                    CombinePath = savepath + FileName;
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, CombinePath);
                }
                else if (input.DocType == "Xlsx")
                {
                    // Declare variables and get the export options.
                    ExportOptions exportOpts = new ExportOptions();
                    //   ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                    DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    exportOpts = cryRpt.ExportOptions;

                    // Set the excel format options.
                    //excelFormatOpts.ExcelAreaType = AreaSectionKind.GroupHeader;
                    //excelFormatOpts.ExcelUseConstantColumnWidth = false;
                    //excelFormatOpts.ExcelTabHasColumnHeadings = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;

                    //  exportOpts.ExportFormatType = ExportFormatType.ExcelWorkbook;
                    //           exportOpts.FormatOptions = excelFormatOpts;
                    // Set the disk file options and export.
                    exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    FileName = "\\PTRV1_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xls";
                    CombinePath = savepath + FileName;
                    diskOpts.DiskFileName = CombinePath;
                    exportOpts.DestinationOptions = diskOpts;

                    cryRpt.Export();



                    // cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, CombinePath);
                }


                models.Data = "\\Assets\\Reports\\ReportLogs" + FileName;
                models.isSuccess = true;
                models.isError = false;

            }
            catch (Exception ex)
            {
                models.isSuccess = false;
                models.isError = true;
                return models;
            }

            return models;
        }
        public ResponseModels GenerateSaleOrderWiseYarnReport(Document_MasterModel input)
        {


            //         input.CardCode = Convert_HexvalueToStringvalue(input.CardCode, System.Text.Encoding.Unicode);
            //          input.DocEntry = Convert.ToInt32(Convert_HexvalueToStringvalue(input.DocEntry.ToString(), System.Text.Encoding.Unicode));
            ResponseModels models = new ResponseModels();
            try
            {

                var FilePath = "\\Assets\\Reports\\Sale Order Wise Yarn Report.rpt";
                input.Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);

                string savepath = System.Web.HttpContext.Current.Server.MapPath("\\Assets\\Reports\\ReportLogs");
                if (!Directory.Exists(savepath)) { Directory.CreateDirectory(savepath); }
                //var foldername = "Customer Ledgers-" + DateTime.Now.ToString("yyyy-MM-dd");



                // Weaving Sales Pending Contract

                ReportDocument cryRpt = new ReportDocument();

                cryRpt.Close();
                cryRpt.Load(input.Path);

                cryRpt.SetDatabaseLogon(DBId, DBPassword);

                ParameterFieldDefinitions Param = cryRpt.DataDefinition.ParameterFields;

                ParameterFieldDefinition pfd1;
                ParameterValues pv1 = new ParameterValues();
                ParameterDiscreteValue pdv1 = new ParameterDiscreteValue();

                pdv1.Value = input.DateFrom;

                pfd1 = Param["Date From"];
                pv1 = pfd1.CurrentValues;
                pv1.Clear();
                pv1.Add(pdv1);
                pfd1.ApplyCurrentValues(pv1);

                ParameterFieldDefinition pfd2;
                ParameterValues pv2 = new ParameterValues();
                ParameterDiscreteValue pdv2 = new ParameterDiscreteValue();
                pdv2.Value = input.DateTo;
                pfd2 = Param["Date To"];
                pv2 = pfd1.CurrentValues;
                pv2.Clear();
                pv2.Add(pdv1);
                pfd2.ApplyCurrentValues(pv2);


                string FileName = "";
                string CombinePath = "";
                if (input.DocType == "PDF")
                {
                    FileName = "\\SOWYR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".pdf";
                    CombinePath = savepath + FileName;
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, CombinePath);
                }
                else if (input.DocType == "Xlsx")
                {
                    // Declare variables and get the export options.
                    ExportOptions exportOpts = new ExportOptions();
                    //   ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                    DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    exportOpts = cryRpt.ExportOptions;

                    // Set the excel format options.
                    //excelFormatOpts.ExcelAreaType = AreaSectionKind.GroupHeader;
                    //excelFormatOpts.ExcelUseConstantColumnWidth = false;
                    //excelFormatOpts.ExcelTabHasColumnHeadings = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;

                    //  exportOpts.ExportFormatType = ExportFormatType.ExcelWorkbook;
                    //           exportOpts.FormatOptions = excelFormatOpts;
                    // Set the disk file options and export.
                    exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    FileName = "\\SOWYR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xls";
                    CombinePath = savepath + FileName;
                    diskOpts.DiskFileName = CombinePath;
                    exportOpts.DestinationOptions = diskOpts;

                    cryRpt.Export();



                    // cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, CombinePath);
                }


                models.Data = "\\Assets\\Reports\\ReportLogs" + FileName;
                models.isSuccess = true;
                models.isError = false;

            }
            catch (Exception ex)
            {
                models.isSuccess = false;
                models.isError = true;
                return models;
            }

            return models;
        }
        public ResponseModels PurchaseOrderReport(string DocNum,string ReportName)
        {
            Document_MasterModel input = new Document_MasterModel();
            input.DocType = "PDF";
            //         input.CardCode = Convert_HexvalueToStringvalue(input.CardCode, System.Text.Encoding.Unicode);
            //          input.DocEntry = Convert.ToInt32(Convert_HexvalueToStringvalue(input.DocEntry.ToString(), System.Text.Encoding.Unicode));
            ResponseModels models = new ResponseModels();
            try
            {

                var FilePath = "\\Assets\\Reports\\"+ ReportName;
                input.Path = System.Web.HttpContext.Current.Server.MapPath(FilePath);

                string savepath = System.Web.HttpContext.Current.Server.MapPath("\\Assets\\Reports\\ReportLogs");
                if (!Directory.Exists(savepath)) { Directory.CreateDirectory(savepath); }
                //var foldername = "Customer Ledgers-" + DateTime.Now.ToString("yyyy-MM-dd");



                // Weaving Sales Pending Contract

                ReportDocument cryRpt = new ReportDocument();

                cryRpt.Close();
                cryRpt.Load(input.Path);

                cryRpt.SetDatabaseLogon(DBId, DBPassword);

                ParameterFieldDefinitions Param = cryRpt.DataDefinition.ParameterFields;

                ParameterFieldDefinition pfd1;
                ParameterValues pv1 = new ParameterValues();
                ParameterDiscreteValue pdv1 = new ParameterDiscreteValue();

                pdv1.Value = DocNum;

                pfd1 = Param["DocNum"];
                pv1 = pfd1.CurrentValues;
                pv1.Clear();
                pv1.Add(pdv1);
                pfd1.ApplyCurrentValues(pv1);

                ParameterFieldDefinition pfd2;
                ParameterValues pv2 = new ParameterValues();
                ParameterDiscreteValue pdv2 = new ParameterDiscreteValue();



                string FileName = "";
                string CombinePath = "";
                if (input.DocType == "PDF")
                {
                    FileName = "\\ACR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".pdf";
                    CombinePath = savepath + FileName;
                    cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, CombinePath);
                }
                else if (input.DocType == "Xlsx")
                {
                    // Declare variables and get the export options.
                    ExportOptions exportOpts = new ExportOptions();
                    //   ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                    DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                    exportOpts = cryRpt.ExportOptions;

                    // Set the excel format options.
                    //excelFormatOpts.ExcelAreaType = AreaSectionKind.GroupHeader;
                    //excelFormatOpts.ExcelUseConstantColumnWidth = false;
                    //excelFormatOpts.ExcelTabHasColumnHeadings = true;
                    exportOpts.ExportFormatType = ExportFormatType.Excel;

                    //  exportOpts.ExportFormatType = ExportFormatType.ExcelWorkbook;
                    //           exportOpts.FormatOptions = excelFormatOpts;
                    // Set the disk file options and export.
                    exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                    FileName = "\\ACR_" + input.DocNum + "-" + DateTime.Now.ToString("dd-MM-yyyy hhmmss") + ".xls";
                    CombinePath = savepath + FileName;
                    diskOpts.DiskFileName = CombinePath;
                    exportOpts.DestinationOptions = diskOpts;

                    cryRpt.Export();



                    // cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, CombinePath);
                }


                models.Data = "\\Assets\\Reports\\ReportLogs" + FileName;
                models.isSuccess = true;
                models.isError = false;

            }
            catch (Exception ex)
            {
                models.isSuccess = false;
                models.isError = true;
                return models;
            }

            return models;
        }

    }
}