//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using GSF;
//using GSF.Diagnostics;
//using GSF.IO;
//using GSF.TimeSeries;
//using GSF.TimeSeries.Adapters;
//using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI;
//using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI.MetaData;
//using Beta_Application_CTPT_LineZ.Model;

//namespace Beta_Application_CTPT_LineZ.openHistorianDataCollection
//{
//    public class HistorianDataCollection
//    {
//        #region [ Members ]

//        // Fields
//        private readonly LogPublisher m_log;
//        private Settings m_settings;
//        private openECARawDataSet m_rawDataSet = new openECARawDataSet();
//        private bool m_functionInterruptFlag = false;

//        #endregion

//        #region [ Constructors ]

//        /// <summary>
//        /// Creates a new <see cref="HistorianDataWalker"/>.
//        /// </summary>
//        public HistorianDataCollection()
//        {
//            //InitializeComponent();
//            m_settings = new Settings();

//            Console.WriteLine("Use DEFAULT Settings? (Y/N): ");
//            string SettingsFlag = "Y";
//            SettingsFlag = Console.ReadLine();

//            if ((SettingsFlag != "Y") && (SettingsFlag != "y"))
//            {
//                Console.WriteLine("Please input QUERY settings:");
//                //Console.Write("Host Address: ");
//                //m_settings.HostAddress = Console.ReadLine();
//                //Console.Write("Data Port: ");
//                //m_settings.DataPort = Convert.ToInt32(Console.ReadLine());
//                //Console.Write("Metadata Port: ");
//                //m_settings.MetadataPort = Convert.ToInt32(Console.ReadLine());
//                //Console.Write("Instance Name: ");
//                //m_settings.InstanceName = Console.ReadLine();
//                //Console.Write("Frame Rate (frame/second): ");
//                //m_settings.MetadataPort = Convert.ToInt32(Console.ReadLine());
//                //Console.Write("Metadata Timeout: ");
//                //m_settings.MetadataTimeout = Convert.ToInt32(Console.ReadLine());
//                //m_settings.MetadataTimeoutSeconds = m_settings.MetadataTimeout;
//                Console.Write("Start Time (mm/dd/yyyy hh:mm:ss.000): ");
//                m_settings.StartTime = Convert.ToDateTime(Console.ReadLine());
//                Console.Write("End Time (mm/dd/yyyy hh:mm:ss.000): ");
//                m_settings.EndTime = Convert.ToDateTime(Console.ReadLine());
//                //Console.Write("Point List or Filter Expression: ");
//                //m_settings.PointList = Console.ReadLine();
//                //Console.Write("Message Interval: ");
//                //m_settings.MessageInterval = Convert.ToInt32(Console.ReadLine());
//                //Console.Write("Enable Logging(Y/N): ");
//                //m_settings.EnableLogging = Convert.ToBoolean(Console.ReadLine());
//            }


//            // Create a new log publisher instance
//            m_log = Logger.CreatePublisher(typeof(HistorianDataCollection), MessageClass.Application);
//        }

//        #endregion

//        #region [ Properties ]
//        public openECARawDataSet RawDataSet
//        {
//            get
//            {
//                return m_rawDataSet;
//            }
//        }
//        #endregion

//        #region [ Methods ]

//        // Form Event Handlers

//        //private void HistorianDataWalker_Load(object sender, EventArgs e)
//        //{
//        //    try
//        //    {
//        //        // Load current settings registering a symbolic reference to this form instance for use by default value expressions
//        //        m_settings = new Settings();

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        m_log.Publish(MessageLevel.Error, "FormLoad", "Failed while loading settings", exception: ex);
//        //    }
//        //}

//        //private void HistorianDataWalker_FormClosing(object sender, FormClosingEventArgs e)
//        //{
//        //    try
//        //    {
//        //        m_formClosing = true;

//        //        // Save current window size/location
//        //        this.SaveLayout();

//        //        // Save any updates to current screen values
//        //        m_settings.Save();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        m_log.Publish(MessageLevel.Error, "FormClosing", "Failed while saving settings", exception: ex);
//        //    }
//        //}

//        //private void buttonGo_Click(object sender, EventArgs e)
//        //{
//        //    SetGoButtonEnabledState(false);
//        //    ClearUpdateMessages();
//        //    UpdateProgressBar(0);
//        //    SetProgressBarMaximum(100);

//        //    // Kick off a thread to start archive read passing in current config file settings
//        //    new Thread(ReadArchive) { IsBackground = true }.Start();
//        //}

//        //// Form Element Accessors -- these functions allow access to form elements from non-UI threads

//        //private void FormElementChanged(object sender, EventArgs e)
//        //{
//        //    if (InvokeRequired)
//        //    {
//        //        BeginInvoke(new Action<object, EventArgs>(FormElementChanged), sender, e);
//        //    }
//        //    else
//        //    {
//        //        if (Visible)
//        //            m_settings?.UpdateProperties();
//        //    }
//        //}

