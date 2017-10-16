using System;
using System.Numerics;
using System.Windows.Forms;
using ECAClientFramework;
using ECAClientUtilities.API;
using Beta_Application_CTPT_LineZ.Model.GPA;
using Beta_Application_CTPT_LineZ.AlgorithmRealization;

namespace Beta_Application_CTPT_LineZ
{
    public static class Algorithm
    {
        public static Measurement_set CurrentFrame = new Measurement_set();

        public static BetaAppUI AppUI = new BetaAppUI();
        public static Hub API { get; set; }

        public class Output
        {
            public Line_data OutputData = new Line_data();
            public _Line_dataMeta OutputMeta = new _Line_dataMeta();
            public static Func<Output> CreateNew { get; set; } = () => new Output();
        }

        public static void UpdateSystemSettings()
        {
            SystemSettings.InputMapping = "Measurement_dataset";
            SystemSettings.OutputMapping = "Output_measurements";
            SystemSettings.ConnectionString = @"server=localhost:6190; interface=0.0.0.0";
            SystemSettings.FramesPerSecond = 30;
            SystemSettings.LagTime = 3;
            SystemSettings.LeadTime = 1;
        }

        public static Output Execute(Measurement_set inputData, _Measurement_setMeta inputMeta)
        {
            Output output = Output.CreateNew();

            try
            {
                // TODO: Implement your algorithm here...
                // You can also write messages to the main window:
                MainWindow.WriteMessage("Real-time Data Streaming!");
                CurrentFrame = inputData;

                UpdateFrame();

            }
            catch (Exception ex)
            {
                // Display exceptions to the main window
                MainWindow.WriteError(new InvalidOperationException($"Algorithm exception: {ex.Message}", ex));
            }

            return output;
        }

        public static void UpdateFrame()
        {
            if (AppUI.InvokeRequired)
            {
                AppUI.Invoke((MethodInvoker)delegate ()
               {
                   UpdateFrame();
               });
            }
            else
            {
                AppUI.UpdateCurrentFrame();
            }
        }

    }
}
