using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared;

public  class CommandExecutionResultGeneric<T>
{
    public bool Success { get; set; }

    public IEnumerable<Error> Errors { get; set; }

    public T Data { get; set; }
}
