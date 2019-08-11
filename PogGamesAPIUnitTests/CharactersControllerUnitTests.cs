using System;
using System.Collections.Generic;
using System.Text;
using PogGames.Controllers;
using PogGames.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace PogGamesAPIUnitTests
{
    [TestClass]
    public class CharactersControllerUnitTests
    {
        public static readonly DbContextOptions<PogGamesContext> options
            = new DbContextOptionsBuilder<PogGamesContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase")
            .Options;

        public static readonly IList<Character> characters = new List<Character>
        {

            new Character()
            {
                ApiCharId = "01",
                CharName = "Genji",
                GameId = "02"
                
            },
            new Character()
            {
                ApiCharId = "02",
                CharName = "Mei",
                GameId = "02"
            }
        };
        

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new PogGamesContext(options))
            {
                //populate the db
                context.Character.Add(characters[0]);
                context.Character.Add(characters[1]);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new PogGamesContext(options))
            {
                // clear the db
                context.Character.RemoveRange(context.Character);
                context.SaveChanges();
            };
        }

        [TestMethod]
        public async Task TestGetSuccessfully()
        {
            using (var context = new PogGamesContext(options))
            {
                CharactersController charactersController = new CharactersController(context);
                ActionResult<IEnumerable<Character>> result = await charactersController.GetCharacter();

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task TestPutCharacterNoContentStatusCode()
        {
            using (var context = new PogGamesContext(options))
            {
                string newCharacter = "mccree";
                Character character1 = context.Character.Where(x => x.CharName == characters[0].CharName).Single();
                character1.CharName = newCharacter;

                CharactersController charactersController = new CharactersController(context);
                IActionResult result = await charactersController.PutCharacter(character1.ApiCharId, character1) as IActionResult;

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }
    }

}
