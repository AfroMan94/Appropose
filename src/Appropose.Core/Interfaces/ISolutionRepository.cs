using System.Collections.Generic;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;

namespace Appropose.Core.Interfaces
{
    public interface ISolutionRepository : IRepository<SolutionEntity>
    {
        Task<IEnumerable<SolutionEntity>> GetSolutionsForPostAsync(string solutionId);
    }
}
