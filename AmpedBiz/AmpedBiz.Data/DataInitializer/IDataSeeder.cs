using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.DataInitializer
{
    public interface IDataSeeder
    {
        int ExecutionOrder { get; }
        void Seed();
    }
}
