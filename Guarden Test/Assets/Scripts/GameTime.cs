using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static GameTime gameTime;
    public int second = 0;
    public static int minute = 0;
    public static int hour = 0;
    public int day = 1;
    public int week = 1;
    public int month = 1;
    public int year = 0;
    public List<string> dayNames = new List<string>();
    public List<string> monthNames = new List<string>();  

    public float timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        second += Mathf.RoundToInt(Time.deltaTime * (60f / timeScale)); // Adjust time scale

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int dayOfMonth = ((week - 1) * 7) + day;
            string tempMonthDay = dayOfMonth.ToString();

            switch (dayOfMonth)
            {
                case 1: tempMonthDay = dayOfMonth.ToString() + "st"; break;
                case 2: tempMonthDay = dayOfMonth.ToString() + "nd"; break;
                case 3: tempMonthDay = dayOfMonth.ToString() + "rd"; break;

                default: tempMonthDay = dayOfMonth.ToString() + "th"; break;
                   
            }

            if (dayOfMonth >= 11 && dayOfMonth <= 13) tempMonthDay = dayOfMonth.ToString() + "th";


            Debug.Log("GameTime: " + year + " : " + monthNames[month] + " : " + tempMonthDay + " : " + dayNames[day] + " : " + hour.ToString("00") + " : " + minute.ToString("00") + " : " + second.ToString("00"));
            Debug.Log("GameTime: " + hour.ToString("00") + " : " + minute.ToString("00") + " : " + second.ToString("00"));
        }
    }
}
