using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Events_And_LINQ
{

    public interface AbstractGen
    {
        public abstract Enemy CreateEnemy();
        public abstract Resourse CreateResourse();
    };

    class HouseGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return null;
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Green Leaf", 1, Items.green_leaf );
        }
    }

    class ForestGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return new Enemy("Bat", 2, Monsters.bat, Items.bat_wings);
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Yellow Leaf", 1, Items.yellow_leaf);
        }
    };

    class DeepForestGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return new Enemy("Ghost", 2, Monsters.ghost, Items.ectoplasm);
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Blue Leaf", 1, Items.blue_leaf);
        }
    }


    class MountainsGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return new Enemy("Golem", 2, Monsters.golem, Items.stone);
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Red Leaf", 1, Items.red_leaf);
        }
    };

    class PlainesGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return new Enemy("Shaman", 2, Monsters.shaman, Items.gunpowder);
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Yellow Leaf", 1, Items.yellow_leaf);
        }
    };

    class LakeGen : AbstractGen
    {
        Enemy AbstractGen.CreateEnemy()
        {
            return new Enemy("Skeleton", 2, Monsters.skeleton, Items.bone);
        }

        Resourse AbstractGen.CreateResourse()
        {
            return new Resourse("Purple Leaf", 1, Items.purple_leaf);
        }
    };
    public class MapManager
    {
         int x;
         int y;
        public Map currentMap;
        public Dictionary<Items, int> inventory;
        List<Map> maps;
        public int health = 0;

        public MapManager()
        {
            inventory = new Dictionary<Items, int>();
            health = 20;
            x = 0;
            y = 0;
            maps = new List<Map>();
            maps.Add(new Map("Home", 0, 0, new HouseGen()));
            maps.Add(new Map("Forest", -1, 0, new ForestGen()));
            maps.Add(new Map("DeepForest", -2, 0, new DeepForestGen()));
            maps.Add(new Map("Mountains", 0, 1, new MountainsGen()));
            maps.Add(new Map("Lake", 0, -1, new LakeGen()));
            maps.Add(new Map("Plains", 1, -1, new PlainesGen()));

            currentMap = maps.Where(mp => (mp.Mx == 0) && (mp.My == 0)).First(); 
        }

        public void OnMoved(object source, DirectionEventArgs dir)
        {
            int testX = x;
            int testY = y;
            switch (dir.direction)
            {
                case 1:
                    {
                        ++testY;
                        break;
                    }
                case 2:
                    {
                        ++testX;
                        break;
                    }
                case 3:
                    {
                        --testY;
                        break;
                    }
                case 4:
                    {
                        --testX;
                        break;
                    }
            }
            Map find = maps.Where(ar => (ar.Mx == testX) && (ar.My == testY)).FirstOrDefault();
            if (find == null) 
                return;
            currentMap = find;
            x = testX;
            y = testY;

        }
        public void OnItemPicked(object source, ButtonEventArgs dir)
        {

            char letter = dir.letter;
            Items type;
            int amount;
            if (currentMap.keyEnemie.ContainsKey(letter))
            {
                Enemy extract = currentMap.keyEnemie[letter];
                currentMap.keyEnemie.Remove(letter);
                currentMap.enemies.Remove(extract);
                type = extract.loot;
                amount = 1;
                health -= extract.damage;

            } else
            if (currentMap.keyResourse.ContainsKey(letter))
            {
                Resourse extract = currentMap.keyResourse[letter];
                currentMap.keyResourse.Remove(letter);
                currentMap.resourses.Remove(extract);
                type = extract.type;
                amount = extract.amounts;
            } else
            {
                return;
            }

             if (inventory.ContainsKey(type))
            {
                inventory[type] += amount;
            } else
            {
                inventory.Add(type, amount);
            }
        }
    }


public class Map
    {
        public String name;
        public int Mx;
        public  int My;
        public  AbstractGen ContentGen;
        public  List<Enemy> enemies;
        public  List<Resourse> resourses;
        public Dictionary<char, Enemy> keyEnemie;
        public Dictionary<char, Resourse> keyResourse;

        public Map(string name, int mx, int my, AbstractGen contentGen)
        {
            keyEnemie = new Dictionary<char, Enemy>();
            keyResourse = new Dictionary<char, Resourse>();
            this.name = name;
            Mx = mx;
            My = my;
            ContentGen = contentGen;
            enemies = new List<Enemy>();
            resourses = new List<Resourse>();
            Enemy run = ContentGen.CreateEnemy();
            if (run != null) enemies.Add(run);
            run = ContentGen.CreateEnemy();
            if (run != null) enemies.Add(run);

            Resourse runR = ContentGen.CreateResourse();
            if (runR != null) resourses.Add(runR);
            runR = ContentGen.CreateResourse();
            if (runR != null) resourses.Add(runR);

            enemies.Sort();
            resourses.Sort();

        }
    };
}
