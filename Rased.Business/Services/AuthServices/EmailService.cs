﻿using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;
using Rased.Business.Dtos.Auths;

namespace Rased.Business.Services.AuthServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GeneralRespnose> SendEmailAsync(string email, string subject, string message)
        {
            var response = new GeneralRespnose();
            var emailMessage = new MimeMessage();
            //Email and Name for sender
            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:DisplayName"], _configuration["EmailSettings:Email"]));
            //Email and Name for receiver
            emailMessage.To.Add(new MailboxAddress("", email));
            //subject for Email
            emailMessage.Subject = subject;
            // plain  : This creates a new text part for the email body (Message : the actual message you want to send.)
            //emailMessage.Body = new TextPart("plain") { Text = message };
            emailMessage.Body = new TextPart("html")
            {
                Text = $@"
                <div dir='rtl' style='font-family: Tahoma, sans-serif; font-size: 15px; color: #333; padding: 10px; line-height: 1.6;'>
                    {message}
                </div>"
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    await client.ConnectAsync(
                        _configuration["EmailSettings:Host"],
                        int.Parse(_configuration["EmailSettings:Port"]),
                        SecureSocketOptions.StartTls
                    );

                    // Authenticate
                    await client.AuthenticateAsync(
                        _configuration["EmailSettings:Email"],
                        _configuration["EmailSettings:Password"]
                    );

                    // Send the email
                    await client.SendAsync(emailMessage);
                    response.successed = true;


                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex.Message);


                }
                finally
                {
                    // Disconnect
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
            }
            return response;

        }
    }
}