//        //private void ShowUpdateMessage(string message)
//        //{
//        //    if (m_formClosing)
//        //        return;

//        //    if (InvokeRequired)
//        //    {
//        //        BeginInvoke(new Action<string>(ShowUpdateMessage), message);
//        //    }
//        //    else
//        //    {
//        //        StringBuilder outputText = new StringBuilder();

//        //        outputText.AppendLine(message);
//        //        outputText.AppendLine();

//        //        lock (textBoxMessageOutput)
//        //            textBoxMessageOutput.AppendText(outputText.ToString());

//        //        m_log.Publish(MessageLevel.Info, "StatusMessage", message);
//        //    }
//        //}

//        //private void ClearUpdateMessages()
//        //{
//        //    if (m_formClosing)
//        //        return;

//        //    if (InvokeRequired)
//        //    {
//        //        BeginInvoke(new Action(ClearUpdateMessages));
//        //    }
//        //    else
//        //    {
//        //        lock (textBoxMessageOutput)
//        //            textBoxMessageOutput.Text = "";
//        //    }
//        //}

//        //private void SetGoButtonEnabledState(bool enabled)
//        //{
//        //    if (m_formClosing)
//        //        return;

//        //    if (InvokeRequired)
//        //        BeginInvoke(new Action<bool>(SetGoButtonEnabledState), enabled);
//        //    else
//        //        buttonGo.Enabled = enabled;
//        //}

//        //private void UpdateProgressBar(int value)
//        //{
//        //    if (m_formClosing)
//        //        return;

//        //    if (InvokeRequired)
//        //    {
//        //        BeginInvoke(new Action<int>(UpdateProgressBar), value);
//        //    }
//        //    else
//        //    {
//        //        if (value < progressBar.Minimum)
//        //            value = progressBar.Minimum;

//        //        if (value > progressBar.Maximum)
//        //            progressBar.Maximum = value;

//        //        progressBar.Value = value;
//        //    }
//        //}

//        //private void SetProgressBarMaximum(int maximum)
//        //{
//        //    if (m_formClosing)
//        //        return;

//        //    if (InvokeRequired)
//        //        BeginInvoke(new Action<int>(SetProgressBarMaximum), maximum);
//        //    else
//        //        progressBar.Maximum = maximum;
//        //}

//        // Internal Functions

//        public void ReadArchive()
//        {
//            try
//            {
//                //m_settings.EndTime  = 
//                double timeRange = (m_settings.EndTime - m_settings.StartTime).TotalSeconds;
//                long receivedPoints = 0;
//                long processedDataBlocks = 0;
//                long duplicatePoints = 0;
//                Ticks operationTime;
//                Ticks operationStartTime;
//                DataPoint point = new DataPoint();
//                CollectingAlgorithm collectionExecution = new CollectingAlgorithm();

//                //algorithm.ShowMessage =  ShowUpdateMessage;
//                collectionExecution.MessageInterval = m_settings.MessageInterval;
//                collectionExecution.Log = m_log;

//                // Load historian meta-data
//                Console.WriteLine(">>> Loading source connection metadata...");

//                operationStartTime = DateTime.UtcNow.Ticks;
//                collectionExecution.Metadata = MetadataRecord.Query(m_settings.HostAddress, m_settings.MetadataPort, m_settings.MetadataTimeout);
//                operationTime = DateTime.UtcNow.Ticks - operationStartTime;

//                Console.WriteLine("*** Metadata Load Complete ***");
//                Console.WriteLine($"Total metadata load time {operationTime.ToElapsedTimeString(3)}...");

//                Console.WriteLine(">>> Processing filter expression for metadata...");

//                operationStartTime = DateTime.UtcNow.Ticks;
//                MeasurementKey[] inputKeys = AdapterBase.ParseInputMeasurementKeys(MetadataRecord.Metadata, false, m_settings.PointList, "MeasurementDetail");
//                IEnumerable<ulong> pointIDList = inputKeys.Select(key => (ulong)key.ID);
//                operationTime = DateTime.UtcNow.Ticks - operationStartTime;

//                Console.WriteLine($">>> Historian read will be for {inputKeys.Length:N0} points based on provided meta-data expression.");

//                Console.WriteLine("*** Filter Expression Processing Complete ***");
//                Console.WriteLine($"Total filter expression processing time {operationTime.ToElapsedTimeString(3)}...");

//                Console.WriteLine(">>> Reading archive...");

//                // Start historian data read
//                operationStartTime = DateTime.UtcNow.Ticks;

//                using (SnapDBClient historianClient = new SnapDBClient(m_settings.HostAddress, m_settings.DataPort, m_settings.InstanceName, m_settings.StartTime, m_settings.EndTime, m_settings.FrameRate, pointIDList))
//                {
//                    // Scan to first record
//                    if (!historianClient.ReadNext(point))
//                        throw new InvalidOperationException("No data for specified time range in openHistorian connection!");

//                    ulong currentTimestamp;
//                    receivedPoints++;

