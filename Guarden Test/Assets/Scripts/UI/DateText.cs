using TMPro;
using UnityEngine;
using TheGuarden.Utility;
using System.Collections.Generic;

namespace TheGuarden.UI
{
    /// <summary>
    /// DateText represents a text on the canvas showing the date
    /// </summary>
    internal class DateText : MonoBehaviour
    {
        [SerializeField, Tooltip("Date text on screen")]
        internal TMP_Text dateText;

        [SerializeField, Tooltip("Days of the week")]
        private List<string> days;
        [SerializeField, Tooltip("Months of the year")]
        private List<string> months;

        private int day = 0;
        private int month = 1;
        private int year = 2024;

        private string DayName => days[(day - 1) % 7];
        private string MonthName => months[month - 1];
        private string DayOfMonth
        {
            get
            {
                string suffix = "th";

                if (day == 1 || (day > 20 && day % 10 == 1))
                {
                    suffix = "st";
                }
                else if (day == 2 || (day > 20 && day % 10 == 2))
                {
                    suffix = "nd";
                }
                else if (day == 3 || (day > 20 && day % 10 == 3))
                {
                    suffix = "rd";
                }

                return $"{day}{suffix}";
            }
        }
        private string Date => $"{DayName}, The {DayOfMonth} Of {MonthName}, {year}";

        public void UpdateDate()
        {
            day += 1;

            // Check if a month has passed
            if (day > 28)
            {
                day = 1;
                month += 1;

                // Check if a year has passed
                if (month > 12)
                {
                    year += 1;
                    month = 1;
                }
            }

            dateText.text = Date;
        }

#if UNITY_EDITOR
        internal void SetDateToDayOne()
        {
            day = 1;
            dateText.text = Date;
            day = 0;
        }
#endif
    }
}
