using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Numerics;
using System.Data;

using MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using Beta_Application_CTPT_LineZ.Model;
using Beta_Application_CTPT_LineZ.MeasurementsDataSet;
using SynchrophasorAnalytics.Matrices;

namespace Beta_Application_CTPT_LineZ.AlgorithmRealization
{
    public class CTPTCalibration
    {
        #region [ Private Members ]
        private VIDataSet m_currentSystem;
        private int[,] m_lineVisited;                            // Record the lines have been calibrated or not: 1/0
        //private Complex[] m_KVBuses;                             // Record the KVs for all the buses as a reference bus for single line calibration start from 0
        private int[] m_NonCalibratableComponentsNum;            // connected components that can not be calibrated due to topology causes
        private Complex[,] m_Z;                                  // True impedance matrix within estimation procedure
        private List<Complex> m_V1;                              // All Voltage measurements of bus 1
        private List<Complex> m_V2;                              // All Voltage measurements of bus 2
        private List<Complex> m_I1;                              // All Current measurements of bus 1
        private List<Complex> m_I2;                              // All Current measurements of bus 2
        private Complex[,] m_Zhat;
        private Complex[,] m_K;                                  // Record calibration results
        private DataTable m_ResultsTable;                        // Demonstration table for the Ks
        private double[] m_RXy;                                  // Record current line's R, X, and y

        private struct LinesKVRecord
        {
            public int Line_ID;
            public int From_bus_num;
            public Complex KV1;
        }
        private LinesKVRecord[] m_KVLines;

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
        public CTPTCalibration(NetworkTopology ConfiguredNetwork)
        {
            m_currentSystem = new VIDataSet(ConfiguredNetwork);
            m_lineVisited = new int[m_currentSystem.Network.LineNum, 2];
            //m_KVBuses = new Complex[m_currentSystem.Network.BusNum];
            m_NonCalibratableComponentsNum = new int[50];
            m_Z = new Complex[2, 2];
            m_V1 = new List<Complex>();
            m_I1 = new List<Complex>();
            m_V2 = new List<Complex>();
            m_I2 = new List<Complex>();
            m_Zhat = new Complex[2, 2];
            m_K = new Complex[m_currentSystem.Network.BusNum, 9];
            m_ResultsTable = new DataTable();
            m_RXy = new double[3];

            m_KVLines = new LinesKVRecord[m_currentSystem.Network.LineNum]; //[line_ID, from_bus_num, KV1]
        }
        #endregion

