//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.0.864.0 using the following command:
// XmlSchemaClassGenerator.Console .\v2\thing.xsd
namespace Common
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("integerValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IntegerValue
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("decimalValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DecimalValue
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public decimal Value { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("stringValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StringValue
    {
        
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("localDateValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LocalDateValue
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 10.</para>
        /// <para xml:lang="en">Maximum length: 10.</para>
        /// <para xml:lang="en">Pattern: [1-9][0-9]{3}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]).</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(10)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[1-9][0-9]{3}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("localDateTimeValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LocalDateTimeValue
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 19.</para>
        /// <para xml:lang="en">Maximum length: 19.</para>
        /// <para xml:lang="en">Pattern: [1-9][0-9]{3}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([0-1][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60).</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(19)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(19)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[1-9][0-9]{3}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01]) ([0-1][0-9]|2[0-3]):[0-5][" +
            "0-9]:([0-5][0-9]|60)")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.864.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("zonedDateTimeValue", Namespace="http://www.boardgamegeek.com/common")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZonedDateTimeValue
    {
        
        /// <summary>
        /// <para xml:lang="en">Minimum length: 31.</para>
        /// <para xml:lang="en">Maximum length: 31.</para>
        /// <para xml:lang="en">Pattern: (Mon|Tue|Wed|Thu|Fri|Sat|Sun),(0[1-9]|[1-2][0-9]|3[0-1]) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) [1-9][0-9]{3} ([0-1][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-9]|60) [+|-]([0-1][0-9]|2[0-3])[0-5][0-9].</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(31)]
        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(31)]
        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("(Mon|Tue|Wed|Thu|Fri|Sat|Sun),(0[1-9]|[1-2][0-9]|3[0-1]) (Jan|Feb|Mar|Apr|May|Jun" +
            "|Jul|Aug|Sep|Oct|Nov|Dec) [1-9][0-9]{3} ([0-1][0-9]|2[0-3]):[0-5][0-9]:([0-5][0-" +
            "9]|60) [+|-]([0-1][0-9]|2[0-3])[0-5][0-9]")]
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
}
