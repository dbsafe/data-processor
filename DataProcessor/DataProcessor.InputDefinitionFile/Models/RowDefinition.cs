﻿using System.Xml.Serialization;

namespace DataProcessor.InputDefinitionFile.Models
{
    public class RowDefinition
    {
        [XmlArray(ElementName = "fields")]
        [XmlArrayItem(ElementName = "field")]
        public FieldDefinition[] Fields { get; set; }
    }
}
