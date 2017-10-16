namespace Beta_Application_CTPT_LineZ
{
    partial class BetaAppUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Function_Selection = new System.Windows.Forms.GroupBox();
            this.Real_Time_Parameters_Calculation_Selection = new System.Windows.Forms.RadioButton();
            this.Line_Parameters_Estimation_Selection = new System.Windows.Forms.RadioButton();
            this.CT_PT_Calibration_Selection = new System.Windows.Forms.RadioButton();
            this.Input_Data_Acquisition = new System.Windows.Forms.GroupBox();
            this.progressBarTrendData = new System.Windows.Forms.ProgressBar();
            this.Data_Collection_Settings = new System.Windows.Forms.GroupBox();
            this.Trend_Data_Button = new System.Windows.Forms.Button();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.textBoxPointList = new System.Windows.Forms.TextBox();
            this.labelPointList = new System.Windows.Forms.Label();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.maskedTextBoxMessageInterval = new System.Windows.Forms.MaskedTextBox();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.labelMessageInterval = new System.Windows.Forms.Label();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.maskedTextBoxMetadataTimeout = new System.Windows.Forms.MaskedTextBox();
            this.labelMetaDataTimeout = new System.Windows.Forms.Label();
            this.maskedTextBoxFrameRate = new System.Windows.Forms.MaskedTextBox();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.dateTimePickerSourceTime = new System.Windows.Forms.DateTimePicker();
            this.labelSourceHistorianInstanceName = new System.Windows.Forms.Label();
            this.textBoxHistorianInstanceName = new System.Windows.Forms.TextBox();
            this.labelSourceHistorianHostAddress = new System.Windows.Forms.Label();
            this.labelPerSec = new System.Windows.Forms.Label();
            this.textBoxHistorianHostAddress = new System.Windows.Forms.TextBox();
            this.maskedTextBoxHistorianDataPort = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxHistorianMetadataPort = new System.Windows.Forms.MaskedTextBox();
            this.labelSourceHistorianDataPort = new System.Windows.Forms.Label();
            this.labelSourceHistorianMetaDataPort = new System.Windows.Forms.Label();
            this.Data_Source_Selection = new System.Windows.Forms.GroupBox();
            this.Historical_Data_Selection = new System.Windows.Forms.RadioButton();
            this.Real_Time_Data_Selection = new System.Windows.Forms.RadioButton();
            this.Messages = new System.Windows.Forms.GroupBox();
            this.textBoxMessages = new System.Windows.Forms.TextBox();
            this.labelSeconds = new System.Windows.Forms.Label();
            this.System_Scheme = new System.Windows.Forms.Panel();
            this.Results_Demonstration_GridView = new System.Windows.Forms.DataGridView();
            this.Launch_Button = new System.Windows.Forms.Button();
            this.Configure_Button = new System.Windows.Forms.Button();
            this.textBoxConfigurationPath = new System.Windows.Forms.TextBox();
            this.FolderDialoguebutton = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.backgroundWorkerForCancel = new System.ComponentModel.BackgroundWorker();
            this.Function_Selection.SuspendLayout();
            this.Input_Data_Acquisition.SuspendLayout();
            this.Data_Collection_Settings.SuspendLayout();
            this.Data_Source_Selection.SuspendLayout();
            this.Messages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Results_Demonstration_GridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Function_Selection
            // 
            this.Function_Selection.Controls.Add(this.Real_Time_Parameters_Calculation_Selection);
            this.Function_Selection.Controls.Add(this.Line_Parameters_Estimation_Selection);
            this.Function_Selection.Controls.Add(this.CT_PT_Calibration_Selection);
            this.Function_Selection.Location = new System.Drawing.Point(12, 12);
            this.Function_Selection.Name = "Function_Selection";
            this.Function_Selection.Size = new System.Drawing.Size(563, 118);
            this.Function_Selection.TabIndex = 0;
            this.Function_Selection.TabStop = false;
            this.Function_Selection.Text = "&Function Selection";
            // 
            // Real_Time_Parameters_Calculation_Selection
            // 
            this.Real_Time_Parameters_Calculation_Selection.AutoSize = true;
            this.Real_Time_Parameters_Calculation_Selection.Location = new System.Drawing.Point(37, 78);
            this.Real_Time_Parameters_Calculation_Selection.Name = "Real_Time_Parameters_Calculation_Selection";
            this.Real_Time_Parameters_Calculation_Selection.Size = new System.Drawing.Size(270, 24);
            this.Real_Time_Parameters_Calculation_Selection.TabIndex = 2;
            this.Real_Time_Parameters_Calculation_Selection.Text = "Real-time Parameters Calculation";
            this.Real_Time_Parameters_Calculation_Selection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Real_Time_Parameters_Calculation_Selection.UseVisualStyleBackColor = true;
            this.Real_Time_Parameters_Calculation_Selection.CheckedChanged += new System.EventHandler(this.Real_Time_Parameters_Calculation_Selection_CheckedChanged);
            // 
            // Line_Parameters_Estimation_Selection
            // 
            this.Line_Parameters_Estimation_Selection.AutoSize = true;
            this.Line_Parameters_Estimation_Selection.Location = new System.Drawing.Point(314, 37);
            this.Line_Parameters_Estimation_Selection.Name = "Line_Parameters_Estimation_Selection";
            this.Line_Parameters_Estimation_Selection.Size = new System.Drawing.Size(229, 24);
            this.Line_Parameters_Estimation_Selection.TabIndex = 1;
            this.Line_Parameters_Estimation_Selection.Text = "Line Parameters Estimation";
            this.Line_Parameters_Estimation_Selection.UseVisualStyleBackColor = true;
            this.Line_Parameters_Estimation_Selection.CheckedChanged += new System.EventHandler(this.Line_Parameters_Estimation_Selection_CheckedChanged);
            // 
            // CT_PT_Calibration_Selection
            // 
            this.CT_PT_Calibration_Selection.AutoSize = true;
            this.CT_PT_Calibration_Selection.Checked = true;
            this.CT_PT_Calibration_Selection.Location = new System.Drawing.Point(37, 37);
            this.CT_PT_Calibration_Selection.Name = "CT_PT_Calibration_Selection";
            this.CT_PT_Calibration_Selection.Size = new System.Drawing.Size(156, 24);
            this.CT_PT_Calibration_Selection.TabIndex = 0;
            this.CT_PT_Calibration_Selection.TabStop = true;
            this.CT_PT_Calibration_Selection.Text = "CT/PT Calibration";
            this.CT_PT_Calibration_Selection.UseVisualStyleBackColor = true;
            this.CT_PT_Calibration_Selection.CheckedChanged += new System.EventHandler(this.CT_PT_Calibration_Selection_CheckedChanged);
            // 
            // Input_Data_Acquisition
            // 
            this.Input_Data_Acquisition.Controls.Add(this.progressBarTrendData);
            this.Input_Data_Acquisition.Controls.Add(this.Data_Collection_Settings);
            this.Input_Data_Acquisition.Controls.Add(this.Data_Source_Selection);
            this.Input_Data_Acquisition.Location = new System.Drawing.Point(12, 136);
            this.Input_Data_Acquisition.Name = "Input_Data_Acquisition";
            this.Input_Data_Acquisition.Size = new System.Drawing.Size(563, 592);
            this.Input_Data_Acquisition.TabIndex = 1;
            this.Input_Data_Acquisition.TabStop = false;
            this.Input_Data_Acquisition.Text = "&Input Data Acquisition";
            // 
            // progressBarTrendData
            // 
            this.progressBarTrendData.Location = new System.Drawing.Point(19, 550);
            this.progressBarTrendData.Name = "progressBarTrendData";
            this.progressBarTrendData.Size = new System.Drawing.Size(524, 32);
            this.progressBarTrendData.TabIndex = 21;
            // 
            // Data_Collection_Settings
            // 
            this.Data_Collection_Settings.Controls.Add(this.Trend_Data_Button);
            this.Data_Collection_Settings.Controls.Add(this.checkBoxEnableLogging);
            this.Data_Collection_Settings.Controls.Add(this.textBoxPointList);
            this.Data_Collection_Settings.Controls.Add(this.labelPointList);
            this.Data_Collection_Settings.Controls.Add(this.dateTimePickerEndTime);
            this.Data_Collection_Settings.Controls.Add(this.maskedTextBoxMessageInterval);
            this.Data_Collection_Settings.Controls.Add(this.labelEndTime);
            this.Data_Collection_Settings.Controls.Add(this.labelMessageInterval);
            this.Data_Collection_Settings.Controls.Add(this.labelFrameRate);
            this.Data_Collection_Settings.Controls.Add(this.maskedTextBoxMetadataTimeout);
            this.Data_Collection_Settings.Controls.Add(this.labelMetaDataTimeout);
            this.Data_Collection_Settings.Controls.Add(this.maskedTextBoxFrameRate);
            this.Data_Collection_Settings.Controls.Add(this.labelStartTime);
            this.Data_Collection_Settings.Controls.Add(this.dateTimePickerSourceTime);
            this.Data_Collection_Settings.Controls.Add(this.labelSourceHistorianInstanceName);
            this.Data_Collection_Settings.Controls.Add(this.textBoxHistorianInstanceName);
            this.Data_Collection_Settings.Controls.Add(this.labelSourceHistorianHostAddress);
            this.Data_Collection_Settings.Controls.Add(this.labelPerSec);
            this.Data_Collection_Settings.Controls.Add(this.textBoxHistorianHostAddress);
            this.Data_Collection_Settings.Controls.Add(this.maskedTextBoxHistorianDataPort);
            this.Data_Collection_Settings.Controls.Add(this.maskedTextBoxHistorianMetadataPort);
            this.Data_Collection_Settings.Controls.Add(this.labelSourceHistorianDataPort);
            this.Data_Collection_Settings.Controls.Add(this.labelSourceHistorianMetaDataPort);
            this.Data_Collection_Settings.Location = new System.Drawing.Point(19, 108);
            this.Data_Collection_Settings.Name = "Data_Collection_Settings";
            this.Data_Collection_Settings.Size = new System.Drawing.Size(524, 436);
            this.Data_Collection_Settings.TabIndex = 1;
            this.Data_Collection_Settings.TabStop = false;
            this.Data_Collection_Settings.Text = "&Data Collection Settings";
            // 
            // Trend_Data_Button
            // 
            this.Trend_Data_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Trend_Data_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Trend_Data_Button.Location = new System.Drawing.Point(342, 388);
            this.Trend_Data_Button.Name = "Trend_Data_Button";
            this.Trend_Data_Button.Size = new System.Drawing.Size(164, 36);
            this.Trend_Data_Button.TabIndex = 24;
            this.Trend_Data_Button.Text = "&Trend Data";
            this.Trend_Data_Button.UseVisualStyleBackColor = true;
            this.Trend_Data_Button.Click += new System.EventHandler(this.Trend_Data_Button_Click);
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Checked = true;
            this.checkBoxEnableLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(17, 388);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(146, 24);
            this.checkBoxEnableLogging.TabIndex = 23;
            this.checkBoxEnableLogging.Text = "Enable Logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogging.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // textBoxPointList
            // 
            this.textBoxPointList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPointList.Location = new System.Drawing.Point(15, 203);
            this.textBoxPointList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxPointList.Multiline = true;
            this.textBoxPointList.Name = "textBoxPointList";
            this.textBoxPointList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPointList.Size = new System.Drawing.Size(491, 99);
            this.textBoxPointList.TabIndex = 21;
            this.textBoxPointList.Text = "FILTER MeasurementDetail WHERE SignalAcronym IN (\'VPHM\', \'IPHM\', \'VPHA\', \'IPHA\')";
            this.textBoxPointList.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelPointList
            // 
            this.labelPointList.AutoSize = true;
            this.labelPointList.Location = new System.Drawing.Point(13, 178);
            this.labelPointList.Name = "labelPointList";
            this.labelPointList.Size = new System.Drawing.Size(207, 20);
            this.labelPointList.TabIndex = 20;
            this.labelPointList.Text = "Point List / Filter Expression:";
            this.labelPointList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTimePickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(138, 139);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(218, 26);
            this.dateTimePickerEndTime.TabIndex = 19;
            this.dateTimePickerEndTime.Value = new System.DateTime(2017, 8, 1, 15, 30, 36, 0);
            this.dateTimePickerEndTime.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // maskedTextBoxMessageInterval
            // 
            this.maskedTextBoxMessageInterval.Location = new System.Drawing.Point(422, 350);
            this.maskedTextBoxMessageInterval.Mask = "0000000";
            this.maskedTextBoxMessageInterval.Name = "maskedTextBoxMessageInterval";
            this.maskedTextBoxMessageInterval.Size = new System.Drawing.Size(78, 26);
            this.maskedTextBoxMessageInterval.TabIndex = 22;
            this.maskedTextBoxMessageInterval.Text = "2000";
            this.maskedTextBoxMessageInterval.ValidatingType = typeof(int);
            this.maskedTextBoxMessageInterval.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelEndTime
            // 
            this.labelEndTime.AutoSize = true;
            this.labelEndTime.Location = new System.Drawing.Point(53, 144);
            this.labelEndTime.Name = "labelEndTime";
            this.labelEndTime.Size = new System.Drawing.Size(80, 20);
            this.labelEndTime.TabIndex = 18;
            this.labelEndTime.Text = "End Time:";
            this.labelEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMessageInterval
            // 
            this.labelMessageInterval.AutoSize = true;
            this.labelMessageInterval.Location = new System.Drawing.Point(282, 353);
            this.labelMessageInterval.Name = "labelMessageInterval";
            this.labelMessageInterval.Size = new System.Drawing.Size(134, 20);
            this.labelMessageInterval.TabIndex = 21;
            this.labelMessageInterval.Text = "Message Interval:";
            this.labelMessageInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFrameRate
            // 
            this.labelFrameRate.AutoSize = true;
            this.labelFrameRate.Location = new System.Drawing.Point(13, 318);
            this.labelFrameRate.Name = "labelFrameRate";
            this.labelFrameRate.Size = new System.Drawing.Size(159, 20);
            this.labelFrameRate.TabIndex = 15;
            this.labelFrameRate.Text = "Frame-rate Estimate:";
            this.labelFrameRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxMetadataTimeout
            // 
            this.maskedTextBoxMetadataTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxMetadataTimeout.Location = new System.Drawing.Point(178, 350);
            this.maskedTextBoxMetadataTimeout.Mask = "000";
            this.maskedTextBoxMetadataTimeout.Name = "maskedTextBoxMetadataTimeout";
            this.maskedTextBoxMetadataTimeout.Size = new System.Drawing.Size(44, 26);
            this.maskedTextBoxMetadataTimeout.TabIndex = 19;
            this.maskedTextBoxMetadataTimeout.Text = "60";
            this.maskedTextBoxMetadataTimeout.ValidatingType = typeof(int);
            this.maskedTextBoxMetadataTimeout.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelMetaDataTimeout
            // 
            this.labelMetaDataTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMetaDataTimeout.AutoSize = true;
            this.labelMetaDataTimeout.Location = new System.Drawing.Point(25, 353);
            this.labelMetaDataTimeout.Name = "labelMetaDataTimeout";
            this.labelMetaDataTimeout.Size = new System.Drawing.Size(147, 20);
            this.labelMetaDataTimeout.TabIndex = 18;
            this.labelMetaDataTimeout.Text = "Meta-data Timeout:";
            this.labelMetaDataTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxFrameRate
            // 
            this.maskedTextBoxFrameRate.Location = new System.Drawing.Point(178, 315);
            this.maskedTextBoxFrameRate.Mask = "000";
            this.maskedTextBoxFrameRate.Name = "maskedTextBoxFrameRate";
            this.maskedTextBoxFrameRate.Size = new System.Drawing.Size(44, 26);
            this.maskedTextBoxFrameRate.TabIndex = 16;
            this.maskedTextBoxFrameRate.Text = "30";
            this.maskedTextBoxFrameRate.ValidatingType = typeof(int);
            this.maskedTextBoxFrameRate.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelStartTime
            // 
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Location = new System.Drawing.Point(47, 109);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(86, 20);
            this.labelStartTime.TabIndex = 16;
            this.labelStartTime.Text = "Start Time:";
            this.labelStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerSourceTime
            // 
            this.dateTimePickerSourceTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTimePickerSourceTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerSourceTime.Location = new System.Drawing.Point(138, 104);
            this.dateTimePickerSourceTime.Name = "dateTimePickerSourceTime";
            this.dateTimePickerSourceTime.Size = new System.Drawing.Size(218, 26);
            this.dateTimePickerSourceTime.TabIndex = 17;
            this.dateTimePickerSourceTime.Value = new System.DateTime(2017, 8, 1, 15, 29, 36, 0);
            this.dateTimePickerSourceTime.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianInstanceName
            // 
            this.labelSourceHistorianInstanceName.AutoSize = true;
            this.labelSourceHistorianInstanceName.Location = new System.Drawing.Point(305, 72);
            this.labelSourceHistorianInstanceName.Name = "labelSourceHistorianInstanceName";
            this.labelSourceHistorianInstanceName.Size = new System.Drawing.Size(121, 20);
            this.labelSourceHistorianInstanceName.TabIndex = 14;
            this.labelSourceHistorianInstanceName.Text = "Instance Name:";
            this.labelSourceHistorianInstanceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHistorianInstanceName
            // 
            this.textBoxHistorianInstanceName.Location = new System.Drawing.Point(434, 69);
            this.textBoxHistorianInstanceName.Name = "textBoxHistorianInstanceName";
            this.textBoxHistorianInstanceName.Size = new System.Drawing.Size(60, 26);
            this.textBoxHistorianInstanceName.TabIndex = 15;
            this.textBoxHistorianInstanceName.Text = "PPA";
            this.textBoxHistorianInstanceName.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianHostAddress
            // 
            this.labelSourceHistorianHostAddress.AutoSize = true;
            this.labelSourceHistorianHostAddress.Location = new System.Drawing.Point(23, 34);
            this.labelSourceHistorianHostAddress.Name = "labelSourceHistorianHostAddress";
            this.labelSourceHistorianHostAddress.Size = new System.Drawing.Size(110, 20);
            this.labelSourceHistorianHostAddress.TabIndex = 8;
            this.labelSourceHistorianHostAddress.Text = "Host Address:";
            this.labelSourceHistorianHostAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPerSec
            // 
            this.labelPerSec.AutoSize = true;
            this.labelPerSec.Location = new System.Drawing.Point(232, 318);
            this.labelPerSec.Name = "labelPerSec";
            this.labelPerSec.Size = new System.Drawing.Size(141, 20);
            this.labelPerSec.TabIndex = 17;
            this.labelPerSec.Text = "frames per second";
            // 
            // textBoxHistorianHostAddress
            // 
            this.textBoxHistorianHostAddress.Location = new System.Drawing.Point(138, 30);
            this.textBoxHistorianHostAddress.Name = "textBoxHistorianHostAddress";
            this.textBoxHistorianHostAddress.Size = new System.Drawing.Size(130, 26);
            this.textBoxHistorianHostAddress.TabIndex = 9;
            this.textBoxHistorianHostAddress.Text = "127.0.0.1";
            this.textBoxHistorianHostAddress.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // maskedTextBoxHistorianDataPort
            // 
            this.maskedTextBoxHistorianDataPort.Location = new System.Drawing.Point(434, 30);
            this.maskedTextBoxHistorianDataPort.Mask = "00000";
            this.maskedTextBoxHistorianDataPort.Name = "maskedTextBoxHistorianDataPort";
            this.maskedTextBoxHistorianDataPort.Size = new System.Drawing.Size(60, 26);
            this.maskedTextBoxHistorianDataPort.TabIndex = 11;
            this.maskedTextBoxHistorianDataPort.Text = "38402";
            this.maskedTextBoxHistorianDataPort.ValidatingType = typeof(int);
            this.maskedTextBoxHistorianDataPort.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // maskedTextBoxHistorianMetadataPort
            // 
            this.maskedTextBoxHistorianMetadataPort.Location = new System.Drawing.Point(138, 67);
            this.maskedTextBoxHistorianMetadataPort.Mask = "00000";
            this.maskedTextBoxHistorianMetadataPort.Name = "maskedTextBoxHistorianMetadataPort";
            this.maskedTextBoxHistorianMetadataPort.Size = new System.Drawing.Size(60, 26);
            this.maskedTextBoxHistorianMetadataPort.TabIndex = 13;
            this.maskedTextBoxHistorianMetadataPort.Text = "6175";
            this.maskedTextBoxHistorianMetadataPort.ValidatingType = typeof(int);
            this.maskedTextBoxHistorianMetadataPort.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianDataPort
            // 
            this.labelSourceHistorianDataPort.AutoSize = true;
            this.labelSourceHistorianDataPort.Location = new System.Drawing.Point(345, 34);
            this.labelSourceHistorianDataPort.Name = "labelSourceHistorianDataPort";
            this.labelSourceHistorianDataPort.Size = new System.Drawing.Size(81, 20);
            this.labelSourceHistorianDataPort.TabIndex = 10;
            this.labelSourceHistorianDataPort.Text = "Data Port:";
            this.labelSourceHistorianDataPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSourceHistorianMetaDataPort
            // 
            this.labelSourceHistorianMetaDataPort.AutoSize = true;
            this.labelSourceHistorianMetaDataPort.Location = new System.Drawing.Point(14, 71);
            this.labelSourceHistorianMetaDataPort.Name = "labelSourceHistorianMetaDataPort";
            this.labelSourceHistorianMetaDataPort.Size = new System.Drawing.Size(119, 20);
            this.labelSourceHistorianMetaDataPort.TabIndex = 12;
            this.labelSourceHistorianMetaDataPort.Text = "Meta-data Port:";
            this.labelSourceHistorianMetaDataPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Data_Source_Selection
            // 
            this.Data_Source_Selection.Controls.Add(this.Historical_Data_Selection);
            this.Data_Source_Selection.Controls.Add(this.Real_Time_Data_Selection);
            this.Data_Source_Selection.Location = new System.Drawing.Point(19, 36);
            this.Data_Source_Selection.Name = "Data_Source_Selection";
            this.Data_Source_Selection.Size = new System.Drawing.Size(524, 62);
            this.Data_Source_Selection.TabIndex = 0;
            this.Data_Source_Selection.TabStop = false;
            this.Data_Source_Selection.Text = "&Data Source Selection";
            // 
            // Historical_Data_Selection
            // 
            this.Historical_Data_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Historical_Data_Selection.AutoSize = true;
            this.Historical_Data_Selection.Checked = true;
            this.Historical_Data_Selection.Location = new System.Drawing.Point(78, 24);
            this.Historical_Data_Selection.Name = "Historical_Data_Selection";
            this.Historical_Data_Selection.Size = new System.Drawing.Size(138, 24);
            this.Historical_Data_Selection.TabIndex = 1;
            this.Historical_Data_Selection.TabStop = true;
            this.Historical_Data_Selection.Text = "Historical Data";
            this.Historical_Data_Selection.UseVisualStyleBackColor = true;
            this.Historical_Data_Selection.CheckedChanged += new System.EventHandler(this.Historical_Data_Selection_CheckedChanged);
            // 
            // Real_Time_Data_Selection
            // 
            this.Real_Time_Data_Selection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Real_Time_Data_Selection.AutoSize = true;
            this.Real_Time_Data_Selection.Location = new System.Drawing.Point(295, 24);
            this.Real_Time_Data_Selection.Name = "Real_Time_Data_Selection";
            this.Real_Time_Data_Selection.Size = new System.Drawing.Size(141, 24);
            this.Real_Time_Data_Selection.TabIndex = 0;
            this.Real_Time_Data_Selection.Text = "Real-time Data";
            this.Real_Time_Data_Selection.UseVisualStyleBackColor = true;
            this.Real_Time_Data_Selection.CheckedChanged += new System.EventHandler(this.Real_Time_Data_Selection_CheckedChanged);
            // 
            // Messages
            // 
            this.Messages.Controls.Add(this.textBoxMessages);
            this.Messages.Location = new System.Drawing.Point(12, 734);
            this.Messages.Name = "Messages";
            this.Messages.Size = new System.Drawing.Size(563, 308);
            this.Messages.TabIndex = 22;
            this.Messages.TabStop = false;
            this.Messages.Text = "&Messages";
            // 
            // textBoxMessages
            // 
            this.textBoxMessages.AcceptsReturn = true;
            this.textBoxMessages.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessages.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxMessages.Location = new System.Drawing.Point(3, 22);
            this.textBoxMessages.Multiline = true;
            this.textBoxMessages.Name = "textBoxMessages";
            this.textBoxMessages.ReadOnly = true;
            this.textBoxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessages.Size = new System.Drawing.Size(557, 283);
            this.textBoxMessages.TabIndex = 0;
            this.textBoxMessages.TabStop = false;
            // 
            // labelSeconds
            // 
            this.labelSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeconds.AutoSize = true;
            this.labelSeconds.Location = new System.Drawing.Point(1919, 628);
            this.labelSeconds.Name = "labelSeconds";
            this.labelSeconds.Size = new System.Drawing.Size(69, 20);
            this.labelSeconds.TabIndex = 20;
            this.labelSeconds.Text = "seconds";
            // 
            // System_Scheme
            // 
            this.System_Scheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.System_Scheme.AutoScroll = true;
            this.System_Scheme.BackColor = System.Drawing.SystemColors.Window;
            this.System_Scheme.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.System_Scheme.Cursor = System.Windows.Forms.Cursors.Cross;
            this.System_Scheme.Location = new System.Drawing.Point(591, 22);
            this.System_Scheme.Name = "System_Scheme";
            this.System_Scheme.Size = new System.Drawing.Size(1299, 516);
            this.System_Scheme.TabIndex = 21;
            // 
            // Results_Demonstration_GridView
            // 
            this.Results_Demonstration_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Results_Demonstration_GridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.Results_Demonstration_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Results_Demonstration_GridView.Location = new System.Drawing.Point(591, 624);
            this.Results_Demonstration_GridView.Name = "Results_Demonstration_GridView";
            this.Results_Demonstration_GridView.RowTemplate.Height = 28;
            this.Results_Demonstration_GridView.Size = new System.Drawing.Size(1299, 414);
            this.Results_Demonstration_GridView.TabIndex = 22;
            // 
            // Launch_Button
            // 
            this.Launch_Button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Launch_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Launch_Button.Location = new System.Drawing.Point(1368, 554);
            this.Launch_Button.Name = "Launch_Button";
            this.Launch_Button.Size = new System.Drawing.Size(164, 49);
            this.Launch_Button.TabIndex = 25;
            this.Launch_Button.Text = "&Launch";
            this.Launch_Button.UseVisualStyleBackColor = true;
            this.Launch_Button.Click += new System.EventHandler(this.Launch_Button_Click);
            // 
            // Configure_Button
            // 
            this.Configure_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Configure_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Configure_Button.Location = new System.Drawing.Point(1153, 554);
            this.Configure_Button.Name = "Configure_Button";
            this.Configure_Button.Size = new System.Drawing.Size(125, 49);
            this.Configure_Button.TabIndex = 26;
            this.Configure_Button.Text = "&Configure";
            this.Configure_Button.UseVisualStyleBackColor = true;
            this.Configure_Button.Click += new System.EventHandler(this.Configure_Button_Click);
            // 
            // textBoxConfigurationPath
            // 
            this.textBoxConfigurationPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConfigurationPath.Location = new System.Drawing.Point(591, 565);
            this.textBoxConfigurationPath.Name = "textBoxConfigurationPath";
            this.textBoxConfigurationPath.Size = new System.Drawing.Size(451, 26);
            this.textBoxConfigurationPath.TabIndex = 27;
            // 
            // FolderDialoguebutton
            // 
            this.FolderDialoguebutton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.FolderDialoguebutton.Location = new System.Drawing.Point(1048, 565);
            this.FolderDialoguebutton.Name = "FolderDialoguebutton";
            this.FolderDialoguebutton.Size = new System.Drawing.Size(50, 32);
            this.FolderDialoguebutton.TabIndex = 28;
            this.FolderDialoguebutton.Text = "...";
            this.FolderDialoguebutton.UseVisualStyleBackColor = true;
            this.FolderDialoguebutton.Click += new System.EventHandler(this.FolderDialoguebutton_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Cancel_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel_Button.Location = new System.Drawing.Point(1549, 554);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(164, 49);
            this.Cancel_Button.TabIndex = 29;
            this.Cancel_Button.Text = "&Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // Export
            // 
            this.Export.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Export.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Export.Location = new System.Drawing.Point(1726, 554);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(164, 49);
            this.Export.TabIndex = 30;
            this.Export.Text = "&Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // backgroundWorkerForCancel
            // 
            this.backgroundWorkerForCancel.WorkerReportsProgress = true;
            this.backgroundWorkerForCancel.WorkerSupportsCancellation = true;
            this.backgroundWorkerForCancel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerForCancel_DoWork);
            this.backgroundWorkerForCancel.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerForCancel_ProgressChanged);
            this.backgroundWorkerForCancel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerForCancel_RunWorkerCompleted);
            // 
            // BetaAppUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1054);
            this.Controls.Add(this.Messages);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.Cancel_Button);
            this.Controls.Add(this.FolderDialoguebutton);
            this.Controls.Add(this.textBoxConfigurationPath);
            this.Controls.Add(this.Configure_Button);
            this.Controls.Add(this.Launch_Button);
            this.Controls.Add(this.Results_Demonstration_GridView);
            this.Controls.Add(this.System_Scheme);
            this.Controls.Add(this.Input_Data_Acquisition);
            this.Controls.Add(this.Function_Selection);
            this.Controls.Add(this.labelSeconds);
            this.Name = "BetaAppUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BetaAppUI";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Function_Selection.ResumeLayout(false);
            this.Function_Selection.PerformLayout();
            this.Input_Data_Acquisition.ResumeLayout(false);
            this.Data_Collection_Settings.ResumeLayout(false);
            this.Data_Collection_Settings.PerformLayout();
            this.Data_Source_Selection.ResumeLayout(false);
            this.Data_Source_Selection.PerformLayout();
            this.Messages.ResumeLayout(false);
            this.Messages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Results_Demonstration_GridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Function_Selection;
        private System.Windows.Forms.RadioButton Real_Time_Parameters_Calculation_Selection;
        private System.Windows.Forms.RadioButton Line_Parameters_Estimation_Selection;
        private System.Windows.Forms.RadioButton CT_PT_Calibration_Selection;
        private System.Windows.Forms.GroupBox Input_Data_Acquisition;
        private System.Windows.Forms.GroupBox Data_Collection_Settings;
        public System.Windows.Forms.TextBox textBoxPointList;
        private System.Windows.Forms.Label labelPointList;
        public System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.Label labelStartTime;
        public System.Windows.Forms.DateTimePicker dateTimePickerSourceTime;
        private System.Windows.Forms.Label labelSourceHistorianInstanceName;
        public System.Windows.Forms.TextBox textBoxHistorianInstanceName;
        private System.Windows.Forms.Label labelSourceHistorianHostAddress;
        public System.Windows.Forms.TextBox textBoxHistorianHostAddress;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxHistorianDataPort;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxHistorianMetadataPort;
        private System.Windows.Forms.Label labelSourceHistorianDataPort;
        private System.Windows.Forms.Label labelSourceHistorianMetaDataPort;
        private System.Windows.Forms.GroupBox Data_Source_Selection;
        private System.Windows.Forms.RadioButton Historical_Data_Selection;
        private System.Windows.Forms.RadioButton Real_Time_Data_Selection;
        private System.Windows.Forms.Label labelFrameRate;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxFrameRate;
        private System.Windows.Forms.Label labelPerSec;
        public System.Windows.Forms.CheckBox checkBoxEnableLogging;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxMessageInterval;
        private System.Windows.Forms.Label labelMessageInterval;
        private System.Windows.Forms.Label labelSeconds;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxMetadataTimeout;
        private System.Windows.Forms.Label labelMetaDataTimeout;
        private System.Windows.Forms.Button Trend_Data_Button;
        private System.Windows.Forms.GroupBox Messages;
        private System.Windows.Forms.ProgressBar progressBarTrendData;
        private System.Windows.Forms.TextBox textBoxMessages;
        private System.Windows.Forms.Panel System_Scheme;
        private System.Windows.Forms.DataGridView Results_Demonstration_GridView;
        private System.Windows.Forms.Button Launch_Button;
        private System.Windows.Forms.Button Configure_Button;
        private System.Windows.Forms.TextBox textBoxConfigurationPath;
        private System.Windows.Forms.Button FolderDialoguebutton;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button Export;
        private System.ComponentModel.BackgroundWorker backgroundWorkerForCancel;
    }
}