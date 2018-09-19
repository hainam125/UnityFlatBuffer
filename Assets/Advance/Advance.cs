using System.Collections.Generic;
using UnityEngine;
using FlatBuffers;
using System.IO;
using MyGame.Demo;

public class Advance : MonoBehaviour {
	public bool isMaster;
	public string fileName;
	public int amount;
	private string pathMasterData;
	private string pathUserData;

	private void Start() {
		pathMasterData = Application.dataPath + "/Advance/Data/M" + fileName + "Saved.dat";
		pathUserData = Application.dataPath + "/Advance/Data/U" + fileName + "Saved.dat";
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.S)) {
			if (isMaster) SaveMaster();
			else SaveUser();
		}
		else if (Input.GetKeyDown(KeyCode.L)) {
			if (isMaster) LoadMaster();
			else LoadUser();
		}
	}

	private void SaveUser() {
		var tests = new List<UNightWalker>();
		for (int i = 0; i < amount; i++) {
			tests.Add(UNightWalker.GetSample());
		}

		DataHelper.SaveUserData(tests, pathUserData);
	}

	private void LoadUser() {
		var list = DataHelper.LoadUserData<UNightWalker>(pathUserData);
		foreach (var i in list) {
			Debug.Log(i.Id);
		}
	}

	private void SaveMaster() {
		var tests = new List<MGrowthBoard>();
		for (int i = 0; i < amount; i++) {
			tests.Add(MGrowthBoard.GetSample());
		}
		DataHelper.SaveMasterData(tests, pathMasterData);
	}

	private void LoadMaster() {
		var tests = DataHelper.LoadMasterData<MGrowthBoard>(pathMasterData);

		Debug.Log(tests.Count + " : " + tests[Random.Range(0, tests.Count)].IdsOfSkillPoint.Count);
		Debug.Log("Load Many!");
	}
}

public class UTest : UBaseTest {

}

public class MTest : MBaseTest {

}

