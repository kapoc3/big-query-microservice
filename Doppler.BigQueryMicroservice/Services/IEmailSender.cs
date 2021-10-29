using Doppler.BigQueryMicroservice.Entitites.EmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Services
{
    public interface IEmailSender
    {
        Task SendWithTemplateAsync(
            string templateId,
            object templateModel,
            IEnumerable<string> to,
            IEnumerable<string> cc = null,
            IEnumerable<string> bcc = null,
            string fromName = null,
            string fromAddress = null,
            IEnumerable<Attachment> attachments = null,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SafeSendWithTemplateAsync(
            string templateId,
            object templateModel,
            IEnumerable<string> to,
            IEnumerable<string> cc = null,
            IEnumerable<string> bcc = null,
            string fromName = null,
            string fromAddress = null,
            IEnumerable<Attachment> attachments = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
