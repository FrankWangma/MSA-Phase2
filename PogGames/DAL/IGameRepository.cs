using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PogGames.Model;

namespace PogGames.DAL
{
    public interface IGameRepository : IDisposable
    {
        IEnumerable<Game> GetGames();
        Game GetGameById(string gameId);
        void InsertGame(Game game);
        void DeleteGame(string gameId);
        void UpdateGame(Game game);
        void Save();

    }
}
