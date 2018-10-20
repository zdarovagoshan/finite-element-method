using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CreateTest2
    {
        public double func(koord x, double lam)
        {
            return 4.0 * lam;
        }
        public double u_analitic(koord x)
        {
            return x.x* x.x + x.y* x.y;
        }
    }
}
