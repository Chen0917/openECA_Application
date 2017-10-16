using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSF.Diagnostics;
using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI;
using Beta_Application_CTPT_LineZ.openHistorianDataCollection.HistorianAPI.MetaData;
using Beta_Application_CTPT_LineZ.LocalModel;


namespace Beta_Application_CTPT_LineZ.openHistorianDataCollection
{
    /// <summary>
    /// Defines algorithm to be executed during historian read.
    /// </summary>
    public class CollectingAlgorithm
    {
        #region [ Members ]

        // Meta-data fields
        private List<MetadataRecord> m_metadata;
        private ulong[] m_voltageMagnitudes;
        private ulong[] m_voltageAngles;
        private ulong[] m_currentMagnitudes;
        private ulong[] m_currentAngles;

        private string[] m_voltageMagnitudesDescriptions;
        private string[] m_voltageAnglesDescriptions;
        private string[] m_currentMagnitudesDescriptions;
        private string[] m_currentAnglesDescriptions;

        // Algorithm analytic fields
        private openECARawDataSet m_rawDataSet = new openECARawDataSet();

        // Algorithm processing statistic fields
        private long m_processedDataBlocks;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets UI message display function.
        /// </summary>
        public Action<string> ShowMessage { get; set; }

        /// <summary>
        /// Gets or sets current message display interval.
        /// </summary>
        public int MessageInterval { get; set; }

        /// <summary>
        /// Gets or sets current logging publisher.
        /// </summary>
        public LogPublisher Log { get; set; }

        /// <summary>
        /// Gets or sets received historian meta-data.
        /// </summary>
        public List<MetadataRecord> Metadata
        {
            get
            {
                return m_metadata;
            }
            set
            {
                // Cache meta-data in case algorithm needs it
                m_metadata = value;

                if ((object)m_metadata == null)
                {
                    m_voltageMagnitudes = new ulong[0];
                    m_currentMagnitudes = new ulong[0];
                }
                else
                {
                    // Load point ID for desired signal types
                    m_voltageMagnitudes = m_metadata
                        .Where(record => record.SignalAcronym == "VPHM") // Load point IDs for all Voltage Magnitudes
                        .Select(record => record.PointID)
                        .ToArray();
                    m_voltageMagnitudesDescriptions = m_metadata.Where(record => record.SignalAcronym == "VPHM")
                        .Select(record => record.Description)
                        .ToArray();

                    m_voltageAngles = m_metadata
                        .Where(record => record.SignalAcronym == "VPHA") // Load point IDs for all Voltage Phase Angles
                        .Select(record => record.PointID)
                        .ToArray();
                    m_voltageAnglesDescriptions = m_metadata.Where(record => record.SignalAcronym == "VPHA")
                        .Select(record => record.Description)
                        .ToArray();

                    m_currentMagnitudes = m_metadata
                        .Where(record => record.SignalAcronym == "IPHM") // Load point IDs for all Current Magnitudes
                        .Select(record => record.PointID)
                        .ToArray();
                    m_currentMagnitudesDescriptions = m_metadata.Where(record => record.SignalAcronym == "IPHM")
                        .Select(record => record.Description)
                        .ToArray();

                    m_currentAngles = m_metadata
                        .Where(record => record.SignalAcronym == "IPHA") // Load point IDs for all Current Phase Angles
                        .Select(record => record.PointID)
                        .ToArray();
                    m_currentAnglesDescriptions = m_metadata.Where(record => record.SignalAcronym == "IPHA")
                        .Select(record => record.Description)
                        .ToArray();
                }

            }
        }

