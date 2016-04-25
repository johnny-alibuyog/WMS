using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.Seeders
{
    public interface ISeeder
    {
        int ExecutionOrder { get; }
        bool DummyData { get; }
        void Seed();
    }
}
