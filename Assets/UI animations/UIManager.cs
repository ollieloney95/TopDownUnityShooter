using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject[] buttons;
    public GameObject[] panels;
    public string[] game_states;
    //public IEnumerator

    public int game_state;
	// Use this for initialization
	void Start () {
        game_state = 0;
        set_actives();
	}

    void setup_game_states(){
        
    }
	
	// Update is called once per frame
	void Update () {
	}
    public void change_game_state(){
        game_state += 1;
        if (game_state > 1)
        {
            game_state = 0;
        }
        set_actives();
    }

    void set_actives(){
        //deactivate_all();
        if(game_state == 0){
            buttons[0].GetComponent<Animator>().SetBool("active", true);
            buttons[0].GetComponent<Button>().interactable = true;
            buttons[1].GetComponent<Animator>().SetBool("active", false);
            buttons[1].GetComponent<Button>().interactable = false;
        }
        if(game_state == 1)
        {
            buttons[1].GetComponent<Animator>().SetBool("active", true);
            buttons[1].GetComponent<Button>().interactable = true;
            buttons[0].GetComponent<Animator>().SetBool("active", false);
            buttons[0].GetComponent<Button>().interactable = false;
        }
    }

    void deactivate_all(){
        foreach(GameObject button_i in buttons){
            button_i.GetComponent<Animator>().SetBool("active", false);
        }
    }
}
