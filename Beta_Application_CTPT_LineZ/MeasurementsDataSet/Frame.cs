using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class Frame
    {
        #region [ Private Members ]
        private long m_timeStamp;
        //private double m_voltageMagnitude;
        private List<VIMeasurement> m_measurementsList;

        // string example:"L552B1TVMAG" =  "L"+line number+"B"+bus number+"T"+type
        //private Dictionary<string, double> m_measurementsDict;
        #endregion

        #region [ Public Properties ]
        [XmlAttribute("Timestamp")]
        public long TimeStamp
        {
            get
            {
                return m_timeStamp;
            }
            set
            {
                m_timeStamp = value;
            }
        }

        /*public Dictionary<string, double> MeasurementsDict
        {
            get
            {
                return m_measurementsDict;
            }
        }*/

        [XmlArray("Measurements")]
        public List<VIMeasurement> MeasurementsList
        {
            get
            {
                return m_measurementsList;
            }
        }

        #endregion

        #region [ Constructors ]
        public Frame()
        {
            m_measurementsList = new List<VIMeasurement>();
            VIMeasurement Measurement_temp = new VIMeasurement();
            m_measurementsList.Add(Measurement_temp);

            /*m_measurementsDict = new Dictionary<string, double>();
            string TKey = "Start";
            double TValue = 0.00;
            m_measurementsDict.Add(TKey, TValue);*/
        }

        #endregion

        #region [ Methods ]
        //public void AddMeasurementToFrameDictionary(Measurement MeasuredResult)
        //{
        //    //m_measuredResults = new Dictionary<string, double>();
        //    //Measurement MeasuredResult = new Measurement();
        //    string TKey = "L" + MeasuredResult.LineNumber.ToString() +
        //        "B" + MeasuredResult.BusNumber.ToString() +
        //        "T" + MeasuredResult.TypeMeasured;

        //    try
        //    {                
        //        m_measurementsDict.Add(TKey, MeasuredResult.ValueMeasured);
        //    }
        //    catch (Exception exception)
        //    {
        //        //throw new Exception("Failed to Add Measurement to Frame");
        //        Console.WriteLine("Failed to Add Measurement to Frame");
        //    }

        //}

        public void AddMeasurementToFrame(VIMeasurement MeasurementResult)
        {
            try
            {
                if (m_measurementsList[0].LineNumber == 0)
                {
                    m_measurementsList[0] = MeasurementResult;
                }
                else
                {
                    m_measurementsList.Add(MeasurementResult);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.WriteLine("Need to be fixed.");
            }
        }
        #endregion
    }
}
