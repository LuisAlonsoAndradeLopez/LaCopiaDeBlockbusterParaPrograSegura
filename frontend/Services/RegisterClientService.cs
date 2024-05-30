using System.Text;

namespace frontendnet.Services
{
    public class EmailClientService(HttpClient client)
    {

        public async Task<bool> SendEmailAsync(string emailTo)
        {
            var requestContent = new StringContent(
                $@"{{
                    ""emailTo"": ""{emailTo}"",
                }}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("api/email", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            var requestContent = new StringContent(
                $@"{{
                    ""email"": ""{email}"",
                    ""code"": ""{code}""
                }}",
                Encoding.UTF8,
                "application/json"
                );

                var response = await client.PostAsync("api/email/verify", requestContent);

                return response.IsSuccessStatusCode;
        }
    }
}