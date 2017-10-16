using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta_Application_CTPT_LineZ.LocalModel
{
    public class Measurement_set
    {
        private List<Line_data> m_measurements;

        public List<Line_data> Measurements
        {
            get
            {
                return m_measurements;
            }
            set
            {
                m_measurements = value;
            }
        }

        public Measurement_set()
        {
            m_measurements = new List<Line_data>();
            Line_data CurrentLineDataTemp = new Line_data();
            m_measurements.Add(CurrentLineDataTemp);
        }
    }
}
