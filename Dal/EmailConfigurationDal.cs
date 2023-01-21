using iSOL_Enterprise.Common;
using iSOL_Enterprise.Models;
using SqlHelperExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace iSOL_Enterprise.Dal
{
    public class EmailConfigurationDal
    {
        public EmailConfigurationModels GetData(int? UserId, string Type)
        {
            string Query = @"select top 1 * from emailConfiguration where Type=@Type and IsActive=1 and RowStatus=1";

            EmailConfigurationModels model = new EmailConfigurationModels();

            SqlParameter[] param = new SqlParameter[]
         {
                new SqlParameter("@UserId",UserId),
                new SqlParameter("@Type",Type),
         };

            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.defaultDB, CommandType.Text, Query, param))
            {
                if (rdr.Read())
                {
                    model.Id = rdr["Id"].ToInt();
                    model.EmailFrom = rdr["EmailFrom"].ToString();
                    //                 model.Name = rdr["Name"].ToString();
                    model.Password = rdr["Password"].ToString();
                    model.CC = rdr["CC"].ToString();
                    model.BCC = rdr["BCC"].ToString();
                    model.Subject = rdr["Subject"].ToString();
                    model.Body = rdr["Body"].ToString();
                    model.SMTP = rdr["SMTP"].ToString();
                    model.Port = rdr["Port"].ToInt();
                    model.SSL = rdr["SSL"].ToBool();
                    model.BaseURL = rdr["BaseURL"].ToString();


                }
            }

            return model;
        }
        //public static bool SendEmail(int? UserId, string email, string TicketCode, string Type, Tickets_MasterModels masterModels)
        //{
        //    string Body = "";
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(TicketCode))
        //        {
        //            EmailConfigurationDal configurationDal = new EmailConfigurationDal();
        //            EmailConfigurationModels data = configurationDal.GetData(UserId, Type);
        //            _usersModels models = new UsersDal().GetData(UserId);
        //            _usersModels AssignToModel = new UsersDal().GetData(masterModels.AssignTo.ToInt());
        //            _usersModels AssignbyModel = new UsersDal().GetData(masterModels.ModifiedBy.ToInt());
        //            using (SmtpClient client = new SmtpClient(data.SMTP, data.Port))
        //            {
        //                MailMessage mailMessage = new MailMessage();
        //                mailMessage.From = new MailAddress(data.EmailFrom);
        //                mailMessage.BodyEncoding = Encoding.UTF8;
        //                mailMessage.To.Add(email);
        //                Body = data.Body;
        //                Body = Body.Replace("//TicketCode//", TicketCode);
        //                Body = Body.Replace("<<'URL'>>", data.BaseURL);
        //                Body = Body.Replace("<<'User'>>", models.FirstName + ' ' + models.LastName);
        //                if (Type == "PND")
        //                {

        //                    Body = Body.Replace("<<'TicketNumber'>>", TicketCode);
        //                    Body = Body.Replace("<<'TaskSubject'>>", masterModels.Project);
        //                }

        //                if (Type == "REJ")
        //                {
        //                    Body = Body.Replace("<<'TicketNumber'>>", TicketCode);
        //                    Body = Body.Replace("<<'TaskSubject'>>", masterModels.Project);
        //                    Body = Body.Replace("<<'Remarks'>>", masterModels.Remarks);

        //                }

        //                if (Type == "COM")
        //                {
        //                    Body = Body.Replace("<<'TicketNumber'>>", TicketCode);
        //                    Body = Body.Replace("<<'TaskSubject'>>", masterModels.Project);
        //                    Body = Body.Replace("<<'AssignedTo'>>", AssignToModel.FirstName + ' ' + AssignToModel.LastName);
        //                    Body = Body.Replace("<<'ActualNumberOfDays'>>", masterModels.DaysTaken.ToString());
        //                    Body = Body.Replace("<<'ActualCompletionDate'>>", Convert.ToDateTime(masterModels.ActualCompletionDate).ToString("dd-MMM-yyy"));
        //                    Body = Body.Replace("<<'ExpectedCompletionDate'>>", Convert.ToDateTime(masterModels.ExpectedCompletionDate).ToString("dd-MMM-yyy"));
        //                    Body = Body.Replace("<<'Remarks'>>", masterModels.Remarks);
        //                }

        //                if (Type == "ING")
        //                {
        //                    Body = Body.Replace("<<'TicketNumber'>>", TicketCode);
        //                    Body = Body.Replace("<<'TaskSubject'>>", masterModels.Project);
        //                    Body = Body.Replace("<<'AssignedTo'>>", AssignToModel.FirstName + ' ' + AssignToModel.LastName);
        //                    Body = Body.Replace("<<'StartDate'>>", Convert.ToDateTime(masterModels.StartDate).ToString("dd-MMM-yyy"));
        //                    Body = Body.Replace("<<'ExpectedCompletionDate'>>", Convert.ToDateTime(masterModels.ExpectedCompletionDate).ToString("dd-MMM-yyy"));
        //                    Body = Body.Replace("<<'AgreedDays'>>", masterModels.DaysAgreed.ToString());
        //                    Body = Body.Replace("<<'Remarks'>>", masterModels.Remarks);
        //                }

        //                mailMessage.Body = Body;
        //                mailMessage.Subject = data.Subject;
        //                string[] ArrayCC = data.CC.Split(',');
        //                foreach (var item in ArrayCC)
        //                {
        //                    mailMessage.CC.Add(item);
        //                }

        //                string[] ArrayBCC = data.BCC.Split(',');
        //                foreach (var item in ArrayCC)
        //                {
        //                    mailMessage.Bcc.Add(item);
        //                }
        //                mailMessage.IsBodyHtml = true;
        //                client.UseDefaultCredentials = false;
        //                client.Credentials = new System.Net.NetworkCredential(data.EmailFrom, data.Password);
        //                client.EnableSsl = true;

        //                client.Send(mailMessage);
        //            }

        //            if (!string.IsNullOrEmpty(masterModels.AssignTo) && masterModels.StatusCode== "PND" || masterModels.StatusCode == "ING")
        //            {
        //                EmailConfigurationModels datax = configurationDal.GetData(UserId, "ASN");
        //                using (SmtpClient client = new SmtpClient(datax.SMTP, datax.Port))
        //                {
        //                    MailMessage mailMessage = new MailMessage();
        //                    mailMessage.From = new MailAddress(datax.EmailFrom);
        //                    mailMessage.BodyEncoding = Encoding.UTF8;
        //                    mailMessage.To.Add(email);
        //                    Body = datax.Body;
        //                    Body = Body.Replace("//TicketCode//", TicketCode);
        //                    Body = Body.Replace("<<'URL'>>", datax.BaseURL);
        //                    Body = Body.Replace("<<'User'>>", models.FirstName + ' ' + models.LastName);
        //                    Body = Body.Replace("<<'TicketNumber'>>", TicketCode);
        //                    Body = Body.Replace("<<'TaskSubject'>>", masterModels.Project);
        //                    Body = Body.Replace("<<'AssignedFrom'>>", AssignbyModel.FirstName + ' ' + AssignbyModel.LastName);
        //                    Body = Body.Replace("<<'StartDate'>>", Convert.ToDateTime(masterModels.StartDate).ToString("dd-MMM-yyy"));
        //                    Body = Body.Replace("<<'Remarks'>>", masterModels.Remarks);

        //                    mailMessage.Body = Body;
        //                    mailMessage.Subject = datax.Subject;
        //                    string[] ArrayCC = datax.CC.Split(',');
        //                    foreach (var item in ArrayCC)
        //                    {
        //                        mailMessage.CC.Add(item);
        //                    }

        //                    string[] ArrayBCC = datax.BCC.Split(',');
        //                    foreach (var item in ArrayCC)
        //                    {
        //                        mailMessage.Bcc.Add(item);
        //                    }
        //                    mailMessage.IsBodyHtml = true;
        //                    client.UseDefaultCredentials = false;
        //                    client.Credentials = new System.Net.NetworkCredential(datax.EmailFrom, datax.Password);
        //                    client.EnableSsl = true;

        //                    client.Send(mailMessage);
        //                }

        //            }
        //        }




        //        return true;

        //    }
        //    catch (Exception ex)
        //    {

        //        return false;

        //    }
        //}

    }
}
