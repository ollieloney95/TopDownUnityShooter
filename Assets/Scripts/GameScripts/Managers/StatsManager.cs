using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField]
    public  int[] maxRound = new int[6];
    public  int totalMaxRounds = 0;
    public  int totalKills=0;
    public  int totalRounds = 0;
    public int totalMoney = 0;
    public  int[] maxMoney = new int[6];
    public  int[] maxScore = new int[6];
    public int[] maxKills = new int[6];

    public int maxRoundSum; 
    void refreshMaxSum()
    {
        totalMaxRounds = 0;
        foreach (int round in maxRound)
        {
            totalMaxRounds += round;
        }
        maxRoundSum = totalMaxRounds;
    }
    public void Startup()
    {
        Debug.Log("Stst manager starting...");
        status = ManagerStatus.Started;
        maxMoney = new int[6];
        maxRound = new int[6];
        maxScore = new int[6];
    }


    void Update()
    {
        //refreshMaxSum();
    }
    public void update_stats_from_round(int level, int round, int score, int kills, int money){
        totalRounds += round;
        totalKills += kills;
        totalMoney += money;

        if(round> maxRound[level]){
            maxRound[level] = round;
        }
        if (money > maxMoney[level])
        {
            maxMoney[level] = money;
        }
        if (score > maxScore[level])
        {
            maxScore[level] = score;
        }
        if (kills > maxKills[level])
        {
            maxKills[level] = kills;
        }
        refreshMaxSum();
        to_master_stats();
    }

    void to_master_stats(){
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");

        master_obj.GetComponent<MasterStats>().maxRound = maxRound;
        master_obj.GetComponent<MasterStats>().totalMaxRounds = totalMaxRounds;
        master_obj.GetComponent<MasterStats>().totalKills = totalKills;
        master_obj.GetComponent<MasterStats>().totalRounds = totalRounds;
        master_obj.GetComponent<MasterStats>().totalMoney = totalMoney;
        master_obj.GetComponent<MasterStats>().maxScore = maxScore;
        master_obj.GetComponent<MasterStats>().maxKills = maxKills;
        master_obj.GetComponent<MasterStats>().maxMoney = maxMoney;
    }
    void from_master_stats()
    {
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");

        maxRound = master_obj.GetComponent<MasterStats>().maxRound;
        totalMaxRounds = master_obj.GetComponent<MasterStats>().totalMaxRounds;
        totalKills= master_obj.GetComponent<MasterStats>().totalKills;
        totalRounds=master_obj.GetComponent<MasterStats>().totalRounds;
        totalMoney=master_obj.GetComponent<MasterStats>().totalMoney;
        maxScore=master_obj.GetComponent<MasterStats>().maxScore;
        maxKills=master_obj.GetComponent<MasterStats>().maxKills;
        maxMoney=master_obj.GetComponent<MasterStats>().maxMoney;
    }


}