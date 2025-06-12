using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.RetosCompletados;

namespace TechTales.Application.Contract
{
    public interface IChallengesCompletedServices : IBaseServices<ChallengesCompletedAddDto, ChallengesCompletedRemoveDto>
    {

        Task<ServiceResult> GetChallengesCompletedByUserId(int usuarioId);

    }
}
