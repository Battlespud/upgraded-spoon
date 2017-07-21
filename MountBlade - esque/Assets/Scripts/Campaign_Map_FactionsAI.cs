using UnityEngine;
using System.Collections;

public class Campaign_Map_FactionsAI : MonoBehaviour {

    float gameTime; //The "real" time of the game, still based on deltatime
    public float DecisionTimerCycle = 30;//After how much time should we evaluate the state
    public float decisionTimer;

    bool startEvaluating;
    Campaign_Map_Manager cmManager;
 
	void Start () 
    {
        cmManager = GetComponent<Campaign_Map_Manager>();
        //decisionTimer = 5;

        //Create AI characters
        for(int i = 0; i < cmManager.FactionList.Count; i++)
        {
            for (int c = 0; c < cmManager.FactionList[i].startingCharacters; c++)
            {
                Vector3 SpawnPosition = cmManager.FactionList[i].FactionCastles[0].transform.position;

                GameObject tempGO = Instantiate(cmManager.FactionList[i].characterPrefab, SpawnPosition, Quaternion.identity) as GameObject;

                CampaignMap_AIUnit aiUnit = tempGO.transform.GetComponent<CampaignMap_AIUnit>();
                cmManager.FactionList[i].FactionCharacters.Add(aiUnit);

                tempGO.GetComponent<CampainMap_POI>().FactionNumber = i;

            }
        }
	}
	
	void Update () 
    {
        if (!startEvaluating)
        {
            decisionTimer += Time.deltaTime;

            if (decisionTimer > DecisionTimerCycle)
            {
                startEvaluating = true;
                decisionTimer = 0;
            }          
        }
        else
        {
            EvaluateFactionAIstate();
        }

        //ticker
        if(cmManager.tick)
        {
            cmManager.tick = false;
        }

        //We want to update faction values every "second" that's why we made this timer
        gameTime += Time.deltaTime;

        if (gameTime > 1)
        {
            for (int i = 0; i < cmManager.FactionList.Count; i++)
            {
                ExecuteAIorders(cmManager.FactionList[i]);
            }

            cmManager.tick = true;
            gameTime = 0;
        }

	}

    //For each Faction, execute their state
    void ExecuteAIorders(FactionsBase faction)
    {
        //We can't do changes directly to the faction base,
        //We have to create a new on first
        FactionsBase newFB = new FactionsBase();
        newFB = faction; //Take the values from the previous stored base and do changes to that

        switch(faction.factionAIstate)
        {
            case FactionsBase.FactionAIstate.GoToWar:
                Debug.Log("WAAAARRGGGHHHH"); //War will be dealth otherwise, we don't have to do anything here
                break;
            case FactionsBase.FactionAIstate.RaiseArmies:
                //If we have any wealth
                if (newFB.factionWealth > 0)
                {
                    //Then convert wealth to army units
                    newFB.currentArmyUnits++;
                    newFB.factionWealth--;
                }
                else//If we don't, there's no need to wait till evaluation, change the AI state 
                    newFB.factionAIstate = FactionsBase.FactionAIstate.RaiseResources;
                break;
            case FactionsBase.FactionAIstate.RaiseResources:
                newFB.factionWealth++;//If we are raising resources, simply add to the wealth
                break;
        }

        //After you do that, then we have to replace it
        ReplaceFactionBase(faction, newFB);
    }

    void ReplaceFactionBase(FactionsBase faction, FactionsBase replacement)
    {
        //Basically, you can't just replace a value,
        //You have to create a new one, therefore

        //We check to see if there's an instance of this in the list
        if(cmManager.FactionList.Contains(faction))
        {
            //If there is, we store the index (where is it in the list)
            int index = cmManager.FactionList.IndexOf(faction);
            //We remove the previous instance
            cmManager.FactionList.Remove(faction);
            //And instead of adding the new one, we insert it in the position of the previous one
            cmManager.FactionList.Insert(index, replacement);
        }
    }

    void EvaluateFactionAIstate()
    {
        //Start deciding what Faction should do next
        for(int i = 0; i < cmManager.FactionList.Count; i++)
        {
            FactionAIstatesLogic(cmManager.FactionList[i]);

            //And change the AI state of each character
            AssignStates(cmManager.FactionList[i]);
        }

        startEvaluating = false;
    }

