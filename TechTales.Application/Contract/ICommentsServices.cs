using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Application.Core;
using TechTales.Application.Dtos.ComentarioDto;

namespace TechTales.Application.Contract
{
    public interface ICommentsServices : IBaseServices<CommentsAddDto, CommentsRemoveDto>
    {

        Task<ServiceResult> GetCommentByUserId(int usuarioId);

        Task<ServiceResult> GetCommentByStoriesId(int historiaId);

        Task<ServiceResult> GetCountCommentByStoriesId(int historiaId);

    }
}
