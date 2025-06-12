using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTales.Application.Core
{
    public interface IBaseServices<AddDto,RemoveDto>
    {

      Task<ServiceResult> Add(AddDto modelDto);
      Task<ServiceResult> Remove(RemoveDto modelDto);

    }
}
