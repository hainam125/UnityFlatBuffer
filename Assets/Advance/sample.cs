// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyGame.Demo
{

using global::System;
using global::FlatBuffers;

public struct Fiend : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Fiend GetRootAsFiend(ByteBuffer _bb) { return GetRootAsFiend(_bb, new Fiend()); }
  public static Fiend GetRootAsFiend(ByteBuffer _bb, Fiend obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Fiend __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public short Hp { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)100; } }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
  public bool Friendly { get { int o = __p.__offset(8); return o != 0 ? 0!=__p.bb.Get(o + __p.bb_pos) : (bool)false; } }
  public string Inventory(int j) { int o = __p.__offset(10); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int InventoryLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
  public Item? Item { get { int o = __p.__offset(12); return o != 0 ? (Item?)(new Item()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public Equipment? Equip { get { int o = __p.__offset(14); return o != 0 ? (Equipment?)(new Equipment()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }

  public static Offset<Fiend> CreateFiend(FlatBufferBuilder builder,
      short hp = 100,
      StringOffset nameOffset = default(StringOffset),
      bool friendly = false,
      VectorOffset inventoryOffset = default(VectorOffset),
      Offset<Item> itemOffset = default(Offset<Item>),
      Offset<Equipment> equipOffset = default(Offset<Equipment>)) {
    builder.StartObject(6);
    Fiend.AddEquip(builder, equipOffset);
    Fiend.AddItem(builder, itemOffset);
    Fiend.AddInventory(builder, inventoryOffset);
    Fiend.AddName(builder, nameOffset);
    Fiend.AddHp(builder, hp);
    Fiend.AddFriendly(builder, friendly);
    return Fiend.EndFiend(builder);
  }

  public static void StartFiend(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddHp(FlatBufferBuilder builder, short hp) { builder.AddShort(0, hp, 100); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(1, nameOffset.Value, 0); }
  public static void AddFriendly(FlatBufferBuilder builder, bool friendly) { builder.AddBool(2, friendly, false); }
  public static void AddInventory(FlatBufferBuilder builder, VectorOffset inventoryOffset) { builder.AddOffset(3, inventoryOffset.Value, 0); }
  public static VectorOffset CreateInventoryVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartInventoryVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddItem(FlatBufferBuilder builder, Offset<Item> itemOffset) { builder.AddOffset(4, itemOffset.Value, 0); }
  public static void AddEquip(FlatBufferBuilder builder, Offset<Equipment> equipOffset) { builder.AddOffset(5, equipOffset.Value, 0); }
  public static Offset<Fiend> EndFiend(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Fiend>(o);
  }
};

public struct Item : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Item GetRootAsItem(ByteBuffer _bb) { return GetRootAsItem(_bb, new Item()); }
  public static Item GetRootAsItem(ByteBuffer _bb, Item obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Item __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
  public short Damage { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }

  public static Offset<Item> CreateItem(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      short damage = 0) {
    builder.StartObject(2);
    Item.AddName(builder, nameOffset);
    Item.AddDamage(builder, damage);
    return Item.EndItem(builder);
  }

  public static void StartItem(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddDamage(FlatBufferBuilder builder, short damage) { builder.AddShort(1, damage, 0); }
  public static Offset<Item> EndItem(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Item>(o);
  }
};

public struct Equipment : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Equipment GetRootAsEquipment(ByteBuffer _bb) { return GetRootAsEquipment(_bb, new Equipment()); }
  public static Equipment GetRootAsEquipment(ByteBuffer _bb, Equipment obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Equipment __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
  public short Damage { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public int Uses { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<Equipment> CreateEquipment(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      short damage = 0,
      int uses = 0) {
    builder.StartObject(3);
    Equipment.AddUses(builder, uses);
    Equipment.AddName(builder, nameOffset);
    Equipment.AddDamage(builder, damage);
    return Equipment.EndEquipment(builder);
  }

  public static void StartEquipment(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddDamage(FlatBufferBuilder builder, short damage) { builder.AddShort(1, damage, 0); }
  public static void AddUses(FlatBufferBuilder builder, int uses) { builder.AddInt(2, uses, 0); }
  public static Offset<Equipment> EndEquipment(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Equipment>(o);
  }
};


}
