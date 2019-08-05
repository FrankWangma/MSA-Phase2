using Microsoft.EntityFrameworkCore;
using PogGames.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogGames.DAL
{
    public class GameRepository : IGameRepository, IDisposable
    {
        private PogGamesContext context;

        public GameRepository(PogGamesContext context)
        {
            this.context = context;
        }

        public IEnumerable<Game> GetGames()
        {
            return context.Game.ToList();
        }

        public Game GetGameById(String id)
        {
            return context.Game.Find(id);
        }

        public void InsertGame(Game game)
        {
            context.Game.Add(game);
        }

        public void DeleteGame(string gameId)
        {
            Game game = context.Game.Find(gameId);
            context.Game.Remove(game);
        }

        public void UpdateGame(Game game)
        {
            context.Entry(game).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
