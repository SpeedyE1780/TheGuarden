using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private static GameTime Instance {get; set;}

    private float second = 0;
    private int minute = 0;
    private int hour = 0;
    private int day = 1;
    private int week = 1;
    private int month = 1;
    private int year = 2024;
    public static string dayOfTheMonth = "1st";

    [SerializeField]
    private List<string> inspectorDayNames;
    [SerializeField]
    private List<string> inspectorMonthNames;
    [SerializeField, Range(0.1f, 100)]
    private float timeScale = 1f;

    public static int Minute => Instance.minute;
    public static int Hour => Instance.hour;
    public static int Day => Instance.day;
    public static int Week => Instance.week;
    public static int Month => Instance.month;
    public static int Year => Instance.year;
    public static string DayName => Instance.inspectorDayNames[Instance.day];
    public static string MonthName => Instance.inspectorMonthNames[Instance.month];

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        Time.timeScale = timeScale;
        second += Time.deltaTime * 60.0f;

        // Check if a minute has passed
        if (second >= 60)
        {
            second -= 60;
            minute++;

            // Check if an hour has passed
            if (minute >= 60)
            {
                minute -= 60;
                hour++;

                // Reset hour if it reaches 24 (end of day)
                if (hour >= 24)
                {
                    hour -= 24;
                    day++;
                    int dayOfMonth = ((week - 1) * 7) + day;


                    switch (dayOfMonth)
                    {
                        case 1: dayOfTheMonth = dayOfMonth.ToString() + "st"; break;
                        case 2: dayOfTheMonth = dayOfMonth.ToString() + "nd"; break;
                        case 3: dayOfTheMonth = dayOfMonth.ToString() + "rd"; break;

                        default: dayOfTheMonth = dayOfMonth.ToString() + "th"; break;

                    }

                    if (dayOfMonth >= 11 && dayOfMonth <= 13) dayOfTheMonth = dayOfMonth.ToString() + "th";

                    // Check if a week has passed
                    if (day > 7)
                    {
                        day = 1;
                        week++;

                        // Check if a month has passed
                        if (week > 4)
                        {
                            week = 1;
                            month++;

                            // Check if a year has passed
                            if (month > 12)
                            {
                                month = 1;
                                year++;
                            }
                        }
                    }
                }
            }
        }

        
    }
    
}
