using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Data;
using System.Windows.Forms;

using MathNet;
using MathNetNumLin = MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using Beta_Application_CTPT_LineZ.Model;
using Beta_Application_CTPT_LineZ.MeasurementsDataSet;
using SynchrophasorAnalytics.Matrices;

namespace Beta_Application_CTPT_LineZ.AlgorithmRealization
{
    public class LineImpedanceEstimation
    {
        #region [ Private Members ]
        private VIDataSet m_currentSystem;
        private int[,] m_lineVisited;                            // Record the lines have been calibrated or not: 1/0

        private struct LinesKVKIRecord
        {
            public int Line_ID;
            public int From_bus_num;
            public Complex KV1;
            public Complex KI1;
        }
        private LinesKVKIRecord[] m_KVKILines;                          // Record the KVs and KIs for all the lines as a reference bus for single line calibration, start from 0        

        private int[] m_NonCalibratableComponentsNum;            // connected components that can not be calibrated due to topology causes
        //private Complex[,] m_Z;                                  // True impedance matrix within estimation procedure
        private List<Complex> m_V1;                              // All Voltage measurements of bus 1
        private List<Complex> m_V2;                              // All Voltage measurements of bus 2
        private List<Complex> m_I1;                              // All Current measurements of bus 1
        private List<Complex> m_I2;                              // All Current measurements of bus 2
        private Complex[,] m_Zhat;
        private Complex[,] m_K;                                  // Record calibration results
        private DataTable m_ResultsTable;                        // Demonstration table for the Ks
        #endregion

        #region [ Public Properties ]
        public VIDataSet CurrentSystem
        {
            get
            {
                return m_currentSystem;
            }

            set
            {
                m_currentSystem = value;
            }
        }

        public Complex[,] K
        {
            get
            {
                return m_K;
            }

            set
            {
                m_K = value;
            }
        }
        #endregion

        #region [ Constructors ]
        public LineImpedanceEstimation(NetworkTopology ConfiguredNetwork)
        {
            m_currentSystem = new VIDataSet(ConfiguredNetwork);
            m_lineVisited = new int[m_currentSystem.Network.LineNum, 2];
            m_KVKILines = new LinesKVKIRecord[m_currentSystem.Network.LineNum]; //[line_ID, from_bus_num, KV1, KI1]
            m_NonCalibratableComponentsNum = new int[50];
            //m_Z = new Complex[2, 2];
            m_V1 = new List<Complex>();
            m_I1 = new List<Complex>();
            m_V2 = new List<Complex>();
            m_I2 = new List<Complex>();
            m_Zhat = new Complex[2, 2];
            m_K = new Complex[m_currentSystem.Network.BusNum, 9];
            m_ResultsTable = new DataTable();
        }
        #endregion

