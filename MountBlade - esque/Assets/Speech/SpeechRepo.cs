using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public struct SpeechKey{
	public int SpeakerID;
	public int SpeechID;

	public SpeechKey(int speak, int speech){
		SpeakerID = speak;
		SpeechID = speech;
	}
}

public static class SpeechRepo {

	public static XmlDocument DialogXML;

	struct SpeechUnit{
		public string dialog;
		public int speakerID;

		SpeechUnit(int id, string d){
			speakerID = id;
			dialog = d;
		}
	}

	private static bool initialized = false;

	public static bool debugInitialized {
		get {
			return initialized;
		}
	}

	private static Dictionary<Vector2, string> SpeechDictionary;

	public static string RetrieveSpeech(SpeechKey key)
	{
		string returnString;
		Vector2 vec = new Vector2 (key.SpeakerID, key.SpeechID);
		SpeechDictionary.TryGetValue(vec, out returnString);
		return returnString;
	}


	public static void InitializeDictionary(){
		if (!initialized) {
			initialized = true;
			initializeDictionary ();	
		}
	}

	private static void initializeDictionary(){

		SpeechDictionary = new Dictionary<Vector2,string> ();

		TextAsset textAsset = (TextAsset)Resources.Load ("DialogueContainer");
		DialogXML = new XmlDocument ();
		DialogXML.LoadXml (textAsset.text);

		//process xml

		XmlNodeList SpeechesList = DialogXML.GetElementsByTagName ("Speech");
		foreach (XmlNode speech in SpeechesList) {
			XmlNodeList SpeechContents = speech.ChildNodes;
				float a = float.Parse(speech.SelectSingleNode("SpeakerID").InnerText);
				float b = float.Parse (speech.SelectSingleNode ("SpeechID").InnerText);
			string dialogLine = speech.SelectSingleNode ("Dialog").InnerText;
			Vector2 ids = new Vector2 (a, b);
			SpeechDictionary.Add(ids, dialogLine) ;
		}

	}

}
