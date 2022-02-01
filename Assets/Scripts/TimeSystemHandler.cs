using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystemHandler : MonoBehaviour
{
    public float timeSeconds, timeMinutes, timeHours = 0f;
    public float timeDays = 1f;
    public float timeMonths = 1f;
    public float timeYears = 1f;
    float secondsPerMinute = 60f;
    float minutesPerHour = 60f;
    float hoursPerDay = 24f;
    float daysPerMonth = 31f;
    float monthsPerYear = 13f;
    float timeSpeed = 1000000f;
    public string currentMonth = "";
    public string currentSeason = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartTimeSystem();
        //ShowFullTime();
    }

    void StartTimeSystem()
    {
        timeSeconds = (timeSeconds + Time.deltaTime) * timeSpeed;
        if(timeSeconds >= secondsPerMinute)
        {
            timeMinutes++;
            timeSeconds = 0f;
            if(timeMinutes >= minutesPerHour)
            {
                timeHours++;
                timeMinutes = 0f;
                if(timeHours >= hoursPerDay)
                {
                    timeDays++;
                    timeHours = 0f;
                    if(timeDays >= daysPerMonth)
                    {
                        timeMonths++;
                        timeDays = 1f;
                        if(timeMonths >= monthsPerYear)
                        {
                            timeYears++;
                            timeMonths = 1f;
                        }
                    }
                }
            }
        }
    }

    void Debugging(string message, float myData) 
    {
        Debug.Log(message + myData);
    }

    void ShowFullTime()
    {
        Debug.Log($"Current time: {timeHours}:{timeMinutes}:{timeSeconds} - Day {timeDays} - Month {ShowCurrentMonth()} - Season {ShowCurrentSeason()} - Year {timeYears}");
    }

    string ShowCurrentMonth()
    {
       switch(timeMonths)
       {
            case 1:
                currentMonth = "January";
                break;
            case 2:
                currentMonth = "February";
                break;
            case 3:
                currentMonth = "March";
                break;
            case 4:
                currentMonth = "April";
                break;
            case 5:
                currentMonth = "May";
                break;
            case 6:
                currentMonth = "June";
                break;
            case 7:
                currentMonth = "July";
                break;
            case 8:
                currentMonth = "August";
                break;
            case 9:
                currentMonth = "September";
                break;
            case 10:
                currentMonth = "October";
                break;
            case 11:
                currentMonth = "November";
                break;
            case 12:
                currentMonth = "December";
                break;
       }

       return currentMonth;
    }

    string ShowCurrentSeason()
    {
        if(timeMonths == 12 || timeMonths == 1 || timeMonths == 2) currentSeason = "Winter";
        if(timeMonths == 3 || timeMonths == 4 || timeMonths == 5) currentSeason = "Spring";
        if(timeMonths == 6 || timeMonths == 7 || timeMonths == 8) currentSeason = "Summer";
        if(timeMonths == 9 || timeMonths == 10 || timeMonths == 11) currentSeason = "Autumn";

        return currentSeason;
    }
}
