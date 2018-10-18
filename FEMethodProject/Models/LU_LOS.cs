using System;
using System.Collections.Generic;
using System.Text;


namespace Solver_SLAU
{
    class LU_LOS : ISolver
    {
        private double disc = 3;
        public double Discrepancy { get { return disc; } set { disc = value; } }//невязка
        private List<double> l=new List<double>();
        private List<double> u = new List<double>();
        private List<double> dl = new List<double>();
        private List<double> r = new List<double>();
        private List<double> p = new List<double>();
        private List<double> z = new List<double>();
        private List<double> res = new List<double>();
        public List<double> di;
        public List<double> gl;
        public List<double> gu;
        public List<double> b;
        public List<int> ig;
        public List<int> jg;
        public List<double> x;
        public int MAXITER;//максимальное количество итераций
        public double EPS;//точность
        public int current_iter;//текущие количество иттераций
     
        public void set(List<double> di, List<double> gl, List<double> gu, List<double> b, List<double> x, List<int> ig, List<int> jg, double EPS, int MAXITER)
        {
            this.di = di; this.gl = gl; this.gu = gu; this.ig = ig; this.jg = jg; this.b = b; this.x = x; this.MAXITER = MAXITER; this.EPS = EPS;
            this.l.Clear(); this.u.Clear(); this.dl.Clear(); r.Clear(); z.Clear(); p.Clear();
            l.Capacity = gl.Count; u.Capacity = gu.Count; dl.Capacity = di.Count;
            r.Capacity = z.Capacity = p.Capacity = res.Capacity = di.Count;
            for(int i=0;i<gl.Count;i++)
            {
                l.Add(0);
                u.Add(0);
            }
            for (int i = 0; i < di.Count; i++)
            {
                r.Add(0); z.Add(0); p.Add(0); res.Add(0);
                dl.Add(0);
            }
            lu();
        }
        public void set(List<double> di, List<double> gl, List<double> gu, List<double> b, List<int> ig, List<int> jg, double EPS, int MAXITER)
        {
            x = new List<double>();
            this.di = di; this.gl = gl; this.gu = gu; this.ig = ig; this.jg = jg; this.b = b; this.MAXITER = MAXITER; this.EPS = EPS;
            this.l.Clear(); this.u.Clear(); this.dl.Clear();this.x.Clear();
            l.Capacity = gl.Count; u.Capacity = gu.Count; dl.Capacity = di.Count;
            r.Capacity = z.Capacity = p.Capacity = res.Capacity=x.Capacity = di.Count;
            for (int i = 0; i < gl.Count; i++)
            {
                l.Add(0);
                u.Add(0);
            }
            for (int i = 0; i < di.Count; i++)
            {
                x.Add(0.1);
                r.Add(0); z.Add(0); p.Add(0); res.Add(0);
             
            }
            dl = di;
            lu();
        }
        public List<double> solve()
        {
            double norm_r = 0.0, norm_f = norm(b,b);
            r = mult(x);
            for (int i = 0; i < r.Count; i++)
            {
                r[i] = b[i] - r[i];
            }
            r = Lx(r);
            for (int i = 0; i < r.Count; i++)
            {
                z[i] = r[i];
                norm_r += r[i] * r[i];
            }
            z = Ux(z);
            p = mult(z);
            p = Lx(p);
            norm_r = Math.Sqrt(norm_r) / norm_f;
            for (current_iter = 1; current_iter < MAXITER && norm_r > EPS; current_iter++)
            {
                double alpha, beta, norm_p;
                norm_p = scalar(p, p);
                alpha = scalar(p, r) / norm_p;
                norm_r = 0.0;
                for (int i = 0; i < r.Count; i++)
                {
                    x[i] += alpha * z[i];
                    r[i] -= alpha * p[i];
                    b[i] = r[i];
                    norm_r += r[i] * r[i];
                }
                b = Ux(b);
                res = mult(b);
                res = Lx(res);
                beta = -scalar(p, res) / norm_p;
                for (int i = 0; i < z.Count; i++)
                {
                    z[i] = b[i] + beta * z[i];
                    p[i] = res[i] + beta * p[i];
                }
                norm_r = Math.Sqrt(norm_r) / norm_f;
            }
            Discrepancy = norm_r;
            return x;
        }
        private List<double> mult(List<double> y)
        {
            List<double> res= new List<double>();
            res.Capacity = di.Count;
            int gi, gi_1;
            for (int i = 0; i < di.Count; i++)
            {
                gi = ig[i]; gi_1 = ig[i + 1];
                res.Add(di[i] * y[i]);
                for (int j = gi; j < gi_1; j++)
                {
                    int stol = jg[j];
                    res[i] += gl[j] * y[stol];
                    res[stol] += gu[j] * y[i];
                }
            }
            return res;
        }
        private double norm(List<double> x, List<double> y)
        {
            return Math.Sqrt(scalar(x, y));
        }
        private double scalar(List<double> x, List<double> y)
        {
            double s = 0;
            for (int i = 0; i < x.Count; i++)
                s += x[i] * y[i];
            return s;
        }
        private List<double> Lx(List<double> y)
        {
            for (int k = 0; k < y.Count; k++)
            {
                double s = 0.0;
                for (int i = ig[k]; i < ig[k + 1]; i++)
                {
                    int j = jg[i];
                    s += l[i] * y[j];
                }
                y[k] = (y[k] - s) / dl[k];
            }
            return y;
        }
        private List<double> Ux(List<double> y)
        {
            for (int k = y.Count - 1; k >= 0; k--)
            {
                double yk = y[k];
                for (int j = ig[k]; j < ig[k + 1]; j++)
                {
                    int i = jg[j];
                    y[i] -= u[j] * yk;
                }
            }
            return y;
        }
        private void lu()
        {
            for (int i = 0; i < di.Count; i++)
            {
                int ig0, ig1;
                double sd = 0.0;
                ig0 = ig[i];
                ig1 = ig[i + 1];
                int j = jg[ig0];
                for (int k = ig0; k < ig1; k++)
                {
                    int jg0, jg1, jk0;
                    jk0 = jg[k];
                    jg0 = ig[jk0];
                    jg1 = ig[jk0 + 1];
                    double sal = 0.0, sul = 0.0;
                    for (int ki = ig0; ki < k; ki++)
                    {
                        for (int kj = jg0; kj < jg1; kj++)
                        {
                            if (jg[ki] == jg[kj])
                            {
                                sal += l[ki] * u[kj];
                                sul += l[kj] * u[ki];
                            }
                        }
                    }
                    l[k] = l[k] - sal;
                    u[k] = (u[k] - sul) / dl[jk0];
                    sd += u[k] * l[k];
                }
                dl[i] = dl[i] - sd;
            }
        }
    }
}
