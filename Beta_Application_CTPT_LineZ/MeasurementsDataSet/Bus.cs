using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beta_Application_CTPT_LineZ.MeasurementsDataSet
{
    [Serializable()]
    public class Bus
    {
        #region [ Private Members ]

        private int m_busnumber;
        private int m_busname;
        private bool m_referenceFlag;

        #endregion

        #region [ Properties ]

        [XmlAttribute("BusNumber")]
        public int BusNumber
        {
            get
            {
                return m_busnumber;
            }
            set
            {
                m_busnumber = value;
            }

        }

        [XmlAttribute("BusName")]
        public int BusName
        {
            get
            {
                return m_busname;
            }
            set
            {
                m_busname = value;
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

        public Bus()
        {

        }

        #endregion
    }
}
