using System.Collections.Generic;
using UnityEngine;
using FlatBuffers;
using System.IO;
using System.Reflection;
using System.Collections;

namespace FlatBuffers {
	public struct Offset {
		public int Value;

		public Offset(int value) {
			Value = value;
		}
	}
}

public class BaseFB {
	private static int initIndex = 4;//4 get from basic schema
	private Table __p;

	private class FieldData {
		public object value;
		public System.Type type;
		public StringOffset stringOffset;
		public VectorOffset vectorOffset;
		public Offset offset;
	}

	private FlatBufferBuilder CreateBuilder(FlatBufferBuilder builder=null) {
		if(builder == null) builder = new FlatBufferBuilder(1024);
		var fields = GetProperties();
		var fieldList = new List<FieldData>();

		for (var index = 0; index < fields.Length; index++) {
			var field = fields[index];
			var value = field.GetValue(this);
			var fieldData = new FieldData { value = value, type = field.PropertyType };
			fieldList.Add(fieldData);
			if (value == null) continue;
			if (IsType<string>(field)) {
				fieldData.stringOffset = builder.CreateString((string)value);
			}
			else if (field.PropertyType.IsEnum) {
				fieldData.stringOffset = builder.CreateString(value.ToString());
			}
			else if (IsType<List<byte>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(1, list.Count, 1);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddByte((byte)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<bool>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(1, list.Count, 1);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddBool((bool)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<short>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(2, list.Count, 2);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddShort((short)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<int>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(4, list.Count, 4);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddInt((int)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<long>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(8, list.Count, 8);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddLong((long)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<float>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(4, list.Count, 4);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddFloat((float)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<double>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(8, list.Count, 8);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddDouble((double)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (IsType<List<string>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					var stringOffsets = new StringOffset[list.Count];
					for (int i = list.Count - 1; i >= 0; i--) stringOffsets[i] = builder.CreateString((string)list[i]);
					builder.StartVector(4, list.Count, 4);//4: value for string from generated files
					for (int i = list.Count - 1; i >= 0; i--) builder.AddOffset(stringOffsets[i].Value);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.PropertyType.IsSubclassOf(typeof(Test))) {
				fieldData.offset = ((Test)value).CreateOffset(builder);
			}
			else if (IsGenericListOfTest(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					var dataOffsets = new Offset[list.Count];
					for (int i = list.Count - 1; i >= 0; i--) dataOffsets[i] = ((Test)list[i]).CreateOffset(builder);
					builder.StartVector(4, list.Count, 4);//4: value for object from generated files
					for (int i = list.Count - 1; i >= 0; i--) builder.AddOffset(dataOffsets[i].Value);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
		}

		builder.StartObject(fields.Length);
		for (var index = 0; index < fields.Length; index++) {
			var field = fieldList[index];
			if (IsType<bool>(field)) {
				builder.AddBool(index, (bool)field.value, false);
			}
			else if (IsType<short>(field)) {
				builder.AddShort(index, (short)field.value, 0);
			}
			else if (IsType<int>(field)) {
				builder.AddInt(index, (int)field.value, 0);
			}
			else if (IsType<long>(field)) {
				builder.AddLong(index, (long)field.value, 0);
			}
			else if (IsType<float>(field)) {
				builder.AddFloat(index, (float)field.value, 0);
			}
			else if (IsType<double>(field)) {
				builder.AddDouble(index, (double)field.value, 0);
			}
			else if (IsType<string>(field)) {
				builder.AddOffset(index, field.stringOffset.Value, 0);
			}
			else if (field.type.IsEnum) {
				builder.AddOffset(index, field.stringOffset.Value, 0);
			}
			else if (IsType<List<byte>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<bool>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<short>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<int>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<long>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<float>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<double>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (IsType<List<string>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
			else if (field.type.IsSubclassOf(typeof(Test))) {
				builder.AddOffset(index, field.offset.Value, 0);
			}
			else if (IsGenericListOfTest(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
		}
		return builder;
	}

	private Offset CreateOffset(FlatBufferBuilder builder = null) {
		CreateBuilder(builder);
		int o = builder.EndObject();
		return new Offset(o);
	}

	public byte[] ToBytes() {
		var builder = CreateBuilder();
		int o = builder.EndObject();
		builder.Finish(new Offset(o).Value);
		using (var ms = new MemoryStream(builder.SizedByteArray())) {
			return ms.ToArray();
		}
	}
	public void Update(byte[] bytes) {
		var bb = new ByteBuffer(bytes);
		__assign(bb.GetInt(bb.Position) + bb.Position, bb);
		UpdateFields();
	}

	private void UpdateFields() {
		var fields = GetProperties();
		for (var i = 0; i < fields.Length; i++) {
			var field = fields[i];
			int index = i * 2;
			if (IsType<bool>(field)) {
				field.SetValue(this, GetBool(index), null);
			}
			else if (IsType<byte>(field)) {
				field.SetValue(this, GetBool(index), null);
			}
			else if (IsType<short>(field)) {
				field.SetValue(this, GetShort(index), null);
			}
			else if (IsType<int>(field)) {
				field.SetValue(this, GetInt(index), null);
			}
			else if (IsType<long>(field)) {
				field.SetValue(this, GetLong(index), null);
			}
			else if (IsType<float>(field)) {
				field.SetValue(this, GetFloat(index), null);
			}
			else if (IsType<double>(field)) {
				field.SetValue(this, GetDouble(index), null);
			}
			else if (IsType<string>(field)) {
				field.SetValue(this, GetString(index), null);
			}
			else if (field.PropertyType.IsEnum) {
				var value = System.Enum.Parse(field.PropertyType, GetString(index));
				field.SetValue(this, value, null);
			}
			else if (IsType<List<bool>>(field)) {
				field.SetValue(this, GetBoolList(index), null);
			}
			else if (IsType<List<byte>>(field)) {
				field.SetValue(this, GetByteList(index), null);
			}
			else if (IsType<List<short>>(field)) {
				field.SetValue(this, GetShortList(index), null);
			}
			else if (IsType<List<int>>(field)) {
				field.SetValue(this, GetIntList(index), null);
			}
			else if (IsType<List<long>>(field)) {
				field.SetValue(this, GetLongList(index), null);
			}
			else if (IsType<List<float>>(field)) {
				field.SetValue(this, GetFloatList(index), null);
			}
			else if (IsType<List<double>>(field)) {
				field.SetValue(this, GetDoubleList(index), null);
			}
			else if (IsType<List<string>>(field)) {
				field.SetValue(this, GetStringList(index), null);
			}
			else if (field.PropertyType.IsSubclassOf(typeof(Test))) {
				field.SetValue(this, GetChildTestType(index, field.PropertyType), null);
			}
			else if (IsGenericListOfTest(field)) {
				field.SetValue(this, GetChildTestTypeList(index, field.PropertyType.GetTypeInfo().GenericTypeArguments[0]), null);
			}
		}
	}

	private void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
	private void __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); }

	private bool GetBool(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? 0 != __p.bb.Get(o + __p.bb_pos) : (bool)false;
	}

	private short GetShort(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0;
	}

	private int GetInt(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0;
	}

	private long GetLong(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.bb.GetLong(o + __p.bb_pos) : (long)0;
	}

	private float GetFloat(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0f;
	}

	private double GetDouble(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.bb.GetDouble(o + __p.bb_pos) : (double)0.0;
	}

	private string GetString(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.__string(o + __p.bb_pos) : null;
	}

	private Test GetChildTestType(int index, System.Type type) {
		int o = __p.__offset(initIndex + index);
		if (o == 0) return null;
		var rawTest = System.Activator.CreateInstance(type);
		Test test = ((Test)rawTest);
		test.__assign(__p.__indirect(o + __p.bb_pos), __p.bb);
		test.UpdateFields();
		return test;
	}

	private IList GetChildTestTypeList(int index, System.Type type) {
		var listType = typeof(List<>).MakeGenericType(type);
		var list = (IList)System.Activator.CreateInstance(listType);
		int length = GetArrayLength(index);

		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			if (o == 0) list.Add(null);
			var rawTest = System.Activator.CreateInstance(type);
			Test test = ((Test)rawTest);
			test.__assign(__p.__indirect(__p.__vector(o) + i * 4), __p.bb);
			test.UpdateFields();
			list.Add(test);
		}
		return list;
	}

	private List<byte> GetByteList(int index) {
		var list = new List<byte>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.Get(__p.__vector(o) + i * 1) : (byte)0;
			list.Add(item);
		}
		return list;
	}

	private List<bool> GetBoolList(int index) {
		var list = new List<bool>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? 0 != __p.bb.Get(__p.__vector(o) + i * 1) : false;
			list.Add(item);
		}
		return list;
	}

	private List<short> GetShortList(int index) {
		var list = new List<short>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.GetShort(__p.__vector(o) + i * 2) : (short)0;
			list.Add(item);
		}
		return list;
	}

	private List<int> GetIntList(int index) {
		var list = new List<int>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.GetInt(__p.__vector(o) + i * 4) : (int)0;
			list.Add(item);
		}
		return list;
	}

	private List<long> GetLongList(int index) {
		var list = new List<long>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.GetLong(__p.__vector(o) + i * 8) : (long)0;
			list.Add(item);
		}
		return list;
	}

	private List<float> GetFloatList(int index) {
		var list = new List<float>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.GetFloat(__p.__vector(o) + i * 4) : (float)0f;
			list.Add(item);
		}
		return list;
	}

	private List<double> GetDoubleList(int index) {
		var list = new List<double>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.bb.GetLong(__p.__vector(o) + i * 8) : (double)0.0;
			list.Add(item);
		}
		return list;
	}

	private List<string> GetStringList(int index) {
		var list = new List<string>();
		int length = GetArrayLength(index);
		for (int i = 0; i < length; i++) {
			int o = __p.__offset(initIndex + index);
			var item = o != 0 ? __p.__string(__p.__vector(o) + i * 4) : null;
			list.Add(item);
		}
		return list;
	}

	private bool IsType<T>(FieldData f) {
		return f.type == typeof(T);
	}

	private bool IsType<T>(PropertyInfo p) {
		return p.PropertyType == typeof(T);
	}

	private bool IsGenericListOfTest(FieldData f) {
		return f.type.IsGenericType && f.type.GetGenericTypeDefinition() == typeof(List<>) &&
					 f.type.GetTypeInfo().GenericTypeArguments[0].IsSubclassOf(typeof(Test));
	}

	private bool IsGenericListOfTest(PropertyInfo f) {
		return f.PropertyType.IsGenericType && f.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
			     f.PropertyType.GetTypeInfo().GenericTypeArguments[0].IsSubclassOf(typeof(Test));
	}
	private int GetArrayLength(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.__vector_len(o) : 0;
	}


	private PropertyInfo[] GetProperties() {
		var type = GetType();
		return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
	}
}

public class Advance : MonoBehaviour {
	private string pathData;

	private void Start() {
		pathData = Application.dataPath + "/Advance/fsaved.dat";
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.S)) {
			SaveBuffer();
		}
		else if (Input.GetKeyDown(KeyCode.L)) {
			LoadBuffer();
		}
	}

	private void SaveBuffer() {
		var other1 = new Other1 {
			MyEnum = TheEnum.Blue,
			MyBool = false,
			MyShort = 4,
			MyInt = 5,
			MyLong = 3030,
			MyFloat = 320.5f,
			MyDouble = 122.44,
			MyString = "Bye World",
			MyBoolList = new List<bool>() { false, true, true },
			MyByteList = new List<byte>() { (byte)11, (byte)4 },
			MyShortList = new List<short>() { 3, 1, 12, 5, 7, 9, 13 },
			MyIntList = new List<int>() { 13, 24, 53 },
			MyLongList = new List<long>() { 510, 360, 570, 2000000 },
			MyFloatList = new List<float>() { -8f, 173f },
			MyDoubleList = new List<double>() { -41.0, 12.02, 22.9, -5000000 },
			MyStringList = new List<string>() { "Nope", "Nai" }
		};

		var bytes = new Other2 {
			MyFriend = other1,
			MyEnemies = new List<Other1> { other1, other1 },
			MyOwnInt2 = 5000,
			MyEnum = TheEnum.Green,
			MyBool = true,
			MyShort = 2,
			MyInt = 51123,
			MyLong = 300,
			MyFloat = 30.5f,
			MyDouble = 22.44,
			MyString = "Hello World",
			MyBoolList = new List<bool>() { true, false, true },
			MyByteList = new List<byte>() { (byte)1,(byte)8 },
			MyShortList = new List<short>() { 6, 8, 10 },
			MyIntList = new List<int>() { 3, 4, 5 },
			MyLongList = new List<long>() { 50, 60, 70 },
			MyFloatList = new List<float>() { 8f, 17f, 56f },
			MyDoubleList = new List<double>() { 41.0, 22.9 },
			MyStringList = new List<string>() { "Ho Ho", "Ahihi", "Ohoho" }
		}.ToBytes();

		using (var ms = new MemoryStream(bytes)) {
			File.WriteAllBytes(pathData, ms.ToArray());
			Debug.Log("SAVED !");
		}
	}

	private void LoadBuffer() {
		byte[] bytes;
		using (var f = new FileStream(pathData, FileMode.Open, FileAccess.Read)) {
			bytes = new byte[f.Length];
			f.Read(bytes, 0, bytes.Length);
		}
		var test = new Other2();
		test.Update(bytes);
		Debug.Log(test);
	}
}

public enum TheEnum { Red, Blue, Green }

public class Other1 : Test {
	public int MyOwnInt1 { get; set; }
}

public class Other2 : Test {
	public Other1 MyFriend { get; set; }
	public List<Other1> MyEnemies { get; set; }
	public int MyOwnInt2 { get; set; }

	public override string ToString() {
		string myEnemyList = string.Empty;
		if (MyEnemies != null) foreach (var v in MyEnemies) myEnemyList += v.ToString() + "\n";
		return string.Format("[Other2: [MyOwnInt2={1}] - {2}] \n MyFriend={0} \n\n MyEnemies: \n{3} ", MyFriend, MyOwnInt2, base.ToString(), myEnemyList);
	}
}

public class Test : BaseFB {
	public TheEnum MyEnum { set; get; }
	public bool MyBool { set; get; }
	public short MyShort { set; get; }
	public int MyInt { set; get; }
	public long MyLong { set; get; }
	public float MyFloat { set; get; }
	public double MyDouble { set; get; }
	public string MyString { set; get; }
	public List<byte> MyByteList { set; get; }
	public List<bool> MyBoolList { set; get; }
	public List<short> MyShortList { set; get; }
	public List<int> MyIntList { set; get; }
	public List<float> MyFloatList { set; get; }
	public List<long> MyLongList { set; get; }
	public List<double> MyDoubleList { set; get; }
	public List<string> MyStringList { set; get; }

	public override string ToString() {
		string myByteList = string.Empty;
		if (MyByteList != null) foreach (var v in MyByteList) myByteList += v.ToString() + ":";
		string myBoolList = string.Empty;
		if (MyBoolList != null) foreach (var v in MyBoolList) myBoolList += v.ToString() + ":";
		string myShortList = string.Empty;
		if (MyShortList != null) foreach (var v in MyShortList) myShortList += v.ToString() + ":";
		string myIntList = string.Empty;
		if (MyIntList != null) foreach (var v in MyIntList) myIntList += v.ToString() + ":";
		string myLongList = string.Empty;
		if (MyLongList != null) foreach (var v in MyLongList) myLongList += v.ToString() + ":";
		string myFloatList = string.Empty;
		if (MyFloatList != null) foreach (var v in MyFloatList) myFloatList += v.ToString() + ":";
		string myDoubleList = string.Empty;
		if (MyDoubleList != null) foreach (var v in MyDoubleList) myDoubleList += v.ToString() + ":";
		string myStringList = string.Empty;
		if (MyStringList != null) foreach (var v in MyStringList) myStringList += v.ToString() + ":";
		return string.Format("[Test: MyBool={0}, MyShort={1}, MyInt={2}, MyLong={3}, MyFloat={4}, MyDouble={5}, MyString={6}," +
												 "MyByteList={7}, MyBoolList={8}, MyShortList={9}, MyIntList={10}, MyFloatList={11}, MyLongList={12}," +
												 "MyDoubleList={13}, MyStringList={14}, MyEnum={15} ]", MyBool, MyShort, MyInt, MyLong, MyFloat, MyDouble, MyString,
												 myByteList, myBoolList, myShortList, myIntList, myFloatList, myLongList, myDoubleList, myStringList, MyEnum);
	}
}

