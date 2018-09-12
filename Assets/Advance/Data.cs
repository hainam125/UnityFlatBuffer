using System.Collections.Generic;
using FlatBuffers;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Linq;

public class Base0 : BaseFB
{
    public virtual long Id { get; set; }
}

public class Base1 : Base0 {
    public override long Id { get; set; }
    public long CreatedAt { get; set; }
	public long UpdatedAt { get; set; }
}

public class UBaseTest : Base1 {
	public long UserId { get; set; }
}

public class MBaseTest : BaseFB {
	public long Id { get; set; }
	public long CreatedAt { get; set; }
	public long UpdatedAt { get; set; }
	public string CreatedBy { get; set; }
	public string UpdatedBy { get; set; }
	public string Name { get; set; }
	public string Label { get; set; }
}

namespace FlatBuffers {
	public struct Offset {
		public int Value;

		public Offset(int value) {
			Value = value;
		}
	}
}

public abstract class BaseFB {
	private static int initIndex = 4;//4 get from basic schema
	private Table __p;

	private class FieldData {
		public object value;
		public System.Type type;
		public StringOffset stringOffset;
		public VectorOffset vectorOffset;
		public Offset offset;
	}

	private FlatBufferBuilder CreateBuilder(FlatBufferBuilder builder = null) {
		if (builder == null) builder = new FlatBufferBuilder(1024);
		var fields = GetProperties();
		var fieldList = new List<FieldData>();

		for (var index = 0; index < fields.Length; index++) {
			var field = fields[index];
			var value = field.GetValue(this);
			var fieldData = new FieldData { value = value, type = field.PropertyType };
			fieldList.Add(fieldData);
			if (value == null) continue;
			if (field.IsType<string>()) {
				fieldData.stringOffset = builder.CreateString((string)value);
			}
			else if (field.PropertyType.IsEnum) {
				fieldData.stringOffset = builder.CreateString(value.ToString());
			}
			else if (field.IsType<List<byte>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(1, list.Count, 1);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddByte((byte)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<bool>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(1, list.Count, 1);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddBool((bool)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<short>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(2, list.Count, 2);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddShort((short)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<int>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(4, list.Count, 4);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddInt((int)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<long>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(8, list.Count, 8);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddLong((long)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<float>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(4, list.Count, 4);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddFloat((float)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<double>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					builder.StartVector(8, list.Count, 8);
					for (int i = list.Count - 1; i >= 0; i--) builder.AddDouble((double)list[i]);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.IsType<List<string>>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					var stringOffsets = new StringOffset[list.Count];
					for (int i = list.Count - 1; i >= 0; i--) stringOffsets[i] = builder.CreateString((string)list[i]);
					builder.StartVector(4, list.Count, 4);//4: value for string from generated files
					for (int i = list.Count - 1; i >= 0; i--) builder.AddOffset(stringOffsets[i].Value);
					fieldData.vectorOffset = builder.EndVector();
				}
			}
			else if (field.PropertyType.IsSubclassOf(typeof(BaseFB))) {
				fieldData.offset = ((BaseFB)value).CreateOffset(builder);
			}
			else if (field.IsGenericList<BaseFB>()) {
				var list = (IList)value;
				if (list.Count > 0) {
					var dataOffsets = new Offset[list.Count];
					for (int i = list.Count - 1; i >= 0; i--) dataOffsets[i] = ((BaseFB)list[i]).CreateOffset(builder);
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
			else if (field.type.IsSubclassOf(typeof(BaseFB))) {
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
			if (field.IsType<bool>()) {
				field.SetValue(this, GetBool(index), null);
			}
			else if (field.IsType<byte>()) {
				field.SetValue(this, GetBool(index), null);
			}
			else if (field.IsType<short>()) {
				field.SetValue(this, GetShort(index), null);
			}
			else if (field.IsType<int>()) {
				field.SetValue(this, GetInt(index), null);
			}
			else if (field.IsType<long>()) {
				field.SetValue(this, GetLong(index), null);
			}
			else if (field.IsType<float>()) {
				field.SetValue(this, GetFloat(index), null);
			}
			else if (field.IsType<double>()) {
				field.SetValue(this, GetDouble(index), null);
			}
			else if (field.IsType<string>()) {
				field.SetValue(this, GetString(index), null);
			}
			else if (field.PropertyType.IsEnum) {
				var value = System.Enum.Parse(field.PropertyType, GetString(index));
				field.SetValue(this, value, null);
			}
			else if (field.IsType<List<bool>>()) {
				field.SetValue(this, GetBoolList(index), null);
			}
			else if (field.IsType<List<byte>>()) {
				field.SetValue(this, GetByteList(index), null);
			}
			else if (field.IsType<List<short>>()) {
				field.SetValue(this, GetShortList(index), null);
			}
			else if (field.IsType<List<int>>()) {
				field.SetValue(this, GetIntList(index), null);
			}
			else if (field.IsType<List<long>>()) {
				field.SetValue(this, GetLongList(index), null);
			}
			else if (field.IsType<List<float>>()) {
				field.SetValue(this, GetFloatList(index), null);
			}
			else if (field.IsType<List<double>>()) {
				field.SetValue(this, GetDoubleList(index), null);
			}
			else if (field.IsType<List<string>>()) {
				field.SetValue(this, GetStringList(index), null);
			}
			else if (field.PropertyType.IsSubclassOf(typeof(BaseFB))) {
				field.SetValue(this, GetChildTestType(index, field.PropertyType), null);
			}
			else if (field.IsGenericList<BaseFB>()) {
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

	private BaseFB GetChildTestType(int index, System.Type type) {
		int o = __p.__offset(initIndex + index);
		if (o == 0) return null;
		var rawTest = System.Activator.CreateInstance(type);
		var test = ((BaseFB)rawTest);
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
			var test = ((BaseFB)rawTest);
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

	private bool IsGenericListOfTest(FieldData f) {
		return f.type.IsGenericType && f.type.GetGenericTypeDefinition() == typeof(List<>) &&
					 f.type.GetTypeInfo().GenericTypeArguments[0].IsSubclassOf(typeof(BaseFB));
	}

	private int GetArrayLength(int index) {
		int o = __p.__offset(initIndex + index); return o != 0 ? __p.__vector_len(o) : 0;
	}

	private bool IsType<T>(FieldData field) {
		return field.type == typeof(T);
	}

	public PropertyInfo[] GetProperties(bool selfProperty = false) {
		var type = GetType();
		var orderList = new List<System.Type>() { type };
		if (!selfProperty) {
			var iteratingType = type.BaseType;
			while (iteratingType != null) {
				orderList.Insert(0, iteratingType);
				iteratingType = iteratingType.BaseType;
			}
		}
		var flags = BindingFlags.Public | BindingFlags.Instance;
		if (selfProperty) flags |= BindingFlags.DeclaredOnly;
		var props = type.GetProperties(flags)
										.OrderBy(x => orderList.IndexOf(x.DeclaringType)).ToArray();

		return props;
	}
}
