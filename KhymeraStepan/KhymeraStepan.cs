using Robot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhymeraStepan
{
    class KhymeraStepan : IRobotAlgorithm   
    {
        static int round = 0;
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
            
             Robot.Common.Robot movingRobot = robots[robotToMoveIndex];
             if ((movingRobot.Energy > 500) && (robots.Count < map.Stations.Count))
             {
                 return new CreateNewRobotCommand();
             }

             Position stationPosition = FindNearestFreeStation(robots[robotToMoveIndex], map, robots);

                 if (stationPosition == null)
                 return null;
             if (stationPosition == movingRobot.Position)
                 return new CollectEnergyCommand();
             else
             {
                 return new MoveCommand() { NewPosition = stationPosition };
             }    

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
        public static int FindDistance(Position a, Position b)
        {
            return (int)(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
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
