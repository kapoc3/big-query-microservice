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
        private readonly IUserAccessByUserRepository _userAccessByUserRepository;
        private readonly IUserRepository _userRepository;


        public BigQueryController(IUserAccessByUserRepository userAccessByUserRepository, IUserRepository userRepository)
        {
            this._userAccessByUserRepository = userAccessByUserRepository;
            this._userRepository = userRepository;
        }

        [HttpGet("/big-query/{accountName}/allowed-emails")]
        public async Task<ActionResult> GetAllowedEmails(string accountName)
        {
            var user = await _userRepository.GetUserByEmail(accountName);
            if (user == null)
            {
                return NotFound();
            }

            var data = await _userAccessByUserRepository.GetAllByUserIdAsync(user.IdUser);
            var result = new AllowedEmails(data);
            return Ok(result);
        }

        [HttpPut("/big-query/{accountName}/allowed-emails")]
        public async Task<ActionResult> SaveAllowedEmails(string accountName, AllowedEmails model)
        {
            var user = await _userRepository.GetUserByEmail(accountName);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userAccessByUserRepository.MergeEmailsAsync(user.IdUser, model.Emails);
            return Ok(result);
        }
    }
}
