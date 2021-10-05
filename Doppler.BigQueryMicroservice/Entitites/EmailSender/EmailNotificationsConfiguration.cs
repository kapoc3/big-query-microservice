using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Entitites.EmailSender
{
    public class EmailNotificationsConfiguration
    {
        public Dictionary<string, string> BigQueryInvitationTemplateId { get; set; }
    }
}
