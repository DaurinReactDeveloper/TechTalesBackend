using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.HistoriaDto;
using TechTales.Application.Dtos.VotosHistoriasDto;
using TechTales.Domain.entities;

namespace TechTales.Application.Contract
{
    public interface IStoriesServices : IBaseServices<StoriesAddDto, StoriesRemoveDto>, IUpdateServices<StoriesUpdateDto>
    {

        Task<ServiceResult> GetStories();

        Task<ServiceResult> GetStoriesByUserId(int usuarioId);



    }
}
