using CleanArchitectureTemplate.Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Exceptions
{
    public class ModelNotFoundException : CustomException
    {

        public ModelNotFoundException()
            : base(string.Empty, null)
        {
        }

    }
}