        #region [ Methods ]
        /* Calibrate CTs and PTs throughout the system starting from reference bus with known KV*/
        public void CTPTCalibrationAlgorithm(TextBox textBoxMessages)
        {
            string reference_bus_number = m_currentSystem.Network.RefBusNumber.ToString();
            string reference_line_number = m_currentSystem.Network.RefLineNumber.ToString();
            Complex KV1_initial = 1;

            // Set the reference line and bus
            int reference_line_num = Convert.ToInt32(reference_line_number);
            int reference_bus_num = Convert.ToInt32(reference_bus_number);
            m_KVLines[0].Line_ID = reference_line_num;
            m_KVLines[0].From_bus_num = reference_bus_num;
            m_KVLines[0].KV1 = KV1_initial;

            // Initialize the line calibration status record
            m_lineVisited = new int[m_currentSystem.Network.LineNum, 2];
            for (int l = 0; l < m_currentSystem.Network.LineNum; l++)
            {
                m_lineVisited[l, 0] = m_currentSystem.Network.LineBusInfo[l, 0];
            }

            //// Set the reference bus
            //int reference_bus_num = Convert.ToInt32(reference_bus_number);
            //m_KVBuses[reference_bus_num - 1] = KV1_initial;

            // Search all the connected components
            int reference_bus_number_int = Convert.ToInt32(reference_bus_number);
            for (int i = 0; i < m_currentSystem.Network.ComponentsNum; i++)
            {
                //The reference bus should be calibrated firstly, or the whole component can NOT be calibrated
                if (m_currentSystem.Network.Components[i, 0, 0] != reference_bus_number_int)
                {
                    m_NonCalibratableComponentsNum[i] = -1;
                    string msg0 = "Component " + (i + 1).ToString() + " can NOT be Calibrated due to LACK of reference." ;
                    textBoxMessages.AppendLine(msg0);
                    continue;
                }                

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

                    //Complex KV1_reference = m_KVBuses[m_currentSystem.Network.Components[i, j, 0] - 1];

                    //set up reference Ks
                    bool line_found_flag = false;
                    int line_ID_index = 0;
                    for (int idx = 0; idx < m_currentSystem.Network.LineNum; idx++)
                    {
                        if (current_line_number == m_KVLines[idx].Line_ID.ToString())
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
                        KV1_reference = m_KVLines[line_ID_index].KV1;
                        From_bus_number = m_KVLines[line_ID_index].From_bus_num;
                    }
                    else
                    {
                        textBoxMessages.AppendLine("!!! No reference !!!");
                        break;
                    }

                    if (KV1_reference != 0)
                    {
                        SingleLineCTPT(current_line_number, KV1_reference, From_bus_number, textBoxMessages);

                        //SingleLineCTPT(current_line_number, KV1_reference, m_currentSystem.Network.Components[i, j, 0], textBoxMessages);
                        //("544", KV1_reference,2); for test
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

                //CalibrationResultsDemonstration();
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
            for (int j = 0; j < 1; j++)//60; j++)
            {
                for (int k = 0; k < m_currentSystem.TimeSeriesFrames.Count(); k++)//m_currentSystem.FramesPerSecond; k++)
                {
                    long CurrentTimeStamp = k + 1;//20160113070000 + j * 100 + (k + 1);

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
                            CurrentV1 = Complex.FromPolarCoordinates(FromBusVM, FromBusVA * Math.PI / 180); // * 1000 / Math.Sqrt(3);
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
                            CurrentV2 = Complex.FromPolarCoordinates(ToBusVM, ToBusVA * Math.PI / 180); //* 1000 / Math.Sqrt(3);
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

        /* Get the Impedance Matrix of the current line*/
        public bool GetLineZ(string LineID)
        {
            bool ImpedanceAvailable = true;

            string RKey = "L" + LineID + "Resistance";
            string XKey = "L" + LineID + "Reactance";
            string SusceptanceKey = "L" + LineID + "Susceptance";

            try //in case that the impedance data of the line can not be acquired
            {
                double R_pu = m_currentSystem.Network.ZyDictionary[RKey];
                double X_pu = m_currentSystem.Network.ZyDictionary[XKey];
                double Susceptance_pu = m_currentSystem.Network.ZyDictionary[SusceptanceKey];

                Complex Z_pu = new Complex(R_pu, X_pu);
                Complex y_pu = new Complex(0, Susceptance_pu);

                double BaseZ = m_currentSystem.Network.BaseKV * m_currentSystem.Network.BaseKV / m_currentSystem.Network.BaseMVA;

                Complex Z = Z_pu * BaseZ;
                Complex y = y_pu / BaseZ;

                m_Z[0, 0] = (1 + Z * y) / (y * (2 + Z * y));
                m_Z[0, 1] = 1 / (y * (2 + Z * y));
                m_Z[1, 0] = m_Z[0, 1];
                m_Z[1, 1] = m_Z[0, 0];

                m_RXy[0] = R_pu;
                m_RXy[1] = X_pu;
                m_RXy[2] = Susceptance_pu;
            }
            catch (Exception exception)
            {
                ImpedanceAvailable = false;
                Console.WriteLine("The Impedance and Susceptance of LINE {0} are not available.", LineID);
            }
            return ImpedanceAvailable;
        }

        public Matrix<Complex> LeastSquareEstimation(Matrix<Complex> Y, Matrix<Complex> X)
        {
            Matrix<Complex> Beta;

            Matrix<Complex> XT = X.ConjugateTranspose();

            Beta = (X.ConjugateTranspose() * X).Inverse() * X.ConjugateTranspose() * Y; // For complex LSE problems, note that there should be conjugate transpose            

            Matrix<Complex> Error;
            Error = Y - X.Multiply(Beta);

            return Beta;
        }

        /* Conduct pi section CTPT calibration of single line*/
        public int SingleLineCTPT(string LineID, Complex KV1, int FromBusNumber, TextBox textBoxMessages) //ALso need Complex KV1, Complex[,] Z
        {
            int CurrentToBus = 0;
            CurrentToBus = GetLineVI(LineID, FromBusNumber);
            bool ImpedanceAvailable = GetLineZ(LineID);

            if (!ImpedanceAvailable)
            {
                textBoxMessages.AppendLine("Calibration FAILED due to lack of Impedance data.\n");
                return 0;
            }

            Complex KI1 = new Complex();
            Complex KV2 = new Complex();
            Complex KI2 = new Complex();

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
                //Complex[] V2_sample_temp = new Complex[60];
                //Complex[] I2_sample_temp = new Complex[60];

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
                    Matrix<Complex> Vhat_T = Vhat.Transpose();
                    Matrix<Complex> Ihat_T = Ihat.Transpose();

                    //estimate Zhat
                    Matrix<Complex> Zhat_temp = null;

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

            if (feasible_flag < 15)
            {
                for (int l = 0; l < 30; l++)
                {
                    m_Zhat[0, 0] = m_Zhat[0, 0] + Zhat11[l];
                    m_Zhat[0, 1] = m_Zhat[0, 1] + Zhat12[l];
                    m_Zhat[1, 0] = m_Zhat[1, 0] + Zhat21[l];
                    m_Zhat[1, 1] = m_Zhat[1, 1] + Zhat22[l];
                }

                m_Zhat[0, 0] = m_Zhat[0, 0] / (30 - feasible_flag);
                m_Zhat[0, 1] = m_Zhat[0, 1] / (30 - feasible_flag);
                m_Zhat[1, 0] = m_Zhat[1, 0] / (30 - feasible_flag);
                m_Zhat[1, 1] = m_Zhat[1, 1] / (30 - feasible_flag);

                KI1 = m_Zhat[0, 0] / m_Z[0, 0] * KV1;
                KV2 = m_Z[1, 0] / m_Zhat[0, 1] * KI1; // Note that the estimated Zhat has been transposed, even though the matrix is symmetric, it should be transposed back for definition causes
                KI2 = m_Zhat[1, 0] / m_Z[0, 1] * KV1;

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
                m_K[m, 7] = new Complex(m_RXy[0], m_RXy[1]);
                m_K[m, 8] = m_RXy[2];

                ////m_KVBuses[from_bus_number_int - 1] = KV1;
                //m_KVBuses[to_bus_number_int - 1] = KV2;

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
                Matrix<Complex> KV_injections_set = KVPropagation(inaccurate_line_set, Line_ID_int, to_bus_number_int, KV2);
                
                int pointer = 0;
                for (int idx1 = 0; idx1 < m_currentSystem.Network.LineNum; idx1++)
                {
                    if (m_KVLines[idx1].Line_ID == 0)
                    {
                        pointer = idx1;
                        break;
                    }
                }
                for (int idx2 = 0; idx2 < KV_injections_set.RowCount; idx2++)
                {
                    if ((pointer + idx2) < m_currentSystem.Network.LineNum)
                    {
                        m_KVLines[pointer + idx2].Line_ID = inaccurate_line_set[idx2];
                        m_KVLines[pointer + idx2].From_bus_num = to_bus_number_int;
                        m_KVLines[pointer + idx2].KV1 = KV_injections_set[idx2, 0];
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
                            m_currentSystem.Network.BusOriginalLibrary[to_bus_number_int-1].ToString() + " has been calibrated.\n";
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
        public Matrix<Complex> KVPropagation(List<int> inaccurate_line_set, int Accurate_line_num, int to_bus_number_int, Complex KV_accurate)
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
            Matrix<Complex> KV_injection_set = Matrix<Complex>.Build.Random(inaccurate_line_num, 1);
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
        
        /* Use table to demonstrate results*/
        public void CalibrationResultsDemonstration()
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


public static class StringExt
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}