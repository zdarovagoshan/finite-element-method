using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface IGlobalMatrix
    {
        void Aglobal_prof();
        void genpor();
        void first_kraev();
        double UChisl(koord x0, List<double> q1);
    }
}
