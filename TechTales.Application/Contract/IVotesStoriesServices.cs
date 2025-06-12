using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.VotosHistoriasDto;
using TechTales.Domain.entities;

namespace TechTales.Application.Contract
{
    public interface IVotesStoriesServices : IBaseServices<VotesStoriesAddDto, VotesStoriesRemoveDto>, IUpdateServices<VotesStoriesUpdateDto>
    {

        Task<ServiceResult> GetVotesByStoriesId(int storieId);

        VotesStoriesUpdateDto BuildUpdateDto(VotesStories existingVote, VotesStoriesAddDto modelDto);

        VotesStoriesRemoveDto BuildRemoveDto(VotesStories existingVote, VotesStoriesAddDto modelDto);

    }
}
