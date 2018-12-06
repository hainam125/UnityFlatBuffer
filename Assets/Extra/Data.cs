﻿using FlatBuffers;
using MyGame.Demo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class Data {
	public static void SaveUserData<T>(List<T> tests, string pathUserData) where T : UBaseTest {
		var builder = new FlatBufferBuilder(1024);

		var modelName = builder.CreateString(tests[0].GetType().ToString());

		var bhOffsets = new Offset<ByteHolder>[tests.Count];
		for (var index = 0; index < tests.Count; index++) {
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
			UnityEngine.Debug.Log("SAVED USER!");
		}
	}

	public static void SaveMasterData<T>(List<T> tests, string pathMasterData) where T : MBaseTest {
		using (var writer = new BinaryWriter(File.Open(pathMasterData, FileMode.Create))) {
			foreach (var t in tests) {
				writer.Write(t.Id);
				var bytes = t.ToBytes();
				writer.Write(bytes.Length);
				writer.Write(bytes);
			}
			UnityEngine.Debug.Log("Save Many!");
		}
	}

	public static IList LoadUserData(string pathUserData, Type type) {
		var listType = typeof(List<>).MakeGenericType(type);
		var list = (IList)Activator.CreateInstance(listType);

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
				var bytes = arraySegment.GetBytes();
				var obj = (BaseFB)Activator.CreateInstance(type);
				obj.Update(bytes);
				list.Add(obj);
			}
		}
		return list;
	}

	public static IList LoadMasterData(string pathMasterData, Type type) {
		var listType = typeof(List<>).MakeGenericType(type);
		var list = (IList)Activator.CreateInstance(listType);

		var reader = new BinaryReader(File.Open(pathMasterData, FileMode.Open));
		while (reader.BaseStream.Position != reader.BaseStream.Length) {
			var id = reader.ReadInt64();
			var length = reader.ReadInt32();
			var bytes = reader.ReadBytes(length);
			var test = (BaseFB)Activator.CreateInstance(type);
			test.Update(bytes);
			list.Add(test);
		}
		reader.Close();
		return list;
	}

    //Not used
	public static List<T> LoadUserData<T>(string pathUserData) where T : UBaseTest {
		byte[] fBytes;
		using (var file = new FileStream(pathUserData, FileMode.Open, FileAccess.Read)) {
			fBytes = new byte[file.Length];
			file.Read(fBytes, 0, fBytes.Length);
		}
		var buffer = new ByteBuffer(fBytes);

		var response = SyncUserDataResp.GetRootAsSyncUserDataResp(buffer);

		List<T> result = new List<T>();

		for (var i = 0; i < response.DataLength; i++) {
			var syncData = response.Data(i);
			if (!syncData.HasValue) continue;

			var model = syncData.Value.Model;

			for (var j = 0; j < syncData.Value.DataLength; j++) {
				var bytesHolder = syncData.Value.Data(j);
				if (!bytesHolder.HasValue) continue;

				var arraySegment = bytesHolder.Value.GetDataBytes();
                var bytes = arraySegment.GetBytes();
                var obj = (T)System.Activator.CreateInstance(typeof(T));
				obj.Update(bytes);
				result.Add(obj);
			}
		}
		return result;
	}

    //Not used
	public static List<T> LoadMasterData<T>(string pathMasterData) where T : MBaseTest {
		var reader = new BinaryReader(File.Open(pathMasterData, FileMode.Open));
		var tests = new List<T>();
		while (reader.BaseStream.Position != reader.BaseStream.Length) {
			var id = reader.ReadInt64();
			var length = reader.ReadInt32();
			var bytes = reader.ReadBytes(length);
			var test = (T)System.Activator.CreateInstance(typeof(T));
			test.Update(bytes);
			tests.Add(test);
		}
		reader.Close();
		return tests;
	}
}
