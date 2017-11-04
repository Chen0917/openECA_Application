using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class Branch
    {
        #region [ Private Members ]

        private int m_lineNumber;
        private int m_fromBusNumber;
        private int m_toBusNumber;
        private int m_lineID;
        private double m_resistance; //pu
        private double m_reactance; //pu
        private double m_susceptance; //pu
        private bool m_highVoltageLineFlag;
        private bool m_referenceFlag;


        #endregion

        #region [ Properties ]

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

        [XmlAttribute("FromBusNumber")]
        public int FromBusNumber
        {
            get
            {
                return m_fromBusNumber;
            }
            set
            {
                m_fromBusNumber = value;
            }

        }

        [XmlAttribute("ToBusNumber")]
        public int ToBusNumber
        {
            get
            {
                return m_toBusNumber;
            }
            set
            {
                m_toBusNumber = value;
            }

        }

        [XmlAttribute("LineID")]
        public int LineID
        {
            get
            {
                return m_lineID;
            }
            set
            {
                m_lineID = value;
            }

        }

        [XmlAttribute("Resistance")]
        public double Resistance
        {
            get
            {
                return m_resistance;
            }
            set
            {
                m_resistance = value;
            }

        }

        [XmlAttribute("Reactance")]
        public double Reactance
        {
            get
            {
                return m_reactance;
            }
            set
            {
                m_reactance = value;
            }

        }

        [XmlAttribute("Susceptance")]
        public double Susceptance
        {
            get
            {
                return m_susceptance;
            }
            set
            {
                m_susceptance = value;
            }

        }

        [XmlAttribute("HighVoltageLineFlag")]
        public bool HighVoltageLineFlag
        {
            get
            {
                return m_highVoltageLineFlag;
            }
            set
            {
                m_highVoltageLineFlag = value;
            }

        }

        [XmlAttribute("ReferenceFlag")]
        public bool ReferenceFlag
        {
            get
            {
                return m_referenceFlag;
            }
            set
            {
                m_referenceFlag = value;
            }

        }


        #endregion

        #region [ Constructor ]

        public Branch()
        {

        }

        #endregion
    }
}
