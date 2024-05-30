using System;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Caching.Memory;

namespace backendnet.Services
{
    public class EmailService
    {
        private readonly string _emailFrom = "RompecabezasFEISoporte@gmail.com";
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "RompecabezasFEISoporte@gmail.com";
        private readonly string _smtpPass = "rvwf plho guzi uxte"; 
        private readonly string subject = "Código de verificación";
        private readonly IMemoryCache _cache;

        public EmailService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task SendEmailAsync(string emailTo)
        {
            
            string verificationCode = GenerateVerificationCode();
             _cache.Set(emailTo, verificationCode, TimeSpan.FromMinutes(10)); 

            
            string message = Environment.NewLine + "Bienvenido a BlockBuster!\n Su código de verificación es: " + verificationCode;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("La copia de Blockbuster para Progra Segura", _emailFrom));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public bool VerifyCode(string email, string code)
        {
            if (_cache.TryGetValue(email, out string? storedCode))
            {
                if (storedCode == code)
                {
                    _cache.Remove(email); 
                    return true;
                }
            }
            return false;
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            StringBuilder verificationCodeBuilder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                verificationCodeBuilder.Append(random.Next(0, 10));
            }
            return verificationCodeBuilder.ToString();
        }
    }
}
