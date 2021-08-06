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
        private readonly IUnitOfWork _unitOfWork;
        public BigQueryController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("/big-query/{accountName}/allowed-emails")]
        public async Task<ActionResult> GetAllowedEmails(string accountName)
        {
            var user = await _unitOfWork.User.GetUserByEmail(accountName);
            if (user == null)
            {
                return NotFound();
            }

            var data = await _unitOfWork.UserAccessByUser.GetAllByUserIdAsync(user.IdUser);
            var result = new AllowedEmails(data);
            return Ok(result);
        }

        [HttpPut("/big-query/{accountName}/allowed-emails")]
        public async Task<ActionResult> SaveAllowedEmails(string accountName, AllowedEmails model)
        {
            var user = await _unitOfWork.User.GetUserByEmail(accountName);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _unitOfWork.UserAccessByUser.MergeEmailsAsync(user.IdUser, model.Emails);
            return Ok(result);
        }
    }

}
