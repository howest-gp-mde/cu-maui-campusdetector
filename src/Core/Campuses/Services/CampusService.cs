using Mde.CampusDetector.Core.Campuses.Models;
using System.Text.Json;

namespace Mde.CampusDetector.Core.Campuses.Services
{
    public class CampusService : ICampusService
    {
        private const string dataUrl = "https://raw.githubusercontent.com/howest-gp-mde/cu-maui-campusdetector/master/data";
        private readonly HttpClient httpClient;
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        { 
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public CampusService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Campus>> GetAllCampuses()
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{dataUrl}/campuses.json");
            if (response.IsSuccessStatusCode)
            {
                //get and parse json
                string jsonContent = await response.Content.ReadAsStringAsync();
                List<CampusDto> campusDtos = 
                        JsonSerializer.Deserialize<List<CampusDto>>(jsonContent, jsonSerializerOptions);

                // map CampusDto objects to Campus objects
                return campusDtos.Select(dto => new Campus
                {
                    Latitude = dto.Coordinates[0],
                    Longitude = dto.Coordinates[1],
                    Name = dto.Description,
                    PhotoUrl = $"{dataUrl}/images/{dto.Image}"
                });
            }
            else
            {
                throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");
            }
        }

    }
}