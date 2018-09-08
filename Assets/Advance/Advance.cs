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
	}

	public byte[] ToBytes() {
		var builder = new FlatBufferBuilder(1024);
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
			else if (IsType<List<byte>>(field)) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(1, list.Count, 1);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddByte((byte)list[i]);
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
			else if (IsType<string>(field)) {
				builder.AddOffset(index, field.stringOffset.Value, 0);
			}
			else if (IsType<List<byte>>(field)) {
				builder.AddOffset(index, field.vectorOffset.Value, 0);
			}
		}
		int o = builder.EndObject();
		builder.Finish(new Offset(o).Value);
		using (var ms = new MemoryStream(builder.SizedByteArray())) {
			return ms.ToArray();
		}
	}

	public void Update(byte[] bytes) {
		var bb = new ByteBuffer(bytes);
		__assign(bb.GetInt(bb.Position) + bb.Position, bb);

		var fields = GetProperties();
		for (var i = 0; i < fields.Length; i++) {
			var field = fields[i];
			int index = i * 2;
			if (IsType<bool>(field)) {
				field.SetValue(this, GetBool(index), null);
			}
			else if (IsType<short>(field)) {
				field.SetValue(this, GetShort(index), null);
			}
			else if (IsType<int>(field)) {
				field.SetValue(this, GetInt(index), null);
			}
			else if (IsType<string>(field)) {
				field.SetValue(this, GetString(index), null);
			}
			else if (IsType<List<byte>>(field)) {
				field.SetValue(this, GetByteList(index), null);
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

	private string GetString(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.__string(o + __p.bb_pos) : null;
	}

	private bool IsType<T>(FieldData f) {
		return f.type == typeof(T);
	}

	private bool IsType<T>(PropertyInfo p) {
		return p.PropertyType == typeof(T);
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
		var bytes = new Fiend {
			Friendly = false,
			Hp = 10,
			Name = "Some Or Any",
			Inventory = new List<byte>() { (byte)1, (byte)20 },
			Mana = 12
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
		var fiend = new Fiend();
		fiend.Update(bytes);
		Debug.Log(fiend.Mana + " -" + fiend.Name + " - " + fiend.Hp + " - " + fiend.Friendly + " - " + fiend.Inventory[1]);
	}
}

public class Entity : BaseFB {
	public int Mana { set; get; }
}

//the order needs to be the same as schema
public class Fiend : Entity {
	public short Hp { set; get; }
	public string Name { set; get; }
	public bool Friendly { set; get; }
	public List<byte> Inventory { set; get; }

	public void Test() {
		
	}
}

