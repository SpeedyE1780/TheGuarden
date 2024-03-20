using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private float second = 0;
    public static int minute = 0;
    public static int hour = 0;
    public static int day = 1;
    public static int week = 1;
    public static int month = 1;
    public static int year = 2024;
    public static string dayOfTheMonth = "1st";
    public static List<string> dayNames = new List<string>();
    public static List<string> monthNames = new List<string>();  
    public List<string> inspectorDayNames;
    public List<string> inspectorMonthNames;

    [SerializeField]
    private float timeScale = 1f;

    void Start()
    {
        dayNames = inspectorDayNames; 
        monthNames = inspectorMonthNames;
    }

    // Update is called once per frame
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
