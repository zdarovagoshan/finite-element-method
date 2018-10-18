using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class LinearTrianglBasis : IBasis
    {
        protected int n { get; set; }
        protected int m { get; set; }
        protected double lambda { get; set; }
        protected List<List<int>> nvtr;
        public List<koord> xy;
        protected List<int> kraev1;
        public List<int> ig=new List<int>();
        public List<int> jg = new List<int>();
        public List<double> ggl = new List<double>();
        public List<double> ggu = new List<double>();
        public List<double> di = new List<double>();
        public List<double> b = new List<double>();
        protected List<List<int>> uzel=new List<List<int>>();
        private int size = 3;
        public int sizebasis { set {size= value ; } get { return size; } }
        protected double[,] alpha = new double[3, 3];
        protected double[,] m_loc = new double[3, 3];
        public List<double> bl=new List<double>();        
        protected double[,] Al = new double[3, 3];
        public CreateTest1 test = new CreateTest1();
        public LinearTrianglBasis(int n, int m, double lambda, List<List<int>> nvtr, List<koord> xy, List<int> kraev1)
        {
            this.n = n;
            this.m = m;
            this.nvtr = nvtr;
            this.xy = xy;
            this.kraev1 = kraev1;
            this.lambda = lambda;
        }
        protected void input()
        {
            uzel.Capacity = this.n + 1;
            for (int i = 0; i < n + 1; i++)
            {
                uzel.Add(new List<int>());
            }
            di.Capacity = n;
            b.Capacity = n;
            for(int i = 0; i < n; i++)
            {
                b.Add(0.0);
                di.Add(0.0);
            }
            bl.Capacity = sizebasis;
            for (int i = 0; i < sizebasis; i++)
            {
                bl.Add(0.0);
            }

        }
        protected double DetD(double[] x, double[] y)
        {
            return (x[1] - x[0]) * (y[2] - y[0]) - (x[2] - x[0]) * (y[1] - y[0]);
        }
        protected void KoefAlpha(double[] x, double[] y)
        {

            double detD = DetD( x, y);
            alpha[0, 0] = (x[1] * y[2] - x[2] * y[1]) / detD;
            alpha[1, 0] = (x[2] * y[0] - x[0] * y[2]) / detD;
            alpha[2, 0] = (x[0] * y[1] - x[1] * y[0]) / detD;
            alpha[0, 1] = (y[1] - y[2]) / detD;
            alpha[0, 2] = (x[2] - x[1]) / detD;
            alpha[1, 1] = (y[2] - y[0]) / detD;
            alpha[1, 2] = (x[0] - x[2]) / detD;
            alpha[2, 1] = (y[0] - y[1]) / detD;
            alpha[2, 2] = (x[1] - x[0]) / detD;
        }
        private double GetAlpha(int i, int j)
        {
            return alpha[i, j];
        }
        public double Basis(int i, int k, koord x0)
        {
            double[] x = new double[3];
            double[] y = new double[3];
            for (int j = 0; j < sizebasis; j++)
            {
                x[j] = xy[nvtr[k][j]].x;
                y[j] = xy[nvtr[k][j]].y;

            }
            KoefAlpha(x, y);
            switch (i)
            {
                case 0: { return GetAlpha(0, 0) + GetAlpha(0, 1) * x0.x + GetAlpha(0, 2) * x0.y; }
                case 1: { return GetAlpha(1, 0) + GetAlpha(1, 1) * x0.x + GetAlpha(1, 2) * x0.y; }
                case 2: { return GetAlpha(2, 0) + GetAlpha(2, 1) * x0.x + GetAlpha(2, 2) * x0.y; }
            }
            return 0;
        }
        public int SearchElem(koord x0)
        {

            int k;
            double[] x = new double[3];
            double[] y = new double[3];
            double[,] g_loc = new double[sizebasis, sizebasis];
            double detD, S = 0;
            for (k = 0; k < m; k++)
            {
                for (int i = 0; i < sizebasis; i++)
                {
                    x[i] = xy[nvtr[k][i]].x;
                    y[i] = xy[nvtr[k][i]].y;
                }
                detD = DetD(x, y);
                S = 0;
                for (int i = 0; i < sizebasis - 1; i++)
                {
                    S += Math.Abs((x[i + 1] - x[i]) * (x0.y - y[i]) - (x0.x - x[i]) * (y[i + 1] - y[i]));
                }
                S += Math.Abs((x[0] - x[2]) * (x0.y - y[2]) - (x0.x - x[2]) * (y[0] - y[2]));

                if (Math.Abs(S - detD)<1E-15) break;
            }
            return k;
        }
        public double UChisl(koord x0, List<double> q1)
        {
            double u = 0.0, psi;
            int area = SearchElem(x0);
            for (int i = 0; i < sizebasis; i++)
            {
                psi = Basis(i, area, x0);
                u += q1[nvtr[area][i]] * psi;
            }
            return u;
        }
        public void create_loc_matrix()
        {
            m_loc[0, 0] = m_loc[1, 1] = m_loc[2, 2] = 1.0 / 12.0;
            m_loc[0, 1]= m_loc[0, 2]=m_loc[1, 0] = m_loc[1, 2] = m_loc[2, 0] = m_loc[2, 1] = 1.0 / 24.0;
        }
        public double create_G_loc(int i, int j, int k)
        {
            return GetAlpha(i, 1) * GetAlpha(j, 1) + GetAlpha(i, 2) * GetAlpha(j, 2);
        }
        public double create_M_loc(int i, int j)
        {
            return m_loc[i, j];
        }
        public void Aloc(int k)
        {
            double Aij, Mij;
            double[] x = new double[3];
            double[] y = new double[3];
            int size = nvtr[k].Count();
            for (int i = 0; i < sizebasis; i++)
            {
                bl[i] = 0.0;
                x[i] = xy[nvtr[k][i]].x;
                y[i] = xy[nvtr[k][i]].y;
            }
            double detD = Math.Abs(DetD(x, y));
            KoefAlpha(x, y);
            for (int i = 0; i < sizebasis; i++)
            {
                for (int j = 0; j < sizebasis; j++)
                {
                    Aij = create_G_loc(i, j, k);
                    Mij = create_M_loc(i, j);
                    Al[i, j] = detD * Aij / 2.0;
                    bl[i] += Mij * detD * test.func(xy[nvtr[k][j]], lambda);
                }

            }
        }
        public void addelem(int i, int j, double a)
        {
            if (i == j) di[i] += a;
            else
        if (i < j)
            {
                int ind;
                for (ind = ig[j]; ind < ig[j + 1]; ind++) { if (jg[ind] == i) break; }
                ggu[ind] = ggu[ind] + a;
            }
            else
            {
                int ind;
                for (ind = ig[i]; ind < ig[i + 1]; ind++) { if (jg[ind] == j) break; }
                ggl[ind] = ggl[ind] + a;
            }
        }
        public void first_kraev()
        {
            for (int i = 0; i < kraev1.Count(); i++)
            {
                int q = kraev1[i];
                di[q] = 1E+42;
                b[q] = test.u_analitic(xy[q]) * di[q];
            }
        }
        public void Aglobal_prof()
        {
            for (int i = 0; i < m; i++)
            {

                Aloc(i);
                for (int j = 0; j < sizebasis; j++)
                {
                    for (int k = 0; k < sizebasis; k++)
                    {
                        addelem(nvtr[i][j], nvtr[i][k], Al[j, k]);
                    }
                    b[nvtr[i][j]] += bl[j];
                }
            }
        }
        public void genpor()
        {
            int k, ind1, ind2;
            this.input();
            for (int kol = 0; kol < m; kol++)
            {
                for (int i = 0; i < sizebasis; i++)
                {
                    k = nvtr[kol][i];
                    for (int j = i + 1; j < sizebasis; j++)
                    {
                        ind1 = k;
                        ind2 = nvtr[kol][j];
                        if (uzel[ind2].Count() == 0)
                        {
                            uzel[ind2].Add(ind1);
                        }
                        else
                        {
                            int fl = 0;
                            int h;
                            for (h = 0; h < uzel[ind2].Count(); h++)
                            {
                                if (ind1 < uzel[ind2][h]) { fl = 2; break; }
                                if (ind1 == uzel[ind2][h]) { fl = 1; break; }
                            }
                            if (fl == 0) uzel[ind2].Add(ind1);
                            if (fl == 2)
                            {
                                uzel[ind2].Insert(h, ind1);
                            }

                        }

                    }
                }
            }
            int kl = 0;
            ig.Add(0);
            for (int i = 0; i < n + 1; i++)
            {
                int s = uzel[i].Count();
                ig.Add(0);
                ig[i + 1] = s + ig[i];
            }
            kl = ig[n];
            ig.Capacity=kl;
            jg.Capacity = kl;
            ggl.Capacity = kl;
            ggu.Capacity = kl;
            for (int i = 0; i < kl; i++)
            {
                ggl.Add(0);
                ggu.Add(0);
            }
            int c = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < uzel[i].Count(); j++, c++)
                {
                    jg.Add(uzel[i][j]);
                }
            }
        }
        public int fun_mu(int i)
        {
            return ((i) % 2);
            // ((i-1)mod3)+1
        }
        public int fun_nu(int i)
        {
            return (Math.Abs((i) / 2) % 2);
            //Math.abs((i-1)/3)+1
        }
    }
}
