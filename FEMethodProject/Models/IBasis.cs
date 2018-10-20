using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface IBasis : ILocalMatrix
    {
        int sizebasis { set; get; }
        double Basis(int i, int k, koord x0);
        int SearchElem(koord x0);
    }
}
