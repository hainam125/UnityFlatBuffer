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
		var tests = new UNightWalker[amount];
		for (int i = 0; i < tests.Length; i++) {
            tests[i] = UNightWalker.GetSample();
		}

		var builder = new FlatBufferBuilder(1024);

		var modelName = builder.CreateString(tests[0].GetType().ToString());

		var bhOffsets = new Offset<ByteHolder>[tests.Length];
		for (var index = 0; index < tests.Length; index++) {
			var testBytes = tests[index].ToBytes();
			ByteHolder.StartDataVector(builder, testBytes.Length);
			for (int i = testBytes.Length - 1; i >= 0; i--) builder.AddByte(testBytes[i]);
			var vector = builder.EndVector();

			var bh = ByteHolder.CreateByteHolder(builder, vector);
			bhOffsets[index] = bh;
		}

		var sdDataVector = SyncData.CreateDataVector(builder, bhOffsets);
		var syncData = SyncData.CreateSyncData(builder, modelName, sdDataVector);
		var suData = SyncUserDataResp.CreateDataVector(builder, new Offset<SyncData>[] { syncData });
		SyncUserDataResp.StartSyncUserDataResp(builder);
		SyncUserDataResp.AddData(builder, suData);
		var r = SyncUserDataResp.EndSyncUserDataResp(builder);
		builder.Finish(r.Value);

		using (var ms = new MemoryStream(builder.SizedByteArray())) {
			File.WriteAllBytes(pathUserData, ms.ToArray());
			Debug.Log("SAVED USER!");
            var list = tests[0].GetProperties();
            var x = string.Empty;
            foreach (var i in list) x += i.Name + ";";
            Debug.Log(x);
		}
	}

	private void LoadUser() {
		byte[] fBytes;
		using (var file = new FileStream(pathUserData, FileMode.Open, FileAccess.Read)) {
			fBytes = new byte[file.Length];
			file.Read(fBytes, 0, fBytes.Length);
		}
		var buffer = new ByteBuffer(fBytes);

		var response = SyncUserDataResp.GetRootAsSyncUserDataResp(buffer);

		for (var i = 0; i < response.DataLength; i++) {
			var syncData = response.Data(i);
			if (!syncData.HasValue) continue;

			var model = syncData.Value.Model;

			for (var j = 0; j < syncData.Value.DataLength; j++) {
				var bytesHolder = syncData.Value.Data(j);
				if (!bytesHolder.HasValue) continue;

				var arraySegment = bytesHolder.Value.GetDataBytes();
				var bytes = Helper.GetBytes(arraySegment);
				var obj = new UNightWalker();
				obj.Update(bytes);
				Debug.Log(obj.Id + " - ");
            }
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
        var tests = DataHelper.LoadMaster<MGrowthBoard>(pathMasterData);

        Debug.Log(tests.Count + " : " + tests[Random.Range(0, tests.Count)].IdsOfSkillPoint.Count);
		Debug.Log("Load Many!");
	}
}

public class UTest : UBaseTest {
    
}

public class MTest : MBaseTest {
    
}

