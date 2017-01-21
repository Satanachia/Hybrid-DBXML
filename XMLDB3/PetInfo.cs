namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;

    [XmlRoot(Namespace="", IsNullable=false)]
    public class PetInfo : Pet
    {
        [XmlIgnore]
        public Hashtable inventory;

        public override string ToString()
        {
            return (base.ToString() + ":" + this.id.ToString());
        }

        [XmlArray("Inventory")]
        public Item[] _inventory
        {
            get
            {
                if (this.inventory != null)
                {
                    Item[] array = new Item[this.inventory.Values.Count];
                    this.inventory.Values.CopyTo(array, 0);
                    return array;
                }
                return null;
            }
            set
            {
                this.inventory = new Hashtable(value.Length);
                foreach (Item item in value)
                {
                    this.inventory.Add(item.id, item);
                }
            }
        }
    }
}

