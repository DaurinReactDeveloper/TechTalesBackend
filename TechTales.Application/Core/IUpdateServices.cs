using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Core
{
    public interface IUpdateServices<UpdateDto>
    {

        Task<ServiceResult> Update(UpdateDto modelDto);

    }
}
