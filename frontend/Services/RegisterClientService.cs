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

            var response = await client.PostAsJsonAsync("api/email", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}