//                    while (!m_functionInterruptFlag)
//                    {
//                        int timeComparison;
//                        bool readSuccess = true;

//                        // Create a new data block for current timestamp and load first/prior point
//                        Dictionary<ulong, DataPoint> dataBlock = new Dictionary<ulong, DataPoint>
//                        {
//                            [point.PointID] = point.Clone()
//                        };

//                        currentTimestamp = point.Timestamp;
//                        //DateTime currentDateTime = new DateTime((long)currentTimestamp);
//                        //string msg = "### Time Stamp " + currentDateTime.ToString() + $" collected. First point {point.PointID:N1} Received, {receivedPoints:N2} in total. ###";
//                        //Console.WriteLine(msg);


//                        // Load remaining data for current timestamp
//                        do
//                        {
//                            // Scan to next record
//                            if (!historianClient.ReadNext(point))
//                            {
//                                readSuccess = false;
//                                break;
//                            }

//                            receivedPoints++;

//                            timeComparison = DataPoint.CompareTimestamps(point.Timestamp, currentTimestamp, m_settings.FrameRate);

//                            if (timeComparison == 0)
//                            {
//                                // Timestamps are compared based on configured frame rate - if archived data rate is
//                                // higher than configured frame rate, then data block will contain only latest values
//                                if (dataBlock.ContainsKey(point.PointID))
//                                    duplicatePoints++;

//                                dataBlock[point.PointID] = point.Clone();


//                                //Console.WriteLine($"### Point {point.PointID:N0} Received, {receivedPoints:N1} in total. ###");
//                            }
//                        }
//                        while (timeComparison == 0);

//                        // Finished with data read
//                        if (!readSuccess)
//                        {
//                            Console.WriteLine(">>> End of data in range encountered...");
//                            Console.WriteLine("*** Historian Read Complete ***");
//                            m_rawDataSet = collectionExecution.RawDataSet;
//                            break;
//                        }

//                        if (++processedDataBlocks % m_settings.MessageInterval == 0)
//                        {
//                            Console.WriteLine($"{Environment.NewLine}{receivedPoints:N0} points{(duplicatePoints > 0 ? $", which included {duplicatePoints:N0} duplicates," : "")} read so far averaging {receivedPoints / (DateTime.UtcNow.Ticks - operationStartTime).ToSeconds():N0} points per second.");
//                        }

//                        try
//                        {
//                            // Analyze data block
//                            collectionExecution.Execute(new DateTime((long)currentTimestamp), dataBlock.Values.ToArray());
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine($"ERROR: Algorithm exception: {ex.Message}");
//                            m_log.Publish(MessageLevel.Error, "AlgorithmError", "Failed while processing data from the historian", exception: ex);
//                        }
//                    }

//                    operationTime = DateTime.UtcNow.Ticks - operationStartTime;

//                    // Show some operational statistics
//                    long expectedPoints = (long)(timeRange * m_settings.FrameRate * inputKeys.Length);
//                    double dataCompleteness = Math.Round(receivedPoints / (double)expectedPoints * 100000.0D) / 1000.0D;

//                    string overallSummary =
//                        $"Total read time {operationTime.ToElapsedTimeString(3)} at {receivedPoints / operationTime.ToSeconds():N0} points per second.{Environment.NewLine}" +
//                        $"{Environment.NewLine}" +
//                        $"           Meta-data points: {collectionExecution.Metadata.Count}{Environment.NewLine}" +
//                        $"          Time-span covered: {timeRange:N0} seconds: {Ticks.FromSeconds(timeRange).ToElapsedTimeString(2)}{Environment.NewLine}" +
//                        $"       Processed timestamps: {processedDataBlocks:N0}{Environment.NewLine}" +
//                        $"            Expected points: {expectedPoints:N0} @ {m_settings.FrameRate:N0} samples per second{Environment.NewLine}" +
//                        $"            Received points: {receivedPoints:N0}{Environment.NewLine}" +
//                        $"           Duplicate points: {duplicatePoints:N0}{Environment.NewLine}" +
//                        $"          Data completeness: {dataCompleteness:N3}%{Environment.NewLine}";

//                    Console.WriteLine(overallSummary);
//                    //Console.ReadLine();
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"!!! Failure during historian read: {ex.Message}");
//                m_log.Publish(MessageLevel.Error, "HistorianDataRead", "Failed while reading data from the historian", exception: ex);
//            }
//            //finally
//            //{
//            //    SetGoButtonEnabledState(true);
//            //}
//        }

//        public static string GetRootTagName(string tagName)
//        {
//            int lastBangIndex = tagName.LastIndexOf('!');
//            return lastBangIndex > -1 ? tagName.Substring(lastBangIndex + 1).Trim() : tagName.Trim();
//        }

//        #endregion

//        #region [ Static ]

//        // Static Constructor
//        static HistorianDataCollection()
//        {
//            // Set default logging path
//            Logger.FileWriter.SetPath(FilePath.GetAbsolutePath(""), VerboseLevel.Ultra);
//        }

//        #endregion
//    }
//}
