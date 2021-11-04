using Doppler.BigQueryMicroservice.Entitites.EmailSender;
using Doppler.BigQueryMicroservice.Repository;
using Doppler.BigQueryMicroservice.Serialization;
using Doppler.BigQueryMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<BigQueryController> _logger;
        private readonly IOptions<EmailNotificationsConfiguration> _emailSettings;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="userAccessByUserRepository">repository with schema datastudio access.</param>
        /// <param name="userRepository">repository for users from doppler database.</param>
        /// <param name="emailSender">service for send mails.</param>
        public BigQueryController(
            IUserAccessByUserRepository userAccessByUserRepository,
            IUserRepository userRepository,
            IEmailSender emailSender,
            ILogger<BigQueryController>
            logger, IOptions<EmailNotificationsConfiguration> emailSettings
            )
        {
            this._userAccessByUserRepository = userAccessByUserRepository;
            this._userRepository = userRepository;
            this._emailSender = emailSender;
            this._logger = logger;
            this._emailSettings = emailSettings;
        }

        [HttpGet("/big-query/{accountName}/allowed-emails")]
        public async Task<ActionResult> GetAllowedEmails(string accountName)
        {
            _logger.LogError("Testing log error");
            _logger.LogDebug("Testing log debug");
            _logger.LogInformation("Testing log information");
            _logger.LogWarning("Testing log warning");

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
            var template = _emailSettings.Value.BigQueryInvitationTemplateId[user.Language ?? "en"];

            _logger.LogInformation("result:{result}", result);
            _logger.LogInformation("count.result:{count}", result.InsertedEmails.Count);
            _logger.LogInformation("template:{template}", template);

            foreach (var email in result.InsertedEmails)
            {
                await _emailSender.SafeSendWithTemplateAsync(
                    templateId: template,
                    templateModel: new { account_name = accountName , year = DateTime.UtcNow.Year, urlImagesBase ="https://cdn.fromdoppler.com"},
                    to: new []{ email }
                    );
            }

            /*if (result.InsertedEmails.Count > 0)
            {
                await _emailSender.SafeSendWithTemplateAsync(
                    templateId: template,
                    templateModel: result.InsertedEmails,
                    to: model.Emails
                    );
            }*/

            return Ok(true);
        }
    }
}
