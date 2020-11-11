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

        public Resourse(string name, int amounts, Items type)
        {
            this.name = name;
            this.amounts = amounts;
            this.type = type;
        }
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

            public Enemy(string name, int damage, Monsters type)
            {
                this.name = name;
                this.damage = damage;
                this.type = type;
            }
        public int CompareTo(Enemy next)
        {
            return next.name.CompareTo(this.name);
        }
        public Enemy(string name, int damage, Monsters type, Items loot) : this(name, damage, type)
        {
            this.loot = loot;
        }
        }
    
};