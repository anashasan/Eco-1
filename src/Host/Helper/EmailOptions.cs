using MimeKit;
using System;
using MailKit.Net.Smtp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;

namespace Host.Helper
{
    public class EmailOptions
    {
        private SmtpOptions Smtpoptions { get; set; }
        public EmailOptions()
        {
            Smtpoptions = new SmtpOptions();
            //For testing purpose only .. later replace with MMC ones
            Smtpoptions.Server = "smtp.gmail.com";
            //Smtpoptions.Port = 25;
            Smtpoptions.Port = 465;
            Smtpoptions.User = "admin@ecoservices.com.pk";
            Smtpoptions.Password = "admin@ecoservices.com.pk";
            Smtpoptions.UseSsl = false;
            Smtpoptions.DefaultEmailFromAddress = "admin@ecoservices.com.pk";
            Smtpoptions.DefaultEmailFromAlias = "Eco Service";
        }

        public bool SendPasswordResetEmail(string to, string subject, string id, string hostedurl)
        {
            try
            {
                //From Address  
                var FromAddress = Smtpoptions.DefaultEmailFromAddress;
                var FromAdressTitle = Smtpoptions.DefaultEmailFromAlias;
                //To Address  
                var ToAddress = to;
                var ToAdressTitle = "Administrator";
                var Subject = "Invitation to Reset Password";
                //Smtp Server  
                var SmtpServer = "smtp.gmail.com";
                //Smtp Port Number  
                var SmtpPortNumber = 465;

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                mimeMessage.Subject = Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = $"<a href='http://{ hostedurl }/Employee/RegisterEmployee?code={ Getencodeddata(id) }' target='_blank'>Click Here</a> to RESET YOUR PASSWORD"
                };

                mimeMessage.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    // Note: only needed if the SMTP server requires authentication  
                    // Error 5.5.1 Authentication   
                    client.Authenticate(Smtpoptions.User, Smtpoptions.Password);
                    client.Send(mimeMessage);
                    //Console.WriteLine("The mail has been sent successfully !!");
                    //Console.ReadLine();
                    //client.Disconnect(true);
                   
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        private string Getencodeddata(string id)
        {
            return EncoderAgent.EncryptString(id.ToString());
        }
    }
}

