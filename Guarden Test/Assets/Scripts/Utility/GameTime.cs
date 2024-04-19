using System.Collections.Generic;
using UnityEngine;

namespace TheGuarden.Utility
{
    /// <summary>
    /// GameTime handles in game clock
    /// </summary>
    public class GameTime : MonoBehaviour
    {
        private const int MinutesInDay = 24 * 60;

        public delegate void DayEnded();

        [SerializeField, Range(0, 23), Tooltip("Hour when level starts")]
        private int startingHour = 12;
        [SerializeField, Tooltip("Days of the week")]
        private List<string> days;
        [SerializeField, Tooltip("Months of the year")]
        private List<string> months;
        [SerializeField, Range(0.1f, 100), Tooltip("Time scale game runs at")]
        private float timeScale = 1f;
        [SerializeField, Tooltip("Clock time scale that makes time go faster")]
        private float clockScale = 1f;

        private float minutes = 0;
        private int day = 1;
        private int month = 1;
        private int year = 2024;
        public event DayEnded OnDayEnded;

        public int Minute => (int)minutes % 60;
        public int Hour => (int)minutes / 60;
        private int Year => year;
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
        public string DateText => $"{DayName}, The {DayOfMonth} Of {MonthName}, {Year}";
        public float DayEndProgress => minutes / MinutesInDay;

        private void Awake()
        {
            minutes = startingHour * 60;
        }

        void Update()
        {
            Time.timeScale = timeScale;
            minutes += Time.deltaTime * clockScale;

            // Reset minutes if it reaches MinutesInDay (end of day)
            if (minutes > MinutesInDay)
            {
                day += 1;
                minutes = 0;
                OnDayEnded?.Invoke();

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
            }
        }
    }
}
