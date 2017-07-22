using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampaignMap_AIUnit_Planet : PlanetCampaignMapUnit {

    public bool overrideAIstate;
    CampainMap_POI enemyToAttack;


    int patrolTicks;


    public AiState_Unit_Campaign aiState;

    public enum AiState_Unit_Campaign
    {
        Patrol,
        Guard,
        Attack,
        RaiseArmy,
        Retreat
    }

	// Use this for initialization
	void Start ()
    {
        cmPOI = GetComponent<CampainMap_POI>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
    void Update()
    {
        cmPOI.UnitListTextAsComponent.text += " " + aiState.ToString();
        gameObject.name = " " + aiState.ToString();

        if (!overrideAIstate)
        {
            RunAIState();
        }
        else
        {       
            AttackTarget();
        }
    }

    void RunAIState()
    {
        switch(aiState)
        {
            case AiState_Unit_Campaign.Attack:
                Attack();
                break;
            case AiState_Unit_Campaign.Guard:
                Guard();
                break;
            case AiState_Unit_Campaign.Patrol:
                Patrol();
                break;
            case AiState_Unit_Campaign.Retreat:
                Retreat();
                break;
            case AiState_Unit_Campaign.RaiseArmy:              
                RaiseArmy();
                break;
        }
    }

    void AttackTarget()
    {
        targetDestination = enemyToAttack.transform.position;
        agent.SetDestination(targetDestination);

        float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

        if (distanceToTarget < 5)
        {
            if (distanceToTarget < 1.8f)
            {
                //agent.Stop();
                //agent.velocity = Vector3.zero;
            }

            //Do battle logic, for now let's just substract from everybody
            //but first, let's check our army and the enemies
            if(cmPOI.UnitListNumber <= 0)
            {                       
                overrideAIstate = false;
                hasDestination = false;
                aiState = AiState_Unit_Campaign.Retreat;

                enemyToAttack = null;
                return;
            }

            if (enemyToAttack.UnitListNumber <= 0)
            {                           
                overrideAIstate = false;
                hasDestination = false;
                enemyToAttack.GetComponent<CampaignMap_AIUnit_Planet>().aiState = AiState_Unit_Campaign.Retreat;
                aiState = AiState_Unit_Campaign.RaiseArmy;

                enemyToAttack = null;
                return;
            }
            
            if (cmPOI.cmManager.tick && aiState != AiState_Unit_Campaign.Retreat)
            {
                cmPOI.UnitListNumber--;
                
                if(enemyToAttack)
                    enemyToAttack.UnitListNumber--;
            }
        }
        
    }

    void Attack()
    {
        if (cmPOI.UnitListNumber <= 0)
        {
            aiState = AiState_Unit_Campaign.RaiseArmy;
            return;
        }

        if (!hasDestination)
        {
            int ourFaction = cmPOI.FactionNumber;
            FactionsBase fb = cmPOI.cmManager.FactionList[ourFaction];
            FactionsBase fbToAttack = cmPOI.cmManager.FactionList[fb.EnemiesWith[0]];

            int randomValue = Random.Range(0, fbToAttack.FactionCastles.Count);

            targetDestination = fbToAttack.FactionCastles[randomValue].transform.position;

            hasDestination = true;

        }
        else
        {
            agent.SetDestination(targetDestination);

            float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

            if (distanceToTarget < 1)
            {
               //We've reached a castle, add logic here
            } 
        }
    }

    void Guard()
    {
        if (!hasDestination)
        {
            int randomValue = Random.Range(0, cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionCastles.Count);

            targetDestination = cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionCastles[randomValue].transform.position;

            hasDestination = true;
        }
        else
        {
            agent.SetDestination(targetDestination);

            float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

            if (distanceToTarget < 1)
            {
                
            } 
        }
    }

    void Patrol()
    {
        if (!hasDestination)
        {
            List<CampainMap_POI> destinationsPOI = new List<CampainMap_POI>();

            destinationsPOI.AddRange(cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionVillages);
            destinationsPOI.AddRange(cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionCastles);

            int randomValue = Random.Range(0, destinationsPOI.Count);

            targetDestination = destinationsPOI[randomValue].transform.position;

            hasDestination = true;
        }
        else
        {
            agent.SetDestination(targetDestination);

            float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

            if (distanceToTarget < 1)
            {
                patrolTicks++;
            }

            if(patrolTicks > 20)
            {
                hasDestination = false;
                patrolTicks = 0;
            }
        }
    }

    void RaiseArmy()
    {
        if(!hasDestination)
        {
            int randomValue = Random.Range(0, cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionVillages.Count);

            targetDestination = cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionVillages[randomValue].transform.position;

            hasDestination = true;
        }
        else
        {
            agent.SetDestination(targetDestination);
            
            float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

            if(distanceToTarget < 1)
            {      
                if(cmPOI.cmManager.tick)
                    cmPOI.UnitListNumber++;
            }
        }

    }

    void Retreat()
    {
        if (!hasDestination)
        {
            int randomValue = Random.Range(0, cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionVillages.Count);

            targetDestination = cmPOI.cmManager.FactionList[cmPOI.FactionNumber].FactionVillages[randomValue].transform.position;

            hasDestination = true;
        }
        else
        {
            agent.SetDestination(targetDestination);

            float distanceToTarget = Vector3.Distance(transform.position, targetDestination);

            if (distanceToTarget < 1)
            {
                aiState = AiState_Unit_Campaign.RaiseArmy;
            }
        }
    }

    public void GiveTargetToAttack(CampainMap_POI _cmPOI)
    {
        //You can add more logic here, if you want the unit to decide if he will attack or retreat for example
        if (enemyToAttack == null)
        {     
            enemyToAttack = _cmPOI;
            overrideAIstate = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CampainMap_POI>())
        {
            CampainMap_POI _cmPOI = other.GetComponent<CampainMap_POI>();

            if(cmPOI == null)
            {
                cmPOI = GetComponent<CampainMap_POI>();
            }

            if(_cmPOI.FactionNumber != cmPOI.FactionNumber) //We have a POI of a different faction
            {
               //And our faction is at the war state
                //if (cmPOI.cmManager.FactionList[cmPOI.FactionNumber].factionAIstate == FactionsBase.FactionAIstate.GoToWar)
                //{
                    //And we are at war with that Faction
                    if (cmPOI.cmManager.FactionList[cmPOI.FactionNumber].EnemiesWith.Contains(_cmPOI.FactionNumber))
                    {
                        //..and that POI is actually a Unit
                        if (_cmPOI.Point_of_Interest_Type == CampainMap_POI.POItype.Unit)
                        {
                            //and he is not retreatig, which means we might have beat him already
						if (_cmPOI.transform.GetComponent<CampaignMap_AIUnit_Planet>().aiState != AiState_Unit_Campaign.Retreat)
                            {
                                //Override AI state and attack the target
                                GiveTargetToAttack(_cmPOI);
                                //and tell the enemy that we are attacking
							_cmPOI.transform.GetComponent<CampaignMap_AIUnit_Planet>().GiveTargetToAttack(cmPOI);
                            }
                            else
                            {
                                overrideAIstate = false;
                            }
                        }
                   }
                //}
            }
        }
    }

}
