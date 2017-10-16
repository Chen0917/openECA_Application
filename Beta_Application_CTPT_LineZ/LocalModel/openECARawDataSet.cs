using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta_Application_CTPT_LineZ.LocalModel
{
    public class openECARawDataSet
    {
        #region [Private Members]
        private List<DateTime> m_timeStamps;
        private List<Measurement_set> m_rawDataSet;
        #endregion

        #region [ Public Properties ]
        public List<DateTime> TimeStamps
        {
            get
            {
                return m_timeStamps;
            }
            set
            {
                m_timeStamps = value;
            }
        }
        public List<Measurement_set> RawDataSet
        {
            get
            {
                return m_rawDataSet;
            }
            set
            {
                m_rawDataSet = value;
            }
        }
        #endregion

        #region [Constructor]
        public openECARawDataSet()
        {
            m_timeStamps = new List<DateTime>();
            DateTime Time_stamp_temp = new DateTime(1969, 12, 31, 23, 59, 59);
            m_timeStamps.Add(Time_stamp_temp);

            m_rawDataSet = new List<Measurement_set>();
            Measurement_set Measurement_set_temp = new Measurement_set();
            m_rawDataSet.Add(Measurement_set_temp);
        }
        #endregion
    }
}
