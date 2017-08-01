using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public static class CharacterXMLReader  {

	public static XmlDocument CharacterXML;






	private static void initializeDictionary(){


		TextAsset textAsset = (TextAsset)Resources.Load ("XML/CharacterXML");
		CharacterXML = new XmlDocument ();
		CharacterXML.LoadXml (textAsset.text);

		//process xml

		XmlNodeList CharacterList = CharacterXML.GetElementsByTagName ("Character");
		foreach (XmlNode character in CharacterList) {
			XmlNodeList SpeechContents = character.ChildNodes;
			float a = float.Parse(character.SelectSingleNode("SpeakerID").InnerText);
			float b = float.Parse (character.SelectSingleNode ("SpeechID").InnerText);
			string dialogLine = character.SelectSingleNode ("Dialog").InnerText;
			Vector2 ids = new Vector2 (a, b);
		}

	}


}
