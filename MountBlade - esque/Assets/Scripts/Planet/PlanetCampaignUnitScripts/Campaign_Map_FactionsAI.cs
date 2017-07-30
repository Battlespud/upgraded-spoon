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
		for(int i = 0; i < FactionController.FactionList.Count; i++)
        {
			for (int c = 0; c < FactionController.FactionList[i].startingCharacters; c++)
            {
				Vector3 SpawnPosition = FactionController.FactionList[i].FactionCastles[0].transform.position;

				GameObject tempGO = Instantiate(FactionController.FactionList[i].characterPrefab, SpawnPosition, Quaternion.identity) as GameObject;

                CampaignMap_AIUnit_Planet aiUnit = tempGO.transform.GetComponent<CampaignMap_AIUnit_Planet>();
				FactionController.FactionList[i].FactionCharacters.Add(aiUnit);

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
			for (int i = 0; i < FactionController.FactionList.Count; i++)
            {
				ExecuteAIorders(FactionController.FactionList[i]);
            }

            cmManager.tick = true;
            gameTime = 0;
        }

	}

    //For each Faction, execute their state
	void ExecuteAIorders(FactionController faction)
    {
        //We can't do changes directly to the faction base,
        //We have to create a new on first


        switch(faction.factionAIstate)
        {
		case FactionController.FactionAIstate.GoToWar:
//                Debug.Log("WAAAARRGGGHHHH"); //War will be dealth otherwise, we don't have to do anything here
                break;
            case FactionController.FactionAIstate.RaiseArmies:
                //If we have any wealth
                if (faction.factionWealth > 0)
                {
                    //Then convert wealth to army units
                    faction.currentArmyUnits++;
                    faction.factionWealth--;
                }
                else//If we don't, there's no need to wait till evaluation, change the AI state 
                    faction.factionAIstate = FactionController.FactionAIstate.RaiseResources;
                break;
            case FactionController.FactionAIstate.RaiseResources:
                faction.factionWealth++;//If we are raising resources, simply add to the wealth
                break;
        }

        //After you do that, then we have to replace it
        ReplaceFactionBase(faction, faction);
    }

    void ReplaceFactionBase(FactionController faction, FactionController replacement)
    {
        //Basically, you can't just replace a value,
        //You have to create a new one, therefore

        //We check to see if there's an instance of this in the list
		if(FactionController.FactionList.Contains(faction))
        {
            //If there is, we store the index (where is it in the list)
			int index = FactionController.FactionList.IndexOf(faction);
            //We remove the previous instance
			FactionController.FactionList.Remove(faction);
            //And instead of adding the new one, we insert it in the position of the previous one
			FactionController.FactionList.Insert(index, replacement);
        }
    }

    void EvaluateFactionAIstate()
    {
        //Start deciding what Faction should do next
		for(int i = 0; i < FactionController.FactionList.Count; i++)
        {
			FactionAIstatesLogic(FactionController.FactionList[i]);

            //And change the AI state of each character
			AssignStates(FactionController.FactionList[i]);
        }

        startEvaluating = false;
    }

    void FactionAIstatesLogic(FactionController faction)
    {
        //Same as above

        FactionController.FactionAIstate aiState = faction.factionAIstate;

        switch(aiState)
        {
            case FactionController.FactionAIstate.GoToWar:
                //For now let's just reset the loop
               // faction.factionAIstate = FactionController.FactionAIstate.RaiseResources;
                //And let's just assume we had casualties on that war
              //  int ranValue = Random.Range(1, faction.currentArmyUnits);
               // faction.currentArmyUnits -= ranValue;

                break;
            case FactionController.FactionAIstate.RaiseArmies:
                //If we haven't reached our target Units
                if (faction.currentArmyUnits < faction.storePreviousArmyUnits + faction.targetArmyUnits)
                {
                    //Then see if you can raise more
                    RaiseArmiesLogic(faction);
                }
                else
                {
                    //If we have, then we can probably go to war
                    ShouldWeGoToWar(faction);
                }
                break;
            case FactionController.FactionAIstate.RaiseResources: //If our previous state was to raise resources
                if (faction.factionWealth < faction.factionWealthTarget) //And the ones we raised wasn't enough
                {
                    //Countinue raising resources
                    faction.factionAIstate = FactionController.FactionAIstate.RaiseResources;
                }
                else
                {   //If we have enough resources, then decide if you want to go to war
                    ShouldWeGoToWar(faction);
                }
                break;
        }

        ReplaceFactionBase(faction, faction);
    }

    void ShouldWeGoToWar(FactionController faction)
    {

        //If the faction's trait is agressive, they have more chances on going to war
		float baseWarValue = (faction.factionTraits == FactionController.FactionTraits.Aggressive) ? 30 : 60;

        baseWarValue -= faction.currentArmyUnits; //Let's just say, if they have a big army, they have more chances on going to war

        int ranValue = Random.Range(0, 100); //Get a random value

        Debug.Log(ranValue);

        if(ranValue > baseWarValue)
        {
            //Then either go to war
            faction.factionAIstate = FactionController.FactionAIstate.GoToWar;
        }
        else
        {
            //Or see if you can raise more armies
            faction.storePreviousArmyUnits = faction.currentArmyUnits; //Store the previous Units we had
            faction.targetArmyUnits = 10; //Let's say they always need 10 more
            RaiseArmiesLogic(faction);
        }

        ReplaceFactionBase(faction, faction);
    }

    void RaiseArmiesLogic(FactionController faction)
    {

        //If we have enough resources, then raise armies, 1 wealth equals 1 unit 
        if (faction.factionWealth >= faction.targetArmyUnits)
        {
            faction.factionAIstate = FactionController.FactionAIstate.RaiseArmies;
        }
        else
        {//If we dont, then raise some resources first
            faction.factionAIstate = FactionController.FactionAIstate.RaiseResources;
        }

        ReplaceFactionBase(faction, faction);
    }

    void AssignStates(FactionController faction)
    {
        foreach (CampaignMap_AIUnit_Planet AIchar in faction.FactionCharacters)
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
            attackValue += (faction.factionAIstate == FactionController.FactionAIstate.GoToWar) ? 50 : -30;

            //However, add the Faction traits to it's decision
			guardValue += (faction.factionTraits == FactionController.FactionTraits.Aggressive) ? -30 : 10;

            //Add more modifiers here until you get what you want, this can be expanded to a ridiculous extend

            //After the modifiers stage, start comparing, 
            //change the hierarchy of this logic to what seems to you, well more logical

            if(attackValue > raiseValue)
            {
                if (guardValue < attackValue && patrolValue < attackValue)
                {
                    //Attack
                    AIchar.aiState = CampaignMap_AIUnit_Planet.AiState_Unit_Campaign.Attack;
                }
                else
                {
                    if(guardValue < patrolValue)
                    {
                        //Patrol
                        AIchar.aiState = CampaignMap_AIUnit_Planet.AiState_Unit_Campaign.Patrol;
                    }
                    else
                    {
                        //Guard
                        AIchar.aiState = CampaignMap_AIUnit_Planet.AiState_Unit_Campaign.Guard;
                    }
                }
            }
            else
            {
                AIchar.aiState = CampaignMap_AIUnit_Planet.AiState_Unit_Campaign.RaiseArmy;
            }

        }
    }

}
