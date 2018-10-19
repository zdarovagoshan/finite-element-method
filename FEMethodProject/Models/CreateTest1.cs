using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CreateTest1 : ICondition
    {
        public double func(koord x, double lam)
        {
            return 0.0 * lam;
        }
        public double u_analitic(koord x)
        {
            return x.x + x.y;
        }
    }
}
