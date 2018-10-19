using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface ICondition
    {

        double func(koord x, double lam);
        double u_analitic(koord x);
    }
}
