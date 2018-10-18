using System;
using System.Collections.Generic;
using System.Text;

namespace Solver_SLAU
{
    interface ISolver
    {
        //сначало установить, потом посчитать
        void set(List<double> di, List<double> gl, List<double> gu, List<double> b, List<double> x, List<int> ig, List<int> jg, double EPS, int MAXITER);
        List<double> solve();
        double Discrepancy { get; set; }//невязка
    }

}
