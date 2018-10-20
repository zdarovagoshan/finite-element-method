using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Solver_SLAU;
public class koord
{
    double s1,s2 ;
    public double x
    {
        set { s1= value; }
        get { return s1; }
    }
    public double y
    {
        set { s2 = value; }
        get { return s2; }
    }

}

namespace ConsoleApplication1
{
    class Program
    {
        /*static void Main(string[] args)
        {
            mke m = new mke();
            m.solution();
            Console.ReadKey();
        }*/
    }
    class mke
    {
        int p = 0;
        protected int n;
        protected int m;
        protected int k_1;
        protected double lambda { get; set; }
        protected List<List<int>> nvtr= new List<List<int>>();

        protected List<koord> xy=new List<koord>();
        protected List<int> kraev1 =new List<int>();
        protected List<double> b=new List<double>();
        protected List<List<int>> usel=new List<List<int>>();
        protected LinearTrianglBasis basis;
        protected void input()
        {
            StreamReader reader = new StreamReader(@"Models/in.txt");
            string[] str = reader.ReadToEnd().Split(' ', '\n');
            int i=0, j;
            foreach (string a in str)
            {
                if (i == 0)
                { this.n = Convert.ToInt32(a); i++; }
                else
                    if (i == 1) { this.m = Convert.ToInt32(a); i++; }
                    else
                    this.k_1 = Convert.ToInt32(a);
            }
            reader.Close();
            /*reader = new StreamReader(@"Models/lam.txt");
            str = reader.ReadToEnd().Split(' ', '\n');
            this.lambda = Convert.ToDouble(str[0]);
            reader.Close();*/
            reader = new StreamReader(@"Models/nvtr.txt");
            str = reader.ReadToEnd().Split(' ','\n');
            i = 0; j=0;
            nvtr.Capacity =m;           
            nvtr.Add(new List<int>());
            foreach (string a in str)
            {
               
                nvtr[i].Add(Convert.ToInt32(a));
                if (j == 2) {
                    i++;
                    j = 0;
                    if (i<m)
                        nvtr.Add(new List<int>());
                }
                else j++;
            }
            reader.Close();
            xy.Capacity = n;
            xy.Add(new koord());
            reader = new StreamReader(@"Models/xy.txt");
            str = reader.ReadToEnd().Split(' ', '\n');
            i = 0;
            j = 0;           
            foreach (string a in str)
            {
                if (j % 2 == 0)
                {
                    xy[i].x = Convert.ToDouble(a);
                    j++;
                }
                else
                {
                    xy[i].y = Convert.ToDouble(a);
                    j=0;
                    i++;
                    if (i < n)
                        xy.Add(new koord());
                }
            }
            reader.Close();
            reader = new StreamReader(@"Models/nvk.txt");
            str = reader.ReadToEnd().Split(' ', '\n');
            kraev1.Capacity = k_1;
            foreach (string a in str)
            {
                this.kraev1.Add(Convert.ToInt32(a));

            }
            reader.Close();
        }
        protected void output() {
            koord x0=new koord();
            for (int j = 0; j < n; j++)
            {
                x0.x = xy[j].x;
                x0.y = xy[j].y;
                Console.WriteLine(x0.x+" "+ x0.y + " " + basis.UChisl(x0, this.b) + " " + basis.test.u_analitic(x0) + "\n");
            }
            
        }
        public void solution() {           
            input();
            basis = new LinearTrianglBasis(this.n, this.m, this.lambda, this.nvtr, this.xy, this.kraev1);
            basis.create_loc_matrix();
            basis.genpor();
            basis.Aglobal_prof();
            basis.first_kraev();
            LU_LOS l= new LU_LOS();
            l.set(basis.di, basis.ggl, basis.ggu, basis.b, basis.ig, basis.jg, 1E-53, 10000);
            this.b =l.solve();
            output();
        }

       
    }
    
    
}
