using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorksPressTooling.Models.Tooling;

namespace SolidWorksPressTooling.Data.Contracts
{
    public interface IDieBodyRepositories
    {
        Task<DieBody> GetDieAsync(string pressSize);
    }
}
