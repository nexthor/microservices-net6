using Mango.Web.Models;
using Mango.Web.Services.IServices;
using System.Text;
using System.Text.Json;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient) 
        { 
            this.httpClient = httpClient;
            this.responseModel = new ResponseDto();
        }

        public async Task<T> SendAsync<T>(ApiRequest request)
        {
            try
            {
                var client = httpClient.CreateClient("MangoAPI");
                var message = new HttpRequestMessage();

                message.Headers.Add("Accept", "application/json");
                if (string.IsNullOrWhiteSpace(request.Url))
                    throw new Exception("Url cannot be null");
                message.RequestUri = new Uri(request.Url);
                client.DefaultRequestHeaders.Clear();
                if (request.Data != null)
                    message.Content = new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                switch (request.ApyType) 
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                response = await client.SendAsync(message);

                var apiContent = new MemoryStream(Encoding.UTF8.GetBytes(await response.Content.ReadAsStringAsync()));
                var apiResponseDto = await JsonSerializer.DeserializeAsync<T>(apiContent, new JsonSerializerOptions());

                return apiResponseDto;
            } catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = $"Error: {ex.Message}",
                    IsSuccess = false,
                    DisplayMessage = "Error",
                    Errors = new List<string>
                    {
                        Convert.ToString(ex.Message)
                    }
                };
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