        #region [ Methods ]
        /* Calibrate CTs and PTs throughout the system starting from reference bus with known KV*/
        public void LineImpedanceEstimationAlgorithm(TextBox textBoxMessages)
        {
            string reference_bus_number = m_currentSystem.Network.RefBusNumber.ToString();
            string reference_line_number = m_currentSystem.Network.RefLineNumber.ToString();
            Complex KV1_initial = 1;
            Complex KI1_initial = 1;
            // Initialize the line calibration status record
            m_lineVisited = new int[m_currentSystem.Network.LineNum, 2];
            for (int l = 0; l < m_currentSystem.Network.LineNum; l++)
            {
                m_lineVisited[l, 0] = m_currentSystem.Network.LineBusInfo[l, 0];
            }

            // Set the reference line and bus
            int reference_line_num = Convert.ToInt32(reference_line_number);
            int reference_bus_num = Convert.ToInt32(reference_bus_number);
            m_KVKILines[0].Line_ID = reference_line_num;
            m_KVKILines[0].From_bus_num = reference_bus_num;
            m_KVKILines[0].KV1 = KV1_initial;
            m_KVKILines[0].KI1 = KI1_initial;

            // Search all the connected components
            int reference_bus_number_int = Convert.ToInt32(reference_bus_number);
            for (int i = 0; i < m_currentSystem.Network.ComponentsNum; i++)
            {
                //The reference bus should be calibrated firstly, or the whole component can NOT be calibrated
                if (m_currentSystem.Network.Components[i, 0, 0] != reference_bus_number_int)
                {
                    m_NonCalibratableComponentsNum[i] = -1;
                    string msg0 = "!!! Component " + (i + 1).ToString() + " can NOT be Calibrated due to LACK of reference.\n";
                    textBoxMessages.AppendLine(msg0);
                    continue;
                }

                //Console.WriteLine("Component {0}: ", i + 1);

                //for (int k = 0; k < m_currentSystem.Network.BusNum; k++)
                //{
                //    if (m_currentSystem.Network.Components[i, k, 0] != 0)
                //    {
                //        Console.Write(m_currentSystem.Network.Components[i, k, 0].ToString());
                //        Console.Write("    ");
                //        Console.Write(m_currentSystem.Network.Components[i, k, 1].ToString());
                //        Console.WriteLine();
                //    }
                //}

                //search all the lines in the component
                int j = 1;
                while (m_currentSystem.Network.Components[i, j, 0] != 0)
                {
                    string current_line_number = m_currentSystem.Network.Components[i, j, 2].ToString();

                    //set up reference Ks
                    bool line_found_flag = false;
                    int line_ID_index = 0;
                    for (int idx = 0; idx < m_currentSystem.Network.LineNum; idx++)
                    {
                        if (current_line_number == m_KVKILines[idx].Line_ID.ToString())
                        {
                            line_found_flag = true;
                            line_ID_index = idx;
                            break;
                        }
                    }
                    Complex KV1_reference = 0;
                    Complex KI1_reference = 0;
                    int From_bus_number = 0;
                    if (line_found_flag)
                    {
                        KV1_reference = m_KVKILines[line_ID_index].KV1;
                        KI1_reference = m_KVKILines[line_ID_index].KI1;
                        From_bus_number = m_KVKILines[line_ID_index].From_bus_num;
                    }
                    else
                    {
                        textBoxMessages.AppendLine("!!! No reference !!!");
                        break;
                    }

                    //start calibration process
                    if (KV1_reference != 0)
                    {
                        SingleLineImpedanceEstimation(current_line_number, From_bus_number, KV1_reference, KI1_reference, textBoxMessages);
                        //from bus number could be m_currentSystem.Components[i, j, 0], might not always be correct
                    }
                    else
                    {
                        string msg1 = "!!! Line " + current_line_number.ToString() + " from bus " +
                            m_currentSystem.Network.Components[i, j, 0].ToString() + " to bus " +
                            m_currentSystem.Network.Components[i, j, 1].ToString() + " can NOT be calibrated due to topology causes.";
                        textBoxMessages.AppendLine(msg1);
                    }

                    j += 1;
                }

                //EstimationResultsDemonstration();
            }

        }

