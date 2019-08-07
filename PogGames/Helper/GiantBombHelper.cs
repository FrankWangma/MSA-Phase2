using Newtonsoft.Json;
using PogGames.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PogGames.Helper
{
    public class GiantBombHelper
    {
        private static String APIKey = "87607bb5a3fe599b22933b83f2226583fec57790";
        public static void testProgram()
        {
            Console.WriteLine(GetCharacterFromGameId("48190"));

            // Pause the program execution
            Console.ReadLine();
        }

        public static Game GetGameFromName(String gameName)
        {
            String GiantBombAPIURL = "http://www.giantbomb.com/api/search/?api_key=" + APIKey + "&format=json&query=" + gameName +"&resources=game" +
                "&field_list=id,name,image,deck,original_game_rating";

            // Use an http client to grab the JSON string from the web.
            WebClient gameClient = new WebClient();
            gameClient.Headers.Add("user-agent", "frankwangma");
            String gameInfoJSON = gameClient.DownloadString(GiantBombAPIURL);
           


            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(gameInfoJSON);

            try
            {
                string id = jsonObj["results"][0]["id"];
                string name = jsonObj["results"][0]["name"];
                string release_date = jsonObj["results"][0]["original_release_date"];
                string coverImageURL = jsonObj["results"][0]["image"]["medium_url"];
                string summary = jsonObj["results"][0]["deck"];
                string rating = jsonObj["results"][0]["original_game_rating"][0]["name"];


                String GiantBombGameURL = "http://www.giantbomb.com/api/game/" + id + "/?api_key=" + APIKey + "&format=json" +
                    "&field_list=name,publishers,genres,expected_release_day,expected_release_month,expected_release_year,guid";
                WebClient newGameClient = new WebClient();
                newGameClient.Headers.Add("user-agent", "frankwangma");
                String furtherInfoJSON = newGameClient.DownloadString(GiantBombGameURL);
                dynamic jsonObj2 = JsonConvert.DeserializeObject<dynamic>(furtherInfoJSON);


                string company = jsonObj2["results"]["publishers"][0]["name"];
                string genre = jsonObj2["results"]["genres"][0]["name"];
                string day = jsonObj2["results"]["expected_release_day"];
                string month = jsonObj2["results"]["expected_release_month"];
                string year = jsonObj2["results"]["expected_release_year"];
                string date = day + "/" + month + "/" + year;
                

                Game game = new Game
                {
                    GameId = id,
                    GameName = name,
                    GameSummary = summary,
                    Rating = rating,
                    CoverImageUrl = coverImageURL,
                    GameCompany = company,
                    Genre = genre,
                    GameRelease = date,
                    IsFavourite = false
                };

                return game;
            } catch (ArgumentOutOfRangeException e)
            {
                throw;
            }
           
            
        }

        public static List<Character> GetCharacterFromGameId(String gameID)
        {
            String guid = getGuidFromGameId(gameID);

            String getCharactersURL = "http://www.giantbomb.com/api/game/" + guid + "/?api_key=" + APIKey + "&format=json" +
                "&field_list=characters";

            // Use an http client to grab the JSON string from the web.
            WebClient gameClient = new WebClient();
            gameClient.Headers.Add("user-agent", "frankwangma");
            String gameInfoJSON = gameClient.DownloadString(getCharactersURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(gameInfoJSON);

            List<String> charIds = new List<string>();
            foreach(dynamic item in jsonObj["results"]["characters"])
            {
                String charID = item["id"];
                charIds.Add(charID);
            }

            List<Character> characters = new List<Character>();
            foreach(String charId in charIds)
            {
                try
                {
                    Character character = getCharacterInfoFromCharId(charId);
                    characters.Add(character);
                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return characters;
        }

        public static Character getCharacterInfoFromCharId(String charID)
        {
            String getCharactersURL = "http://www.giantbomb.com/api/character/" + charID + "/?api_key=" + APIKey + "&format=json" +
                "&field_list=name,image,deck,locations,gender";

            // Use an http client to grab the JSON string from the web.
            WebClient gameClient = new WebClient();
            gameClient.Headers.Add("user-agent", "frankwangma");
            String gameInfoJSON = gameClient.DownloadString(getCharactersURL);

            // Using dynamic object helps us to more efficiently extract information from a large JSON String.
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(gameInfoJSON);
            Console.WriteLine(jsonObj);

            string name = jsonObj["results"]["name"];
            string description = jsonObj["results"]["deck"];
            string genderNum = jsonObj["results"]["gender"];
            string gender;
            switch(genderNum)
            {
                case "1":
                    gender = "Male";
                    break;
                case "2":
                    gender = "Female";
                    break;
                default:
                    gender = "Other";
                    break;
            }
            string imageURL = jsonObj["results"]["image"]["small_url"];
            string country = "Unknown";
            try
            {
                country = jsonObj["results"]["locations"][0]["name"];
            } catch
            {
            }

            Character character = new Character()
            {
                CharName = name,
                ApiCharId = charID,
                CharDescription = description,
                CharCountry = country,
                CharGender = gender,
                CharImageUrl = imageURL,
            };

            return character;
        }

        public static String getGuidFromGameId(String GameId)
        {
            String GiantBombGameURL = "http://www.giantbomb.com/api/game/" + GameId + "/?api_key=" + APIKey + "&format=json" +
            "&field_list=guid";
            WebClient newGameClient = new WebClient();
            newGameClient.Headers.Add("user-agent", "frankwangma");
            String guidJSON = newGameClient.DownloadString(GiantBombGameURL);
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(guidJSON);
            String guid = jsonObj["results"]["guid"];
            return guid;
        }
    }
}
