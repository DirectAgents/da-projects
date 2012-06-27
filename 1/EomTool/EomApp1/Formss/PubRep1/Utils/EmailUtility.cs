using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace EomApp1.Formss.PubRep1.Utils
{
    static class EmailUtility
    {
        internal static void SendNotificationEmail(
            string to, 
            String subject, 
            String body,
            int pubRepInstId)
        {
            MailMessage msg = new MailMessage();

            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            msg.From = new MailAddress(
                "ap@directagents.com");

            msg.To.Add(to);

            msg.CC.Add(
                "ap@directagents.com");

            SmtpClient SmtpMailer = new SmtpClient();
            SmtpMailer.Host = "smtp.gmail.com";
            SmtpMailer.Port = 587;
            SmtpMailer.Timeout = 50000;
            SmtpMailer.EnableSsl = true;

            SmtpMailer.Credentials = 
                new System.Net.NetworkCredential(
                    "reporting@directagents.com", 
                    "1423qrwe");

            SmtpMailer.Send(msg);
            
            // Create PubReportEmail record
            Data.PRDataDataContext db = new Data.PRDataDataContext();
            var pre = new Data.PubReportEmail();
            pre.body = body;
            pre.sent_date = DateTime.Now;
            pre.subject = subject;
            pre.pub_report_instance_id = pubRepInstId;
            pre.to_email_address = to;
            db.PubReportEmails.InsertOnSubmit(pre);

            // Update PubReportInstance.email_status_msg
            var pri = (from c in db.PubReportInstances
                       where c.id == pubRepInstId
                       select c).First();

            pri.email_status_msg = string.Format(
                "sent to {0} at {1}",
                pre.to_email_address,
                pre.sent_date);

            // Save all changes
            db.SubmitChanges();
        }
    }
}