        /* Get the Voltages and Currents of the decided line*/
        public int GetLineVI(string LineID, int FromBusNumber)
        {

            int LineID_int = Convert.ToInt32(LineID);   // for the convinience of Console input

            // Find from_bus and to_bus
            int CurrentFromBus = 0;
            int CurrentToBus = 0;
            for (int i = 0; i < m_currentSystem.Network.LineNum; i++)
            {
                if (m_currentSystem.Network.LineBusInfo[i, 0] == LineID_int)
                {
                    if (m_currentSystem.Network.LineBusInfo[i, 1] == FromBusNumber)
                    {
                        CurrentFromBus = m_currentSystem.Network.LineBusInfo[i, 1];
                        CurrentToBus = m_currentSystem.Network.LineBusInfo[i, 2];
                        break;
                    }
                    else if (m_currentSystem.Network.LineBusInfo[i, 2] == FromBusNumber)
                    {
                        CurrentFromBus = m_currentSystem.Network.LineBusInfo[i, 2];
                        CurrentToBus = m_currentSystem.Network.LineBusInfo[i, 1];
                        break;
                    }
                    else
                    {                        
                        MessageBox.Show("The line info and bus info do NOT match!");
                        return 0;
                    }
                }
            }

            // Construct TKeys to find proper value in the dataset dictionary
            List<Complex> tempV1 = new List<Complex>();
            List<Complex> tempI1 = new List<Complex>();
            List<Complex> tempV2 = new List<Complex>();
            List<Complex> tempI2 = new List<Complex>();
            for (int j = 0; j < 1; j++)// 60; j++)
            {
                for (int k = 0; k < m_currentSystem.TimeSeriesFrames.Count(); k++)// m_currentSystem.FramesPerSecond; k++)
                {
                    long CurrentTimeStamp = k + 1;// 20160113070000 + j * 100 + (k + 1);

                    string CurrentTKey_frombus_VM = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentFromBus.ToString() + "T" + "PositiveSequenceVoltageMagnitude";
                    string CurrentTKey_frombus_VA = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentFromBus.ToString() + "T" + "PositiveSequenceVoltageAngle";
                    Complex CurrentV1 = new Complex();
                    try
                    {
                        double FromBusVM = m_currentSystem.VIDataDictionary[CurrentTKey_frombus_VM];
                        double FromBusVA = m_currentSystem.VIDataDictionary[CurrentTKey_frombus_VA];
                        if ((FromBusVM != -9999) && (FromBusVA != -9999))
                        {
                            CurrentV1 = Complex.FromPolarCoordinates(FromBusVM, FromBusVA * Math.PI / 180);// * 1000 / Math.Sqrt(3);
                            // may need Math.PI / 2 - 
                        }
                        else
                        {
                            CurrentV1 = -9999;
                        }
                    }
                    catch //(Exception exception)
                    {
                        CurrentV1 = -9999;
                    }
                    tempV1.Add(CurrentV1);

                    string CurrentTKey_frombus_IM = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentFromBus.ToString() + "T" + "PositiveSequenceCurrentMagnitude";
                    string CurrentTKey_frombus_IA = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentFromBus.ToString() + "T" + "PositiveSequenceCurrentAngle";
                    Complex CurrentI1 = new Complex();
                    try
                    {
                        double FromBusIM = m_currentSystem.VIDataDictionary[CurrentTKey_frombus_IM];
                        double FromBusIA = m_currentSystem.VIDataDictionary[CurrentTKey_frombus_IA];
                        if ((FromBusIM != -9999) && (FromBusIA != -9999))
                        {
                            CurrentI1 = Complex.FromPolarCoordinates(FromBusIM, FromBusIA * Math.PI / 180);
                        }
                        else
                        {
                            CurrentI1 = -9999;
                        }
                    }
                    catch
                    {
                        CurrentI1 = -9999;
                    }
                    tempI1.Add(CurrentI1);

                    string CurrentTKey_tobus_VM = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentToBus.ToString() + "T" + "PositiveSequenceVoltageMagnitude";
                    string CurrentTKey_tobus_VA = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentToBus.ToString() + "T" + "PositiveSequenceVoltageAngle";
                    Complex CurrentV2 = new Complex();
                    try
                    {
                        double ToBusVM = m_currentSystem.VIDataDictionary[CurrentTKey_tobus_VM];
                        double ToBusVA = m_currentSystem.VIDataDictionary[CurrentTKey_tobus_VA];
                        if ((ToBusVM != -9999) && (ToBusVA != -9999))
                        {
                            CurrentV2 = Complex.FromPolarCoordinates(ToBusVM, ToBusVA * Math.PI / 180);// * 1000 / Math.Sqrt(3);
                        }
                        else
                        {
                            CurrentV2 = -9999;
                        }
                    }
                    catch
                    {
                        CurrentV2 = -9999;
                    }
                    tempV2.Add(CurrentV2);

                    string CurrentTKey_tobus_IM = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentToBus.ToString() + "T" + "PositiveSequenceCurrentMagnitude";
                    string CurrentTKey_tobus_IA = CurrentTimeStamp.ToString() + "L" + LineID +
                        "B" + CurrentToBus.ToString() + "T" + "PositiveSequenceCurrentAngle";
                    Complex CurrentI2 = new Complex();
                    try
                    {
                        double ToBusIM = m_currentSystem.VIDataDictionary[CurrentTKey_tobus_IM];
                        double ToBusIA = m_currentSystem.VIDataDictionary[CurrentTKey_tobus_IA];
                        if ((ToBusIM != -9999) && (ToBusIA != -9999))
                        {
                            CurrentI2 = Complex.FromPolarCoordinates(ToBusIM, ToBusIA * Math.PI / 180);
                        }
                        else
                        {
                            CurrentI2 = -9999;
                        }
                    }
                    catch
                    {
                        CurrentI2 = -9999;
                    }
                    tempI2.Add(CurrentI2);
                }
            }
            m_V1 = tempV1;
            m_I1 = tempI1;
            m_V2 = tempV2;
            m_I2 = tempI2;

            return CurrentToBus;
        }

        public MathNetNumLin.Matrix<Complex> LeastSquareEstimation(MathNetNumLin.Matrix<Complex> Y, MathNetNumLin.Matrix<Complex> X)
        {
            MathNetNumLin.Matrix<Complex> Beta;

            MathNetNumLin.Matrix<Complex> A = X.ConjugateTranspose() * X;
            MathNetNumLin.Matrix<Complex> B = A.Inverse();

            Beta = (X.ConjugateTranspose() * X).Inverse() * X.ConjugateTranspose() * Y; // For complex LSE problems, note that there should be conjugate transpose            

            MathNetNumLin.Matrix<Complex> Error;
            Error = Y - X.Multiply(Beta);

            return Beta;
        }

