using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PogGames.Model;
using System.Net.Http;
using System.Text;

namespace PogGames.Helper
{
    public class IGDBHelper
    {
        public static void testProgram()
        {
            
            Console.WriteLine(GetGameInfoAsync("Overwatch"));


            Console.ReadLine();
        }

        

        public static async Task<Game> GetGameInfoAsync(String gameName)
        {
            string APIKey = ("d5c8b25f428f3bd850594d5f587f9095");

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api-v3.igdb.com/games/?search=" + gameName + "&fields=id,name,first_release_date,cover,genres,involved_companies,summary,rating,rating_count, url"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    request.Headers.TryAddWithoutValidation("user-key", APIKey);

                    request.Content = new StringContent("fields age_ratings,aggregated_rating,aggregated_rating_count,alternative_names,artworks,bundles,category,collection,cover,created_at,dlcs,expansions,external_games,first_release_date,follows,franchise,franchises,game_engines,game_modes,genres,hypes,involved_companies,keywords,multiplayer_modes,name,parent_game,platforms,player_perspectives,popularity,pulse_count,rating,rating_count,release_dates,screenshots,similar_games,slug,standalone_expansions,status,storyline,summary,tags,themes,time_to_beat,total_rating,total_rating_count,updated_at,url,version_parent,version_title,videos,websites;", Encoding.UTF8, "application/x-www-form-urlencoded");

                    var response = await httpClient.SendAsync(request);
               
                    response.EnsureSuccessStatusCode();
                    string responsebody = await response.Content.ReadAsStringAsync();
                    
                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(responsebody);
                    Console.WriteLine(jsonObj);
                 
                    string id = jsonObj[0]["id"];
                    string name = jsonObj[0]["name"];
                    string release_date = jsonObj[0]["first_release_date"];
                    string company = jsonObj[0]["involved_companies"][0];
                    string coverImageURL = jsonObj[0]["cover"];
                    string genre = jsonObj[0]["genres"][0];
                    string summary = jsonObj[0]["summary"];
                    string rating = jsonObj[0]["rating"];
                    string rating_count = jsonObj[0]["rating_count"];
                    Game game = new Game();
                    game.GameId = int.Parse(id);
                    game.GameName = name;
                    game.GameRelease = release_date;
                    game.GameSummary = summary
                    game.GameCompany = company;
                    game.Genre = genre;
                    game.Rating = rating;
                    game.RatingCount = int.Parse(rating_count);
                    game.CoverImageUrl = coverImageURL;
                    game.IsFavourite = false;
                    
                    return game;
                }
            }
        }
    }
}
