using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Events_And_LINQ
{
    public class DirectionEventArgs : EventArgs
    {
        public int direction { get; set; }
    }
    public class ButtonEventArgs : EventArgs
    {
        public char letter { get; set; }
    }
    class Game
    {
        public delegate void ItemPickedEventHandler(object source, ButtonEventArgs args);
        public event ItemPickedEventHandler ItemPicked;
        protected virtual void OnItemPicked(char button)
        {
            if (ItemPicked != null)
                ItemPicked(this, new ButtonEventArgs() { letter  = button});
        }


        public delegate void ArrowsPressedEventHandler(object source, DirectionEventArgs args);
        public event ArrowsPressedEventHandler ArrowsPressed;
        protected virtual void OnArrowsPressed(int dir)
        {
            if (ArrowsPressed != null)
                ArrowsPressed(this, new DirectionEventArgs() { direction = dir });
        }

        public delegate void WASDPressedEventHandler(object source, DirectionEventArgs args);
        public event WASDPressedEventHandler WASDPressed;
        protected virtual void OnWASDPressed(int dir)
        {
            if (WASDPressed != null)
                WASDPressed(this, new DirectionEventArgs() { direction = dir });
        }

        string eightDashes;
        int ConHeight;
        int ConWidth;
        MapManager MapManager;
        bool WASDControl;

        public Game()
        {
            Console.CursorVisible = false;
            MapManager = new MapManager();
            this.ArrowsPressed += MapManager.OnMoved;
            ItemPicked += MapManager.OnItemPicked;
            WASDControl = false;
            eightDashes = new string('-', 80);

            ConHeight = 30;
            ConWidth = 80;

            Console.WindowHeight = ConHeight;
            Console.WindowWidth = ConWidth;
        }
        public void Run()
        {
            while (true)
            {
                SetMap();
                ConsoleKeyInfo input = Console.ReadKey(true); // BLOCKING TO WAIT FOR INPUT
                //Console.WriteLine(input.Key.ToString());
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        OnArrowsPressed(1);
                        break;
                    case ConsoleKey.RightArrow:
                        OnArrowsPressed(2);
                        break;
                    case ConsoleKey.DownArrow:
                        OnArrowsPressed(3);
                        break;
                    case ConsoleKey.LeftArrow:
                        OnArrowsPressed(4);
                        break;

                    case ConsoleKey.W:
                        OnWASDPressed(1);
                        break;
                    case ConsoleKey.D:
                        OnWASDPressed(2);
                        break;
                    case ConsoleKey.S:
                        OnWASDPressed(3);
                        break;
                    case ConsoleKey.A:
                        OnWASDPressed(4);
                        break;
                    case ConsoleKey.Tab:
                        changeControl();
                        break;
                    default:
                        char letter = input.Key.ToString().First();
                        if (letter > 69 && letter < 86 && input.Key.ToString().Length == 1)
                        {
                            OnItemPicked(letter);
                        }
                        break;
                }

            }
        }
      
        void changeControl()
        {
            if (WASDControl)
            {
                ArrowsPressed += MapManager.OnMoved;
                WASDPressed -= MapManager.OnMoved;
            } else
            {
                ArrowsPressed -= MapManager.OnMoved;
                WASDPressed += MapManager.OnMoved;

            }
        }

        readonly string[] ItemStrings = { "Red leaf", "Orange leaf", "Yellow leaf", "Green leaf", "Blue leaf", "Purple leaf",
                                      "Bat wings", "Bone", "Cursed dust", "Ectoplasm", "Gunpowder", "Stone" };

        public void SetMap()
        {
            Console.Clear();
            Map currentMap = MapManager.currentMap;
            currentMap.keyEnemie.Clear();
            currentMap.keyResourse.Clear();
            String location = currentMap.name;
            Console.WriteLine(eightDashes);
            Console.WriteLine(new string(' ', (80-location.Length)/2) + location + new string(' ', (80 - location.Length) / 2));
            Console.WriteLine(eightDashes);
            Random ran = new Random();
            if (currentMap.enemies.Any())
            {
                Console.WriteLine("You spotted few enemies moving towards you:");

                if(currentMap.enemies[0] != null)
                {
                    char button = (char)ran.Next(69, 86);
                    Console.WriteLine(currentMap.enemies[0].name + " (Press \"" + button + "\" to kill)");
                    currentMap.keyEnemie.Add(button, currentMap.enemies[0]);
                }


                if (currentMap.enemies.Count() > 1 && currentMap.enemies[1] != null)
                {
                    char button = (char)ran.Next(69, 86);
                    if (currentMap.keyEnemie.ContainsKey(button)) button = (char)((int)button +1);
                    Console.WriteLine(currentMap.enemies[1].name + " (Press \"" + button + "\" to kill)");

                    currentMap.keyEnemie.Add(button, currentMap.enemies[1]);
                }
            }

            if (currentMap.resourses.Any())
            {
                Console.WriteLine("You see some usefull herbs:");
                if (currentMap.resourses[0] != null)
                {
                    char button = (char)ran.Next(69, 86);
                    if (currentMap.keyResourse.ContainsKey(button)) button = (char)((int)button + 1);

                    Console.WriteLine(currentMap.resourses[0].name + " (Press \"" + button + "\" to harvest)");
                    currentMap.keyResourse.Add(button, currentMap.resourses[0]);
                }

                   
                if (currentMap.resourses.Count() > 1 && currentMap.resourses[1] != null)
                {
                    char button = (char)ran.Next(69, 86);
                    if (currentMap.keyResourse.ContainsKey(button)) button = (char)((int)button + 1);
                    Console.WriteLine(currentMap.resourses[1].name + " (Press \"" + button + "\" to harvest)");
                    currentMap.keyResourse.Add(button, currentMap.resourses[1]);
                }

                   
            }
            Console.WriteLine(eightDashes);
            Console.WriteLine("HEALTH: " + new string('#', MapManager.health) );
            Console.WriteLine("Inventory:");
            foreach (KeyValuePair<Items, int> entry in this.MapManager.inventory)
            {
                Console.WriteLine(ItemStrings[(int)(entry.Key)] + ": " + entry.Value.ToString());
            }
            Console.WriteLine("");
        }
    }
}
    