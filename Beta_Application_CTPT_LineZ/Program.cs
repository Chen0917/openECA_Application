using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ECAClientFramework;
using ECAClientUtilities.API;

using Beta_Application_CTPT_LineZ.openHistorianDataCollection;
using Beta_Application_CTPT_LineZ.MeasurementsDataSet;
using Beta_Application_CTPT_LineZ.AlgorithmRealization;

namespace Beta_Application_CTPT_LineZ
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            #region [ System Configuration ]
            //StudyCase CurrentCase = new StudyCase();

            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 1,
            //    FromBusNumber = 8,
            //    ToBusNumber = 9,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 2,
            //    FromBusNumber = 8,
            //    ToBusNumber = 30,
            //    Resistance = 0.00431,
            //    Reactance = 0.050399998,
            //    Susceptance = 0.256999996,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 3,
            //    FromBusNumber = 9,
            //    ToBusNumber = 10,
            //    Resistance = 0.00258,
            //    Reactance = 0.032200001,
            //    Susceptance = 0.615000005,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 4,
            //    FromBusNumber = 26,
            //    ToBusNumber = 30,
            //    Resistance = 0.00799,
            //    Reactance = 0.086000005,
            //    Susceptance = 0.454000003,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 5,
            //    FromBusNumber = 30,
            //    ToBusNumber = 38,
            //    Resistance = 0.00464,
            //    Reactance = 0.054,
            //    Susceptance = 0.210999996,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 6,
            //    FromBusNumber = 38,
            //    ToBusNumber = 65,
            //    Resistance = 0.00901,
            //    Reactance = 0.098600002,
            //    Susceptance = 0.522999992,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 7,
            //    FromBusNumber = 63,
            //    ToBusNumber = 64,
            //    Resistance = 0.00172,
            //    Reactance = 0.02,
            //    Susceptance = 0.108000002,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 8,
            //    FromBusNumber = 64,
            //    ToBusNumber = 65,
            //    Resistance = 0.00269,
            //    Reactance = 0.0302,
            //    Susceptance = 0.189999998,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 9,
            //    FromBusNumber = 65,
            //    ToBusNumber = 68,
            //    Resistance = 0.00138,
            //    Reactance = 0.015999995,
            //    Susceptance = 0.319000001,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 10,
            //    FromBusNumber = 68,
            //    ToBusNumber = 81,
            //    Resistance = 0.00175,
            //    Reactance = 0.020200001,
            //    Susceptance = 0.404000015,
            //    HighVoltageLineFlag = true,
            //    ReferenceFlag = true
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 11,
            //    FromBusNumber = 68,
            //    ToBusNumber = 116,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 12,
            //    FromBusNumber = 5,
            //    ToBusNumber = 8,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 13,
            //    FromBusNumber = 17,
            //    ToBusNumber = 30,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 14,
            //    FromBusNumber = 25,
            //    ToBusNumber = 26,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 15,
            //    FromBusNumber = 37,
            //    ToBusNumber = 38,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 16,
            //    FromBusNumber = 59,
            //    ToBusNumber = 63,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 17,
            //    FromBusNumber = 61,
            //    ToBusNumber = 64,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 18,
            //    FromBusNumber = 65,
            //    ToBusNumber = 66,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 19,
            //    FromBusNumber = 68,
            //    ToBusNumber = 69,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 20,
            //    FromBusNumber = 80,
            //    ToBusNumber = 81,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 21,
            //    FromBusNumber = 8,
            //    ToBusNumber = -1,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 22,
            //    FromBusNumber = 10,
            //    ToBusNumber = -1,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 23,
            //    FromBusNumber = 26,
            //    ToBusNumber = -1,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Branches.Add(new Branch()
            //{
            //    LineNumber = 24,
            //    FromBusNumber = 65,
            //    ToBusNumber = -1,
            //    Resistance = 0.00244,
            //    Reactance = 0.030499999,
            //    Susceptance = 0.580999961,
            //    HighVoltageLineFlag = false,
            //    ReferenceFlag = false
            //});

            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 1,
            //    BusName = 8,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 2,
            //    BusName = 9,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 3,
            //    BusName = 10,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 4,
            //    BusName = 26,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 5,
            //    BusName = 30,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 6,
            //    BusName = 38,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 7,
            //    BusName = 63,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 8,
            //    BusName = 64,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 9,
            //    BusName = 65,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 10,
            //    BusName = 68,
            //    ReferenceFlag = false
            //});
            //CurrentCase.Buses.Add(new Bus()
            //{
            //    BusNumber = 11,
            //    BusName = 81,
            //    ReferenceFlag = true
            //});

            //CurrentCase.BaseKV = 345;

            //CurrentCase.BaseMVA = 100;

            //CurrentCase.SerializeToXml(@"C:\Users\wangc\Desktop\118SystemConfiguration.xml");

            #endregion            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Algorithm.UpdateSystemSettings();
            var MainWindowTread = new Thread(MainWindowThreadStart);
            MainWindowTread.TrySetApartmentState(ApartmentState.STA);
            MainWindowTread.Start();

            Application.Run(new BetaAppUI());            
        }

        public static void MainWindowThreadStart()
        {
            try
            {
                Framework framework = FrameworkFactory.Create();
                Algorithm.API = new Hub(framework);

                MainWindow mainWindow = new MainWindow(framework);
                mainWindow.Text = "C# Beta_Application_CTPT_LineZ";
                Application.Run(mainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
