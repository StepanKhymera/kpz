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
    public class KhymeraStepan : IRobotAlgorithm
    {
        public static int round = 0;
        public static Dictionary<int, int> fate = new Dictionary<int, int>();
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

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            
            Robot.Common.Robot r = robots[robotToMoveIndex];
            var mybits = robots.Where(xr => xr.OwnerName.Equals("StepanKhymera")).ToList();

            int status = -1;
            if (!fate.ContainsKey(robotToMoveIndex))
            {
                if (round == 0)
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

                    var murderOptions = whoToAttack(r.Position, r.Energy, map, robots).ToList() ; // murder check
                    if (murderOptions.Any())
                    {
                        Position murderPosition = null;
                        for (int i = 10; i >= 0; --i)
                        {
                            if (murderOptions[i] != null)
                            {
                               
                                murderPosition = murderOptions[i];
                                break;
                            }
                        } 
                        if(murderPosition != null)
                        {
                            fate.Add(robotToMoveIndex, 1);
                            return new MoveCommand() { NewPosition = murderPosition };

                        }
                    }

                    //check if wanna stay
                    bool stay = wannaStay(r.Position, map, robots);
                    if (stay) {
                        fate.Add(robotToMoveIndex, 0); // stay and duplicate
                        status = 0; 
                    } else
                    {
                        fate.Add(robotToMoveIndex, 1); // jump and look for better spot 
                        status = 2;
                    }

                }
            }
            if(status < 0)
            {
                status = fate[robotToMoveIndex] ;
            }

            //switch of fate
            switch (status)
            {
                case (0): //stay and duplicate + chance to attack
                    {
                        Random ran = new Random();
                        if(ran.Next()%100 <= 1)
                        {
                            fate[robotToMoveIndex] = 1;

                        }

                        //check if can duplicate
                        if (r.Energy > 300 && wannaAddMore(r.Position, map, mybits) && mybits.Count() < 100 ){
                            return new CreateNewRobotCommand();
                        }
                        else  //collect energy if not
                        {
                            return new CollectEnergyCommand();
                        }

                     break;
                    }
                case (1): //look for better position
                    {
                        var best = FindBest(r.Position, r.Energy, map, robots, robotToMoveIndex);
                        Position bestPosition = new Position();
                        for (int i = 10; i > 0; --i)
                        {
                            if (best[i] != null)
                            {
                                bestPosition = best[i];
                                break;
                            } 
                        }
                        if(bestPosition == null) {
                            //jump

                            Position jumpP = jump(robots, robotToMoveIndex, map);
                            if (jumpP == null)
                            {
                                fate[robotToMoveIndex] = 1;
                                return new CollectEnergyCommand();

                            }
                            return new MoveCommand() { NewPosition = jump(robots, robotToMoveIndex, map)};

                            //double energySacrifice = (r.Energy * 0.6);
                            //int distance = (int)Math.Sqrt(energySacrifice);
                            //Random ran = new Random();
                            //if (ran.Next() % 2 == 0)
                            //{
                            //    distance *= (-1); 
                            //}
                            //Position jump = r.Position;
                            //if (ran.Next() % 2 == 0)
                            //{
                            //    jump.X += distance;
                            //}
                            //else
                            //{
                            //    jump.Y += distance;
                            //}
                            //fate[robotToMoveIndex] = 1;
                            //return new MoveCommand() { NewPosition = jump };
                        }
                        fate[robotToMoveIndex] = 0;
                        if (bestPosition.Equals(r.Position))
                        {
                            return new CollectEnergyCommand();
                        } 

                        return new MoveCommand() { NewPosition = bestPosition };
                        break;
                    }

                case (2): //jump away
                    {
                        Position jumpP = jump(robots, robotToMoveIndex, map);
                        if (jumpP == null)
                        {
                            fate[robotToMoveIndex] = 1;
                            return new CollectEnergyCommand();

                        }
                        return new MoveCommand() { NewPosition = jump(robots, robotToMoveIndex, map) };
                        //double energySacrifice = (r.Energy * 0.6);
                        //int distance = (int)Math.Sqrt(energySacrifice);
                        //Random ran = new Random();
                        //if(ran.Next()%2 == 0)
                        //{
                        //    distance *= (-1);
                        //}
                        //Position jump = r.Position;
                        //if (ran.Next() % 2 == 0)
                        //{
                        //    jump.X += distance;
                        //} else
                        //{
                        //    jump.Y += distance;
                        //}
                        //fate[robotToMoveIndex] = 1;
                        //return new MoveCommand() { NewPosition = jump };
                    }
            }
            
            return new CollectEnergyCommand();
        }

        public Position jump(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            Robot.Common.Robot j = robots[robotToMoveIndex];
            double energySacrifice = (j.Energy * 0.6);
            int distance = (int)Math.Sqrt(energySacrifice);
            Position bestSpot = new Position();
            Position run = new Position();
            Position toJump = new Position();
            var bestPositions = FindBest(new Position(j.Position.X, j.Position.Y - distance), (int)(j.Energy * 0.4), map, robots, robotToMoveIndex);
            bestPositions.ForEach(x =>
            {
                if(x != null)
                {
                    bestSpot = x;
                }
            });
            toJump = new Position(j.Position.X, j.Position.Y - distance);

            bestPositions.Clear();

            bestPositions = FindBest(new Position(j.Position.X, j.Position.Y + distance), (int)(j.Energy * 0.4), map, robots, robotToMoveIndex);
            bestPositions.ForEach(x =>
            {
                if (x != null)
                {
                    run = x;
                }
            });

            if (bestSpot.Equals(new Position()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X, j.Position.Y + distance);

            }

            if (HowManyStations(bestSpot, map.Stations.ToList()) < HowManyStations(run, map.Stations.ToList()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X, j.Position.Y + distance);

            }
            bestPositions.Clear();

            bestPositions = FindBest(new Position(j.Position.X + distance, j.Position.Y ), (int)(j.Energy * 0.4), map, robots, robotToMoveIndex);
            bestPositions.ForEach(x =>
            {
                if (x != null)
                {
                    run = x;
                }
            });
            if (bestSpot.Equals(new Position()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X + distance, j.Position.Y);

            }
            if (HowManyStations(bestSpot, map.Stations.ToList()) < HowManyStations(run, map.Stations.ToList()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X + distance , j.Position.Y);

            }
            bestPositions.Clear();

            bestPositions = FindBest(new Position(j.Position.X - distance, j.Position.Y ), (int)(j.Energy * 0.4), map, robots, robotToMoveIndex);
            bestPositions.ForEach(x =>
            {
                if (x != null)
                {
                    run = x;
                }
            });
            if (bestSpot.Equals(new Position()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X - distance, j.Position.Y);

            }
            if (HowManyStations(bestSpot, map.Stations.ToList()) < HowManyStations(run, map.Stations.ToList()))
            {
                bestSpot = run;
                toJump = new Position(j.Position.X - distance, j.Position.Y);

            }
            if(bestSpot.Equals(new Position()))
            {
                return null;
            }

            return toJump;



        }

        public bool wannaAddMore(Position center, Map map, IList<Robot.Common.Robot> robots)
        {
            int howManyBotsAround = HowManyRobots(center, robots, 5).Count();
            int howManyStationsAround = HowManyStations(center, map.Stations.ToList());
            if (howManyBotsAround - 1 > howManyStationsAround)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool wannaStay(Position center, Map map, IList<Robot.Common.Robot> robots)
        {
            int howManyBotsAround = HowManyRobots(center, robots, 5).Count();
            int howManyStationsAround = HowManyStations(center, map.Stations.ToList());
            if(howManyBotsAround > howManyStationsAround)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Position> whoToAttack(Position center, int EnergyToSpare, Map map, IList<Robot.Common.Robot> robots)
        {
            List<Position> result = new List<Position>() { null, null, null, null, null, null, null, null, null, null, null, null };

            var nearbyRobots = HowManyRobots(center, robots, 4).Where(r => !r.OwnerName.Equals("StepanKhymera")).ToList();
            foreach(var enemy in nearbyRobots)
            {
                int distance = FindDistance(center, enemy.Position);
                if (distance > EnergyToSpare) continue;
                var hisStations = HowManyStations(enemy.Position, map.Stations.ToList());
                if (result[hisStations] == null) { result[hisStations] = enemy.Position; }
                else
                {
                    if (distance < FindDistance(center, result[hisStations]))
                    {
                        result[hisStations] = enemy.Position;
                    }
                }
                result[hisStations] = enemy.Position;
            }
            return result;

        }

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
                        var myNearbyBots = HowManyRobots(run, mybits, 4);
                        var onesThatMoved = myNearbyBots.Where(b => robots.IndexOf(b) <  robotToMoveIndex).ToList();
                        if (onesThatMoved.Count() > 0) {
                            continue; 
                        }
                    } else
                    {
                        var myNearbyBots = HowManyRobots(run, mybits, 4);
                        int nearbyStations = HowManyStations(run, stations);
                        if (myNearbyBots.Count() >= nearbyStations) 
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
    }
}
