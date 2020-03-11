using System.Threading.Tasks;
using sm_coding_challenge.Models;

namespace sm_coding_challenge.Services.DataProvider
{
    public interface IDataProvider
    {
        // Made method async through rename, and return type
        Task<PlayerModel> GetPlayerByIdAsync(string id);

        // Added method to get latest players
        Task<PlayerDataModel> GetLatestPlayersAsync(string[] ids);
    }
}