        public MathNetNumLin.Matrix<Complex> LeastSquareEstimationInequalityConstrained(MathNetNumLin.Matrix<Complex> b, MathNetNumLin.Matrix<Complex> A)
        {
            int Row_number = A.RowCount;
            int Column_number = A.ColumnCount;

            MathNetNumLin.Matrix<double> A_realpart = MathNetNumLin.Matrix<double>.Build.Dense(Row_number, Column_number);
            MathNetNumLin.Matrix<double> A_imagpart = MathNetNumLin.Matrix<double>.Build.Dense(Row_number, Column_number);

            MathNetNumLin.Vector<double> b_realpart = MathNetNumLin.Vector<double>.Build.Dense(Row_number);
            MathNetNumLin.Vector<double> b_imagpart = MathNetNumLin.Vector<double>.Build.Dense(Row_number);

            for (int idx0 = 0; idx0 < Row_number; idx0++)
            {
                for (int idx1 = 0; idx1 < Column_number; idx1++)
                {
                    A_realpart[idx0, idx1] = A[idx0, idx1].Real;
                    A_imagpart[idx0, idx1] = A[idx0, idx1].Imaginary;
                }
                b_realpart[idx0] = b[idx0, 0].Real;
                b_imagpart[idx0] = b[idx0, 0].Imaginary;
            }

            MathNetNumLin.Vector<double> xInitial_realpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);
            MathNetNumLin.Vector<double> xInitial_imagpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);

