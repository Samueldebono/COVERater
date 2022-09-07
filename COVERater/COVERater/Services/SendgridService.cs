using System;
using System.Threading.Tasks;
using COVERater.API.Models;
using COVERater.API.Services;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Traidy.Services
{


    public class SendgridService/* : ISendGridClient, IDisposable*/
    {
        const string ForgotTemplateId = "d-561d957da3e048f2ab3c5dbd5cc86d5bc";
        const string WelcomeTempleteId = "d-da0f10dfb62c41d49ee6742c5347312a";
        const string ReminderTempleteId = " d-ca640c8858c2414b8ae843d0ddc4dd0f";
        private SendGridClient _client;
        private readonly ICoveraterRepository _repository;

        public SendgridService(ICoveraterRepository repository)
        {
            _repository =
                repository ?? throw new ArgumentNullException(nameof(repository));

            _client = new SendGridClient(ApiKey);
        }
        public async Task SendActivationEmailAsync(AuthUsers user, string password)
        {
            var props = new
            {
                password = password
            };
            await SendEmailAsync(new EmailAddress(user.Email, "Coverater User"), "", WelcomeTempleteId, props);
        }
        public async Task SendDetailsAsync(AuthUsers user)
        {
            var props = new
            {
            
            };
            await SendEmailAsync(new EmailAddress(user.Email, "Coverater User"), "", ReminderTempleteId, props);
        }

        public async Task SendResetPasswordAsync(AuthUsers user, string password)
        {
            var props = new
            {
                password = password
            };
            await SendEmailAsync(new EmailAddress(user.Email, "Coverater User"), "", ForgotTemplateId, props);
        }

        public async Task<bool> AddContact(AuthUsers user)
        {
            var client = new SendGridClient(ApiKey);

            var data = @"{
            ""list_ids"":[],
            ""contacts"": [
                {
                ""address_line_1"": """",
                ""address_line_2"": """",
                ""alternate_emails"": [],
                ""city"": """",
                ""country"":"""",
                ""email"": """ + user.Email+ @""",
                ""first_name"":"""",
                ""last_name"": """",
                ""pospostal_code"": """",
                ""state_province_region"": """",
                ""custom_fields"":  {}
                }
            ]
        }";




            var response = await client.RequestAsync(
                method: SendGridClient.Method.PUT,
                urlPath: "marketing/contacts",
                requestBody: data
            );

            var emaillogs = new EmailLogs()
            {
                Email = user.Email,
                Status = response.StatusCode.ToString(),
                Response = response.Body.ToString(),
                EmailSent = "add Contact",
                Time = DateTime.UtcNow
            };

            _repository.CreateEmailLogs(emaillogs);

            return true;
        }

        private async Task<bool> SendEmailAsync(EmailAddress to, string subject, string templateId, object? properties = null)
        {
            //TODO add email log
            SendGridMessage sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress("admin@coverater.com", "Coverator"),
                TemplateId = templateId,
                Subject = subject,
            };

            if (properties != null)
                sendGridMessage.SetTemplateData(properties);
            sendGridMessage.AddTo(to);
            var response = await _client.SendEmailAsync(sendGridMessage).ConfigureAwait(false);
            var emaillogs = new EmailLogs()
            {
                Email = to.Email,
                Status = response.StatusCode.ToString(),
                Response = response.Body.ToString(),
                EmailSent = templateId,
                Time = DateTime.UtcNow
            };

            _repository.CreateEmailLogs(emaillogs);


            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }
            return false;
        }

     
    }
}