using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Pet
{
    public PetAppearance appearance;
    [XmlArrayItem("condition", IsNullable=false)]
    public PetCondition[] conditions;
    public PetData data;
    [XmlAttribute]
    public long id;
    public PetMacroChecker macroChecker;
    [XmlArrayItem("memory", IsNullable=false)]
    public PetMemory[] memorys;
    [XmlAttribute]
    public string name;
    public PetParameter parameter;
    public PetParameterEx parameterEx;
    public PetPrivate @private;
    [XmlArrayItem("skill", IsNullable=false)]
    public PetSkill[] skills;
    public PetSummon summon;
    [XmlAttribute]
    public DateTime updatetime;
}

