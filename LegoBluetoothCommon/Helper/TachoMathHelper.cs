// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    public static class TachoMathHelper
    {
        /// <summary>
        /// Calculates the individual tachocounter values for synchronized movement.
        /// </summary>
        /// <param name="degrees">The total degrees to run.</param>
        /// <param name="speedL">The speed for the left motor.</param>
        /// <param name="speedR">The speed for the right motor.</param>
        /// <param name="tachoL">The calculated tachocounter value for the left motor.</param>
        /// <param name="tachoR">The calculated tachocounter value for the right motor.</param>
        public static void CalculateTacho(int degrees, int speedL, int speedR, out int tachoL, out int tachoR)
        {
            if (degrees < 0 || degrees > 10000000)
            {
                throw new ArgumentException("Degrees must be between 0 and 10000000.", nameof(degrees));
            }

            int absSpeedL = speedL < 0 ? -speedL : speedL;
            int absSpeedR = speedR < 0 ? -speedR : speedR;

            tachoL = ((degrees * 2) * absSpeedL * speedL < 0 ? -1 : (speedL == 0 ? 0 : 1)) / (absSpeedL + absSpeedR);
            tachoR = ((degrees * 2) * absSpeedR * speedR < 0 ? -1 : (speedR == 0 ? 0 : 1)) / (absSpeedL + absSpeedR);
        }
    }
}
