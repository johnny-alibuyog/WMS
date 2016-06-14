using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services
{
    public interface IState<TStatus>
       where TStatus : struct, IConvertible
    {
        TStatus Status { get; }
        void Next();
        void Previous();
    }
}
