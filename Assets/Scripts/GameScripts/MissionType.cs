using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionType : MonoBehaviour
{

    private string line1;
    private string line2;
    private string line3;
    private string line4;
    private string line5;
    private string line6;

    private float character_delay = 0.02f;
    private float space_delay = 0.1f;
    private float line_delay = 1f;

    public GameObject textObj;

    // Use this for initialization
    void Start()
    {
        setup_text();
        StartCoroutine(type_text());
    }

    // Update is called once per frame
    void Update()
    {

    }
    string get_mission_text(int i)
    {
        //int i = 0;
        //for (i = 0; i < 3; i++)
        //{
        if (Managers.missions.current_mission_indices[i] == 1) { return ("Reach a total of " + Managers.missions.current_mission_targets[i] + " kills"); }
        else if (Managers.missions.current_mission_indices[i] == 2) { return ("Reach a total of " + Managers.missions.current_mission_targets[i] + " rounds passed"); }
        else if (Managers.missions.current_mission_indices[i] == 3) { return ("Reach a maximum round of " + Managers.missions.current_mission_targets[i]); }
        else if (Managers.missions.current_mission_indices[i] == 4) { return ("Reach a maximum score of " + Managers.missions.current_mission_targets[i]); }
        else if (Managers.missions.current_mission_indices[i] == 5) { return ("Reach a maximum kill count of " + Managers.missions.current_mission_targets[i]); }
        else if (Managers.missions.current_mission_indices[i] == 6) { return ("Reach a maximum cash count of " + Managers.missions.current_mission_targets[i]); }
        //}
        return ("");
    }
    void setup_text(){
        line1 = "Location: level one \n";
        line2 = "Mission 1: " + get_mission_text(0);
        line3 = "Mission 2: " + get_mission_text(1);
        line4 = "Mission 3: " + get_mission_text(2);
    }
    IEnumerator type_text()
    {
        textObj.active = true;
        Text t = textObj.GetComponent<Text>();
        t.text = "";
        string[] lines = new string[]{line1,line2,line3,line4};
        yield return new WaitForSeconds(1f);
        foreach(string line in lines){
            foreach(char c in line){
                t.text += c;
                if (""+c != " ")
                {
                    yield return new WaitForSeconds(character_delay);
                }else{
                    yield return new WaitForSeconds(space_delay);
                }
            }
            t.text += "\n";
            yield return new WaitForSeconds(line_delay);
        }
        yield return new WaitForSeconds(3f);
        Destroy(textObj);
    }
}
