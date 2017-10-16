using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class StudyCase
    {
        #region [ Private Members ]

        private List<Branch> m_branches;
        private List<Bus> m_buses;
        private double m_baseMVA;
        private double m_baseKV;

        #endregion

        #region [ Properties ]

        [XmlArray("Branches")]
        public List<Branch> Branches
        {
            get
            {
                return m_branches;
            }
            set
            {
                m_branches = value;
            }
        }

        [XmlArray("Buses")]
        public List<Bus> Buses
        {
            get
            {
                return m_buses;
            }
            set
            {
                m_buses = value;
            }
        }

        [XmlAttribute("BaseMVA")]
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

        [XmlAttribute("BaseKV")]
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

        #region [ Constructor ]

        public StudyCase()
        {
            m_branches = new List<Branch>();
            m_buses = new List<Bus>();
            m_baseKV = 0;
            m_baseMVA = 0;
        }

        #endregion

        #region [ Xml Serialization/Deserialization methods ]

        public static StudyCase DeserializeFromXml(string pathName)
        {
            try
            {
                // Create an empy NetworkMeasurements object reference.
                StudyCase collection = null;

                // Create an XmlSerializer with the type of NetworkMeasurements.
                XmlSerializer deserializer = new XmlSerializer(typeof(StudyCase));

                // Read the data in from the file.
                StreamReader reader = new StreamReader(pathName);

                // Cast the deserialized data as a NetworkMeasurements object.
                collection = (StudyCase)deserializer.Deserialize(reader);

                // Close the connection.
                reader.Close();

                return collection;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to Deserialize the Network from the Configuration File: " + exception.ToString());
            }
        }

        public void SerializeToXml(string pathName)
        {
            try
            {
                // Create an XmlSerializer with the type of Network
                XmlSerializer serializer = new XmlSerializer(typeof(StudyCase));

                // Open a connection to the file and path.
                TextWriter writer = new StreamWriter(pathName);

                // Serialize this instance of NetworkMeasurements
                serializer.Serialize(writer, this);

                // Close the connection
                writer.Close();
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to Serialize the Network to the Configuration File: " + exception.ToString());
            }
        }
        #endregion

    }
}
