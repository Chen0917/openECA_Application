// COMPILER GENERATED CODE
// THIS WILL BE OVERWRITTEN AT EACH GENERATION
// EDIT AT YOUR OWN RISK

using System;
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
    public class Unmapper : UnmapperBase
    {
        #region [ Constructors ]

        public Unmapper(Framework framework, MappingCompiler mappingCompiler)
            : base(framework, mappingCompiler, SystemSettings.OutputMapping)
        {
            Algorithm.Output.CreateNew = () => new Algorithm.Output()
            {
                OutputData = FillOutputData(),
                OutputMeta = FillOutputMeta()
            };
        }

        #endregion

        #region [ Methods ]

        public Beta_Application_CTPT_LineZ.Model.GPA.Line_data FillOutputData()
        {
            TypeMapping outputMapping = MappingCompiler.GetTypeMapping(OutputMapping);
            Reset();
            return FillGPALine_data(outputMapping);
        }

        public Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta FillOutputMeta()
        {
            TypeMapping outputMeta = MappingCompiler.GetTypeMapping(OutputMapping);
            Reset();
            return FillGPA_Line_dataMeta(outputMeta);
        }

        public IEnumerable<IMeasurement> Unmap(Beta_Application_CTPT_LineZ.Model.GPA.Line_data outputData, Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta outputMeta)
        {
            List<IMeasurement> measurements = new List<IMeasurement>();
            TypeMapping outputMapping = MappingCompiler.GetTypeMapping(OutputMapping);

            CollectFromGPALine_data(measurements, outputMapping, outputData, outputMeta);

            return measurements;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.Line_data FillGPALine_data(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.Line_data obj = new Beta_Application_CTPT_LineZ.Model.GPA.Line_data();

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "From_bus" field
                FieldMapping fieldMapping = fieldLookup["From_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.From_bus = this.FillGPAVI_data(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "To_bus" field
                FieldMapping fieldMapping = fieldLookup["To_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.To_bus = this.FillGPAVI_data(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta FillGPA_Line_dataMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta();

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta UDT for "From_bus" field
                FieldMapping fieldMapping = fieldLookup["From_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.From_bus = this.FillGPA_VI_dataMeta(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta UDT for "To_bus" field
                FieldMapping fieldMapping = fieldLookup["To_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.To_bus = this.FillGPA_VI_dataMeta(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.VI_data FillGPAVI_data(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.VI_data obj = new Beta_Application_CTPT_LineZ.Model.GPA.VI_data();

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Voltage" field
                FieldMapping fieldMapping = fieldLookup["Voltage"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.Voltage = this.FillGPAPhasor(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Current" field
                FieldMapping fieldMapping = fieldLookup["Current"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.Current = this.FillGPAPhasor(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta FillGPA_VI_dataMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta();

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta UDT for "Voltage" field
                FieldMapping fieldMapping = fieldLookup["Voltage"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.Voltage = this.FillGPA_PhasorMeta(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            {
                // Initialize Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta UDT for "Current" field
                FieldMapping fieldMapping = fieldLookup["Current"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);

                PushRelativeFrameTime(fieldMapping);
                obj.Current = this.FillGPA_PhasorMeta(nestedMapping);
                PopRelativeFrameTime(fieldMapping);
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA.Phasor FillGPAPhasor(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA.Phasor obj = new Beta_Application_CTPT_LineZ.Model.GPA.Phasor();

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            return obj;
        }

        private Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta FillGPA_PhasorMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta obj = new Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta();

            {
                // Initialize meta value structure to "Magnitude" field
                FieldMapping fieldMapping = fieldLookup["Magnitude"];
                obj.Magnitude = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "Angle" field
                FieldMapping fieldMapping = fieldLookup["Angle"];
                obj.Angle = CreateMetaValues(fieldMapping);
            }

            return obj;
        }

        private void CollectFromGPALine_data(List<IMeasurement> measurements, TypeMapping typeMapping, Beta_Application_CTPT_LineZ.Model.GPA.Line_data data, Beta_Application_CTPT_LineZ.Model.GPA._Line_dataMeta meta)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);

            {
                // Convert values from Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "From_bus" field to measurements
                FieldMapping fieldMapping = fieldLookup["From_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);
                CollectFromGPAVI_data(measurements, nestedMapping, data.From_bus, meta.From_bus);
            }

            {
                // Convert values from Beta_Application_CTPT_LineZ.Model.GPA.VI_data UDT for "To_bus" field to measurements
                FieldMapping fieldMapping = fieldLookup["To_bus"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);
                CollectFromGPAVI_data(measurements, nestedMapping, data.To_bus, meta.To_bus);
            }
        }

        private void CollectFromGPAVI_data(List<IMeasurement> measurements, TypeMapping typeMapping, Beta_Application_CTPT_LineZ.Model.GPA.VI_data data, Beta_Application_CTPT_LineZ.Model.GPA._VI_dataMeta meta)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);

            {
                // Convert values from Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Voltage" field to measurements
                FieldMapping fieldMapping = fieldLookup["Voltage"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);
                CollectFromGPAPhasor(measurements, nestedMapping, data.Voltage, meta.Voltage);
            }

            {
                // Convert values from Beta_Application_CTPT_LineZ.Model.GPA.Phasor UDT for "Current" field to measurements
                FieldMapping fieldMapping = fieldLookup["Current"];
                TypeMapping nestedMapping = GetTypeMapping(fieldMapping);
                CollectFromGPAPhasor(measurements, nestedMapping, data.Current, meta.Current);
            }
        }

        private void CollectFromGPAPhasor(List<IMeasurement> measurements, TypeMapping typeMapping, Beta_Application_CTPT_LineZ.Model.GPA.Phasor data, Beta_Application_CTPT_LineZ.Model.GPA._PhasorMeta meta)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);

            {
                // Convert value from "Magnitude" field to measurement
                FieldMapping fieldMapping = fieldLookup["Magnitude"];
                IMeasurement measurement = MakeMeasurement(meta.Magnitude, (double)data.Magnitude);
                measurements.Add(measurement);
            }

            {
                // Convert value from "Angle" field to measurement
                FieldMapping fieldMapping = fieldLookup["Angle"];
                IMeasurement measurement = MakeMeasurement(meta.Angle, (double)data.Angle);
                measurements.Add(measurement);
            }
        }

        #endregion
    }
}
