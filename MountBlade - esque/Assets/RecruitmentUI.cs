using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentUI : MonoBehaviour {

	References refer;
	UIManager manager;

	public Dropdown UnitTypeDropdown;
	public Text UnitDescription;
	public Image UnitCard;

	List<UnitType> AvailableUnitTypes;

	UnitType selectedUnitType;

	// Use this for initialization
	void Start () {
		refer = GameObject.FindGameObjectWithTag ("CampaignMapManager").GetComponent<References> ();
		manager = refer.gameObject.GetComponent<UIManager> ();
		UnitTypeDropdown.ClearOptions ();
		UnitTypeDropdown.AddOptions (FactionsEnum.FactionUnitLists[(int)refer.PlayerCharacter.faction].CombinedNames);
		AvailableUnitTypes = FactionsEnum.FactionUnitLists [(int)refer.PlayerCharacter.faction].Combined;
	}
	
	// Update is called once per frame
	void Update () {
		selectedUnitType = AvailableUnitTypes[UnitTypeDropdown.value];
		UnitDescription.text = selectedUnitType.UnitDescription;
		UnitCard.sprite = selectedUnitType.UnitCard;
	}

	public void RecruitUnit(){
		Debug.Log ("Recruiting " + selectedUnitType.UnitTypeName);
		refer.PlayerCharacter.armyList.AddUnits (selectedUnitType,1);
	}
}
