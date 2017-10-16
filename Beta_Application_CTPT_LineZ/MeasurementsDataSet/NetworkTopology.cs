using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    public class NetworkTopology
    {
        #region [ Private Members ]
        private List<int> m_busOriginalLibrary;                      // Save all original bus numbers
        private int[] m_busLibrary;                              // Save all indexed bus numbers
        private List<int> m_lineLibrary;                             // Save all line numbers
        private List<int[]> m_lineBusInfoOriginal;                    // original [line number, from bus number, to bus number] 
        private int[,] m_lineBusInfo;                            // [line number, from bus number, to bus number] 
        private List<int> m_lineSetCalibrateOriginal;                // Original line number set of the lines to be calibrated
        private List<int> m_lineSetCalibrate;                        // line number set of the lines to be calibrated
        private List<double[]> m_lineParameters;                      // [Original line number, R, X, y] -> [Line number, R, X, y] p.u.
        private Dictionary<string, double> m_zyDictionary;       // Dictionary of the line parameters
        private int m_busNum;                                    // number of buses
        private int m_lineNum;                                   // number of lines
        private double m_baseMVA;
        private double m_baseKV;

        private int m_refBusNumber;                         // Bus number where the Accurate PT and CT installed
        private int m_refLineNumber;                        // Line number where the Accurate PT and CT installed to identify the PMU

        private int[,] m_connectivityMatrix;                     // bus_num*bus_num matrix, entry = 1: bus connected; 0: bus NOT connected
        private int m_componentsNum;                             // number of connected components of the system
        private int[,,] m_components;                            // record all the connected components [component number, from bus, to bus, line_num]

        /* Assisting COmponents*/
        private int[] m_visited = new int[100];                  // record status during BFS
        private int[,] m_tempComponent = new int[50, 2];         // record the branches of current component
        private int m_currentComponentLength = 0;                // record the number of branches in the current component (-1 for array index cause)

        #endregion

        #region [ Public Properties ]

        public int[] BusLibrary
        {
            get
            {
                return m_busLibrary;
            }
        }
        public List<int> BusOriginalLibrary
        {
            get
            {
                return m_busOriginalLibrary;
            }
        }

        public List<int> LineLibrary
        {
            get
            {
                return m_lineLibrary;
            }

        }

        public int[,] LineBusInfo
        {
            get
            {
                return m_lineBusInfo;
            }

        }
        public List<int[]> LineBusInfoOriginal
        {
            get
            {
                return m_lineBusInfoOriginal;
            }

        }

        public List<int> LineSetCalibrateOriginal
        {
            get
            {
                return m_lineSetCalibrateOriginal;
            }
        }

        public List<int> LineSetCalibrate
        {
            get
            {
                return m_lineSetCalibrate;
            }
        }

        public int BusNum
        {
            get
            {
                return m_busNum;
            }
        }

        public int LineNum
        {
            get
            {
                return m_lineNum;
            }
        }

        public List<double[]> LineParameters
        {
            get
            {
                return m_lineParameters;
            }
        }

        public Dictionary<string, double> ZyDictionary
        {
            get
            {
                return m_zyDictionary;
            }
        }

        public int RefBusNumber
        {
            get
            {
                return m_refBusNumber;
            }
        }

        public int RefLineNumber
        {
            get
            {
                return m_refLineNumber;
            }
        }

        public int[,] ConnectivityMatrix
        {
            get
            {
                return m_connectivityMatrix;
            }

            //set
            //{
            //    m_connectivityMatrix = value;
            //}
        }

        public int ComponentsNum
        {
            get
            {
                return m_componentsNum;
            }

            //set
            //{
            //    m_componentsNum = value;
            //}
        }

        public int[,,] Components
        {
            get
            {
                return m_components;
            }

            //set
            //{
            //    m_components = value;
            //}
        }
        
        public double BaseMVA
        {
            get
            {
                return m_baseMVA;
            }
            set
            {
                m_baseMVA = value;
            }
        }
        
        public double BaseKV
        {
            get
            {
                return m_baseKV;
            }
            set
            {
                m_baseKV = value;
            }
        }
        #endregion

        #region [ Constructors ]
        public NetworkTopology(string FilePath)  // System Configuration
        {
            #region [ Original Configuration ]
            //m_busOriginalLibrary = new int[] { 8, 9, 10, 26, 30, 38, 63, 64, 65, 68, 81 };
            //m_lineLibrary = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            //m_lineSetCalibrateOriginal = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //m_lineBusInfoOriginal = new int[24, 3] {{   1   ,   8   ,   9   }   ,
            //                                        {   2   ,   8   ,   30  }   ,
            //                                        {   3   ,   9   ,   10  }   ,
            //                                        {   4   ,   26  ,   30  }   ,
            //                                        {   5   ,   30  ,   38  }   ,
            //                                        {   6   ,   38  ,   65  }   ,
            //                                        {   7   ,   63  ,   64  }   ,
            //                                        {   8   ,   64  ,   65  }   ,
            //                                        {   9   ,   65  ,   68  }   ,
            //                                        {   10  ,   68  ,   81  }   ,
            //                                        {   11  ,   68  ,   116 }   ,
            //                                        {   12  ,   5   ,   8   }   ,
            //                                        {   13  ,   17  ,   30  }   ,
            //                                        {   14  ,   25  ,   26  }   ,
            //                                        {   15  ,   37  ,   38  }   ,
            //                                        {   16  ,   59  ,   63  }   ,
            //                                        {   17  ,   61  ,   64  }   ,
            //                                        {   18  ,   65  ,   66  }   ,
            //                                        {   19  ,   68  ,   69  }   ,
            //                                        {   20  ,   80  ,   81  }   ,
            //                                        {   21  ,   8   ,   -1  }   ,
            //                                        {   22  ,   10  ,   -1  }   ,
            //                                        {   23  ,   26  ,   -1  }   ,
            //                                        {   24  ,   65  ,   -1  }   };


            //m_lineParameters = new double[24, 4] {{   1   ,   0.00244 ,   0.030499999 ,   0.580999961 }   ,
            //                                     {   2   ,   0.00431 ,   0.050399998 ,   0.256999996 }   ,
            //                                     {   3   ,   0.00258 ,   0.032200001 ,   0.615000005 }   ,
            //                                     {   4   ,   0.00799 ,   0.086000005 ,   0.454000003 }   ,
            //                                     {   5   ,   0.00464 ,   0.054       ,   0.210999996 }   ,
            //                                     {   6   ,   0.00901 ,   0.098600002 ,   0.522999992 }   ,
            //                                     {   7   ,   0.00172 ,   0.02        ,   0.108000002 }   ,
            //                                     {   8   ,   0.00269 ,   0.0302      ,   0.189999998 }   ,
            //                                     {   9   ,   0.00138 ,   0.015999995 ,   0.319000001 }   ,
            //                                     {   10  ,   0.00175 ,   0.020200001 ,   0.404000015 }   ,
            //                                     {   11  ,   0       ,   0           ,   0           }   ,
            //                                     {   12  ,   0       ,   0           ,   0           }   ,
            //                                     {   13  ,   0       ,   0           ,   0           }   ,
            //                                     {   14  ,   0       ,   0           ,   0           }   ,
            //                                     {   15  ,   0       ,   0           ,   0           }   ,
            //                                     {   16  ,   0       ,   0           ,   0           }   ,
            //                                     {   17  ,   0       ,   0           ,   0           }   ,
            //                                     {   18  ,   0       ,   0           ,   0           }   ,
            //                                     {   19  ,   0       ,   0           ,   0           }   ,
            //                                     {   20  ,   0       ,   0           ,   0           }   ,
            //                                     {   21  ,   0       ,   0           ,   0           }   ,
            //                                     {   22  ,   0       ,   0           ,   0           }   ,
            //                                     {   23  ,   0       ,   0           ,   0           }   ,
            //                                     {   24  ,   0       ,   0           ,   0           }   };

            //m_busNum = m_busOriginalLibrary.Count();
            //m_lineNum = m_lineLibrary.Count();

            //Console.WriteLine("Please choose starting bus: ");
            //string starting_bus = Console.ReadLine();
            //Console.WriteLine("Please choose starting line: ");
            //string starting_line = Console.ReadLine();
            //string starting_bus_original = "81";
            //string starting_line_original = "10";
            #endregion

            int starting_bus_original_int = 0;
            int starting_line_original_int = 0;

            m_busOriginalLibrary = new List<int>();
            m_lineLibrary = new List<int>();
            m_lineSetCalibrateOriginal = new List<int>();
            m_lineBusInfoOriginal = new List<int[]>();
            m_lineParameters = new List<double[]>();

            // Configuration Data Acquisition
            StudyCase CurrentCase = StudyCase.DeserializeFromXml(FilePath);

            m_lineNum = 0;
            foreach (Branch CurrentLine in CurrentCase.Branches)
            {
                m_lineNum += 1;
                m_lineLibrary.Add(CurrentLine.LineNumber);
                if (CurrentLine.HighVoltageLineFlag == true)
                {
                    m_lineSetCalibrateOriginal.Add(CurrentLine.LineNumber);
                }
                if (CurrentLine.ReferenceFlag == true)
                {
                    starting_line_original_int = CurrentLine.LineNumber;
                }

                int[] TempLineBusInfo = new int[3] { CurrentLine.LineNumber, CurrentLine.FromBusNumber, CurrentLine.ToBusNumber };               
                m_lineBusInfoOriginal.Add(TempLineBusInfo);

                double[] TempLineParameters = new double[4] { CurrentLine.LineNumber, CurrentLine.Resistance, CurrentLine.Reactance, CurrentLine.Susceptance };
                m_lineParameters.Add(TempLineParameters);
            }

            m_busNum = 0;
            foreach (Bus CurrentBus in CurrentCase.Buses)
            {
                m_busNum += 1;
                m_busOriginalLibrary.Add(CurrentBus.BusName);
                if (CurrentBus.ReferenceFlag == true)
                {
                    starting_bus_original_int = CurrentBus.BusName;
                }
            }

            m_baseKV = CurrentCase.BaseKV;
            m_baseMVA = CurrentCase.BaseMVA;


            // Data Processing
            m_lineSetCalibrate = m_lineSetCalibrateOriginal;
            
            m_busLibrary = new int[m_busNum];
            for (int idx1 = 0; idx1 < m_busNum; idx1++)
            {
                m_busLibrary[idx1] = idx1 + 1;
            }

            m_lineBusInfo = new int[m_lineNum, 3];
            for (int idx2 = 0; idx2 < m_lineNum; idx2++)
            {
                int TempFromBusNumber = m_lineBusInfoOriginal[idx2][1];
                int TempToBusNumber = m_lineBusInfoOriginal[idx2][2];

                int TempFromBusIndex = -1;
                int TempToBusIndex = -1;

                for (int idx3 = 0; idx3 < m_busNum; idx3++)
                {
                    if (m_busOriginalLibrary[idx3] == TempFromBusNumber)
                    {
                        TempFromBusIndex = idx3 + 1;
                    }
                    if (m_busOriginalLibrary[idx3] == TempToBusNumber)
                    {
                        TempToBusIndex = idx3 + 1;
                    }
                }

                m_lineBusInfo[idx2, 0] = idx2 + 1; // m_lineBusInfoOriginal[idx2, 0];
                m_lineBusInfo[idx2, 1] = TempFromBusIndex;
                m_lineBusInfo[idx2, 2] = TempToBusIndex;

                m_lineParameters[idx2][0] = idx2 + 1; // m_lineParameters [Original line number, R, X, y] -> [Line number, R, X, y]

            }

            m_zyDictionary = new Dictionary<string, double>();
            Zy_to_dictionary();

            for (int idx6 = 0; idx6 < m_lineSetCalibrateOriginal.Count(); idx6++)
            {
                int TempLineNumber = m_lineSetCalibrateOriginal[idx6];
                for (int idx7 = 0; idx7 < m_lineNum; idx7++)
                {
                    if (m_lineBusInfoOriginal[idx7][0] == TempLineNumber)
                    {
                        m_lineSetCalibrate[idx6] = idx7 + 1;
                        break;
                    }
                }
            }

            m_connectivityMatrix = new int[m_busNum, m_busNum];
            m_componentsNum = 0;
            m_components = new int[25, 25, 3];//[25, m_lineSetCalibrate.Count(), 3];

            

            // project the opriginal bus number and line number to the indices
            string starting_bus = null;
            string starting_line = null;
            for (int idx4 = 0; idx4 < m_busNum; idx4++)
            {
                if (m_busOriginalLibrary[idx4] == starting_bus_original_int)
                {
                    m_refBusNumber = m_busLibrary[idx4];
                    starting_bus = m_busLibrary[idx4].ToString();
                    break;
                }
            }
            for (int idx5 = 0; idx5 < m_lineNum; idx5++)
            {
                if (m_lineBusInfoOriginal[idx5][0] == starting_line_original_int)
                {
                    m_refLineNumber = m_lineBusInfo[idx5, 0];
                    starting_line = m_lineBusInfo[idx5, 0].ToString();
                    break;
                }
            }

            ConnectivityAnalysis(starting_bus, starting_line);

        }
        #endregion

        #region [ Methods ]
        /* Save the impedance data into dictionary for computation convenience */
        public void Zy_to_dictionary()
        {
            for (int idx0 = 0; idx0 < m_lineNum; idx0++)
            {
                int Linenumber = Convert.ToInt32(m_lineParameters[idx0][0]);

                string TempKey;
                double TempValue;

                TempKey = "L" + Linenumber.ToString() + "Resistance";
                TempValue = m_lineParameters[idx0][1];
                m_zyDictionary.Add(TempKey, TempValue);

                TempKey = "L" + Linenumber.ToString() + "Reactance";
                TempValue = m_lineParameters[idx0][2];
                m_zyDictionary.Add(TempKey, TempValue);

                TempKey = "L" + Linenumber.ToString() + "Susceptance";
                TempValue = m_lineParameters[idx0][3];
                m_zyDictionary.Add(TempKey, TempValue);

            }

        }

        /* Analyze the connectivity of the system, return all connectivity components */
        public void ConnectivityAnalysis(string starting_bus_number, string starting_line_number)
        {
            //Get Connectivity Matrix
            for (int i = 0; i < m_lineNum; i++)
            {
                int row_number = m_lineBusInfo[i, 1];
                int col_number = m_lineBusInfo[i, 2];

                if ((row_number != -1) && (col_number != -1))
                {
                    m_connectivityMatrix[(row_number - 1), (col_number - 1)] = 1;
                    m_connectivityMatrix[(col_number - 1), (row_number - 1)] = 1;
                }
            }

            //Get Connected Components
            int starting_bus = Convert.ToInt32(starting_bus_number);

            for (int n = 0; n < m_lineSetCalibrate.Count(); n++)
            {
                m_visited[n] = 0;
            }
            m_componentsNum = 0;

            m_visited[0] = Convert.ToInt32(starting_line_number);
            m_componentsNum += 1;
            int ComponentLengthLimit = 25;
            m_tempComponent = new int[ComponentLengthLimit, 3];//[m_lineSetCalibrate.Count(), 3];
            m_tempComponent[0, 0] = starting_bus;
            m_tempComponent[0, 1] = starting_bus;
            m_tempComponent[0, 2] = find_line_from_buses(m_tempComponent[0, 0], m_tempComponent[0, 1]);
            m_currentComponentLength += 1;
            m_tempComponent[1, 0] = starting_bus;
            m_tempComponent[1, 2] = Convert.ToInt32(starting_line_number);
            for (int l = 0; l < m_lineNum; l++)
            {
                if (m_lineBusInfo[l, 0] == m_tempComponent[1, 2])
                {
                    if (m_tempComponent[1, 0] == m_lineBusInfo[l, 1])
                    {
                        m_tempComponent[1, 1] = m_lineBusInfo[l, 2];
                    }
                    else
                    {
                        m_tempComponent[1, 1] = m_lineBusInfo[l, 1];
                    }
                }
            }
            m_currentComponentLength += 1;
            int current_root = m_tempComponent[1, 1] - 1;
            m_currentComponentLength = breadth_first_search(current_root);

            for (int k = 0; k < ComponentLengthLimit; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    m_components[(m_componentsNum - 1), k, l] = m_tempComponent[k, l];
                }
            }

        }

        /* Depth First Search begin from node i */
        public int depth_first_search(int i)
        {
            for (int j = 0; j < m_busNum; j++)
            {
                if (m_connectivityMatrix[i, j] == 1)
                {
                    if (m_visited[j] == 1)
                    {
                        continue;
                    }
                    else
                    {
                        m_visited[j] = 1;
                        m_tempComponent[m_currentComponentLength, 0] = i + 1;
                        m_tempComponent[m_currentComponentLength, 1] = j + 1;
                        m_tempComponent[m_currentComponentLength, 2] = find_line_from_buses(m_tempComponent[m_currentComponentLength, 0], m_tempComponent[m_currentComponentLength, 1]);
                        m_currentComponentLength += 1;
                        m_currentComponentLength = depth_first_search(j);
                    }
                }
            }
            return m_currentComponentLength;
        }

        /* Breadth First Search begin from node i */
        public int breadth_first_search(int i)
        {
            List<int> ChildSet = new List<int>();

            for (int j = 0; j < m_busNum; j++)
            {
                if (m_connectivityMatrix[i, j] == 1)
                {
                    int current_line_number = find_line_from_buses(i + 1, j + 1);
                    bool current_line_visited_flag = false;
                    int new_line_pointer = 0;
                    for (int k = 0; k < m_lineNum; k++)
                    {
                        if (m_visited[k] == 0)
                        {
                            new_line_pointer = k;
                            break;
                        }
                        else if (m_visited[k] == current_line_number)
                        {
                            current_line_visited_flag = true;
                            break;
                        }
                    }
                    if (current_line_visited_flag)
                    {
                        continue;
                    }
                    else
                    {
                        m_visited[new_line_pointer] = current_line_number;
                        m_tempComponent[m_currentComponentLength, 0] = i + 1;
                        m_tempComponent[m_currentComponentLength, 1] = j + 1;
                        m_tempComponent[m_currentComponentLength, 2] = current_line_number;
                        m_currentComponentLength += 1;
                        ChildSet.Add(j);
                    }
                }
            }

            foreach (int SingleChild in ChildSet)
            {
                breadth_first_search(SingleChild);
            }
            return m_currentComponentLength;
        }

        public int find_line_from_buses(int bus1, int bus2)
        {
            int current_line_num = 0;

            for (int i = 0; i < m_lineNum; i++)
            {
                int current_bus1 = m_lineBusInfo[i, 1];
                int current_bus2 = m_lineBusInfo[i, 2];
                if (((current_bus1 == bus1) && (current_bus2 == bus2)) || ((current_bus1 == bus2) && (current_bus2 == bus1)))
                {
                    current_line_num = m_lineBusInfo[i, 0];
                    break;
                }
            }

            return current_line_num;
        }
        #endregion
    }
}
