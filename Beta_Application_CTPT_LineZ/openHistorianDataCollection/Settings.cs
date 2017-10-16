using System;
using System.ComponentModel;
using System.Configuration;
using ExpressionEvaluator;
using GSF.ComponentModel;
using GSF.Configuration;

namespace Beta_Application_CTPT_LineZ.openHistorianDataCollection
{
    /// <summary>
    /// Defines settings for the Historian Data Walker application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Default value expressions in this class reference the primary form instance, as a result,
    /// instances of this class should only be created from the primary UI thread or otherwise
    /// use <see cref="System.Windows.Forms.Form.Invoke(Delegate)"/>.
    /// </para>
    /// <para>
    /// In order for properties of this class decorated with <see cref="TypeConvertedValueExpressionAttribute"/>
    /// to have access to form element values, the elements should be declared with "public" access.
    /// </para>
    /// </remarks>
    public sealed class Settings : CategorizedSettingsBase<Settings>
    {
        //#region [ Members ]
        //private string m_hostAddress;
        //private int m_dataPort;
        //private int m_metaDataPort;
        //private string m_instanceName;
        //private int m_frameRate;
        //private int m_metadataTimeout;
        //private int m_metadataTimeoutSeconds;
        //private DateTime m_startTime;
        //private DateTime m_endTime;
        //private string m_pointList;
        //private int m_messageInterval;
        //private bool m_enableLogging;

        //#endregion

        //#region [ Constructors ]
        //public Settings()
        //{
        //    m_hostAddress = "127.0.0.1";
        //    m_dataPort = 38402;
        //    m_metaDataPort = 6175;
        //    m_instanceName = "PPA";
        //    m_frameRate = 30;
        //    m_metadataTimeout = 60000;
        //    m_metadataTimeoutSeconds = m_metadataTimeout / 1000;
        //    m_startTime = new DateTime(2017, 7, 5, 12, 50, 8, 0);
        //    m_endTime = new DateTime(2017, 7, 5, 12, 51, 8, 0);
        //    m_pointList = "FILTER MeasurementDetail WHERE SignalAcronym IN (\'VPHM\', \'IPHM\', \'VPHA\', \'IPHA\')";
        //    m_messageInterval = 2000;
        //    m_enableLogging = true;

        //}
        //#endregion

        /// <summary>
        /// Creates a new <see cref="Settings"/> instance.
        /// </summary>
        /// <param name="typeRegistry">
        /// Type registry to use when parsing <see cref="TypeConvertedValueExpressionAttribute"/> instances,
        /// or <c>null</c> to use <see cref="ValueExpressionParser.DefaultTypeRegistry"/>.
        /// </param>
        public Settings(TypeRegistry typeRegistry) : base("systemSettings", typeRegistry)
        {
        }

        /// <summary>
        /// Gets or sets host address for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxHistorianHostAddress.Text")]
        [Description("Host address for historian connection.")]
        [UserScopedSetting]
        public string HostAddress { get; set; }

        /// <summary>
        /// Gets or sets data port for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxHistorianDataPort.Text")]
        [Description("Data port for historian connection.")]
        [UserScopedSetting]
        public int DataPort { get; set; }

        /// <summary>
        /// Gets or sets meta-data port for historian connection.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxHistorianMetadataPort.Text")]
        [Description("Meta-data port for historian connection.")]
        [UserScopedSetting]
        public int MetadataPort { get; set; }

        /// <summary>
        /// Gets or sets name of historian instance to access.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxHistorianInstanceName.Text")]
        [Description("Name of historian instance to access.")]
        [UserScopedSetting]
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets frame-rate, in frames per second, used to estimate total data for timespan.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxFrameRate.Text")]
        [Description("Frame rate, in frames per second, used to estimate total data for timespan.")]
        [UserScopedSetting]
        public int FrameRate { get; set; }

        /// <summary>
        /// Gets or sets meta-data retriever timeout, in milliseconds.
        /// </summary>
        [UserScopedSetting]
        [Description("Meta-data retriever timeout, in milliseconds.")]
        public int MetadataTimeout { get; set; }

        /// <summary>
        /// Gets or sets meta-data retriever timeout, in seconds.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxMetadataTimeout.Text")]
        [SerializeSetting(false)] // <-- Do not synchronize to config file
        public int MetadataTimeoutSeconds
        {
            get
            {
                return MetadataTimeout / 1000;
            }
            set
            {
                MetadataTimeout = value * 1000;
            }
        }

        /// <summary>
        /// Gets or sets selected start time range for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.dateTimePickerSourceTime.Value")]
        [Description("Selected start time range for historian read.")]
        [UserScopedSetting]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets selected end time range for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.dateTimePickerEndTime.Value")]
        [Description("Selected end time range for historian read.")]
        [UserScopedSetting]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets selected point list or filter expression for historian read.
        /// </summary>
        [TypeConvertedValueExpression("Form.textBoxPointList.Text")]
        [Description("Selected point list or filter expression for historian read.")]
        [UserScopedSetting]
        public string PointList { get; set; }

        /// <summary>
        /// Gets or sets message display interval.
        /// </summary>
        [TypeConvertedValueExpression("Form.maskedTextBoxMessageInterval.Text")]
        [Description("Message display interval.")]
        [UserScopedSetting]
        public int MessageInterval { get; set; }

        /// <summary>
        /// Gets or sets flag that determines if logging should be enabled.
        /// </summary>
        [TypeConvertedValueExpression("Form.checkBoxEnableLogging.Checked")]
        [Description("Flag that determines if logging should be enabled.")]
        [UserScopedSetting]
        public bool EnableLogging { get; set; }
    }
}
