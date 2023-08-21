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
using Common;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace GameSync.Business.BoardGamesGeek.Schemas.Thing
{


    public class Name
    {
        [XmlAttribute("type")]
        public string Type { get; set;}

        [XmlAttribute("value")]
        public string Value { get; set;}
    }

    [Serializable]
    public class ThingItem
    {

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("yearpublished")]
        public IntegerValue YearPublished { get; set; }
        
        [XmlElement("minplayers")]
        public IntegerValue MinPlayers { get; set; }
        
        [XmlElement("maxplayers")]
        public IntegerValue MaxPlayers { get; set; }

        [XmlElement("playingtime")]
        public IntegerValue PlayingTime { get; set; }
        
        [XmlElement("minage")]
        public IntegerValue MinAge { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("name")]
        public Collection<Name> Names { get; set; }

        [XmlElement("image")]
        public string Image { get; set; }
        
        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "items")]
    public class ThingRoot
    {

        [XmlElement(ElementName = "item")]
        public Collection<ThingItem> Items { get; set; } 

    }
    
}
