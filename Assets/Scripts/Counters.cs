using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counters : MonoBehaviour
{
    [SerializeField] GameObject myCube;
    public int greenPopulation, bluePopulation, greenMale, greenFemale, blueMale, blueFemale, totalPopulation, greenMaleDead, greenFemaleDead, blueMaleDead, blueFemaleDead, totalDeaths, totalBirths, blueDeaths, greenDeaths;
    public List<List<int>> statsList = new List<List<int>>();
    public List<int> minuteStatsList = new List<int>();
    public float statsTimerLimit = 60f;
    public float statsTimerIncrement = 0f;
    public float statsTimerIncrease = 1f;
    public int statsChangeListIndex = 0;
    public float newCubePositionIncrement = 20f;
    // Start is called before the first frame update
    //     List<List<int>> numbers = new List<List<int>>(); // List of List<int>
    //     numbers.Add(new List<int>());  // Adding a new List<int> to the List.
    //     numbers[0].Add(2);  // Add the integer '2' to the List<int> at index '0' of numbers.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: get stats every minute and add to a list of vectors so minute 1 will be List<1> and data will be [] 1 2 3 ?
        // NOTE: Currently working
        // FIX: make the cube prefab thinner and add a longer time increment
        // FIX: initialize the graph with the initial values before hand
        // BUG: graph goes out of bounds
        // BUG: graph not showing correct value as totalpopulation is not deducting the deaths
        // TODO: create more graphs for different stats and only show one at a time by key press?
        // TODO: add text on top of the cube to show values and what graph it is text to face camera??
        Transform cubeTransform = myCube.GetComponent<Transform>();
        statsTimerIncrement += Time.deltaTime * statsTimerIncrease;
        if(statsTimerIncrement >= statsTimerLimit)
        {
            Debug.Log("intrat");
            minuteStatsList.Insert(0, totalPopulation);
            minuteStatsList.Insert(1, totalBirths);
            minuteStatsList.Insert(2, totalDeaths);
            statsList.Insert(statsChangeListIndex, minuteStatsList);
            //minuteStatsList.Clear();
            Debug.Log(statsList[0][0]);
            if(statsChangeListIndex == 0)
            {
                cubeTransform.localScale = new Vector3 (cubeTransform.localScale.x, statsList[statsChangeListIndex][0], cubeTransform.localScale.z);
            }
            else
            {
                var newCube = Instantiate(cubeTransform, new Vector3(cubeTransform.position.x + newCubePositionIncrement, cubeTransform.position.y, cubeTransform.position.z), Quaternion.identity);
                newCube.transform.localScale = new Vector3 (cubeTransform.localScale.x, statsList[statsChangeListIndex][0], cubeTransform.localScale.z);
                newCubePositionIncrement += 20f;
            }
            
            statsChangeListIndex++;
            Debug.Log(statsChangeListIndex);
            statsTimerIncrement = 0;
            // do the change in game
            // totalpopulation
            
        }
        // if(statsList.Count > 0)
        // {
        // for (int i = 0; i < statsList.Count; i++)
        //     {
        //         if(i == 0)
        //         {
        //             Debug.Log(i);
        //             //
        //         }
        //         else
        //         {

        //         }
        //     }
        // }
        
    }
}
