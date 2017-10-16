using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using MathNet;
using MathNetNumLin = MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using SynchrophasorAnalytics.Matrices;

using Beta_Application_CTPT_LineZ.AlgorithmRealization.alglib;

namespace Beta_Application_CTPT_LineZ.AlgorithmRealization
{
    public class LeastSquaresSolver
    {
        #region [ Private Members ]
        static MathNetNumLin.Matrix<double> m_A;
        static MathNetNumLin.Vector<double> m_xInitial;
        static MathNetNumLin.Vector<double> m_xLowerBound;
        static MathNetNumLin.Vector<double> m_xUpperBound;
        static MathNetNumLin.Vector<double> m_b;
        static MathNetNumLin.Vector<double> m_results;

        static int RowNumber;
        static int ColumnNumber;
        #endregion

        #region [ Properties ]
        public MathNetNumLin.Matrix<double> A
        {
            get
            {
                return m_A;
            }
            set
            {
                m_A = value;
            }
        }

        public MathNetNumLin.Vector<double> xInitial
        {
            get
            {
                return m_xInitial;
            }
            set
            {
                m_xInitial = value;
            }
        }

        public MathNetNumLin.Vector<double> xLowerBound
        {
            get
            {
                return m_xLowerBound;
            }
            set
            {
                m_xLowerBound = value;
            }
        }

        public MathNetNumLin.Vector<double> xUpperBound
        {
            get
            {
                return m_xUpperBound;
            }
            set
            {
                m_xUpperBound = value;
            }
        }
        public MathNetNumLin.Vector<double> b
        {
            get
            {
                return m_b;
            }
            set
            {
                m_b = value;
            }
        }

        public MathNetNumLin.Vector<double> Results
        {
            get
            {
                return m_results;
            }
            set
            {
                m_results = value;
            }
        }
        #endregion

        #region [ Constructor ]
        public LeastSquaresSolver(int Row_number, int Column_number)
        {
            m_A = MathNetNumLin.Matrix<double>.Build.Dense(Row_number, Column_number);
            m_xInitial = MathNetNumLin.Vector<double>.Build.Dense(Column_number, 1);
            m_xLowerBound = MathNetNumLin.Vector<double>.Build.Dense(Column_number, 1);
            m_xUpperBound = MathNetNumLin.Vector<double>.Build.Dense(Column_number, 1);
            m_b = MathNetNumLin.Vector<double>.Build.Dense(Row_number, 1);
            m_results = MathNetNumLin.Vector<double>.Build.Dense(Column_number, 1);

            RowNumber = m_A.RowCount;
            ColumnNumber = m_A.ColumnCount;
        }
        #endregion

        #region [ Methods ]
        public void LeastSqauresInequalityConstrainedEstimation()
        {
            double[] x = new double[ColumnNumber];
            for (int idx0 = 0; idx0 < ColumnNumber; idx0++)
            {
                x[idx0] = m_xInitial[idx0];
            }
            double[] bndl = new double[ColumnNumber];
            for (int idx1 = 0; idx1 < ColumnNumber; idx1++)
            {
                bndl[idx1] = m_xLowerBound[idx1];
            }
            double[] bndu = new double[ColumnNumber];
            for (int idx2 = 0; idx2 < ColumnNumber; idx2++)
            {
                bndu[idx2] = m_xUpperBound[idx2];
            }
            alglib.alglib.minbleicstate state;
            alglib.alglib.minbleicreport rep;

            double epsg = 0.000000001;
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;

            try
            {
                alglib.alglib.minbleiccreate(x, out state);
                alglib.alglib.minbleicsetbc(state, bndl, bndu);
                alglib.alglib.minbleicsetcond(state, epsg, epsf, epsx, maxits);
                alglib.alglib.minbleicoptimize(state, objectiveFuncGradientLSE, null, null);
                alglib.alglib.minbleicresults(state, out x, out rep);

                for (int idx3 = 0; idx3 < ColumnNumber; idx3++)
                {
                    m_results[idx3] = x[idx3];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static void objectiveFuncGradientLSE(double[] x, ref double ObjFunc, double[] grad, object obj)
        {
            if (x.Count() != ColumnNumber)
            {
                return;
            }
            
            ObjFunc = 0;

            for (int idx0 = 0; idx0 < RowNumber; idx0++)
            {
                // Calculates objective function value
                double[] current_A_row = new double[ColumnNumber];
                double LHS = 0;
                for (int idx1 = 0; idx1 < ColumnNumber; idx1++)
                {
                    current_A_row[idx1] = m_A[idx0, idx1];

                    LHS = LHS + m_A[idx0, idx1] * x[idx1];
                }
                double RHS = m_b[idx0];
                double Difference = LHS - RHS;
                ObjFunc = ObjFunc + Math.Pow(Difference, 2);

                // Calculate derivatives of the objective function
                for (int idx2 = 0; idx2 < ColumnNumber; idx2++)
                {
                    grad[idx2] = grad[idx2] + 2 * current_A_row[idx2] * Difference;
                }
            }            

            return;
        }
        
        #endregion
    }
}
