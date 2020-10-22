using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Robot.Common;
using KhymeraStepan;
using System.Collections.Generic;
using System.Linq;

namespace Khymera_UnitTest
{
    [TestClass]
    public class KhymeraStepan_test
    {
        [TestMethod]
        public void TestAttack()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(12, 10) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 10), Energy = 100, OwnerName = "NotStepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(8, 10), Energy = 100, OwnerName = "StepanKhymera" });
            
            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            Logger.LogRound(2);
            RobotCommand response =  algorythm.DoStep(robots, 1, map);

            Assert.IsTrue(response is MoveCommand);

            Assert.AreEqual(((MoveCommand)response).NewPosition, new Position(10, 10));

        }
        [TestMethod]
        public void TestBestPosition()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();
            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(10, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(11, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(12, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(10, 14) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(11, 14) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(12, 14) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(11, 8), Energy = 1000, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 0, map);

            Assert.AreEqual( new Position(11, 12), ((MoveCommand)response).NewPosition);

        }

        [TestMethod]
        public void TestBestPositionWithRobotsAround()
        {
            KhymeraStepan.KhymeraStepan.round = 1;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            KhymeraStepan.KhymeraStepan.fate.Add(0, 1);
            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 0) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 1) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 2) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(15, 0) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(15, 1) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(15, 2) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(7, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 0), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 2), Energy = 1000, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 0, map);

            Assert.AreEqual(new Position(13, 1), ((MoveCommand)response).NewPosition);

        }

        [TestMethod]
        public void TestDuplication()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();
            KhymeraStepan.KhymeraStepan.fate.Add(0, 0);

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 0) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 1) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 2) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(2, 1), Energy = 1000, OwnerName = "StepanKhymera" });


            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 0, map);

            Assert.IsTrue(response is CreateNewRobotCommand);

        }

        [TestMethod]
        public void TestNotDuplicating()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();
            KhymeraStepan.KhymeraStepan.fate.Add(0, 0);

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 0) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 1) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 2) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(2, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 0), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 2), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(2, 2), Energy = 1000, OwnerName = "StepanKhymera" });


            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 0, map);

            Assert.IsTrue(response is CollectEnergyCommand);

        }

        [TestMethod]
        public void TestJump()
        {
            KhymeraStepan.KhymeraStepan.round = 1;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 0) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 1) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(1, 2) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(2, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 0), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 1), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(0, 2), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(2, 2), Energy = 1000, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 0, map);

            Assert.IsTrue(response is MoveCommand);

        }

        [TestMethod]
        public void TestHowManyRobots()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();
            KhymeraStepan.KhymeraStepan.fate.Add(0, 0);

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 10), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 15), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 14), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(9, 10), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(11, 11), Energy = 1000, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            int response = algorythm.HowManyRobots(new Position(10, 10), robots, 4).ToList().Count();

            Assert.AreEqual(4, response);

        }

        [TestMethod]
        public void TestWhoToAttack()
        {
            KhymeraStepan.KhymeraStepan.round = 1;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(14, 11) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(4, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(14, 10) });

            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 10), Energy = 1000, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(4, 10), Energy = 1000, OwnerName = "NotStepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(14, 10), Energy = 1000, OwnerName = "NotStepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 11), Energy = 1000, OwnerName = "NotStepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            var response = algorythm.whoToAttack(new Position(10, 10), 1000, map, robots).ToList();
            Position murderPosition = null;
            for (int i = 10; i >= 0; --i)
            {
                if (response[i] != null)
                {

                    murderPosition = response[i];
                    break;
                }
            }
            Assert.AreEqual(new Position(14, 10), murderPosition);
        }
        [TestMethod]
        public void TestInitialSpread()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(10, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(9, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(20, 10) });


            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 10), Energy = 100, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(14, 10), Energy = 100, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            RobotCommand response = algorythm.DoStep(robots, 1, map);

            Assert.IsTrue(response is MoveCommand);

            Assert.AreEqual(new Position(18, 10), ((MoveCommand)response).NewPosition);

        }
        [TestMethod]
        public void TestWannaStay()
        {
            KhymeraStepan.KhymeraStepan.round = 0;
            KhymeraStepan.KhymeraStepan.fate.Clear();

            Map map = new Map();
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(10, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(9, 10) });
            map.Stations.Add(new Robot.Common.EnergyStation() { Energy = 100, Position = new Position(11, 10) });


            var robots = new List<Robot.Common.Robot>();
            robots.Add(new Robot.Common.Robot() { Position = new Position(9, 11), Energy = 100, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(10, 9), Energy = 100, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(9, 9), Energy = 100, OwnerName = "StepanKhymera" });
            robots.Add(new Robot.Common.Robot() { Position = new Position(11, 9), Energy = 100, OwnerName = "StepanKhymera" });

            KhymeraStepan.KhymeraStepan algorythm = new KhymeraStepan.KhymeraStepan();
            bool response = algorythm.wannaStay(new Position(9, 11), map, robots);

            Assert.IsTrue(response == false);

        }
    }

}
