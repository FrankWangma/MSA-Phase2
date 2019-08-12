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

        [TestMethod]
        public async Task TestPostSuccessfully()
        {
            using (var context = new PogGamesContext(options))
            {
                CharactersController charactersController = new CharactersController(context);

                Character chara = new Character()
                {
                    ApiCharId = "03",
                    CharName = "Mccree",
                    GameId = "02"
                };

                ActionResult<IEnumerable<Character>> result1 = await charactersController.GetCharacter();

                ActionResult<Character> post = await charactersController.PostCharacter(chara);

                ActionResult<IEnumerable<Character>> result2 = await charactersController.GetCharacter();

               
                //Asser that the lists are no longer equal
                Assert.AreNotEqual(result1, result2);
            }
        }

        // Test post method DeleteCharacter()
        [TestMethod]
        public async Task TestDeleteSuccessfully()
        {
            using (var context = new PogGamesContext())
            {
                CharactersController charactersController = new CharactersController(context);

                ActionResult<IEnumerable<Character>> result1 = await charactersController.GetCharacter();

                ActionResult<Character> delete = await charactersController.DeleteCharacter("01");

                ActionResult<IEnumerable<Character>> result2 = await charactersController.GetCharacter();

                // Make sure that the characters list changed after delete 
                Assert.AreNotEqual(result1, result2);
            }
        }
    }

}
