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
        RobotCommand IRobotAlgorithm.DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            var newpos = robots[robotToMoveIndex].Position;
            ++newpos.X;
            ++newpos.Y;

            return new MoveCommand() { NewPosition = newpos};
        }

        private void Logger_OnLogRound(object sender, LogRoundEventArgs e)
        {
            ++round;
        }




    }
}