    void FactionAIstatesLogic(FactionsBase faction)
    {
        //Same as above
        FactionsBase newFB = new FactionsBase();
        newFB = faction;

        FactionsBase.FactionAIstate aiState = newFB.factionAIstate;

        switch(aiState)
        {
            case FactionsBase.FactionAIstate.GoToWar:
                //For now let's just reset the loop
               // newFB.factionAIstate = FactionsBase.FactionAIstate.RaiseResources;
                //And let's just assume we had casualties on that war
              //  int ranValue = Random.Range(1, newFB.currentArmyUnits);
               // newFB.currentArmyUnits -= ranValue;

                break;
            case FactionsBase.FactionAIstate.RaiseArmies:
                //If we haven't reached our target Units
                if (newFB.currentArmyUnits < newFB.storePreviousArmyUnits + newFB.targetArmyUnits)
                {
                    //Then see if you can raise more
                    RaiseArmiesLogic(newFB);
                }
                else
                {
                    //If we have, then we can probably go to war
                    ShouldWeGoToWar(newFB);
                }
                break;
            case FactionsBase.FactionAIstate.RaiseResources: //If our previous state was to raise resources
                if (newFB.factionWealth < newFB.factionWealthTarget) //And the ones we raised wasn't enough
                {
                    //Countinue raising resources
                    newFB.factionAIstate = FactionsBase.FactionAIstate.RaiseResources;
                }
                else
                {   //If we have enough resources, then decide if you want to go to war
                    ShouldWeGoToWar(newFB);
                }
                break;
        }

        ReplaceFactionBase(faction, newFB);
    }

    void ShouldWeGoToWar(FactionsBase faction)
    {
        FactionsBase newFB = new FactionsBase();
        newFB = faction;

        //If the faction's trait is agressive, they have more chances on going to war
        float baseWarValue = (newFB.factionTraits == FactionsBase.FactionTraits.Aggresive) ? 30 : 60;

        baseWarValue -= newFB.currentArmyUnits; //Let's just say, if they have a big army, they have more chances on going to war

        int ranValue = Random.Range(0, 100); //Get a random value

        Debug.Log(ranValue);

        if(ranValue > baseWarValue)
        {
            //Then either go to war
            newFB.factionAIstate = FactionsBase.FactionAIstate.GoToWar;
        }
        else
        {
            //Or see if you can raise more armies
            newFB.storePreviousArmyUnits = faction.currentArmyUnits; //Store the previous Units we had
            newFB.targetArmyUnits = 10; //Let's say they always need 10 more
            RaiseArmiesLogic(newFB);
        }

        ReplaceFactionBase(faction, newFB);
    }

    void RaiseArmiesLogic(FactionsBase faction)
    {
        FactionsBase newFB = new FactionsBase();
        newFB = faction;

        //If we have enough resources, then raise armies, 1 wealth equals 1 unit 
        if (newFB.factionWealth >= newFB.targetArmyUnits)
        {
            newFB.factionAIstate = FactionsBase.FactionAIstate.RaiseArmies;
        }
        else
        {//If we dont, then raise some resources first
            newFB.factionAIstate = FactionsBase.FactionAIstate.RaiseResources;
        }

        ReplaceFactionBase(faction, newFB);
    }

    void AssignStates(FactionsBase faction)
    {
        foreach (CampaignMap_AIUnit AIchar in faction.FactionCharacters)
        {
            AIchar.hasDestination = false;

            //Assign a random value for every state the AI unit can have
            float patrolValue = Random.Range(0, 100 + 1);
            float guardValue = Random.Range(0, 100 + 1);
            float attackValue = Random.Range(0, 100 + 1);
            float raiseValue = Random.Range(0, 100 + 1);
            
            //The above values represent the base chances a unit has to choose a state
            //Now on the base chances, apply modifiers to further affect their decision
            //For example:

            //Since if the Unit has a big army, he has more chances to attack, so..
            attackValue += AIchar.cmPOI.UnitListNumber; //Add the unit's the POI has to the attackValue

            //And if the Faction is at war, let's just swift the balance in favour of war
            attackValue += (faction.factionAIstate == FactionsBase.FactionAIstate.GoToWar) ? 50 : -30;

            //However, add the Faction traits to it's decision
            guardValue += (faction.factionTraits == FactionsBase.FactionTraits.Aggresive) ? -30 : 10;

            //Add more modifiers here until you get what you want, this can be expanded to a ridiculous extend

            //After the modifiers stage, start comparing, 
            //change the hierarchy of this logic to what seems to you, well more logical

            if(attackValue > raiseValue)
            {
                if (guardValue < attackValue && patrolValue < attackValue)
                {
                    //Attack
                    AIchar.aiState = CampaignMap_AIUnit.AiState_Unit_Campaign.Attack;
                }
                else
                {
                    if(guardValue < patrolValue)
                    {
                        //Patrol
                        AIchar.aiState = CampaignMap_AIUnit.AiState_Unit_Campaign.Patrol;
                    }
                    else
                    {
                        //Guard
                        AIchar.aiState = CampaignMap_AIUnit.AiState_Unit_Campaign.Guard;
                    }
                }
            }
            else
            {
                AIchar.aiState = CampaignMap_AIUnit.AiState_Unit_Campaign.RaiseArmy;
            }

        }
    }

}