        public openECARawDataSet RawDataSet
        {
            get
            {
                return m_rawDataSet;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Default data processing entry point for <see cref="Algorithm"/>.
        /// </summary>
        /// <param name="timestamp">Timestamp of <paramref name="dataBlock"/>.</param>
        /// <param name="dataBlock">Points values read at current timestamp.</param>
        public void Execute(DateTime timestamp, DataPoint[] raw_dataBlock, int lineNumber)
        {
            DataPoint[] dataBlock = raw_dataBlock;

            if (m_rawDataSet.TimeStamps[0].Year == 1969)
            {
                m_rawDataSet.TimeStamps[0] = timestamp;
            }
            else
            {
                m_rawDataSet.TimeStamps.Add(timestamp);
            }

            // Check dataBlock completeness
            if (dataBlock.Count() < (lineNumber * 8))
            {                
                for (int idx0 = 0; idx0 < (lineNumber * 8 - 1); idx0++)
                {
                    if (idx0 >= (dataBlock.Count()-1))
                    {
                        DataPoint[] TempDataBlock = new DataPoint[dataBlock.Count() + 1];

                        for (int idx1 = 0; idx1 < dataBlock.Count(); idx1++)
                        {
                            TempDataBlock[idx1] = dataBlock[idx1];
                        }

                        int idx2 = dataBlock.Count() - 1;
                        DataPoint TempPoint = new DataPoint();
                        TempPoint.PointID = dataBlock[idx2].PointID + 1;
                        TempPoint.Timestamp = dataBlock[idx2].Timestamp;
                        TempPoint.Value = 0;
                        TempPoint.Flags = dataBlock[idx2].Flags;
                        TempDataBlock[dataBlock.Count()] = TempPoint;

                        TempDataBlock[dataBlock.Count()] = TempPoint;
                        dataBlock = TempDataBlock;
                    }
                    else
                    {
                        if ((dataBlock[idx0 + 1].PointID != (dataBlock[idx0].PointID + 1)))
                        {
                            DataPoint TempPoint = new DataPoint();
                            TempPoint.PointID = dataBlock[idx0].PointID + 1;
                            TempPoint.Timestamp = dataBlock[idx0].Timestamp;
                            TempPoint.Value = 0;
                            TempPoint.Flags = dataBlock[idx0].Flags;

                            dataBlock = InsertPoint(dataBlock, TempPoint, (idx0 + 1), lineNumber);
                        }
                    }
                }
                //dataBlock = TempDataBlock;
            }

            int AssignValueCount = 0;
            Measurement_set CurrentFrameMeasurementSet = new Measurement_set();
            do
            {
                Phasor FromBusVTemp = new Phasor();
                Phasor FromBusITemp = new Phasor();
                Phasor ToBusVTemp = new Phasor();
                Phasor ToBusITemp = new Phasor();

                VI_data FromBusVITemp = new VI_data();
                VI_data ToBusVITemp = new VI_data();

                Line_data CurrentLineDataTemp = new Line_data();

                FromBusVTemp.Magnitude = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                FromBusVTemp.Angle = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                FromBusVITemp.Voltage = FromBusVTemp;

                FromBusITemp.Magnitude = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                FromBusITemp.Angle = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                FromBusVITemp.Current = FromBusITemp;

                ToBusVTemp.Magnitude = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                ToBusVTemp.Angle = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                ToBusVITemp.Voltage = ToBusVTemp;

                ToBusITemp.Magnitude = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                ToBusITemp.Angle = dataBlock[AssignValueCount].ValueAsSingle;
                AssignValueCount += 1;
                ToBusVITemp.Current = ToBusITemp;

                CurrentLineDataTemp.From_bus = FromBusVITemp;
                CurrentLineDataTemp.To_bus = ToBusVITemp;

                if (CurrentFrameMeasurementSet.Measurements[0].From_bus == null)
                {
                    CurrentFrameMeasurementSet.Measurements[0] = CurrentLineDataTemp;
                }
                else
                {
                    CurrentFrameMeasurementSet.Measurements.Add(CurrentLineDataTemp);
                }

            } while (AssignValueCount < (dataBlock.Count() - 7));

            if (m_rawDataSet.RawDataSet[0].Measurements[0].From_bus == null)
            {
                m_rawDataSet.RawDataSet[0] = CurrentFrameMeasurementSet;
            }
            else
            {
                m_rawDataSet.RawDataSet.Add(CurrentFrameMeasurementSet);
            }

            m_processedDataBlocks++;
            //string message = $"Analyzed {m_processedDataBlocks:N0} timestamps so far.{Environment.NewLine}";
            //Console.WriteLine(message);

        }

        public DataPoint[] InsertPoint(DataPoint[] dataBlock, DataPoint CurrentPoint, int Place, int lineNumber)
        {
            DataPoint[] TempDataBlock = new DataPoint[dataBlock.Count()+1];

            DataPoint TempPoint = new DataPoint();
            TempPoint.PointID = 0;
            TempPoint.Flags = 0;
            TempPoint.Timestamp = 0;
            TempPoint.Value = 0;
            TempDataBlock[dataBlock.Count()] = TempPoint;
            
            for (int idx0 = dataBlock.Count(); idx0 > Place; idx0--)
            {
                TempDataBlock[idx0] = TempDataBlock[idx0 - 1];
            }

            TempDataBlock[Place] = CurrentPoint;

            return TempDataBlock;
        }
        #endregion
    }
}
