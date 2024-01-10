using InkersCore.Domain.IServices;
using Microsoft.Extensions.Logging;

namespace InkersCore.Services
{
    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Function to log error message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogError(string message, bool sendEmail = false)
        {
            _logger.LogError(message);
        }

        /// <summary>
        /// Function to log exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogException(Exception exception, bool sendEmail = false)
        {
            _logger.LogError(exception.Message.ToString());
        }

        /// <summary>
        /// Function to log warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogWarning(string message, bool sendEmail = false)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Function to log information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendEmail">SendEmail</param>
        public void LogInformation(string message, bool sendEmail = false)
        {
            _logger.LogInformation(message);
        }
    }
}
