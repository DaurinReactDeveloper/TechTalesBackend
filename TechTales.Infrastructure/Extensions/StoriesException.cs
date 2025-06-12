using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechTales.Domain.entities;
using TechTales.Infrastructure.Models;

namespace TechTales.Infrastructure.Extensions
{
    public static class StoriesException

    {

        public static StoriesModel ConvertHistoriaEntityToModel(this Stories historiaEntity)
        {

            var historiaMoodel = new StoriesModel()
            {

                Id = historiaEntity.Id,
                Title = historiaEntity.Title,
                Content = historiaEntity.Content,
                DatePublication = historiaEntity.DatePublication,
                IdUser = historiaEntity.IdUser,

            };

            return historiaMoodel;
        }

    }
}
