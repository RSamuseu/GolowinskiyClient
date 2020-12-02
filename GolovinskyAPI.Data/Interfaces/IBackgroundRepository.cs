using GolovinskyAPI.Data.Models.Background;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GolovinskyAPI.Data.Interfaces
{
    public interface IBackgroundRepository
    {
        Task<string> Create(Background background);
        Task<List<Background>> GetBackground(Background background);
        Task<string> Update(Background background);
        Task<string> Delete(Background background);
    }
}
