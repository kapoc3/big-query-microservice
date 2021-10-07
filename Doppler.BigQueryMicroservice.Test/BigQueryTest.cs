using Dapper;
using Doppler.BigQueryMicroservice.Entitites;
using Doppler.BigQueryMicroservice.Repository;
using Doppler.BigQueryMicroservice.Serialization;
using Doppler.BigQueryMicroservice.Services;
using Doppler.BigQueryMicroservice.Utils;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
            }
            };
            var expectedContent = "{\"emails\":[\"KapocEmail@gmail.com\"]}";
            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<UserAccessByUser>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(dbResponse);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Get, "big-query/cgil@makingsense.com/allowed-emails")
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
            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<UserAccessByUser>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(Enumerable.Empty<UserAccessByUser>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Get, "big-query/test1@test.com/allowed-emails")
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
            bool exist = responseContent.Contains("404");
            Assert.True(exist);
            #endregion

        }

        [Fact]
        public async Task PUT_save_allowed_emails_user_not_found()
        {
            #region Arrange
            var parameter = new AllowedEmails();
            parameter.Emails.Add("cristiancamilo11033@gmail.com");
            var json = JsonConvert.SerializeObject(parameter);
            var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");


            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<User>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(Enumerable.Empty<User>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Put, "big-query/cgil@makinsense.com/allowed-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } },
                Content = content
            };

            #endregion

            #region Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseContent);
            #endregion

            #region Assert
            bool exist = responseContent.Contains("404");
            Assert.True(exist);
            #endregion

        }

        [Fact]
        public async Task PUT_save_allowed_emails_user_found()
        {
            #region Arrange
            var dbResponse = new[] { new User { Email = "cgil@makingsense.com", IdUser = 103021, Languaje = "en" } };
            var parameter = new AllowedEmails();
            parameter.Emails.Add("cristiancamilo110133@gmail.com");
            var json = JsonConvert.SerializeObject(parameter);
            var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var expectedContent = "true";

            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupDapperAsync(c => c.QueryAsync<User>(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(dbResponse);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.SetupConnectionFactory(mockConnection.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Put, "big-query/cgil@makinsense.com/allowed-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } },
                Content = content
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

        [Fact]
        public async Task SaveAllowedEmails_should_return_200_when_email_notification_fails()
        {
            // Arrange
            using var httpTest = new HttpTest();
            httpTest.RespondWith("", 500);
            var userId = 1;
            var userEmail = "dummy@email.com";
            var emails = new List<string> { "nuevo@gmail.com", "nuevo2@gmail.com" };
            var user = new User { Email = userEmail, IdUser = userId, Languaje = "en" };
            var mergeResult = new MergeEmailResult(emails);
            var requestBody = new { Emails = emails };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByEmail(userEmail)).ReturnsAsync(user);

            var userAccessByUserRepositoryMock = new Mock<IUserAccessByUserRepository>();
            userAccessByUserRepositoryMock.Setup(x => x.MergeEmailsAsync(userId, emails)).ReturnsAsync(mergeResult);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(userRepositoryMock.Object);
                    services.AddSingleton(userAccessByUserRepositoryMock.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Put, $"big-query/{userEmail}/allowed-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } },
                Content = JsonContent.Create(requestBody)
            };

            // Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseContent);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SaveAllowedEmails_should_not_call_email_when_not_has_new_permissions()
        {
            // Arrange
            using var httpTest = new HttpTest();
            var userId = 1;
            var userEmail = "dummy@email.com";
            var emails = new List<string> { "dummy@email.com" };
            var user = new User { Email = userEmail, IdUser = userId, Languaje = "en" };
            var mergeResult = new MergeEmailResult(emails);
            var requestBody = new { Emails = emails };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByEmail(userEmail)).ReturnsAsync(user);

            var userAccessByUserRepositoryMock = new Mock<IUserAccessByUserRepository>();
            userAccessByUserRepositoryMock.Setup(x => x.MergeEmailsAsync(userId, emails)).ReturnsAsync(mergeResult);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(userRepositoryMock.Object);
                    services.AddSingleton(userAccessByUserRepositoryMock.Object);
                });

            }).CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Put, $"big-query/{userEmail}/allowed-emails")
            {
                Headers = { { "Authorization", $"Bearer {TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518}" } },
                Content = JsonContent.Create(requestBody)
            };

            // Act
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var log = httpTest.CallLog;

            //Assert
            httpTest.ShouldNotHaveMadeACall();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
