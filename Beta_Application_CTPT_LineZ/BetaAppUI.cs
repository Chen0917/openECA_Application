using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Numerics;
using System.IO;
using System.ComponentModel;

using GSF;
using GSF.TimeSeries;
using GSF.TimeSeries.Adapters;
using GSF.Diagnostics;
using GSF.IO;
using GSF.Windows.Forms;
using GSF.ComponentModel;
using ECAClientFramework;
using ECAClientUtilities.API;

using Excel = Microsoft.Office.Interop.Excel;

using Beta_Application_CTPT_LineZ.MeasurementsDataSet;
using Beta_Application_CTPT_LineZ.LocalModel;
using Beta_Application_CTPT_LineZ.Model;
using Beta_Application_CTPT_LineZ.openHistorianDataCollection;
using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI;
using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI.MetaData;
using Beta_Application_CTPT_LineZ.AlgorithmRealization;


namespace Beta_Application_CTPT_LineZ
{
    public struct Coor
    {
        public int CoorX;
        public int CoorY;
    }
    public partial class BetaAppUI : Form
    {
        #region [ Members ]
        // System Scheme Paint
        private NetworkTopology m_systemTopology;
        private Pen blackPen;
        private SolidBrush blackBrush;
        private List<int>[] m_tree; //Array -> Tree levels; Inner List Node numbers of each tree level
        private int m_treeLevelNum;
        private List<Coor>[] m_treeCoordinates;
        private List<Coor> m_treeLineCoordinates;
        private List<int> m_treeLineNumber;
        private Model.GPA.Measurement_set CurrentFrame;
        // Functionality
        private string m_functionSelection;
        private string m_dataSource;
        private readonly LogPublisher m_log;
        private Settings m_trendDataSettings;
        private bool m_functionStopFlag;
        private openECARawDataSet m_openECARawDataSet;
        private bool m_functionInterruptFlag = false;
        private Complex[,] m_lineParameters;
        private bool m_functionsLaunchedFlag = false;
        
        #endregion

        #region [ Constructor ]
        public BetaAppUI()
        {
            InitializeComponent();

            m_functionStopFlag = false;

            // Create a new log publisher instance
            m_log = Logger.CreatePublisher(typeof(BetaAppUI), MessageClass.Application);

            // Results Demonstration GridView initialization
            Results_Demonstration_GridView_Load_Empty_Table();

            //Functionality
            try
            {
                // Load current settings registering a symbolic reference to this form instance for use by default value expressions
                m_trendDataSettings = new Settings(new Dictionary<string, object> { { "Form", this } }.RegisterSymbols());

                // Restore last window size/location
                //this.RestoreLayout();
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormLoad", "Failed while loading settings", exception: ex);
            }


            //if ((m_functionSelection == "CT/PT Calibration") || (m_functionSelection == "Line Parameters Estimation"))
            //{
                
            //}
            //else
            //{
            //    // Real-time Impedance Calculation. Integrate openECA projects here!!!
            //}

            // Functionality
            m_functionSelection = "CT/PT Calibration";
            m_dataSource = "Historical Data";

            m_openECARawDataSet = new openECARawDataSet();

            ClearUpdateMessages();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerForCancel = new BackgroundWorker();
            backgroundWorkerForCancel.WorkerSupportsCancellation = true;
            backgroundWorkerForCancel.WorkerReportsProgress = true;

            backgroundWorkerForCancel.DoWork +=
                new DoWorkEventHandler(backgroundWorkerForCancel_DoWork);
            backgroundWorkerForCancel.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorkerForCancel_RunWorkerCompleted);
            backgroundWorkerForCancel.ProgressChanged +=
                new ProgressChangedEventHandler(
                    backgroundWorkerForCancel_ProgressChanged);
        }

        #endregion

        #region [ Methods ]

