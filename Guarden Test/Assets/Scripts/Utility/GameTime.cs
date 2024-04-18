using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// GameTime handles in game clock
    /// </summary>
    public class GameTime : MonoBehaviour
    {
        public delegate void DayEnded();

        private float second = 0;
        private int minute = 0;
        private int hour = 0;
        private int day = 1;
        private int week = 1;
        private int month = 1;
        private int year = 2024;
        private int dayOfTheMonth = 1;

        [SerializeField, Tooltip("Days of the week")]
        private List<string> days;
        [SerializeField, Tooltip("Months of the year")]
        private List<string> months;
        [SerializeField, Range(0.1f, 100), Tooltip("Time scale game runs at")]
        private float timeScale = 1f;
        [SerializeField, Tooltip("Clock time scale that makes time go faster")]
        private float clockScale = 1f;

        public int Minute => minute;
        public int Hour => hour;
        private int Year => year;
        private string DayName => days[day - 1];
        private string MonthName => months[month - 1];
        private string DayOfMonth
        {
            get
            {
                string suffix = "th";

                if (dayOfTheMonth == 1 || (dayOfTheMonth > 20 && dayOfTheMonth % 10 == 1))
                {
                    suffix = "st";
                }
                else if (dayOfTheMonth == 2 || (dayOfTheMonth > 20 && dayOfTheMonth % 10 == 2))
                {
                    suffix = "nd";
                }
                else if (dayOfTheMonth == 3 || (dayOfTheMonth > 20 && dayOfTheMonth % 10 == 3))
                {
                    suffix = "rd";
                }

                return $"{dayOfTheMonth}{suffix}";
            }
        }
        public string DateText => $"{DayName}, The {DayOfMonth} Of {MonthName}, {Year}";

        public event DayEnded OnDayEnded;

        void Update()
        {
            Time.timeScale = timeScale;
            second += Time.deltaTime * 60.0f * clockScale;

            // Check if a minute has passed
            if (second >= 60)
            {
                minute += Mathf.FloorToInt(second / 60.0f);
                second %= 60.0f;

                // Check if an hour has passed
                if (minute >= 60)
                {
                    hour += minute / 60;
                    minute %= 60;

                    // Reset hour if it reaches 24 (end of day)
                    if (hour >= 24)
                    {
                        day += hour / 24;
                        dayOfTheMonth += hour / 24;
                        hour %= 24;
                        OnDayEnded?.Invoke();

                        // Check if a week has passed
                        if (day > 7)
                        {
                            week += day / 7;
                            day %= 7;

                            // Check if a month has passed
                            if (week > 4)
                            {
                                month += week / 4;
                                week %= 4;
                                dayOfTheMonth = day;

                                // Check if a year has passed
                                if (month > 12)
                                {
                                    year += month / 12;
                                    month %= 12;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
