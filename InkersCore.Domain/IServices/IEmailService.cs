using InkersCore.Models.ServiceModels;

namespace InkersCore.Domain.IServices
{
    public interface IEmailService
    {
        /// <summary>
        /// Function to send single email
        /// </summary>
        /// <param name="senderData">SenderData</param>
        /// <param name="recipientData">RecipientData</param>
        /// <param name="content">EmailContent</param>
        public void SendEmail(EmailSenderData senderData,EmailRecipientData recipientData, EmailContentData content);
    }
}
