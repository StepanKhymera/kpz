using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Events_And_LINQ
{
    public enum Items { red_leaf, orange_leaf, yellow_leaf, green_leaf, blue_leaf, purple_leaf, bat_wings, bone, cursed_dust, ectoplasm, gunpowder, stone };
    public enum Monsters { bat, skeleton, ghost, skull, shaman, golem };
    public class Resourse : IComparable<Resourse>
    {
        public String name;
        public int amounts;
        public Items type;

  
        public int CompareTo(Resourse next)
        {
            return next.name.CompareTo(this.name);
        }
    }
        public class Enemy : IComparable<Enemy>
    {
            public String name;
            public int damage;
            public Monsters type;
            public Items loot;

 
        public int CompareTo(Enemy next)
        {
            return next.name.CompareTo(this.name);
        }

        }
    
};