using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public static class CharacterXMLReader  {

	//XML loading class.  Works fine, but we dont have an actual xml file setup yet so it has no purpose yet. The values sitting there now are from our other project, we'll update everything when we decide how to organize the XML.

	public static XmlDocument CharacterXML;

	private static void LoadCharacterXML(){


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
