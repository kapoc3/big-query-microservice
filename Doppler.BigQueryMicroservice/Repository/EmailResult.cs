using System.Collections.Generic;

namespace Doppler.BigQueryMicroservice.Entitites
{
    public record MergeEmailResult(List<string> InsertedEmails);
}
