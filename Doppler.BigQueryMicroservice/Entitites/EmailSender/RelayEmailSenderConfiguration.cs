using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Entitites.EmailSender
{
    /// <summary>
    /// Relay email sender configuration
    /// </summary>
    public class RelayEmailSenderConfiguration
    {
        public string SendUrlTemplate { get; set; }
        public string SendTemplateUrlTemplate { get; set; }
        public string ApiKey { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Username { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ReplyToAddress { get; set; }
    }
}
