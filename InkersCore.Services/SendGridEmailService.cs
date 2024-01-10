using InkersCore.Domain.IServices;
using InkersCore.Models.ServiceModels;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace InkersCore.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ILoggerService<SendGridEmailService> _logger;
        private readonly string _apiKey;

        public SendGridEmailService(ILoggerService<SendGridEmailService> logger)
        {
            _logger = logger;
            _apiKey = @"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";
        }

        /// <summary>
        /// Function to send single email
        /// </summary>
        /// <param name="senderData">SenderData</param>
        /// <param name="recipientData">RecipientData</param>
        /// <param name="content">EmailContent</param>
        public void SendEmail(EmailSenderData senderData, EmailRecipientData recipientData, EmailContentData content)
        {
            try
            {
                var response = ExecuteAsync(senderData, recipientData, content);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        /// <summary>
        /// Function to send single email using SendGrid
        /// </summary>
        /// <param name="senderData"></param>
        /// <param name="recipientData"></param>
        /// <param name="contentData"></param>
        /// <returns></returns>
        async Task<Response> ExecuteAsync(EmailSenderData senderData, EmailRecipientData recipientData, EmailContentData contentData)
        {
            var apiKey = _apiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(senderData.SenderEmail, senderData.SenderName);
            var subject = contentData.Subject;
            var to = new EmailAddress(recipientData.RecipientEmail, recipientData.RecipientName);
            var plainTextContent = contentData.PlainText;
            var htmlContent = contentData.HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
