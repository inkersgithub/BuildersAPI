namespace InkersCore.Domain.IServices
{
    public interface ILoggerService<T> where T : class
    {
        /// <summary>
        /// Function to log error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogError(string message, bool sendEmail = false);

        /// <summary>
        /// Function to log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogException(Exception exception, bool sendEmail = false);

        /// <summary>
        /// Function to log warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogWarning(string message, bool sendEmail = false);

        /// <summary>
        /// Function to log information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogInformation(string message, bool sendEmail = false);
    }
}
