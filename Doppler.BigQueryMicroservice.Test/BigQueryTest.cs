using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Moq.Dapper;
using System;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Doppler.BigQueryMicroservice
{
    /// <summary>
    /// Unit test for BigQuery controller
    /// </summary>
    public class BigQueryTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";
        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20010908 = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoxMDAwMDAwMDAwfQ.JBmiZBgKVSUtB4_NhD1kiUhBTnH2ufGSzcoCwC3-Gtx0QDvkFjy2KbxIU9asscenSdzziTOZN6IfFx6KgZ3_a3YB7vdCgfSINQwrAK0_6Owa-BQuNAIsKk-pNoIhJ-OcckV-zrp5wWai3Ak5Qzg3aZ1NKZQKZt5ICZmsFZcWu_4pzS-xsGPcj5gSr3Iybt61iBnetrkrEbjtVZg-3xzKr0nmMMqe-qqeknozIFy2YWAObmTkrN4sZ3AB_jzqyFPXN-nMw3a0NxIdJyetbESAOcNnPLymBKZEZmX2psKuXwJxxekvgK9egkfv2EjKYF9atpH5XwC0Pd4EWvraLAL2eg";

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public BigQueryTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        /// <summary>
        /// Method for validate endpoint response with result
        /// </summary>
        /// <returns>Null</returns>
        [Fact]
        public async Task GET_allow_emails_with_data_in_reponse()
        {
            #region Arrange
            var now = DateTime.Now;
            var dbResponse = new[] { new UserAccessByUser {
                IdUser = 123456,
                Email = "KapocEmail@gmail.com",
                CreateAt = now,
                UpdateAt = now,
                ValidFrom = now,
                ValidTo = now
            },
            new UserAccessByUser {
                IdUser = 987654,
                Email = "KapocEmail2@gmail.com",
                CreateAt = now,
                UpdateAt = now,
                ValidFrom = now,
                ValidTo = now
            }
            };

            var expectedContent = "{\"emails\":[\"KapocEmail@gmail.com\",\"KapocEmail2@gmail.com\"]}";
            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<UserAccessByUser>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(dbResponse);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Get, "big-query/test1@test.com/allow-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } }
            };

            #endregion

            #region Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseContent);
            #endregion

            #region Assert
            Assert.Equal(expectedContent, responseContent);
            #endregion

        }

        /// <summary>
        /// Method for validate endpoint response with result not empty
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GET_allow_emails_with_out_data_in_reponse()
        {
            #region Arrange
            var expectedContent = "{\"emails\":[]}";
            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<UserAccessByUser>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(Enumerable.Empty<UserAccessByUser>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Get, "big-query/test1@test.com/allow-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } }
            };

            #endregion

            #region Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseContent);
            #endregion

            #region Assert
            Assert.Equal(expectedContent, responseContent);
            #endregion

        }

    }
}
