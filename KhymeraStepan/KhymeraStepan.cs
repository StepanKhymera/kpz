using Robot.Common;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


/* 
Дядя Богдан все бачить
 ▄███████▀▀▀▀▀▀███████▄ 
░▐████▀▒▒▒▒▒▒▒▒▒▒▀██████▄
░███▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▀█████
░▐██▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████▌
░▐█▌▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒████▌
░░█▒▄▀▀▀▀▀▄▒▒▄▀▀▀▀▀▄▒▐███▌
░░░▐░░░▄▄░░▌▐░░░▄▄░░▌▐███▌
░▄▀▌░░░▀▀░░▌▐░░░▀▀░░▌▒▀▒█▌
░▌▒▀▄░░░░▄▀▒▒▀▄░░░▄▀▒▒▄▀▒▌
░▀▄▐▒▀▀▀▀▒▒▒▒▒▒▀▀▀▒▒▒▒▒▒█
░░░▀▌▒▄██▄▄▄▄████▄▒▒▒▒█▀
░░░░▄██████████████▒▒▐▌
░░░▀███▀▀████▀█████▀▒▌
░░░░░▌▒▒▒▄▒▒▒▄▒▒▒▒▒▒▐
░░░░░▌▒▒▒▒▀▀▀▒▒▒▒▒▒▒▐
Дядя Богдан оберігає код
 */

namespace KhymeraStepan
{
    class KhymeraStepan : IRobotAlgorithm   
    {
        static int round = 0;
        static Dictionary<int , int > fate;
        public KhymeraStepan()
        {
            Logger.OnLogRound += Logger_OnLogRound;
        }

        public string Author
        {
            get { return "StepanKhymera"; }
        }
        private void Logger_OnLogRound(object sender, LogRoundEventArgs e)
        {
            ++round; 
        }

        RobotCommand IRobotAlgorithm.DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot r = robots[robotToMoveIndex];


            int status;
            if (!fate.TryGetValue(robotToMoveIndex, out status))
            {
                if(round == 0)
                {
                    //first 10 bot action
                    var best = FindBest(r.Position, r.Energy, map, robots, robotToMoveIndex);
                    Position bestPosition = new Position();
                    for (int i = 10; i >= 0; --i)
                    {
                        if (best[i] != null)
                        {
                            bestPosition = best[i];
                            break;
                        }
                    }
                    fate.Add(robotToMoveIndex, 0); // 0 - fate for first 10 bots
                    if (bestPosition.Equals(r.Position))
                    {
                        return new CollectEnergyCommand();
                    }
                    return new MoveCommand() { NewPosition = bestPosition };
                } else
                {
                    //new bot action

                    //murder

                    //check if wanna stay
                }
            } else
            {
                //switch of fate
                switch (status)
                {
                    case (0):
                        {
                            //check if can duplicate
                            //collect energy if not
                            break;
                        }
                     //case (1)
                }
            }

            var mybits = robots.Where(x => x.OwnerName.Equals("StepanKhymera")).ToList();
            Position corner = new Position(r.Position.X-5, r.Position.Y-5);
           

            for (int i = 0; i < 10; ++i){ 
                corner.X += 1;
                corner.Y -= 10;
                    for (int J = 0; J < 10; ++J) {
                    corner.Y += 1; 
                     
                    var found = robots.Where(fr => fr.Position == corner).ToList();
                        if (found.Any())
                        {
                             //if( found.FirstOrDefault().OwnerName != "StepanKhymera")
                            return new MoveCommand() { NewPosition = corner };
                        }
                    }
                } 
             
            return new CollectEnergyCommand();
        }
         
         




















        public bool IsCellFree(Position cell, Robot.Common.Robot movingrobot,  IList<Robot.Common.Robot> robots)
        {
            foreach(var robot in robots)
            {
                if (robot.Position == cell)
                {
                    if(robot != movingrobot)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //public List<Position> WhoToAttack()

       public int HowManyStations(Position center, List<Robot.Common.EnergyStation> stations)
        {
            int delta = 2;
            var localStations = stations.Where(s => s.Position.X >= center.X - delta && s.Position.X <= center.X + delta && s.Position.Y >= center.Y - delta && s.Position.Y <= center.Y + delta).ToList();
            return localStations.Count();
        }

        public List<Position> FindBest(Position center, int EnergyToSpare, Map map, IList<Robot.Common.Robot> robots, int robotToMoveIndex)
        {
            var mybits = robots.Where(xr => xr.OwnerName.Equals("StepanKhymera")).ToList();

            int delta = (int)Math.Sqrt(EnergyToSpare)+2;
            var stations = map.Stations.Where(s => s.Position.X >= center.X - delta && s.Position.X <= center.X + delta && s.Position.Y >= center.Y - delta && s.Position.Y <= center.Y + delta).ToList();
           
            delta -= 2;
            int x = center.X - delta;
            int y = center.Y - delta;
            List<Position> result = new List<Position>() {null, null, null, null, null, null, null, null, null, null, null, null};
            
            for (;x < center.X + delta; ++x)
            {
                y = center.Y - delta;  
                for (; y < center.Y + delta; ++y)
                { 
                    Position run = new Position(x, y);
                    int distance = FindDistance(center, run);
                    if (distance > EnergyToSpare) continue;
                    if (mybits.Where(m => m.Position.Equals(run)).Count() > 0) { continue; }
                    if (round == 0)
                    {
                        var myNearbyBots = HowManyRobots(run, mybits, 2);
                        var onesThatMoved = myNearbyBots.Where(b => robots.IndexOf(b) <  robotToMoveIndex).ToList();
                        if (onesThatMoved.Count() > 0) {
                            continue; 
                        }
                    } else
                    {
                        var myNearbyBots = HowManyRobots(run, mybits, 4);
                        int nearbyStations = HowManyStations(run, stations);
                        if (myNearbyBots.Count() > nearbyStations)
                        {
                            continue;
                        }
                    }
                    int numberOfStations = HowManyStations(run, stations);

                    if (result[numberOfStations] == null) { result[numberOfStations] = run; }
                    else
                    {
                        if(distance < FindDistance(center, result[numberOfStations]))
                        {
                            result[numberOfStations] = run;
                        }
                       

                    }
                    


                }
            }

            return result;
        }

        public List<Robot.Common.Robot> HowManyRobots(Position center, IList<Robot.Common.Robot> robots, int delta)
        {
            var localRobits = robots.Where(s => s.Position.X >= center.X - delta && s.Position.X <= center.X + delta && s.Position.Y >= center.Y - delta && s.Position.Y <= center.Y + delta).ToList();
            return localRobits;
        }

        public static int FindDistance(Position a, Position b)
        {
            return (int)(((a.X - b.X)*(a.X - b.X)) + (a.Y - b.Y)* (a.Y - b.Y));
        }

        public Position FindNearestFreeStation(Robot.Common.Robot movingRobot, Map map, IList<Robot.Common.Robot> robots)
        {
            EnergyStation nearest = null;
            int minDistance = int.MaxValue;
            foreach (var station in map.Stations)
            {
                if (IsCellFree(station.Position, movingRobot, robots))
                {
                    int d = FindDistance(station.Position, movingRobot.Position);
                    if (d < minDistance)

                    {

                        minDistance = d;
                        nearest = station;

                    }
                }
            }
            return nearest == null ? null : nearest.Position;
        }


    }
}
