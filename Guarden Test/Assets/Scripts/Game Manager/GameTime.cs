using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private float second = 0;
    private int minute = 0;
    private int hour = 0;
    private int day = 1;
    private int week = 1;
    private int month = 1;
    private int year = 2024;
    private int dayOfTheMonth = 1;

    [SerializeField]
    private List<string> inspectorDayNames;
    [SerializeField]
    private List<string> inspectorMonthNames;
    [SerializeField, Range(0.1f, 100)]
    private float timeScale = 1f;
    [SerializeField]
    private float clockScale = 1f;

    public int Minute => minute;
    public int Hour => hour;
    public int Day => day;
    public int Week => week;
    public int Month => month;
    public int Year => year;
    public string DayName => inspectorDayNames[day];
    public string MonthName => inspectorMonthNames[month];
    public string DayOfMonth
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
