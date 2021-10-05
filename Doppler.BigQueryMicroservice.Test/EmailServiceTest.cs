using AutoFixture;
using Doppler.BigQueryMicroservice.Entitites.EmailSender;
using Doppler.BigQueryMicroservice.Services;
using Doppler.BigQueryMicroservice.Services.Implementation;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.BigQueryMicroservice
{
    /// <summary>
    /// Unit test for email service
    /// </summary>
    public class EmailServiceTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string SendTemplateUrl = "https://api.dopplerrelay.com/accounts/{accountId}/templates/{templateId}/message";

        [Fact]
        public async Task SendWithTemplateAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var apiKey = fixture.Create<string>();
            var relayAccountId = fixture.Create<int>();
            var relayAccountName = fixture.Create<string>();
            var relayUserEmail = "salesrelay@dopplerrelay.com";
            var templateId = fixture.Create<string>();
            var demoData = fixture.Create<string>();
            var toEmail = "email@gmail.com";
            var bccEmail = "copy@copy.com";
            var expectedUrl = $"https://api.dopplerrelay.com/accounts/{relayAccountId}/templates/{templateId}/message";

            var configuration = new RelayEmailSenderConfiguration()
            {
                SendTemplateUrlTemplate = SendTemplateUrl,
                ApiKey = apiKey,
                AccountId = relayAccountId,
                AccountName = relayAccountName,
                Username = relayUserEmail
            };

            IFlurlClientFactory factory = new PerBaseUrlFlurlClientFactory();
            var sut = new RelayEmailSender(Options.Create(configuration), Mock.Of<ILogger<RelayEmailSender>>() , factory);

            using (var httpTest = new HttpTest())
            {
                // Act
                await sut.SendWithTemplateAsync(
                    templateId,
                    new { demoData = demoData },
                    new[] { toEmail },
                    bcc: new[] { bccEmail });

                // Assert
                httpTest
                    .ShouldHaveCalled(expectedUrl)
                    .WithVerb(HttpMethod.Post)
                    .WithOAuthBearerToken(apiKey)
                    .WithRequestJson(new
                    {
                        from_name = (string)null,
                        from_email = (string)null,
                        recipients = new[]
                        {
                            new { email = toEmail, type = "to" },
                            new { email = bccEmail, type = "bcc" },
                        },
                        attachments = (object)null,
                        model = new { demoData = demoData }
                    });
            }
        }
    }
}
