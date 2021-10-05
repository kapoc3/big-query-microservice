using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Entitites.EmailSender
{
    /// <summary>
    /// Attachment Abstraction for email sender
    /// </summary>
    public class Attachment
    {
        public string ContentType { get; set; }
        public string Filename { get; set; }
        public byte[] Content { get; set; }
    }
}
