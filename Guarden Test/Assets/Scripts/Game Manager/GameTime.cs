using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private static GameTime Instance { get; set; }

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

    public static int Minute => Instance.minute;
    public static int Hour => Instance.hour;
    public static int Day => Instance.day;
    public static int Week => Instance.week;
    public static int Month => Instance.month;
    public static int Year => Instance.year;
    public static string DayName => Instance.inspectorDayNames[Instance.day];
    public static string MonthName => Instance.inspectorMonthNames[Instance.month];
    public static string DayOfMonth
    {
        get
        {
            string suffix = "th";

            if (Instance.dayOfTheMonth == 1 || (Instance.dayOfTheMonth > 20 && Instance.dayOfTheMonth % 10 == 1))
            {
                suffix = "st";
            }
            else if (Instance.dayOfTheMonth == 2 || (Instance.dayOfTheMonth > 20 && Instance.dayOfTheMonth % 10 == 2))
            {
                suffix = "nd";
            }
            else if (Instance.dayOfTheMonth == 3 || (Instance.dayOfTheMonth > 20 && Instance.dayOfTheMonth % 10 == 3))
            {
                suffix = "rd";
            }

            return $"{Instance.dayOfTheMonth}{suffix}";
        }
    }

    void Start()
    {
        Instance = this;
    }

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
