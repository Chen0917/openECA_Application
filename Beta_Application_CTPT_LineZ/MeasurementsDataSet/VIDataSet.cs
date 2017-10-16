using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;
using Beta_Application_CTPT_LineZ.LocalModel;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class VIDataSet
    {
        #region [ Private Members ]

        private int m_framesPerSecond;                      // Cached frames per second
        private List<Frame> m_timeSeriesFrames;
        private Dictionary<string, double> m_vIDataDictionary;     // save data set into dictionary

        private List<string> m_busLibrary;                  // String array storing bus names, the bus number is the respective index + 1.
        private List<int> m_lineLibrary;                    // Int array storing line numbers
        private NetworkTopology m_network;                  // Network topology and raw data set configuration info
        #endregion

        #region [ Public Properties ]

        [XmlAttribute("FramesPerSecond")]
        public int FramesPerSecond
        {
            get
            {
                return m_framesPerSecond;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Frames per second must be greater than 0");

                m_framesPerSecond = value;
            }
        }

        [XmlArray("Frames")]
        public List<Frame> TimeSeriesFrames
        {
            get
            {
                return m_timeSeriesFrames;
            }
            set
            {
                m_timeSeriesFrames = value;
            }
        }

        public Dictionary<string, double> VIDataDictionary
        {
            get
            {
                return m_vIDataDictionary;
            }
        }

        [XmlArray("BusLibrary")]
        public List<string> BusLibrary
        {
            get
            {
                return m_busLibrary;
            }
            set
            {
                m_busLibrary = value;
            }
        }

        [XmlArray("LineLibrary")]
        public List<int> LineLibrary
        {
            get
            {
                return m_lineLibrary;
            }
            set
            {
                m_lineLibrary = value;
            }
        }

        public NetworkTopology Network
        {
            get
            {
                return m_network;
            }
        }
        #endregion

        #region [ Constructors ]

        public VIDataSet(NetworkTopology ConfiguredNetwork)
        {
            m_timeSeriesFrames = new List<Frame>();
            Frame Frame_temp = new Frame();
            m_timeSeriesFrames.Add(Frame_temp);

            m_framesPerSecond = 30;

            m_busLibrary = new List<string>();
            string busNameTemp = "NoBus";
            m_busLibrary.Add(busNameTemp);

            m_lineLibrary = new List<int>();
            m_lineLibrary.Add(-1);

            //string FilePath = null;
            //m_network = new NetworkTopology(FilePath);

            m_network = ConfiguredNetwork;

            m_vIDataDictionary = new Dictionary<string, double>();
        }

        #endregion

        #region [ Xml Serialization/Deserialization methods ]
        public void SerializeToXml(string pathName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(VIDataSet));

                TextWriter writer = new StreamWriter(pathName);

                serializer.Serialize(writer, this);

                writer.Close();
            }
            catch (Exception exception)
            {
                //throw new Exception("Failed to Serialzie");
                Console.WriteLine(exception);
                Console.WriteLine("Failed to Serialize.");
            }
        }

        public VIDataSet DeserializeFromXml(string pathname)
        {
            try
            {
                VIDataSet CurrentDataSet = null;

                XmlSerializer deserializer = new XmlSerializer(typeof(VIDataSet));

                StreamReader reader = new StreamReader(pathname);

                CurrentDataSet = (VIDataSet)deserializer.Deserialize(reader);

                reader.Close();

                return CurrentDataSet;
            }
            catch (Exception exception)
            {
                throw new Exception("failed to deserialzie");
            }
        }

        #endregion

        #region [ Methods ]
        public void AddFrameToTimeSeriesFrames(Frame SingleFrame)
        {
            try
            {
                if (m_timeSeriesFrames[0].TimeStamp == 0)
                {
                    m_timeSeriesFrames[0] = SingleFrame;
                }
                else
                {
                    m_timeSeriesFrames.Add(SingleFrame);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Need to be fixed.");
            }
        }

        public void openECADataIntegration(openECARawDataSet openECADataStreams)
        {
            int Frame_number = openECADataStreams.RawDataSet.Count;
            int Measurement_number = openECADataStreams.RawDataSet[0].Measurements.Count();

            for (int idx0 = 0; idx0 < Frame_number; idx0++) // select raw measurements by frame
            {
                Frame CurrentFrame = new Frame();
                CurrentFrame.TimeStamp = idx0 + 1;
                Measurement_set CurrentMeasurementSet = openECADataStreams.RawDataSet[idx0];

                for (int idx1 = 0; idx1 < Measurement_number; idx1++) // find measuremnents line by line
                {
                    int CurrentLineNumber = m_network.LineBusInfo[idx1, 0];
                    int CurrentFromBusNumber = m_network.LineBusInfo[idx1, 1];
                    int CurrentToBusNumber = m_network.LineBusInfo[idx1, 2];

                    //From bus voltage Mag and Ang
                    VIMeasurement CurrentMeasurementV1M = new VIMeasurement();
                    CurrentMeasurementV1M.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].From_bus.Voltage.Magnitude;
                    CurrentMeasurementV1M.TypeMeasured = VIMeasurementType.PositiveSequenceVoltageMagnitude;
                    CurrentMeasurementV1M.LineNumber = CurrentLineNumber;
                    CurrentMeasurementV1M.BusNumber = CurrentFromBusNumber;

                    VIMeasurement CurrentMeasurementV1A = new VIMeasurement();
                    CurrentMeasurementV1A.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].From_bus.Voltage.Angle;
                    CurrentMeasurementV1A.TypeMeasured = VIMeasurementType.PositiveSequenceVoltageAngle;
                    CurrentMeasurementV1A.LineNumber = CurrentLineNumber;
                    CurrentMeasurementV1A.BusNumber = CurrentFromBusNumber;

                    //From bus current Mag and Ang
                    VIMeasurement CurrentMeasurementI1M = new VIMeasurement();
                    CurrentMeasurementI1M.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].From_bus.Current.Magnitude;
                    CurrentMeasurementI1M.TypeMeasured = VIMeasurementType.PositiveSequenceCurrentMagnitude;
                    CurrentMeasurementI1M.LineNumber = CurrentLineNumber;
                    CurrentMeasurementI1M.BusNumber = CurrentFromBusNumber;

                    VIMeasurement CurrentMeasurementI1A = new VIMeasurement();
                    CurrentMeasurementI1A.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].From_bus.Current.Angle;
                    CurrentMeasurementI1A.TypeMeasured = VIMeasurementType.PositiveSequenceCurrentAngle;
                    CurrentMeasurementI1A.LineNumber = CurrentLineNumber;
                    CurrentMeasurementI1A.BusNumber = CurrentFromBusNumber;

                    //To bus voltage Mag and Ang
                    VIMeasurement CurrentMeasurementV2M = new VIMeasurement();
                    CurrentMeasurementV2M.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].To_bus.Voltage.Magnitude;
                    CurrentMeasurementV2M.TypeMeasured = VIMeasurementType.PositiveSequenceVoltageMagnitude;
                    CurrentMeasurementV2M.LineNumber = CurrentLineNumber;
                    CurrentMeasurementV2M.BusNumber = CurrentToBusNumber;

                    VIMeasurement CurrentMeasurementV2A = new VIMeasurement();
                    CurrentMeasurementV2A.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].To_bus.Voltage.Angle;
                    CurrentMeasurementV2A.TypeMeasured = VIMeasurementType.PositiveSequenceVoltageAngle;
                    CurrentMeasurementV2A.LineNumber = CurrentLineNumber;
                    CurrentMeasurementV2A.BusNumber = CurrentToBusNumber;

                    //To bus current Mag and Ang
                    VIMeasurement CurrentMeasurementI2M = new VIMeasurement();
                    CurrentMeasurementI2M.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].To_bus.Current.Magnitude;
                    CurrentMeasurementI2M.TypeMeasured = VIMeasurementType.PositiveSequenceCurrentMagnitude;
                    CurrentMeasurementI2M.LineNumber = CurrentLineNumber;
                    CurrentMeasurementI2M.BusNumber = CurrentToBusNumber;

                    VIMeasurement CurrentMeasurementI2A = new VIMeasurement();
                    CurrentMeasurementI2A.ValueMeasured = CurrentMeasurementSet.Measurements[idx1].To_bus.Current.Angle;
                    CurrentMeasurementI2A.TypeMeasured = VIMeasurementType.PositiveSequenceCurrentAngle;
                    CurrentMeasurementI2A.LineNumber = CurrentLineNumber;
                    CurrentMeasurementI2A.BusNumber = CurrentToBusNumber;

                    if (CurrentFrame.MeasurementsList[0].ValueMeasured == 0)
                    {
                        CurrentFrame.MeasurementsList[0] = CurrentMeasurementV1M;
                    }
                    else
                    {
                        CurrentFrame.MeasurementsList.Add(CurrentMeasurementV1M);
                    }
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementV1A);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementI1M);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementI1A);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementV2M);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementV2A);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementI2M);
                    CurrentFrame.MeasurementsList.Add(CurrentMeasurementI2A);

                }

                AddFrameToTimeSeriesFrames(CurrentFrame);
            }
        }

        public void VIDataset_to_dictionary()
        {
            foreach (Frame CurrentFrame in TimeSeriesFrames)
            {
                foreach (VIMeasurement CurrentMeasurement in CurrentFrame.MeasurementsList)
                {
                    double CurrentTValue = CurrentMeasurement.ValueMeasured;
                    string CurrentYKey = CurrentFrame.TimeStamp.ToString() + "L" + CurrentMeasurement.LineNumber.ToString() +
                                         "B" + CurrentMeasurement.BusNumber.ToString() + "T" + CurrentMeasurement.TypeMeasured.ToString();
                    m_vIDataDictionary.Add(CurrentYKey, CurrentTValue);
                }
            }
        }
        #endregion


    }
}
