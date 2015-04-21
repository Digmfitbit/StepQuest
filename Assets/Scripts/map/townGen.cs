using UnityEngine;
using System.Collections;

public class townGen : MonoBehaviour {

	private string[] townNames = new string[]{"Morcote", "Grungedich", "Heltower", "Marshen", "Ringmer", "Wormsdump", "Carsby", "Someringdene", "Wrongdump", "Alderster", "Grimehovel", "Hurttump",	"Meatylis", "Fulenhill", "Illsneke", "Pontfan", "Sutscastle", "Stenchdump", "Bexsey", "Bournetock",	"Macminster", "Balagate", "Poldock", "Smellbane", "Puckhenge", "Caermore", "Bredean", "Stinkford", "Wormsylis", "Meatester", "Bledtease", "Wealdtown", "Evilerscrutch", "Biteter", "Belysport", "Leverton", "Careter", "Porthster", "Fulthwaite", "Witchcrack", "Billeter", "Baleter", "Carsham", "Bexgate", "Mousebugga", "Duncanvy", "Holmham", "Aldfos", "Knightfield", "Lhanwardine", "Moorend", "Fleshech", "Wealdbury", "Vilewood", "Vileclapp", "Dinaswich", "Culcester", "Shipsay", "Rockdump", "Carnoflea", "Bonefold", "Nantlock", "Shibug", "Biteclapp", "Winterbridge", "Treburgh", "Frogfast", "Enderdale", "Palmside", "Balbryde", "Nastichute", "Nastidich", "Puckoch", "Elslot", "Grimcrack", "Uglysneke", "Belyshaw", "Deverton", "Smellbane", "Fatllyn", "Evilfold", "Frogoflea", "Mousemede", "Somercester", "Impsmarshe", "Dripslip", "Cunkne", "Lewdon", "Mynyddwardine", "Tillypool", "Bourneton", "Brigwick", "Dalworth", "Rotwind", "Coldcatt", "Nantlock", "Grotslide", "Witchach", "Exeberry", "Wrongshagg"};
	public string townName;

	void Start () {
		townName = townNames[Random.Range (0,townNames.Length)];
	}

}