            MathNetNumLin.Vector<double> xLowerBound_realpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);
            MathNetNumLin.Vector<double> xLowerBound_imagpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);

            MathNetNumLin.Vector<double> xUpperBound_realpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);
            MathNetNumLin.Vector<double> xUpperBound_imagpart = MathNetNumLin.Vector<double>.Build.Dense(Column_number);

            for (int idx2 = 0; idx2 < Column_number; idx2++)
            {
                xInitial_realpart[idx2] = 1;
                xInitial_imagpart[idx2] = 0;

                xLowerBound_realpart[idx2] = 0.9452;
                xLowerBound_imagpart[idx2] = -0.1005;

                xUpperBound_realpart[idx2] = 1.0526;
                xUpperBound_imagpart[idx2] = 0.1005;
            }

            MathNetNumLin.Matrix<Complex> Beta = MathNetNumLin.Matrix<Complex>.Build.Random(Column_number, 1);
            MathNetNumLin.Vector<double> RealBeta = MathNetNumLin.Vector<double>.Build.Random(Column_number);

            LeastSquaresSolver RealPartSolver = new LeastSquaresSolver(Row_number, Column_number);
            RealPartSolver.A = A_realpart;
            RealPartSolver.b = b_realpart;
            RealPartSolver.xInitial = xInitial_realpart;
            RealPartSolver.xLowerBound = xLowerBound_realpart;
            RealPartSolver.xUpperBound = xUpperBound_realpart;
            RealPartSolver.LeastSqauresInequalityConstrainedEstimation();

            for (int idx3 = 0; idx3 < RealPartSolver.Results.Count; idx3++)
            {
                RealBeta[idx3] = RealPartSolver.Results[idx3];
            }

            LeastSquaresSolver ImagPartSolver = new LeastSquaresSolver(Row_number, Column_number);
            ImagPartSolver.A = A_imagpart;
            ImagPartSolver.b = b_imagpart;
            ImagPartSolver.xInitial = xInitial_imagpart;
            ImagPartSolver.xLowerBound = xLowerBound_imagpart;
            ImagPartSolver.xUpperBound = xUpperBound_imagpart;
            ImagPartSolver.LeastSqauresInequalityConstrainedEstimation();

            for (int idx4 = 0; idx4 < RealPartSolver.Results.Count; idx4++)
            {
                Beta[idx4, 0] = new Complex(RealBeta[idx4], ImagPartSolver.Results[idx4]);
            }

            return Beta;
        }

        /* Conduct PI section Impedance calibration of single line*/
        public int SingleLineImpedanceEstimation(string LineID, int FromBusNumber, Complex KV1, Complex KI1, TextBox textBoxMessages) //ALso need Complex KV1, Complex[,] Z
        {
            int CurrentToBus = 0;
            CurrentToBus = GetLineVI(LineID, FromBusNumber);

            //if (LineID == "2")
            //{ int a = 1; }

            Complex KV2 = new Complex();
            Complex KI2 = new Complex();
            Complex Z = new Complex();
            Complex y = new Complex();

            //Find the sample sets 30 samples per second, 60 seconds in all => 30 sets of samples, 60 samples per set, rule out -9999
            int i = 0;//set number
            int feasible_flag = 0;//estimation feasible indicator, shows missing data quantity, if too large then the estimation is not feasible

            Complex[] Zhat11 = new Complex[30];
            Complex[] Zhat12 = new Complex[30];
            Complex[] Zhat21 = new Complex[30];
            Complex[] Zhat22 = new Complex[30];

            int SecondsNumber = (int)Math.Floor((double)(m_V1.Count() / 30));

            while (i < 30)
            {
                //get samples
                Complex[,] V_sample_temp = new Complex[SecondsNumber, 2];
                Complex[,] I_sample_temp = new Complex[SecondsNumber, 2];

                int current_row = 0;

                for (int j = 0; j < SecondsNumber; j++)
                {
                    current_row = j * 30 + i;
                    if ((m_V1[current_row] != -9999) && (m_I1[current_row] != -9999) && (m_V2[current_row] != -9999) && (m_I2[current_row] != -9999))
                    {
                        V_sample_temp[j, 0] = m_V1[current_row];
                        I_sample_temp[j, 0] = m_I1[current_row];
                        V_sample_temp[j, 1] = m_V2[current_row];
                        I_sample_temp[j, 1] = m_I2[current_row];
                    }
                    else
                    {
                        V_sample_temp[j, 0] = -9999;
                        I_sample_temp[j, 0] = -9999;
                        V_sample_temp[j, 1] = -9999;
                        I_sample_temp[j, 1] = -9999;
                    }
                }

                //transfer into matrix mode
                Complex initial = 0;
                DenseMatrix Vhat = DenseMatrix.Create(2, 1, initial);
                DenseMatrix Ihat = DenseMatrix.Create(2, 1, initial);

                for (int k = 0; k < SecondsNumber; k++)
                {
                    if ((V_sample_temp[k, 0] != -9999) && (V_sample_temp[k, 0] != null))
                    {
                        Complex[,] V_temp_array = new Complex[2, 1];
                        V_temp_array[0, 0] = V_sample_temp[k, 0];
                        V_temp_array[1, 0] = V_sample_temp[k, 1];
                        DenseMatrix V_temp = DenseMatrix.OfArray(V_temp_array);

                        if ((Vhat != null) && (V_temp != null))
                        {
                            if (Vhat[0, 0] == 0)
                            {
                                Vhat = V_temp;
                            }
                            else
                            {
                                Vhat = MatrixCalculationExtensions.HorizontallyConcatenate(Vhat, V_temp);
                            }
                        }


                        Complex[,] I_temp_array = new Complex[2, 1];
                        I_temp_array[0, 0] = I_sample_temp[k, 0];
                        I_temp_array[1, 0] = I_sample_temp[k, 1];
                        DenseMatrix I_temp = DenseMatrix.OfArray(I_temp_array);

                        if ((Ihat != null) && (I_temp != null))
                        {
                            if (Ihat[0, 0] == 0)
                            {
                                Ihat = I_temp;
                            }
                            else
                            {
                                Ihat = MatrixCalculationExtensions.HorizontallyConcatenate(Ihat, I_temp);
                            }
                        }

                    }
                }

                //check sample quantity, enough to do estimation or not
                if (Vhat.ColumnCount < 20)
                {
                    feasible_flag += 1;
                    i += 1;
                    continue;
                }
                else
                {
                    MathNetNumLin.Matrix<Complex> Vhat_T = Vhat.Transpose();
                    MathNetNumLin.Matrix<Complex> Ihat_T = Ihat.Transpose();

                    //estimate Zhat
                    MathNetNumLin.Matrix<Complex> Zhat_temp = null;

                    Zhat_temp = LeastSquareEstimation(Vhat_T, Ihat_T);// Ihat_T.Transpose().Multiply(Ihat_T).Inverse().Multiply(Ihat_T.Transpose()).Multiply(Vhat_T);
                    Zhat11[i] = Zhat_temp[0, 0];
                    Zhat12[i] = Zhat_temp[0, 1];
                    Zhat21[i] = Zhat_temp[1, 0];
                    Zhat22[i] = Zhat_temp[1, 1];

                    //Zhat12[i] = Zhat_temp[1, 0];
                    //Zhat21[i] = Zhat_temp[0, 1];
                }

                i += 1;
            }

            // Take average of Zhat to eliminate PMU error, if there is enough estimation results
            Complex from_bus_number = FromBusNumber;
            Complex to_bus_number = CurrentToBus;
            int from_bus_number_int = FromBusNumber;
            int to_bus_number_int = CurrentToBus;

            double BaseZ = m_currentSystem.Network.BaseKV * m_currentSystem.Network.BaseKV / m_currentSystem.Network.BaseMVA;

            if (feasible_flag < 15)
            {
                Complex W = new Complex();
                Complex[] KV2_hat = new Complex[30];
                Complex[] KI2_hat = new Complex[30];
                Complex[] Z_hat_pu = new Complex[30];
                Complex[] y_hat_pu = new Complex[30];
                Complex KV2_hat_sum = new Complex();
                Complex KI2_hat_sum = new Complex();
                Complex Z_hat_pu_sum = new Complex();
                Complex y_hat_pu_sum = new Complex();

                for (int l = 0; l < 30; l++)
                {
                    if ((Zhat11[l] != null) || (Zhat12[l] != null) || (Zhat21[l] != null) || (Zhat22[l] != null))
                    {
                        Complex Zhat11_temp = Zhat11[l];
                        Complex Zhat12_temp = Zhat12[l];
                        Complex Zhat21_temp = Zhat21[l];
                        Complex Zhat22_temp = Zhat22[l];

                        W = (Zhat11_temp * Zhat22_temp) / (Zhat12_temp * Zhat21_temp);
                        W = Complex.Sqrt(W);
                        if (W.Imaginary < 0)
                        {
                            W = -1 * W;
                        }

                        KV2_hat[l] = 1 / W * Zhat11_temp / Zhat12_temp * KV1;
                        KI2_hat[l] = W * Zhat21_temp / Zhat11_temp * KI1;

                        if (KV2_hat[l].Real < 0)
                        {
                            KV2_hat[l] = KV2_hat[l] * (-1);
                        }
                        if (KI2_hat[l].Real < 0)
                        {
                            KI2_hat[l] = KI2_hat[l] * (-1);
                        }
                        y_hat_pu[l] = BaseZ * Complex.Sqrt((KI2_hat[l] * (W - 1)) / (KV2_hat[l] * (W + 1) * (Zhat11_temp * Zhat22_temp - Zhat12_temp * Zhat21_temp)) * KI1 / KV1);
                        //y_hat_pu[l] = Complex.Sqrt((KI2_hat[l] * (W - 1)) / (KV2_hat[l] * (W + 1) * (Zhat11_temp * Zhat22_temp - Zhat12_temp * Zhat21_temp)) * KI1 / KV1);
                                                
                        if (y_hat_pu[l].Imaginary < 0)
                        {
                            y_hat_pu[l] = Complex.Conjugate(y_hat_pu[l]);
                        }

                        Z_hat_pu[l] = (W - 1) / y_hat_pu[l];

                        if (Z_hat_pu[l].Real < 0)
                        {
                            Z_hat_pu[l] = Complex.Conjugate(-1 * Z_hat_pu[l]);
                        }

                        if (Z_hat_pu[l].Imaginary < 0)
                        {
                            Z_hat_pu[l] = Complex.Conjugate(Z_hat_pu[l]);
                        }

                    }

                    KV2_hat_sum = KV2_hat_sum + KV2_hat[l];
                    KI2_hat_sum = KI2_hat_sum + KI2_hat[l];
                    Z_hat_pu_sum = Z_hat_pu_sum + Z_hat_pu[l];
                    y_hat_pu_sum = y_hat_pu_sum + y_hat_pu[l];


                }

                KV2 = KV2_hat_sum / (30 - feasible_flag);
                KI2 = KI2_hat_sum / (30 - feasible_flag);
                Z = Z_hat_pu_sum / (30 - feasible_flag);// * BaseZ;
                y = y_hat_pu_sum / (30 - feasible_flag);// / BaseZ;

                //adding the computation results to K matrix and record KVs for both from_bus and to_bus for future calibrations
                int m = 0;
                while (m_K[m, 0] != 0)
                {
                    m += 1;
                }

                int Line_ID_int = Convert.ToInt32(LineID);
                m_K[m, 0] = Line_ID_int;
                m_K[m, 1] = from_bus_number;
                m_K[m, 2] = KV1;
                m_K[m, 3] = KI1;
                m_K[m, 4] = to_bus_number;
                m_K[m, 5] = KV2;
                m_K[m, 6] = KI2;
                m_K[m, 7] = Z;
                m_K[m, 8] = y.Imaginary;

                //update the propagation references
                int[] injections_set = FindInjections(to_bus_number_int);
                List<int> inaccurate_line_set = new List<int>(); //List to save all inaccurate lines
                for (int idx1 = 0; idx1 < injections_set.Count(); idx1++)
                {
                    if (injections_set[idx1] != Line_ID_int)
                    {
                        inaccurate_line_set.Add(injections_set[idx1]);
                    }
                }
                //Accurate Voltage Propagation
                MathNetNumLin.Matrix<Complex> KV_injections_set = KVPropagation(inaccurate_line_set, Line_ID_int, to_bus_number_int, KV2);
                //Accurate Current Propagation
                MathNetNumLin.Matrix<Complex> KI_injections_set = KIPropagation(inaccurate_line_set, Line_ID_int, to_bus_number_int, KI2);

                int pointer = 0;
                for (int idx1 = 0; idx1 < m_currentSystem.Network.LineNum; idx1++)
                {
                    if (m_KVKILines[idx1].Line_ID == 0)
                    {
                        pointer = idx1;
                        break;
                    }
                }
                for (int idx2 = 0; idx2 < KV_injections_set.RowCount; idx2++)
                {
                    if ((pointer + idx2) < m_currentSystem.Network.LineNum)
                    {
                        m_KVKILines[pointer + idx2].Line_ID = inaccurate_line_set[idx2];
                        m_KVKILines[pointer + idx2].From_bus_num = to_bus_number_int;
                        m_KVKILines[pointer + idx2].KV1 = KV_injections_set[idx2, 0];
                        m_KVKILines[pointer + idx2].KI1 = KI_injections_set[idx2, 0];
                    }
                }

                //Set visited line to be 1
                for (int n = 0; n < m_currentSystem.Network.LineNum; n++)
                {
                    if (Line_ID_int == m_lineVisited[n, 0])
                    {
                        m_lineVisited[n, 1] = 1;
                        break;
                    }
                }

                string msg0 = "### Line " + LineID.ToString() + " from bus " +
                            m_currentSystem.Network.BusOriginalLibrary[from_bus_number_int-1].ToString() + " to bus " +
                            m_currentSystem.Network.BusOriginalLibrary[to_bus_number_int-1].ToString() + " has been estimated.\n";
                textBoxMessages.AppendLine(msg0);
            }
            else
            {
                string msg1 = "!!! Line " + LineID.ToString() + " from bus " +
                            m_currentSystem.Network.BusOriginalLibrary[from_bus_number_int-1].ToString() + " to bus " +
                            m_currentSystem.Network.BusOriginalLibrary[to_bus_number_int-1].ToString() + " can NOT be calibrated due to limited data.\n";
                textBoxMessages.AppendLine(msg1);
            }

            return 0;
        }

        /* Find all the injections based on the current line number and from bus number*/
        public int[] FindInjections(int From_bus_num)
        {
            int[] injection_line_set = new int[m_currentSystem.Network.LineNum];
            int pointer = 0;
            for (int idx = 0; idx < m_currentSystem.Network.LineNum; idx++)
            {
                if (m_currentSystem.Network.LineBusInfo[idx, 1] == From_bus_num || m_currentSystem.Network.LineBusInfo[idx, 2] == From_bus_num)
                {
                    injection_line_set[pointer] = m_currentSystem.Network.LineBusInfo[idx, 0];
                    pointer += 1;
                }
            }
            return injection_line_set;
        }

        /* Propagate the accuracy of the voltage measurements*/
        public MathNetNumLin.Matrix<Complex> KVPropagation(List<int> inaccurate_line_set, int Accurate_line_num, int to_bus_number_int, Complex KV_accurate)
        {
            // Colloect accurate line V data
            GetLineVI(Accurate_line_num.ToString(), to_bus_number_int);
            Complex initial = 0;
            DenseMatrix accurate_line_V = DenseMatrix.Create(m_V1.Count(), 1, initial);
            //Matrix<Complex> accurate_line_V = null;
            for (int idx2 = 0; idx2 < m_V1.Count(); idx2++)
            {
                accurate_line_V[idx2, 0] = m_V1[idx2] * KV_accurate;
            }

            // Colloect inaccurate lines V data
            int inaccurate_line_num = 0;
            for (int idx7 = 0; idx7 < inaccurate_line_set.Count(); idx7++)
            {
                if (inaccurate_line_set[idx7] == 0)
                {
                    inaccurate_line_num = idx7;
                    break;
                }
            }

            DenseMatrix inaccurate_line_V = DenseMatrix.Create(m_V1.Count(), inaccurate_line_num, initial);
            //Matrix<Complex> inaccurate_line_V = null;
            for (int idx3 = 0; idx3 < inaccurate_line_num; idx3++)
            {
                GetLineVI(inaccurate_line_set[idx3].ToString(), to_bus_number_int);
                for (int idx4 = 0; idx4 < m_V1.Count(); idx4++)
                {
                    inaccurate_line_V[idx4, idx3] = m_V1[idx4];
                }
            }

            // Propagate voltage accuracy
            MathNetNumLin.Matrix<Complex> KV_injection_set = MathNetNumLin.Matrix<Complex>.Build.Random(inaccurate_line_num, 1);
            for (int idx5 = 0; idx5 < inaccurate_line_num; idx5++)
            {
                Complex[] temp_KV = new Complex[inaccurate_line_V.RowCount];
                Complex temp_KV_sum = 0;
                for (int idx6 = 0; idx6 < inaccurate_line_V.RowCount; idx6++)
                {
                    temp_KV[idx6] = accurate_line_V[idx6, 0] / inaccurate_line_V[idx6, idx5];
                    temp_KV_sum = temp_KV_sum + temp_KV[idx6];
                }
                KV_injection_set[idx5, 0] = temp_KV_sum / inaccurate_line_V.RowCount;
            }
            return KV_injection_set;
        }

        /* Propagate the accuracy of the current measurements*/
        public MathNetNumLin.Matrix<Complex> KIPropagation(List<int> inaccurate_line_set, int Accurate_line_num, int to_bus_number_int, Complex KI_accurate)
        {
            // Collect accurate line I data
            GetLineVI(Accurate_line_num.ToString(), to_bus_number_int);
            Complex initial = 0;
            DenseMatrix accurate_line_I = DenseMatrix.Create(m_I1.Count(), 1, initial);
            for (int idx2 = 0; idx2 < m_I1.Count(); idx2++)
            {
                accurate_line_I[idx2, 0] = m_I1[idx2] * KI_accurate;
            }

            // Collect inaccurate lines I data
            int inaccurate_line_num = 0;
            for (int idx7 = 0; idx7 < m_I1.Count(); idx7++)
            {
                if (inaccurate_line_set[idx7] == 0)
                {
                    inaccurate_line_num = idx7;
                    break;
                }
            }
            DenseMatrix inaccurate_line_I = DenseMatrix.Create(m_I1.Count(), inaccurate_line_num, initial);
            for (int idx3 = 0; idx3 < inaccurate_line_num; idx3++)
            {
                GetLineVI(inaccurate_line_set[idx3].ToString(), to_bus_number_int);
                for (int idx4 = 0; idx4 < m_V1.Count(); idx4++)
                {
                    inaccurate_line_I[idx4, idx3] = m_I1[idx4];
                }
            }

            // Propagate current accuracy
            MathNetNumLin.Matrix<Complex> KI_injection_set = MathNetNumLin.Matrix<Complex>.Build.Random(inaccurate_line_num, 1);

            MathNetNumLin.Matrix<Complex> accurate_line_I_matrix = accurate_line_I;
            MathNetNumLin.Matrix<Complex> inaccurate_line_I_matrix = inaccurate_line_I;

            MathNetNumLin.Matrix<Complex> I_right_side = accurate_line_I_matrix * (-1);
            MathNetNumLin.Matrix<Complex> I_left_side = inaccurate_line_I_matrix;

            KI_injection_set = LeastSquareEstimationInequalityConstrained(I_right_side, I_left_side);

            return KI_injection_set;
        }

        /* Use table to demonstrate results*/
        public void EstimationResultsDemonstration()
        {
            // create row names by columns
            DataColumn tempColumn;
            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "Line_ID";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "From_Bus_Num";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "KV1";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "KI1";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "To_Bus_Num";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "KV2";
            m_ResultsTable.Columns.Add(tempColumn);

            tempColumn = new DataColumn();
            tempColumn.DataType = System.Type.GetType("System.String");
            tempColumn.ColumnName = "KI2";
            m_ResultsTable.Columns.Add(tempColumn);

            int idx = 0;
            while (m_K[idx, 0] != 0)
            {
                DataRow tempRow = m_ResultsTable.NewRow();
                tempRow["Line_ID"] = m_K[idx, 0].Real;
                tempRow["From_Bus_Num"] = m_K[idx, 1].Real;
                tempRow["KV1"] = m_K[idx, 2].Real.ToString().Truncate(6) + "+" + m_K[idx, 2].Imaginary.ToString().Truncate(6) + "j";
                tempRow["KI1"] = m_K[idx, 3].Real.ToString().Truncate(6) + "+" + m_K[idx, 3].Imaginary.ToString().Truncate(6) + "j";
                tempRow["To_Bus_Num"] = m_K[idx, 4].Real;
                tempRow["KV2"] = m_K[idx, 5].Real.ToString().Truncate(6) + "+" + m_K[idx, 5].Imaginary.ToString().Truncate(6) + "j";
                tempRow["KI2"] = m_K[idx, 6].Real.ToString().Truncate(6) + "+" + m_K[idx, 6].Imaginary.ToString().Truncate(6) + "j";
                m_ResultsTable.Rows.Add(tempRow);

                idx += 1;
            }

            // display
            DataRow[] currentRows = m_ResultsTable.Select(null, null, DataViewRowState.CurrentRows);

            if (currentRows.Length < 1)
                Console.WriteLine("No Current Rows Found");
            else
            {
                foreach (DataColumn column in m_ResultsTable.Columns)
                    Console.Write("\t{0}", column.ColumnName);

                Console.WriteLine("\t");

                foreach (DataRow row in currentRows)
                {
                    foreach (DataColumn column in m_ResultsTable.Columns)
                        Console.Write("\t{0}", row[column]);

                    Console.WriteLine("\t");
                }
            }

        }
        #endregion
    }
}
