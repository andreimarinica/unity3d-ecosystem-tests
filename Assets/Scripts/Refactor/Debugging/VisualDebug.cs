using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VisualDebug : MonoBehaviour
{
    Stats stats;
    string sex;
    bool toggleVisualRangeDebugger = false;
    bool toggleStatsDebugger = true;
    bool toggleStateDebugger = true;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stats.sex == 0) 
        {
            sex = "Male";
        } 
        else if(stats.sex == 1)
        {
            sex = "Female";
        }
        // NOTE: 1 - toggle visual range (sphere collider area)
        // NOTE: 2 - toggle visual stats
        // NOTE: 3 - toggle visual state
        // NOTE: 4 - toggle players in debugging mode (never hungry, slower)
        // NOTE: - - decrease speed
        // NOTE: + - increase speed
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(!toggleVisualRangeDebugger) toggleVisualRangeDebugger = true;
            else toggleVisualRangeDebugger = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(!toggleStatsDebugger) toggleStatsDebugger = true;
            else toggleStatsDebugger = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(!toggleStateDebugger) toggleStateDebugger = true;
            else toggleStateDebugger = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            stats.healthDecreaseRatio = 0f;
            stats.hungerDecreaseRatio = 0f;
            stats.hunger = 0f;
            stats.health = 100f;
            stats.GetComponent<Movement>().speed = 4f;
        }
        if(Input.GetKeyDown(KeyCode.Equals))
        {
            stats.GetComponent<Movement>().speed++;
            Debug.Log("speed increased");
        }
        if(Input.GetKeyDown(KeyCode.Minus))
        {
            stats.GetComponent<Movement>().speed--;
            Debug.Log("speed decreased");
        }
    }
    // draw gizmos to show visual range
    private void OnDrawGizmos() {
        if(toggleVisualRangeDebugger)
        {
        if(stats.Species == Species.Fox)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.2f); 
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 4);
        }

        if(stats.Species == Species.Rabbit)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 2);
        }

        if(stats.Species == Species.Food)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); 
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
        }

        if(stats.Species == Species.Fox || stats.Species == Species.Rabbit)
        {
            if(toggleStatsDebugger)
            {
        GUIStyle paramsGUI = new GUIStyle();
        paramsGUI.fontSize = 15;
        paramsGUI.fontStyle = FontStyle.Normal;
        paramsGUI.normal.textColor = Color.black;

        GUIStyle maleGUI = new GUIStyle();
        maleGUI.fontSize = 15;
        maleGUI.fontStyle = FontStyle.Normal;
        maleGUI.normal.textColor = Color.blue;

        GUIStyle femaleGUI = new GUIStyle();
        femaleGUI.fontSize = 15;
        femaleGUI.fontStyle = FontStyle.Normal;
        femaleGUI.normal.textColor = Color.magenta;

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), "health " + (int)stats.health, paramsGUI);

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), "hunger " + (int)stats.hunger, paramsGUI);

        Handles.Label(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), "reprod " + (int)stats.reproductiveUrge, paramsGUI);

        if(sex == "Male") Handles.Label(new Vector3(transform.position.x, transform.position.y + 2.7f, transform.position.z), "Sex: " + sex, maleGUI);

        if(sex == "Female") Handles.Label(new Vector3(transform.position.x, transform.position.y + 2.7f, transform.position.z), "Sex: " + sex, femaleGUI);
            }

        

        

        if(toggleStateDebugger)
        {
        GUIStyle stateGUI = new GUIStyle();
        stateGUI.fontSize = 17;
        stateGUI.fontStyle = FontStyle.Bold;
        stateGUI.normal.textColor = Color.red;
        
        Handles.Label(new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z), GetComponent<PlayerStateManager>().currentState.ToString(), stateGUI);  
        }
        }                        
        
    }
}
