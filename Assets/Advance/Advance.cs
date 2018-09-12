using System.Collections.Generic;
using UnityEngine;
using FlatBuffers;
using System.IO;
using MyGame.Demo;

public class Advance : MonoBehaviour {
	public bool isMaster;
	private string pathMasterData;
	private string pathUserData;

	private void Start() {
		pathMasterData = Application.dataPath + "/Advance/mSaved.dat";
		pathUserData = Application.dataPath + "/Advance/uSaved.dat";
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
		var tests = new UTest[6];
		for (int i = 0; i < tests.Length; i++) {
            tests[i] = new UTest {
                Id = i + 1,
                IdOfIcon = Random.Range(0, 12),
                Status = (short)Random.Range(0, 3),
                ExpiredTime = Helper.RandomDate().Ticks,
                ClaimedTime = Helper.RandomDate().Ticks,
                Name = "Present No " + Random.Range(0, 100),
                IdsOfItem = new List<long>() { Random.Range(0, 5), Random.Range(6, 4) }
            };
		}

		var builder = new FlatBufferBuilder(1024);

		var modelName = builder.CreateString("UPresent");

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
				var obj = new UTest();
				obj.Update(bytes);
				Debug.Log(obj.Id + " - " + obj.IdsOfItem.Count + " - " + obj.Status);
            }
		}
	}

	private void SaveMaster() {
		var tests = new List<MTest>();
		for (int i = 0; i < 12; i++) {
			tests.Add(new MTest {
				Id = i,
				IconName = "Icon Name: " + Random.Range(0, 1000),
				Title = "My title: " + Random.Range(0, 10000)
			});
		}
        Helper.SaveMasterData(tests, pathMasterData);
	}

	private void LoadMaster() {
		var reader = new BinaryReader(File.Open(pathMasterData, FileMode.Open));
		var tests = new List<MTest>();
		while (reader.BaseStream.Position != reader.BaseStream.Length) {
			var id = reader.ReadInt64();
			var length = reader.ReadInt32();
			var bytes = reader.ReadBytes(length);
			var test = new MTest();
			test.Update(bytes);
			if(id != test.Id) Debug.Log(id + " - " + test.Id);
			tests.Add(test);
		}
		reader.Close();
		Debug.Log(tests.Count + " : " + tests[Random.Range(0, tests.Count)]);
		Debug.Log("Load Many!");
	}
}

public class UTest : UBaseTest {
    public long IdOfIcon { get; set; }
    public List<long> IdsOfItem { set; get; }
    public long ExpiredTime { set; get; }
    public long ClaimedTime { set; get; }
    public short Status { set; get; }
    public string Name { set; get; }
}

public class MTest : MBaseTest {
	public string IconName { get; set; }
	public string Title { get; set; }
    public List<string> List { get; set; }
	public override string ToString() {
        return string.Empty;
	}
}

