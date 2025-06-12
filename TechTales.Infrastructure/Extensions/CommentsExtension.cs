using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class CommentsExtension
    {

        public static CommentsModel ConvertComentarioEntityToModel(this Comments comentarioEntity)
        {

            var comentarioModel = new CommentsModel()
            {
                Id = comentarioEntity.Id,
                Content = comentarioEntity.Content,
                DateComment = comentarioEntity.DateComment,
                IdUser = comentarioEntity.IdUser,
                IdStorie = comentarioEntity.IdStorie,
            };


            return comentarioModel;

        }


    }
}
