using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetoDto;

namespace TechTales.Application.Contract
{
    public interface IChallengesServices
        : IBaseServices<ChallengesAddDto, ChallengesRemoveDto>, IUpdateServices<ChallengesUpdateDto>
    {
        Task<ServiceResult> GetChallenges();

        Task<ServiceResult> GetChallengesForType(string type);
    }
}
