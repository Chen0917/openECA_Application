using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class VIMeasurement
    {
        #region [ Private Members ]
        private double m_valueMeasured;
        private VIMeasurementType m_typeMeasured;
        private int m_lineNumber;
        private int m_busNumber;
        private string m_busName;
        private string m_key;
        #endregion

        #region [ Public Properties ]
        [XmlAttribute("Value")]
        public double ValueMeasured
        {
            get
            {
                return m_valueMeasured;
            }
            set
            {
                m_valueMeasured = value;
            }
        }

        [XmlAttribute("Type")]
        public VIMeasurementType TypeMeasured
        {
            get
            {
                return m_typeMeasured;
            }
            set
            {
                m_typeMeasured = value;
            }
        }

        [XmlAttribute("LineNumber")]
        public int LineNumber
        {
            get
            {
                return m_lineNumber;
            }
            set
            {
                m_lineNumber = value;
            }
        }

        [XmlAttribute("BusNumber")]
        public int BusNumber
        {
            get
            {
                return m_busNumber;
            }
            set
            {
                m_busNumber = value;
            }
        }

        [XmlAttribute("BusName")]
        public string BusName
        {
            get
            {
                return m_busName;
            }
            set
            {
                m_busName = value;
            }
        }

        [XmlAttribute("Key")]
        public string Key
        {
            get
            {
                return m_key;
            }
            set
            {
                m_key = value;
            }
        }

        [XmlIgnore()]
        public string SpecialKey
        {
            get
            {
                return "L" + m_lineNumber.ToString() + "B" +
                    m_busNumber.ToString() + "T" + m_typeMeasured.ToString();
            }
        }
        #endregion
    }
}
