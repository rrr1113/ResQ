using System.Text.Json.Serialization;

namespace Domain.Dto;

public class GeocodingApiResponse
{
    [JsonPropertyName("name")]
    public double Name  { get; set; }
    
    [JsonPropertyName("lat")]
    public double Latitude  { get; set; }
    
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
   
}

/*
 https://nominatim.openstreetmap.org/search?q=5%20Avenue%20Anatole%20France%2C%20Paris%2C%20France&format=json&limit=1
 
[
  {
    "place_id": 98616190,
    "licence": "Data © OpenStreetMap contributors, ODbL 1.0. http://osm.org/copyright",
    "osm_type": "way",
    "osm_id": 5013364,
    "lat": "48.8582599",
    "lon": "2.2945006",
    "class": "man_made",
    "type": "tower",
    "place_rank": 30,
    "importance": 0.620593772435312,
    "addresstype": "man_made",
    "name": "Eiffel Tower",
    "display_name": "Eiffel Tower, 5, Avenue Anatole France, Quartier du Gros-Caillou, 7th Arrondissement, Париз, Ile-de-France, Европска Франција, 75007, Франција",
    "boundingbox": [
      "48.8574753",
      "48.8590453",
      "2.2933119",
      "2.2956897"
    ]
  }
]
*/