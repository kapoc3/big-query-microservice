using Doppler.BigQueryMicroservice.Repository;
using Doppler.BigQueryMicroservice.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doppler.BigQueryMicroservice.Controllers
{
    /// <summary>
    /// Big query Api.
    /// </summary>
    [Authorize]
    [ApiController]
    public class BigQueryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public BigQueryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("/big-query/{accountName}/allow-emails")]
        public async Task<ActionResult> GetAllowEmails(string accountName)
        {
            var data = await unitOfWork.UserAccessByUser.GetAllByUserIdAsync(accountName);
            var result = new AllowEmails(data);
            return Ok(result);

        }
    }
}
