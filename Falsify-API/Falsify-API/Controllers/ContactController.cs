using Falsify_Site.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Security;
using Amazon.SecurityToken.Model;
using System.Net;


namespace Falsify_Site.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {        
       public Dictionary<string, string> contactList = new Dictionary<string, string>
                {
                    {"Admin", "Falsifymanagementnj23@gmail.com"},
                    {"Ian",  "ian.spencer.rosenberg@gmail.com" },
                    {"Ivan", "iviolante93@gmail.com" },
                    {"Marcus", "marxis0zero@gmail.com"},
                    {"Billy", "rigwill340@gmail.com"},
                    {"Bryan", "bocas538@gmail.com"}
                };

        public ContactController()
        {
        }


        [HttpPost]
        public string SendMessage(ContactModel contact)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("Notification", "falsifynjnotifications@gmail.com"));
                foreach (var email in contactList) {
                    message.To.Add(new MimeKit.MailboxAddress(email.Key, email.Value));
                }
                message.To.Add(new MimeKit.MailboxAddress(contact.Name, contact.Email));
                var body = new TextPart("plain"){ Text = contact.Message };
                message.Body = body;
                message.Subject = @"Incoming chat request from: {contact.name}";

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate(userName: "falsifynjnotifications", password: "nxcr yyap xfdf kpqd");

                    smtp.Send(message);
                    smtp.Disconnect(true);
                }

                Console.WriteLine($"Message sent to {contact.Email}. Message: {contact.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                return "Failure!";
            }
            
            return "Success!";
        }
    }
}
