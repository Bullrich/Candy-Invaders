using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Systems {
	public class SystemCalculations {

        /// <summary>Return an int giving the percentage of numPos between 0 and Max </summary>
        public int FloatToPercentage(float numPos, float Max)
        {
            float HundredPercent = Max;
            float actualPoint = HundredPercent - numPos;
            float distance = HundredPercent - actualPoint;
            float limit = (distance / HundredPercent) * 100;
            int percentageCompleted = (int)limit;

            if (percentageCompleted < 0) percentageCompleted = 0;
            else if (percentageCompleted > 100) percentageCompleted = 100;

            string percentage = percentageCompleted + "%";
            return percentageCompleted;
        }

        public float PercentageToFloat(int percentage, float Max)
        {
            float result = (Max / 100) * percentage;
            return result;
        }
    }
}