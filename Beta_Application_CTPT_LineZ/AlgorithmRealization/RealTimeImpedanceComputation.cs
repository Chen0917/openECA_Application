using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using System.Data;

using MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using Beta_Application_CTPT_LineZ.Model.GPA;
using Beta_Application_CTPT_LineZ.MeasurementsDataSet;
using SynchrophasorAnalytics.Matrices;

namespace Beta_Application_CTPT_LineZ.AlgorithmRealization
{
    public static class RealTimeImpedanceComputation
    {
        #region [ Private Members ]
        //private static Complex[,] LineParameters;         // save results [line_number, Z, y]        
        #endregion
        
        #region [ Methods ]
        public static Complex[,] LineParameterComputation(Measurement_set CurrentFrameData)
        {
            Complex KV1 = 1;
            Complex KI1 = 1;
            Complex KV2 = 1;
            Complex KI2 = 1;

            Complex[,] LineParameters = new Complex[CurrentFrameData.Measurements.Count(), 3];

            int ResultIndex = 0;

            for (int idx0 = 0; idx0 < CurrentFrameData.Measurements.Count(); idx0++)
            {
                int CurrentLineNumberIndex = idx0;
                int CurrentLineNumber = CurrentLineNumberIndex + 1;

                double V1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Magnitude;
                double V1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Voltage.Angle;
                Complex V1 = Complex.FromPolarCoordinates(V1M, V1A * Math.PI / 180);

                double I1M = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Magnitude;
                double I1A = CurrentFrameData.Measurements[CurrentLineNumberIndex].From_bus.Current.Angle;
                Complex I1 = Complex.FromPolarCoordinates(I1M, I1A * Math.PI / 180);


                double V2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Magnitude;
                double V2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Voltage.Angle;
                Complex V2 = Complex.FromPolarCoordinates(V2M, V2A * Math.PI / 180);

                double I2M = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Magnitude;
                double I2A = CurrentFrameData.Measurements[CurrentLineNumberIndex].To_bus.Current.Angle;
                Complex I2 = Complex.FromPolarCoordinates(I2M, I2A * Math.PI / 180);

                Complex[] LineParameterResults = SingleLineImpedanceComputation(V1, KV1, I1, KI1, V2, KV2, I2, KI2);

                LineParameters[ResultIndex, 0] = CurrentLineNumber;
                LineParameters[ResultIndex, 1] = LineParameterResults[0];
                LineParameters[ResultIndex, 2] = LineParameterResults[1];

                ResultIndex += 1;

            }

            return LineParameters;
        }

        public static Complex[] SingleLineImpedanceComputation(Complex V1, Complex KV1, Complex I1,
            Complex KI1, Complex V2, Complex KV2, Complex I2, Complex KI2)
        /* Compute the real-time impedance and susceptance of the perticular line based on known conditions*/
        {
            Complex Vs = V1 * KV1;
            Complex Is = I1 * KI1;
            Complex Vr = V2 * KV2;
            Complex Ir = I2 * KI2;

            Complex Z = (Vs * Vs - Vr * Vr) / (Is * Vr - Ir * Vs);
            Complex y = 2 * ((Is + Ir) / (Vs + Vr));


            Complex[] results = new Complex[2] { Z, y.Imaginary };

            return results;
        }
        #endregion

    }
}