        #region [ System Scheme Paint ]
        /// <summary>
        /// Paint System Scheme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Configure_Button_Click(object sender, EventArgs e)
        {            
            // System Scheme Paint Settings
            System_Scheme.SizeChanged += new EventHandler(System_Scheme_SizeChanged);
            System_Scheme.Font = new Font("SansSerif", 8.0f, FontStyle.Bold);            
            blackPen = new Pen(Color.Black);
            blackBrush = new SolidBrush(Color.Black);

            if (File.Exists(textBoxConfigurationPath.Text))
            {
                // System Scheme Paint
                m_systemTopology = new NetworkTopology(textBoxConfigurationPath.Text);
                m_tree = new List<int>[m_systemTopology.BusCalibrateNum];
                float Font_size = (float)(80 / m_systemTopology.BusCalibrateNum + 2.5);
                System_Scheme.Font = new Font("SansSerif", Font_size, FontStyle.Bold);
                for (int idx0 = 0; idx0 < m_systemTopology.BusCalibrateNum; idx0++)
                {
                    List<int> TempList = new List<int>();
                    m_tree[idx0] = TempList;
                }
                m_treeLevelNum = 0;

                m_treeCoordinates = new List<Coor>[m_systemTopology.BusCalibrateNum];
                for (int idx1 = 0; idx1 < m_systemTopology.BusCalibrateNum; idx1++)
                {
                    List<Coor> TempList = new List<Coor>();
                    m_treeCoordinates[idx1] = TempList;
                }

                m_treeLineCoordinates = new List<Coor>();
                m_treeLineNumber = new List<int>();

                try
                {
                    //System_Scheme.Controls.Clear();
                    System_Scheme.Invalidate();
                    System_Scheme.Paint += new PaintEventHandler(System_Scheme_Paint);
                    MessageBox.Show("System configured!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
            else
            {
                MessageBox.Show("Error! Can NOT Find Configuration File!");
            }
        }

        private void FolderDialoguebutton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog FolderBrowser = new OpenFileDialog();
                FolderBrowser.Filter = "XML|*.xml";
                //FolderBrowser.ShowDialog();
                if (FolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    textBoxConfigurationPath.Text = FolderBrowser.FileName;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error: " + ex;
                MessageBox.Show(msg);
            }
        }

        private void System_Scheme_Paint(object sender, PaintEventArgs e)
        {
            bool Success = Create_Tree();

            if (!Success)
            {
                MessageBox.Show("Error: No Valid System Topology Available!");
            }
            else
            {
                try
                {
                    DrawTree(e.Graphics);
                }
                catch (Exception ex)
                {
                    string msg = "Error: " + ex;
                    MessageBox.Show(msg);
                }
                
                //DrawingControl.SuspendDrawing(System_Scheme);                
            }

        }

        private void System_Scheme_SizeChanged(object sender, EventArgs e)
        {
            System_Scheme.Invalidate();
        }

        private bool Create_Tree()
        {
            List<int>[] Tree = new List<int>[m_systemTopology.BusCalibrateNum];
            for (int idx0 = 0; idx0 < m_systemTopology.BusCalibrateNum; idx0++)
            {
                List<int> TempList = new List<int>();
                Tree[idx0] = TempList;
            }
            int TreeLevelNum = 0;

            int[,,] ConnectedComponents = m_systemTopology.Components;

            //Find level of each Node
            
            int[] LevelRecord = new int[m_systemTopology.BusCalibrateNum + 1];

            if (ConnectedComponents[0, 0, 0] == 0)
            {
                Tree = null;
                return false;
            }
            else
            {
                int LevelCount = 0;
                LevelRecord[ConnectedComponents[0, 0, 1]] = LevelCount;
                for (int idx0 = 1; idx0 < ConnectedComponents.GetLength(1); idx0++)
                {
                    LevelRecord[ConnectedComponents[0, idx0, 1]] = LevelRecord[ConnectedComponents[0, idx0, 0]] + 1;
                }
            }

            //Compose Tree list   
            for (int idx1 = 1; idx1 < LevelRecord.Count(); idx1++)
            {
                int CurrentLevel = LevelRecord[idx1];
                Tree[CurrentLevel].Add(idx1);

                if (CurrentLevel >= TreeLevelNum)
                {
                    TreeLevelNum = CurrentLevel;
                }
            }

            TreeLevelNum = TreeLevelNum + 1;
            m_treeLevelNum = TreeLevelNum;

            // Adjust buses' orders to avoid crossing when drawing
            if (m_systemTopology.BusCalibrateNum > 15)
            {
                for (int idx2 = 1; idx2 < TreeLevelNum; idx2++)
                {
                    List<int> TempTreeLevel = new List<int>(Tree[idx2].Count());

                    for (int idx3 = 0; idx3 < Tree[idx2 - 1].Count(); idx3++)
                    {
                        int CurrentParentBus = Tree[idx2 - 1][idx3];

                        for (int idx4 = 0; idx4 < Tree[idx2].Count(); idx4++)
                        {
                            int CurrentChildBus = Tree[idx2][idx4];

                            if (m_systemTopology.ConnectivityMatrix[CurrentParentBus - 1, CurrentChildBus - 1] == 1)
                            {
                                TempTreeLevel.Add(CurrentChildBus);
                            }
                        }
                    }

                    Tree[idx2] = TempTreeLevel;
                }
            }
            
            m_tree = Tree;

            return true;
        }

        private void DrawTree(Graphics g)
        {
            List<Coor>[] TreeCoordinates = new List<Coor>[m_systemTopology.BusCalibrateNum];
            for (int idx1 = 0; idx1 < m_systemTopology.BusCalibrateNum; idx1++)
            {
                List<Coor> TempList = new List<Coor>();
                TreeCoordinates[idx1] = TempList;
            }

            int Width = System_Scheme.Width - 10;
            int Height = System_Scheme.Height - 10;

            int XSCALE = (int)(Width / m_treeLevelNum);
            int YSCALE = (int)(Height / m_treeLevelNum * 3);

            // Draw the tree by level
            int OriginalCoorX = 30;
            int OriginalCoorY = (int) (Height / 3);
            int Xpos = 0;
            int Ypos = 0;
            int dX = 5 + 80 / m_systemTopology.BusCalibrateNum;
            int dY = 10 + 80 / m_systemTopology.BusCalibrateNum;
            int dWords = 10;
            
            //Draw all the buses
            for (int idx0 = 0; idx0 < m_treeLevelNum; idx0++)
            {
                Xpos = idx0;
                int CoorX = OriginalCoorX + Xpos * XSCALE;

                for (int idx1 = 0; idx1 < m_tree[idx0].Count(); idx1++)
                {
                    Ypos = idx1;
                    int CoorY = OriginalCoorY + Ypos * YSCALE;

                    //string BusName = m_systemTopology.BusOriginalLibrary[m_tree[idx0][idx1]-1].ToString();
                    string BusName = m_systemTopology.BusNameOriginalLibrary[m_tree[idx0][idx1] - 1];
                    g.DrawString(BusName, System_Scheme.Font, blackBrush, new PointF(CoorX + dX - dWords, CoorY - dY - dWords));
                    g.DrawLine(blackPen, CoorX, CoorY - (int)(YSCALE / 3), CoorX, CoorY + (int)(YSCALE / 3));

                    Coor TempCoor;
                    TempCoor.CoorX = CoorX;
                    TempCoor.CoorY = CoorY;
                    TreeCoordinates[idx0].Add(TempCoor);
                }
            }
            m_treeCoordinates = TreeCoordinates;

            // Draw lines one by one based on the connectivity matrix
            List<Coor> TreeLineCoordinates = new List<Coor>();
            List<int> TreeLineNumber = new List<int>();
            for (int idx2 = 0; idx2 < (m_treeCoordinates.Count() - 1); idx2++)
            {
                for (int idx3 = 0; idx3 < m_treeCoordinates[idx2].Count(); idx3++)
                {
                    int CurrentBus = m_tree[idx2][idx3] - 1;
                    int YDeviationCount = 0;
                    int ChildCount = 0;
                    int ChildDeviation = 0;
                    for (int idx5 = 0; idx5 < m_systemTopology.ConnectivityMatrix.GetLength(1); idx5++)
                    {
                        ChildCount = ChildCount + m_systemTopology.ConnectivityMatrix[CurrentBus, idx5];
                    }
                    if (ChildCount != 0)
                    {
                        ChildDeviation = (int)(YSCALE / 3 / ChildCount);
                    }
                    for (int idx4 = 0; idx4 < m_treeCoordinates[idx2 + 1].Count(); idx4++)
                    {
                        int ChildBus = m_tree[idx2 + 1][idx4] - 1;

                        if (m_systemTopology.ConnectivityMatrix[CurrentBus, ChildBus] == 1)
                        {
                            
                            g.DrawLine(blackPen, m_treeCoordinates[idx2][idx3].CoorX, m_treeCoordinates[idx2][idx3].CoorY + YDeviationCount * ChildDeviation,
                                m_treeCoordinates[idx2 + 1][idx4].CoorX, m_treeCoordinates[idx2 + 1][idx4].CoorY);

                            Coor TempCoor;
                            TempCoor.CoorX = (m_treeCoordinates[idx2][idx3].CoorX + m_treeCoordinates[idx2 + 1][idx4].CoorX) / 2;
                            TempCoor.CoorY = (m_treeCoordinates[idx2][idx3].CoorY + YDeviationCount * ChildDeviation + m_treeCoordinates[idx2 + 1][idx4].CoorY) / 2;
                            TreeLineCoordinates.Add(TempCoor);

                            int TempLineNumber = m_systemTopology.find_line_from_buses(CurrentBus + 1, ChildBus + 1);
                            TreeLineNumber.Add(TempLineNumber);
                            YDeviationCount += 1;
                        }

                    }
                }
            }
            m_treeLineCoordinates = TreeLineCoordinates;
            m_treeLineNumber = TreeLineNumber;


        }

        #endregion

        #region [ Results Demonstration GridView ]
        /// <summary>
        /// Demonstrate the Computation Results
        /// </summary>
        /// 
        private void Results_Demonstration_GridView_Load_Empty_Table()
        {
            Results_Demonstration_GridView.ColumnCount = 14;
            Results_Demonstration_GridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            Results_Demonstration_GridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            Results_Demonstration_GridView.ColumnHeadersDefaultCellStyle.Font =
                new Font(Results_Demonstration_GridView.Font, FontStyle.Bold);

            Results_Demonstration_GridView.RowsDefaultCellStyle.BackColor = Color.White;
            Results_Demonstration_GridView.AlternatingRowsDefaultCellStyle.BackColor =
                Color.LightBlue;

            Results_Demonstration_GridView.AutoSizeRowsMode =
        DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Results_Demonstration_GridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            Results_Demonstration_GridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            Results_Demonstration_GridView.GridColor = Color.Black;
            Results_Demonstration_GridView.RowHeadersVisible = false;
            

            Results_Demonstration_GridView.Columns[0].Name = "Line Number";
            Results_Demonstration_GridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Results_Demonstration_GridView.Columns[1].Name = "Frome Bus Name";
            Results_Demonstration_GridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Results_Demonstration_GridView.Columns[2].Name = "KV1 Magnitude";
            Results_Demonstration_GridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[3].Name = "KV1 Angle";
            Results_Demonstration_GridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[4].Name = "KI1 Magnitude";
            Results_Demonstration_GridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[5].Name = "KI1 Angle";
            Results_Demonstration_GridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[6].Name = "To Bus Name";
            Results_Demonstration_GridView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Results_Demonstration_GridView.Columns[7].Name = "KV2 Magnitude";
            Results_Demonstration_GridView.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[8].Name = "KV2 Angle";
            Results_Demonstration_GridView.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[9].Name = "KI2 Magnitude";
            Results_Demonstration_GridView.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[10].Name = "KI2 Angle";
            Results_Demonstration_GridView.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[11].Name = "Resistance";
            Results_Demonstration_GridView.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[12].Name = "Reactance";
            Results_Demonstration_GridView.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.Columns[13].Name = "Susceptance";
            Results_Demonstration_GridView.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            Results_Demonstration_GridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            this.Results_Demonstration_GridView.Columns["Line Number"].Frozen = true;
            
        }

        private void Results_Demonstration_Table(Complex[,] K)
        {
            Results_Demonstration_GridView.Rows.Clear();
            Results_Demonstration_GridView.Refresh();
            for (int idx0 = 0; idx0 < K.GetLength(0); idx0++)
            {
                if (K[idx0, 0] != 0)
                {
                    string[] TempRow = {
                        K[idx0, 0].Real.ToString(),
                        m_systemTopology.BusNameOriginalLibrary[(int)K[idx0,1].Real -1].ToString(),
                        Math.Round(K[idx0, 2].Magnitude, 4).ToString(),
                        Math.Round(K[idx0, 2].Phase / Math.PI * 180, 4).ToString(),
                        Math.Round(K[idx0, 3].Magnitude, 4).ToString(),
                        Math.Round(K[idx0, 3].Phase / Math.PI * 180, 4).ToString(),
                        m_systemTopology.BusNameOriginalLibrary[(int)K[idx0,4].Real -1].ToString(),
                        Math.Round(K[idx0, 5].Magnitude, 4).ToString(),
                        Math.Round(K[idx0, 5].Phase / Math.PI * 180, 4).ToString(),
                        Math.Round(K[idx0, 6].Magnitude, 4).ToString(),
                        Math.Round(K[idx0, 6].Phase / Math.PI * 180, 4).ToString(),
                        Math.Round(K[idx0, 7].Real, 4).ToString(),
                        Math.Round(K[idx0, 7].Imaginary, 4).ToString(),
                        Math.Round(K[idx0, 8].Real, 6).ToString(), };

                    Results_Demonstration_GridView.Rows.Add(TempRow);
                }

            }
        }

        private void Results_Demonstration_System_Scheme_line_Parameters(Complex[,] LineParameter)
        {
            var LineParametersTextBoxes = System_Scheme.Controls.OfType<RichTextBox>().ToList();

            for (int idx0 = 0; idx0 < LineParameter.GetLength(0); idx0++)
            {
                int CurrentLineNumber = (int)LineParameter[idx0, 0].Real;

                string R = "NaN";
                string X = "NaN";
                string y = "NaN";

                //if (LineParameter[idx0, 1].Real >= 0)
                //{
                //    R = "R: " + Math.Abs(Math.Round(LineParameter[idx0, 1].Real, 4)).ToString();
                //    X = "X: " + Math.Abs(Math.Round(LineParameter[idx0, 1].Imaginary, 4)).ToString();
                //    y = "y: " + Math.Abs(Math.Round(LineParameter[idx0, 2].Real, 4)).ToString();
                //}

                R = "R: " + Math.Abs(Math.Round(LineParameter[idx0, 1].Real, 4)).ToString();
                X = "X: " + Math.Abs(Math.Round(LineParameter[idx0, 1].Imaginary, 4)).ToString();
                y = "y: " + Math.Abs(Math.Round(LineParameter[idx0, 2].Real, 4)).ToString();


                for (int idx1 = 0; idx1 < m_treeLineNumber.Count(); idx1++)
                {
                    if (CurrentLineNumber == m_treeLineNumber[idx1])
                    {
                        //string message = R + "\n" + X + "\n" + y;
                        //UpdateRichTextBox(LineParametersTextBoxes[idx1], message);
                        LineParametersTextBoxes[idx1].Text = R + "\n" + X + "\n" + y;
                        LineParametersTextBoxes[idx1].ContentsResized += (object sender, ContentsResizedEventArgs e) =>
                        {
                            var richTextBox = (RichTextBox)sender;
                            richTextBox.Width = e.NewRectangle.Width + 5;
                            richTextBox.Height = e.NewRectangle.Height + 5;
                        };

                        LineParametersTextBoxes[idx1].MouseHover += (object sender, EventArgs e) =>
                        {
                            var richTextBox = (RichTextBox)sender;
                            richTextBox.Font = new Font("Tahoma", 12, FontStyle.Italic);
                            richTextBox.BackColor = Color.PaleTurquoise;                            
                        };

                        LineParametersTextBoxes[idx1].MouseLeave += (object sender, EventArgs e) =>
                        {
                            var richTextBox = (RichTextBox)sender;
                            richTextBox.Font = new Font("Tahoma", 6, FontStyle.Italic);
                            richTextBox.BackColor = Color.White;
                        };
                    }
                }
            }

            System_Scheme.Refresh();

            Thread.Sleep(200);
        }

        private void Generate_TextBoxes_System_Scheme()
        {
            for (int idx0 = 0; idx0 < m_treeLineNumber.Count(); idx0++)
            {
                RichTextBox CurrentLineParameters = new RichTextBox();
                CurrentLineParameters.Font = new Font("Tahoma", 6, FontStyle.Italic);
                CurrentLineParameters.ForeColor = System.Drawing.Color.Blue;
                CurrentLineParameters.Location = new Point(m_treeLineCoordinates[idx0].CoorX - 10, m_treeLineCoordinates[idx0].CoorY + 5);
                CurrentLineParameters.Clear();
                CurrentLineParameters.WordWrap = false;
                CurrentLineParameters.ScrollBars = RichTextBoxScrollBars.None;
                CurrentLineParameters.Width = 50;
                CurrentLineParameters.Height = 40;
                CurrentLineParameters.ReadOnly = true;
                CurrentLineParameters.BackColor = Color.White;

                System_Scheme.Controls.Add(CurrentLineParameters);
            }
        }

        #endregion

        #region [ Functionality Realization ]
        //Function Selection
        private void CT_PT_Calibration_Selection_CheckedChanged(object sender, EventArgs e)
        {
            m_functionSelection = "CT/PT Calibration";
            Input_Data_Acquisition.Enabled = true;
        }

        private void Line_Parameters_Estimation_Selection_CheckedChanged(object sender, EventArgs e)
        {
            m_functionSelection = "Line Parameters Estimation";
            Input_Data_Acquisition.Enabled = true;
        }

        private void Real_Time_Parameters_Calculation_Selection_CheckedChanged(object sender, EventArgs e)
        {
            m_functionSelection = "Real-time Parameters Calculation";
            Input_Data_Acquisition.Enabled = false;
        }

        // Data Source Selection and Data Collection
        private void Real_Time_Data_Selection_CheckedChanged(object sender, EventArgs e)
        {
            m_dataSource = "Real-time Data";
            Data_Collection_Settings.Enabled = false;
            Messages.Enabled = true;
        }

        private void Historical_Data_Selection_CheckedChanged(object sender, EventArgs e)
        {
            m_dataSource = "Historical Data";
            Data_Collection_Settings.Enabled = true;
            Messages.Enabled = true;
        }

        public void ShowUpdateMessage(string message)
        {
            if (m_functionStopFlag)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowUpdateMessage), message);
            }
            else
            {
                StringBuilder outputText = new StringBuilder();

                outputText.AppendLine(message);
                outputText.AppendLine();

                lock (textBoxMessages)
                    textBoxMessages.AppendText(outputText.ToString());

                m_log.Publish(MessageLevel.Info, "StatusMessage", message);
            }
        }

        public void ClearUpdateMessages()
        {
            if (m_functionStopFlag)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearUpdateMessages));
            }
            else
            {
                lock (textBoxMessages)
                    textBoxMessages.Text = "";
            }
        }

        private void FormElementChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs>(FormElementChanged), sender, e);
            }
            else
            {
                if (Visible)
                    m_trendDataSettings?.UpdateProperties();
            }
        }

        private void SetTrendDataButtonEnabledState(bool enabled)
        {
            if (m_functionStopFlag)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<bool>(SetTrendDataButtonEnabledState), enabled);
            else
                Trend_Data_Button.Enabled = enabled;
        }

        private void UpdateProgressBar(int value)
        {
            if (m_functionStopFlag)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                if (value < progressBarTrendData.Minimum)
                    value = progressBarTrendData.Minimum;

                if (value > progressBarTrendData.Maximum)
                    progressBarTrendData.Maximum = value;

                progressBarTrendData.Value = value;
            }
        }

        private void SetProgressBarMaximum(int maximum)
        {
            if (m_functionStopFlag)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<int>(SetProgressBarMaximum), maximum);
            else
                progressBarTrendData.Maximum = maximum;
        }

        private void Trend_Data_Button_Click(object sender, EventArgs e)
        {            
            m_openECARawDataSet = new openECARawDataSet();

            SetTrendDataButtonEnabledState(false);
            
            UpdateProgressBar(0);
            SetProgressBarMaximum(100);

            // Kick off a thread to start archive read passing in current config file settings
            new Thread(ReadArchive) { IsBackground = true }.Start();
            
        }

        public void ReadArchive(object state)
        {
            Application.UseWaitCursor = true;

            try
            {
                //m_trendDataSettings.EndTime  = 
                double timeRange = (m_trendDataSettings.EndTime - m_trendDataSettings.StartTime).TotalSeconds;
                long receivedPoints = 0;
                long processedDataBlocks = 0;
                long duplicatePoints = 0;
                Ticks operationTime;
                Ticks operationStartTime;
                DataPoint point = new DataPoint();
                CollectingAlgorithm collectionExecution = new CollectingAlgorithm();

                collectionExecution.ShowMessage =  ShowUpdateMessage;
                collectionExecution.MessageInterval = m_trendDataSettings.MessageInterval;
                collectionExecution.Log = m_log;

                // Load historian meta-data
                ShowUpdateMessage(">>> Loading source connection metadata...");

                operationStartTime = DateTime.UtcNow.Ticks;
                collectionExecution.Metadata = MetadataRecord.Query(m_trendDataSettings.HostAddress, m_trendDataSettings.MetadataPort, m_trendDataSettings.MetadataTimeout);
                operationTime = DateTime.UtcNow.Ticks - operationStartTime;

                ShowUpdateMessage("*** Metadata Load Complete ***");
                ShowUpdateMessage($"Total metadata load time {operationTime.ToElapsedTimeString(3)}...");

                ShowUpdateMessage(">>> Processing filter expression for metadata...");

                operationStartTime = DateTime.UtcNow.Ticks;
                MeasurementKey[] inputKeys = AdapterBase.ParseInputMeasurementKeys(MetadataRecord.Metadata, false, m_trendDataSettings.PointList, "MeasurementDetail");
                IEnumerable<ulong> pointIDList = inputKeys.Select(key => (ulong)(key.ID + 318));
                //IEnumerable<ulong> pointIDList = inputKeys.Select(key => (ulong)(key.ID));

                ShowUpdateMessage($">>> Historian read will be for {inputKeys.Length:N0} points based on provided meta-data expression.");

                ShowUpdateMessage("*** Filter Expression Processing Complete ***");
                ShowUpdateMessage($"Total filter expression processing time {operationTime.ToElapsedTimeString(3)}...");

                ShowUpdateMessage(">>> Reading archive...");

                // Start historian data read
                operationStartTime = DateTime.UtcNow.Ticks;

                using (SnapDBClient historianClient = new SnapDBClient(m_trendDataSettings.HostAddress, m_trendDataSettings.DataPort, m_trendDataSettings.InstanceName, m_trendDataSettings.StartTime, m_trendDataSettings.EndTime, m_trendDataSettings.FrameRate, pointIDList))
                {
                    // Scan to first record
                    if (!historianClient.ReadNext(point))
                        throw new InvalidOperationException("No data for specified time range in openHistorian connection!");

                    ulong currentTimestamp;
                    receivedPoints++;

                    while (!m_functionInterruptFlag)
                    {
                        int timeComparison;
                        bool readSuccess = true;

                        // Create a new data block for current timestamp and load first/prior point
                        Dictionary<ulong, DataPoint> dataBlock = new Dictionary<ulong, DataPoint>
                        {
                            [point.PointID] = point.Clone()
                        };

                        currentTimestamp = point.Timestamp;
                        //DateTime currentDateTime = new DateTime((long)currentTimestamp);
                        //string msg = "### Time Stamp " + currentDateTime.ToString() + $" collected. First point {point.PointID:N1} Received, {receivedPoints:N2} in total. ###";
                        //ShowUpdateMessage(msg);


                        // Load remaining data for current timestamp
                        do
                        {
                            // Scan to next record
                            if (!historianClient.ReadNext(point))
                            {
                                readSuccess = false;
                                break;
                            }

                            receivedPoints++;

                            timeComparison = DataPoint.CompareTimestamps(point.Timestamp, currentTimestamp, m_trendDataSettings.FrameRate);

                            if (timeComparison == 0)
                            {
                                // Timestamps are compared based on configured frame rate - if archived data rate is
                                // higher than configured frame rate, then data block will contain only latest values
                                if (dataBlock.ContainsKey(point.PointID))
                                    duplicatePoints++;

                                dataBlock[point.PointID] = point.Clone();
                                UpdateProgressBar((int)((1.0D - new Ticks(m_trendDataSettings.EndTime.Ticks - (long)point.Timestamp).ToSeconds() / timeRange) * 100.0D));


                                //ShowUpdateMessage($"### Point {point.PointID:N0} Received, {receivedPoints:N1} in total. ###");
                            }
                        }
                        while (timeComparison == 0);

                        if (++processedDataBlocks % m_trendDataSettings.MessageInterval == 0)
                        {
                            ShowUpdateMessage($"{Environment.NewLine}{receivedPoints:N0} points{(duplicatePoints > 0 ? $", which included {duplicatePoints:N0} duplicates," : "")} read so far averaging {receivedPoints / (DateTime.UtcNow.Ticks - operationStartTime).ToSeconds():N0} points per second.");
                            UpdateProgressBar((int)((1.0D - new Ticks(m_trendDataSettings.EndTime.Ticks - (long)point.Timestamp).ToSeconds() / timeRange) * 100.0D));
                        }

                        try
                        {
                            // Analyze data block
                            int LineNnumber = inputKeys.Length / 8;
                            collectionExecution.Execute(new DateTime((long)currentTimestamp), dataBlock.Values.ToArray(), LineNnumber);
                        }
                        catch (Exception ex)
                        {
                            ShowUpdateMessage($"ERROR: Algorithm exception: {ex.Message}");
                            m_log.Publish(MessageLevel.Error, "AlgorithmError", "Failed while processing data from the historian", exception: ex);
                        }

                        // Finished with data read
                        if (!readSuccess)
                        {
                            ShowUpdateMessage(">>> End of data in range encountered...");
                            ShowUpdateMessage("*** Historian Read Complete ***");
                            m_openECARawDataSet = collectionExecution.RawDataSet;
                            UpdateProgressBar(100);
                            break;
                        }                        
                    }

                    operationTime = DateTime.UtcNow.Ticks - operationStartTime;

                    // Show some operational statistics
                    long expectedPoints = (long)(timeRange * m_trendDataSettings.FrameRate * inputKeys.Length);
                    double dataCompleteness = Math.Round(receivedPoints / (double)expectedPoints * 100000.0D) / 1000.0D;

                    string overallSummary =
                        $"Total read time {operationTime.ToElapsedTimeString(3)} at {receivedPoints / operationTime.ToSeconds():N0} points per second.{Environment.NewLine}" +
                        $"{Environment.NewLine}" +
                        $"           Meta-data points: {collectionExecution.Metadata.Count}{Environment.NewLine}" +
                        $"          Time-span covered: {timeRange:N0} seconds: {Ticks.FromSeconds(timeRange).ToElapsedTimeString(2)}{Environment.NewLine}" +
                        $"       Processed timestamps: {processedDataBlocks:N0}{Environment.NewLine}" +
                        $"            Expected points: {expectedPoints:N0} @ {m_trendDataSettings.FrameRate:N0} samples per second{Environment.NewLine}" +
                        $"            Received points: {receivedPoints:N0}{Environment.NewLine}" +
                        $"           Duplicate points: {duplicatePoints:N0}{Environment.NewLine}" +
                        $"          Data completeness: {dataCompleteness:N3}%{Environment.NewLine}";

                    ShowUpdateMessage(overallSummary);
                    ShowUpdateMessage(">>> Historical Data Collection Completed!");
                }
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"!!! Failure during historian read: {ex.Message}");
                m_log.Publish(MessageLevel.Error, "HistorianDataRead", "Failed while reading data from the historian", exception: ex);
            }
            finally
            {
                SetTrendDataButtonEnabledState(true);
            }

            Application.UseWaitCursor = false;

        }

        // Function Implementation
        private void Launch_Button_Click(object sender, EventArgs e)
        {
            ShowUpdateMessage($">>> {m_functionSelection} Functionality Launched!");

            Launch_Button.Enabled = false;
            Export.Enabled = false;
            m_functionsLaunchedFlag = true;

            string FunctionsSeperation = "%-----------------------------------------------------------------------------------------%";

            if (m_functionSelection == "Real-time Parameters Calculation")
            {

                //ShowUpdateMessage(">>> Real-time Data Streaming ... ");    
                Generate_TextBoxes_System_Scheme();

                Cancel_Button.Enabled = true;

                InitializeBackgroundWorker();

                backgroundWorkerForCancel.RunWorkerAsync();

                Launch_Button.Enabled = true;

                ShowUpdateMessage(FunctionsSeperation);
            }
            else
            {
                //Check Data Collection from openHistorian, originally from openECA
                if (m_dataSource == "Historical Data")
                {
                    if (m_openECARawDataSet.TimeStamps[0].Year == 1969)
                    {
                        MessageBox.Show("Error! NO Data Collected!");
                    }
                    else
                    {
                        // Original VI measurements data set
                        VIDataSet PosSeqVIDataSet = new VIDataSet(m_systemTopology);
                        PosSeqVIDataSet.openECADataIntegration(m_openECARawDataSet);
                        //PosSeqVIDataSet.VIDataset_to_dictionary();
                        ShowUpdateMessage(">>> Historical Data Processing Completed!");

                        if (m_functionSelection == "CT/PT Calibration")
                        {
                            // CT PT Calibration
                            ShowUpdateMessage(">>> Starting CT / PT Calibration ...");
                            CTPTCalibration CTPTCalibrationCurrentSystem = new CTPTCalibration(m_systemTopology);
                            CTPTCalibrationCurrentSystem.CurrentSystem = PosSeqVIDataSet;
                            //CTPTCalibrationCurrentSystem.CurrentSystem.openECADataIntegration(m_openECARawDataSet);
                            CTPTCalibrationCurrentSystem.CurrentSystem.VIDataset_to_dictionary();

                            CTPTCalibrationCurrentSystem.CTPTCalibrationAlgorithm(textBoxMessages);
                            Results_Demonstration_Table(CTPTCalibrationCurrentSystem.K);

                            Export.Enabled = true;

                            Launch_Button.Enabled = true;

                            ShowUpdateMessage(FunctionsSeperation);

                        }
                        else if (m_functionSelection == "Line Parameters Estimation")
                        {
                            // Line Parameter Estimation
                            ShowUpdateMessage(">>> Starting Transmission Line Parameter Estimation ...");
                            LineImpedanceEstimation LineImpedanceEstimationCurrentSystem = new LineImpedanceEstimation(m_systemTopology);
                            LineImpedanceEstimationCurrentSystem.CurrentSystem = PosSeqVIDataSet;
                            //LineImpedanceEstimationCurrentSystem.CurrentSystem.openECADataIntegration(m_openECARawDataSet);
                            LineImpedanceEstimationCurrentSystem.CurrentSystem.VIDataset_to_dictionary();

                            LineImpedanceEstimationCurrentSystem.LineImpedanceEstimationAlgorithm(textBoxMessages);
                            Results_Demonstration_Table(LineImpedanceEstimationCurrentSystem.K);

                            Export.Enabled = true;

                            Launch_Button.Enabled = true;

                            ShowUpdateMessage(FunctionsSeperation);
                        }
                        else
                        {
                            MessageBox.Show("Error: No Valid Functionality Selected!");
                        }
                    }
                }
                else if (m_dataSource == "Real-time Data")
                {
                    m_openECARawDataSet = new openECARawDataSet();

                    int TotalFrameNumber = 1800;
                    
                    ShowUpdateMessage(">>> Real-time Sreaming Data Collection Starts.");

                    for (int idx0 = 0; idx0 < TotalFrameNumber; idx0++)
                    {
                        UpdateCurrentFrame();

                        Measurement_set CurrentFrame_local = ConvertFrameFromModelToLocalModel(CurrentFrame, m_systemTopology);

                        if (m_openECARawDataSet.RawDataSet[0].Measurements.Count() == 1)
                        {
                            m_openECARawDataSet.RawDataSet[0] = CurrentFrame_local;
                            m_openECARawDataSet.TimeStamps[0] = DateTime.Now;

                            string CollectionStartTime = "Start Time: " + m_openECARawDataSet.TimeStamps[0].ToString();
                            ShowUpdateMessage(CollectionStartTime);
                        }
                        else
                        {
                            m_openECARawDataSet.RawDataSet.Add(CurrentFrame_local);
                            m_openECARawDataSet.TimeStamps.Add(DateTime.Now);
                        }                        
                        Thread.Sleep(33);                        
                    }

                    string CollectionEndTime = "End Time: " + m_openECARawDataSet.TimeStamps[TotalFrameNumber - 1].ToString();
                    ShowUpdateMessage(CollectionEndTime);

                    // Original VI measurements data set
                    VIDataSet PosSeqVIDataSet = new VIDataSet(m_systemTopology);
                    PosSeqVIDataSet.openECADataIntegration(m_openECARawDataSet);
                    //PosSeqVIDataSet.VIDataset_to_dictionary();
                    ShowUpdateMessage(">>> Real-time Sreaming Data Processing Completed!");

                    if (m_functionSelection == "CT/PT Calibration")
                    {
                        // CT PT Calibration
                        ShowUpdateMessage(">>> Starting CT / PT Calibration ...");
                        CTPTCalibration CTPTCalibrationCurrentSystem = new CTPTCalibration(m_systemTopology);
                        CTPTCalibrationCurrentSystem.CurrentSystem = PosSeqVIDataSet;
                        //CTPTCalibrationCurrentSystem.CurrentSystem.openECADataIntegration(m_openECARawDataSet);
                        CTPTCalibrationCurrentSystem.CurrentSystem.VIDataset_to_dictionary();

                        CTPTCalibrationCurrentSystem.CTPTCalibrationAlgorithm(textBoxMessages);
                        Results_Demonstration_Table(CTPTCalibrationCurrentSystem.K);

                        Export.Enabled = true;

                        Launch_Button.Enabled = true;

                        ShowUpdateMessage(FunctionsSeperation);

                    }
                    else if (m_functionSelection == "Line Parameters Estimation")
                    {
                        // Line Parameter Estimation
                        ShowUpdateMessage(">>> Starting Transmission Line Parameter Estimation ...");
                        LineImpedanceEstimation LineImpedanceEstimationCurrentSystem = new LineImpedanceEstimation(m_systemTopology);
                        LineImpedanceEstimationCurrentSystem.CurrentSystem = PosSeqVIDataSet;
                        //LineImpedanceEstimationCurrentSystem.CurrentSystem.openECADataIntegration(m_openECARawDataSet);
                        LineImpedanceEstimationCurrentSystem.CurrentSystem.VIDataset_to_dictionary();

                        LineImpedanceEstimationCurrentSystem.LineImpedanceEstimationAlgorithm(textBoxMessages);
                        Results_Demonstration_Table(LineImpedanceEstimationCurrentSystem.K);

                        Export.Enabled = true;

                        Launch_Button.Enabled = true;

                        ShowUpdateMessage(FunctionsSeperation);
                    }
                    else
                    {
                        MessageBox.Show("Error: No Valid Functionality Selected!");
                    }

                }
                else
                {
                    MessageBox.Show("Error: No Valid Input Data!");
                }
            }
        }

        public void MainWindowThreadStart()
        {
            try
            {
                Framework framework = FrameworkFactory.Create();
                Algorithm.API = new Hub(framework);

                MainWindow mainWindow = new MainWindow(framework);
                mainWindow.Text = "C# Beta_Application_CTPT_LineZ";
                Application.Run(mainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateCurrentFrame()
        {
            CurrentFrame = Algorithm.CurrentFrame;
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            if (m_functionsLaunchedFlag)
            {
                backgroundWorkerForCancel.CancelAsync();

                System_Scheme.Controls.Clear();
                System_Scheme.Paint += new PaintEventHandler(System_Scheme_Paint);

                Cancel_Button.Enabled = false;
                Launch_Button.Enabled = true;
            }


            //if (InvokeRequired)
            //{
            //    BeginInvoke(new Action<object, EventArgs>(Cancel_Button_Click), sender, e);
            //}
            //else
            //{
            //    try
            //    {
            //        m_functionStopFlag = true;

            //        // Save current window size/location
            //        this.SaveLayout();

            //        // Save any updates to current screen values
            //        m_trendDataSettings.Save();
            //    }
            //    catch (Exception ex)
            //    {
            //        m_log.Publish(MessageLevel.Error, "FormClosing", "Failed while saving settings", exception: ex);
            //    }
            //}

        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Worksheets (*.xls)|*.xls";
            sfd.FileName = m_functionSelection + "_Results.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Copy DataGridView results to clipboard
                copyAlltoClipboard();

                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Excel.Application();

                xlexcel.DisplayAlerts = false; // Without this you will get two confirm overwrite prompts
                Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // Paste clipboard results to worksheet range
                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                // Save the excel file under the captured location from the SaveFileDialog
                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlexcel.DisplayAlerts = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlexcel.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlexcel);

                // Clear Clipboard and DataGridView selection
                Clipboard.Clear();
                Results_Demonstration_GridView.ClearSelection();

                // Open the newly saved excel file
                if (File.Exists(sfd.FileName))
                    System.Diagnostics.Process.Start(sfd.FileName);
            }

            Export.Enabled = false;
        }

        private void copyAlltoClipboard()
        {
            Results_Demonstration_GridView.RowHeadersVisible = false;
            Results_Demonstration_GridView.SelectAll();
            
            DataObject dataObj = Results_Demonstration_GridView.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void backgroundWorkerForCancel_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            do
            {
                UpdateCurrentFrame();

                m_lineParameters = new Complex[m_systemTopology.LineSetCalibrate.Count(), 3];
                m_lineParameters = LineParameterComputation_BackgroundWorker(CurrentFrame, m_systemTopology, worker, e);

                //System_Scheme.Controls.Clear();
                //System_Scheme.Paint += new PaintEventHandler(System_Scheme_Paint);               
                Thread.Sleep(500);

                e.Result = m_lineParameters;
            }
            while (!m_functionStopFlag);


        }

        private void backgroundWorkerForCancel_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Results_Demonstration_System_Scheme_line_Parameters(m_lineParameters);
        }

        private void backgroundWorkerForCancel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                ShowUpdateMessage("Canceled.");
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                ShowUpdateMessage("Function Completed.");
            }

            Launch_Button.Enabled = true;
            Cancel_Button.Enabled = false;
        }
        #endregion

        #region [ Static ]

        // Static Constructor
        static BetaAppUI()
        {
            // Set default logging path
            Logger.FileWriter.SetPath(FilePath.GetAbsolutePath(""), VerboseLevel.Ultra);
        }

        public static Complex[,] LineParameterComputation(Model.GPA.Measurement_set CurrentFrameData, NetworkTopology CurrentSystem)
        {
            Complex KV1 = 1;
            Complex KI1 = 1;
            Complex KV2 = 1;
            Complex KI2 = 1;

            Complex[,] LineParameters = new Complex[CurrentSystem.LineSetCalibrate.Count(), 3];

            int ResultIndex = 0;

            for (int idx0 = 0; idx0 < CurrentSystem.LineSetCalibrate.Count(); idx0++)
            {
                int CurrentLineNumberIndex = idx0;
                int CurrentLineNumber = CurrentLineNumberIndex + 1;

                double V1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Magnitude;
                double V1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Angle;
                Complex V1 = Complex.FromPolarCoordinates(V1M, V1A * Math.PI / 180);

                double I1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Magnitude;
                double I1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Angle;
                Complex I1 = Complex.FromPolarCoordinates(I1M, I1A * Math.PI / 180);


                double V2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Magnitude;
                double V2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Angle;
                Complex V2 = Complex.FromPolarCoordinates(V2M, V2A * Math.PI / 180);

                double I2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Magnitude;
                double I2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Angle;
                Complex I2 = Complex.FromPolarCoordinates(I2M, I2A * Math.PI / 180);

                Complex[] LineParameterResults = SingleLineImpedanceComputation(V1, KV1, I1, KI1, V2, KV2, I2, KI2);

                LineParameters[ResultIndex, 0] = CurrentLineNumber;
                LineParameters[ResultIndex, 1] = LineParameterResults[0];
                LineParameters[ResultIndex, 2] = LineParameterResults[1];

                ResultIndex += 1;

            }

            return LineParameters;
        }
        public static Complex[,] LineParameterComputation_BackgroundWorker(Model.GPA.Measurement_set CurrentFrameData, NetworkTopology CurrentSystem, BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return null;
            }
            else
            {
                Complex KV1 = 1;
                Complex KI1 = 1;
                Complex KV2 = 1;
                Complex KI2 = 1;

                Complex[,] LineParameters = new Complex[CurrentSystem.LineSetCalibrate.Count(), 3];

                int ResultIndex = 0;

                for (int idx0 = 0; idx0 < CurrentSystem.LineSetCalibrate.Count(); idx0++)
                {
                    int CurrentLineNumber = CurrentSystem.LineSetCalibrate[idx0];
                    int CurrentLineNumberIndex = CurrentSystem.LineSetCalibrate[idx0] - 1;

                    // For DVP system
                    if (CurrentSystem.LineSetCalibrate.Count() > 11)
                    {
                        CurrentLineNumberIndex = CurrentLineNumberIndex + 24;
                    }

                    double V1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Magnitude;
                    double V1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Angle;
                    Complex V1 = Complex.FromPolarCoordinates(V1M, V1A * Math.PI / 180);

                    double I1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Magnitude;
                    double I1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Angle;
                    Complex I1 = Complex.FromPolarCoordinates(I1M, I1A * Math.PI / 180);


                    double V2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Magnitude;
                    double V2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Angle;
                    Complex V2 = Complex.FromPolarCoordinates(V2M, V2A * Math.PI / 180);

                    double I2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Magnitude;
                    double I2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Angle;
                    Complex I2 = Complex.FromPolarCoordinates(I2M, I2A * Math.PI / 180);

                    Complex[] LineParameterResults = SingleLineImpedanceComputation(V1, KV1, I1, KI1, V2, KV2, I2, KI2);
                    
                    LineParameters[ResultIndex, 0] = CurrentLineNumber;
                    LineParameters[ResultIndex, 1] = LineParameterResults[0];
                    LineParameters[ResultIndex, 2] = LineParameterResults[1];

                    ResultIndex += 1;

                }

                int ProgressReached = 1;
                worker.ReportProgress(ProgressReached);

                return LineParameters;
            }
        }

        public static Complex[] SingleLineImpedanceComputation(Complex V1, Complex KV1, Complex I1,
            Complex KI1, Complex V2, Complex KV2, Complex I2, Complex KI2)
        /* Compute the real-time impedance and susceptance of the perticular line based on known conditions*/
        {
            Complex Vs = V1 * KV1;
            Complex Is = I1 * KI1;
            Complex Vr = V2 * KV2;
            Complex Ir = I2 * KI2;

            Complex Z = Complex.FromPolarCoordinates(double.NaN, double.NaN);
            Complex y = Complex.FromPolarCoordinates(double.NaN, double.NaN);

            if ((Vs != 0) && (Vr != 0) && (Is != 0) && (Ir != 0))            
            {
                Z = (Vs * Vs - Vr * Vr) / (Is * Vr - Ir * Vs);
                y = 2 * ((Is + Ir) / (Vs + Vr));
            }
            
            
            Complex[] results = new Complex[2] { Z, y.Imaginary };

            return results;
        }

        public static Measurement_set ConvertFrameFromModelToLocalModel(Model.GPA.Measurement_set CurrentFrame, NetworkTopology CurrentSystem)
        {
            Measurement_set CurrentFrame_local = new Measurement_set();

            int LineCount = CurrentFrame.Measurements.Count();

            int Start_measurement_line_count = 0;
            int End_measurement_line_count = 0;

            if (CurrentSystem.LineNum <= 30)
            {
                Start_measurement_line_count = 0;
                End_measurement_line_count = CurrentSystem.LineNum;
            }
            else
            {
                Start_measurement_line_count = 24;
                End_measurement_line_count = LineCount;
            }

            for (int idx0 = Start_measurement_line_count; idx0 < End_measurement_line_count; idx0++)
            {
                Line_data CurrentLineData = new Line_data();

                VI_data CurrentFromBus = new VI_data();
                VI_data CurrentToBus = new VI_data();

                Phasor V1 = new Phasor();
                Phasor I1 = new Phasor();
                Phasor V2 = new Phasor();
                Phasor I2 = new Phasor();

                V1.Magnitude = CurrentFrame.Measurements[idx0].From_bus.Voltage.Magnitude;
                V1.Angle = CurrentFrame.Measurements[idx0].From_bus.Voltage.Angle;
                I1.Magnitude = CurrentFrame.Measurements[idx0].From_bus.Current.Magnitude;
                I1.Angle = CurrentFrame.Measurements[idx0].From_bus.Current.Angle;
                CurrentFromBus.Voltage = V1;
                CurrentFromBus.Current = I1;
                CurrentLineData.From_bus = CurrentFromBus;

                V2.Magnitude = CurrentFrame.Measurements[idx0].To_bus.Voltage.Magnitude;
                V2.Angle = CurrentFrame.Measurements[idx0].To_bus.Voltage.Angle;
                I2.Magnitude = CurrentFrame.Measurements[idx0].To_bus.Current.Magnitude;
                I2.Angle = CurrentFrame.Measurements[idx0].To_bus.Current.Angle;
                CurrentToBus.Voltage = V2;
                CurrentToBus.Current = I2;
                CurrentLineData.To_bus = CurrentToBus;

                if (CurrentFrame_local.Measurements[0].From_bus == null)
                    CurrentFrame_local.Measurements[0] = CurrentLineData;
                else
                {
                    CurrentFrame_local.Measurements.Add(CurrentLineData);
                }                
            }

            return CurrentFrame_local;
        }

        #endregion

        #endregion

    }

    public static class WinFormsExtensions
    {
        public static void AppendLine(this RichTextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }

        public static void AppendLine(this TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
    }

    

}
