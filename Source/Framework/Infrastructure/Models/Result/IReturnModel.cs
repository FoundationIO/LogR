using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Models.Result
{
    public interface IReturnModel
    {
        int HttpCode { get; set; }

        bool IsSuccess { get; set; }
    }
}
