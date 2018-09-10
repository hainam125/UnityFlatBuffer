using System.IO;
using FlatBuffers;
using MyGame.Other;
using UnityEngine;

public class Other : MonoBehaviour {
	private string pathData;

	private void Start() {
		pathData = Application.dataPath + "/Other/saved.dat";
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.S)) {
			SaveBuffer();
		}
		else if (Input.GetKeyDown(KeyCode.L)) {
			LoadBuffer();
		}
	}

	private byte[] CreateFiendBytes() {
		var builder = new FlatBufferBuilder(1024);
		var weaponOneName = builder.CreateString("Lance");
		var weaponOneDamage = 5;
		var lance = Weapon.CreateWeapon(builder, weaponOneName, (short)weaponOneDamage);
		var name = builder.CreateString("Goblin");
		Fiend.StartFiend(builder);
		Fiend.AddName(builder, name);
		Fiend.AddHp(builder, (short)100);
		Fiend.AddEquippedType(builder, Equipment.Weapon);
		Fiend.AddEquipped(builder, lance.Value);
		var fiend = Fiend.EndFiend(builder);
		builder.Finish(fiend.Value);
		return builder.SizedByteArray();
	}

	private void SaveBuffer() {
		byte[] fiendBytes = CreateFiendBytes();

		var builder = new FlatBufferBuilder(1024);

		var name = builder.CreateString("Super Orc");

		var weaponOneName = builder.CreateString("Sword");
		var weaponOneDamage = 3;
		var weaponTwoName = builder.CreateString("Axe");
		var weaponTwoDamage = 5;
		var sword = Weapon.CreateWeapon(builder, weaponOneName, (short)weaponOneDamage);
		var axe = Weapon.CreateWeapon(builder, weaponTwoName, (short)weaponTwoDamage);
		var weaps = new Offset<Weapon>[2] { sword, axe };
		var weapons = Monster.CreateWeaponsVector(builder, weaps);

		Monster.StartInventoryVector(builder, 10);
		for (int i = 9; i >= 0; i--) builder.AddByte((byte)i);
		var inv = builder.EndVector();

		Monster.StartDataVector(builder, fiendBytes.Length);
		for (int i = fiendBytes.Length - 1; i >= 0; i--) builder.AddByte(fiendBytes[i]);
		var dataBytes = builder.EndVector();

		Monster.StartMonster(builder);
		Monster.AddHp(builder, (short)300);
		Monster.AddName(builder, name);
		Monster.AddInventory(builder, inv);
		Monster.AddWeapons(builder, weapons);
		Monster.AddEquippedType(builder, Equipment.Weapon);
		Monster.AddEquipped(builder, axe.Value);
		Monster.AddData(builder, dataBytes);
		var orc = Monster.EndMonster(builder);
		builder.Finish(orc.Value);

		using (var ms = new MemoryStream(builder.SizedByteArray())) {
			File.WriteAllBytes(pathData, ms.ToArray());
			Debug.Log("SAVED !");
		}
	}

	private void LoadBuffer() {
		byte[] monsterBytes;
		using (var file = new FileStream(pathData, FileMode.Open, FileAccess.Read)) {
			monsterBytes = new byte[file.Length];
			file.Read(monsterBytes, 0, monsterBytes.Length);
		}
		var buf = new ByteBuffer(monsterBytes);
		var monster = Monster.GetRootAsMonster(buf);

		Debug.Log(monster.Name);

		var arraySegment = monster.GetDataBytes();
		var array = arraySegment.Value;
		var bytes = new byte[array.Count];
		Debug.Log(array.Offset);
		System.Array.Copy(array.Array, array.Offset, bytes, 0, array.Count);
		var fiendBuffer = new ByteBuffer(bytes);
		var fiend = Fiend.GetRootAsFiend(fiendBuffer);
		Debug.Log(fiend.Name + " - " + fiend.Hp);
	}
}
