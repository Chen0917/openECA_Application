// COMPILER GENERATED CODE
// THIS WILL BE OVERWRITTEN AT EACH GENERATION
// EDIT AT YOUR OWN RISK

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ECAClientFramework;
using ECAClientUtilities;
using ECACommonUtilities;
using ECACommonUtilities.Model;
using GSF.TimeSeries;

namespace Beta_Application_CTPT_LineZ.Model
{
    [CompilerGenerated]
    public class Mapper : MapperBase
    {
        #region [ Members ]

        // Fields
        private readonly Unmapper m_unmapper;

        #endregion

        #region [ Constructors ]

        public Mapper(Framework framework)
            : base(framework, SystemSettings.InputMapping)
        {
            m_unmapper = new Unmapper(framework, MappingCompiler);
            Unmapper = m_unmapper;
        }

        #endregion

        #region [ Methods ]

        public override void Map(IDictionary<MeasurementKey, IMeasurement> measurements)
        {
            SignalLookup.UpdateMeasurementLookup(measurements);
            TypeMapping inputMapping = MappingCompiler.GetTypeMapping(InputMapping);

            Reset();
            Beta_Application_CTPT_LineZ.Model.GPA.Measurement_set inputData = CreateGPAMeasurement_set(inputMapping);
            Reset();
            Beta_Application_CTPT_LineZ.Model.GPA._Measurement_setMeta inputMeta = CreateGPA_Measurement_setMeta(inputMapping);

            Algorithm.Output algorithmOutput = Algorithm.Execute(inputData, inputMeta);
            Subscriber.SendMeasurements(m_unmapper.Unmap(algorithmOutput.OutputData, algorithmOutput.OutputMeta));
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.Measurement_set CreateGPAMeasurement_set(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.Measurement_set obj = new Beta_Application_CTPT_LineZ.Model.GPA.Measurement_set();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA.Line_data UDT array for "Measurements" field
                ArrayMapping arrayMapping = (ArrayMapping)fieldLookup["Measurements"];
                PushWindowFrame(arrayMapping);

                List<Beta_Application_CTPT_LineZ.Model.GPA.Line_data> list = new List<Beta_Application_CTPT_LineZ.Model.GPA.Line_data>();
                int count = GetUDTArrayTypeMappingCount(arrayMapping);

                for (int i = 0; i < count; i++)
                {
                    TypeMapping nestedMapping = GetUDTArrayTypeMapping(arrayMapping, i);
                    list.Add(CreateGPALine_data(nestedMapping));
                }

                obj.Measurements = list.ToArray();
                PopWindowFrame(arrayMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._Measurement_setMeta CreateGPA_Measurement_setMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._Measurement_setMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._Measurement_setMeta();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta UDT array for "Measurements" field
                ArrayMapping arrayMapping = (ArrayMapping)fieldLookup["Measurements"];
                PushWindowFrame(arrayMapping);

                List<Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta> list = new List<Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta>();
                int count = GetUDTArrayTypeMappingCount(arrayMapping);

                for (int i = 0; i < count; i++)
                {
                    TypeMapping nestedMapping = GetUDTArrayTypeMapping(arrayMapping, i);
                    list.Add(CreateGPA_Line_dataMeta(nestedMapping));
                }

                obj.Measurements = list.ToArray();
                PopWindowFrame(arrayMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.Line_data CreateGPALine_data(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.Line_data obj = new Beta_Application_CTPT_LineZ.Model.GPA.Line_data();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "From_bus" field
                FieldMapping fieldMapping = fieldLookup["From_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.From_bus = CreateGPAVI_data(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "To_bus" field
                FieldMapping fieldMapping = fieldLookup["To_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.To_bus = CreateGPAVI_data(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta CreateGPA_Line_dataMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta UDT for "From_bus" field
                FieldMapping fieldMapping = fieldLookup["From_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.From_bus = CreateGPA_VI_dataMeta(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta UDT for "To_bus" field
                FieldMapping fieldMapping = fieldLookup["To_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.To_bus = CreateGPA_VI_dataMeta(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.VI_data CreateGPAVI_data(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.VI_data obj = new Beta_Application_CTPT_LineZ.Model.GPA.VI_data();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Voltage" field
                FieldMapping fieldMapping = fieldLookup["Voltage"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.Voltage = CreateGPAPhasor(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Current" field
                FieldMapping fieldMapping = fieldLookup["Current"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.Current = CreateGPAPhasor(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta CreateGPA_VI_dataMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta();

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta UDT for "Voltage" field
                FieldMapping fieldMapping = fieldLookup["Voltage"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.Voltage = CreateGPA_PhasorMeta(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            {
                // Create Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta UDT for "Current" field
                FieldMapping fieldMapping = fieldLookup["Current"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrame(fieldMapping);
                obj.Current = CreateGPA_PhasorMeta(nestedMapping);
                PopRelativeFrame(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.Phasor CreateGPAPhasor(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.Phasor obj = new Beta_Application_CTPT_LineZ.Model.GPA.Phasor();

            {
                // Assign double value to "Magnitude" field
                FieldMapping fieldMapping = fieldLookup["Magnitude"];
                IMeasurement measurement = GetMeasurement(fieldMapping);
                obj.Magnitude = (double)measurement.Value;
            }

            {
                // Assign double value to "Angle" field
                FieldMapping fieldMapping = fieldLookup["Angle"];
                IMeasurement measurement = GetMeasurement(fieldMapping);
                obj.Angle = (double)measurement.Value;
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta CreateGPA_PhasorMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta();

            {
                // Assign MetaValues value to "Magnitude" field
                FieldMapping fieldMapping = fieldLookup["Magnitude"];
                IMeasurement measurement = GetMeasurement(fieldMapping);
                obj.Magnitude = GetMetaValues(measurement);
            }

            {
                // Assign MetaValues value to "Angle" field
                FieldMapping fieldMapping = fieldLookup["Angle"];
                IMeasurement measurement = GetMeasurement(fieldMapping);
                obj.Angle = GetMetaValues(measurement);
            }

            return obj;
        }

        #endregion
    }
}
