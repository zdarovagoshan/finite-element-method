using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface ILocalMatrix : IGlobalMatrix
    {

        void create_loc_matrix();
        void Aloc(int k);
        double create_G_loc(int i, int j, int k);
        double create_M_loc(int i, int j);
        void addelem(int i, int j, double a);
        int fun_mu(int i);
        int fun_nu(int i);

    }
}
