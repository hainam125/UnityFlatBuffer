using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatBuffers;
using MyGame.Sample;
using System.IO;

public class Demo : MonoBehaviour {
	private string pathData;

	private void Start() {
		pathData = Application.dataPath + "/saved.dat";
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
		// Create a `FlatBufferBuilder`, which will be used to create our monsters' FlatBuffers.
		// You can pass an initial size of the buffer (here 1024 bytes), which will grow automatically if needed
		var builder = new FlatBufferBuilder(1024);

		var weaponOneName = builder.CreateString("Sword");
		var weaponOneDamage = 3;
		var weaponTwoName = builder.CreateString("Axe");
		var weaponTwoDamage = 5;

		// Use the `CreateWeapon()` helper function to create the weapons, since we set every field.
		var sword = Weapon.CreateWeapon(builder, weaponOneName, (short)weaponOneDamage);
		var axe = Weapon.CreateWeapon(builder, weaponTwoName, (short)weaponTwoDamage);

		// Serialize a name for our monster, called "Orc".
		var name = builder.CreateString("Orc");
		// Create a `vector` representing the inventory of the Orc. Each number
		// could correspond to an item that can be claimed after he is slain.
		// Note: Since we prepend the bytes, this loop iterates in reverse order.
		// as buffers are built back to front.
		Monster.StartInventoryVector(builder, 10);
		for (int i = 9; i >= 0; i--) {
			builder.AddByte((byte)i);
		}
		var inv = builder.EndVector();
		// Note: To create a vector of nested objects (e.g. tables, strings, or other vectors), collect their
		// offsets into a temporary data structure, and then create an additional vector containing their offsets.
		var weaps = new Offset<Weapon>[2] { sword, axe };

		var weapons = Monster.CreateWeaponsVector(builder, weaps);

		Monster.StartPathVector(builder, 2);
		Vec3.CreateVec3(builder, 1f, 2f, 3f);
		Vec3.CreateVec3(builder, 4f, 5f, 6f);
		var path = builder.EndVector();

		// Create our monster using `StartMonster()` and `EndMonster()`
		Monster.StartMonster(builder);
		Monster.AddPos(builder, Vec3.CreateVec3(builder, 1f, 2f, 3f));
		Monster.AddHp(builder, (short)300);
		Monster.AddName(builder, name);
		Monster.AddInventory(builder, inv);
		Monster.AddColor(builder, MyGame.Sample.Color.Red);
		Monster.AddWeapons(builder, weapons);
		Monster.AddEquippedType(builder, Equipment.Weapon);
		Monster.AddEquipped(builder, axe.Value);
		Monster.AddPath(builder, path);
		var orc = Monster.EndMonster(builder);

		// Call `Finish()` to instruct the builder that this monster is complete.
		builder.Finish(orc.Value); // You could also call `Monster.FinishMonsterBuffer(builder, orc);`.

		// This must be called after `Finish()`.
		// var buf = builder.DataBuffer;
		// Of type `FlatBuffers.ByteBuffer`.
		// The data in this ByteBuffer does NOT start at 0, but at buf.Position.
		// The end of the data is marked by buf.Length, so the size is
		// buf.Length - buf.Position.
		// Alternatively this copies the above data out of the ByteBuffer for you:
		// byte[] buf = builder.SizedByteArray();

		//Now you can write the bytes to a file, send them over the network.
		//Make sure your file mode (or tranfer protocol) is set to BINARY, not text

		// Save the data into "SAVE_FILENAME.whatever" file, name doesn't matter obviously
		using (var ms = new MemoryStream(builder.SizedByteArray())) {
			File.WriteAllBytes(pathData, ms.ToArray());
			Debug.Log("SAVED !");
		}
	}

	private void LoadBuffer() {
		byte[] bytes;
		using (var file = new FileStream(pathData, FileMode.Open, FileAccess.Read)) {
			bytes = new byte[file.Length];
			file.Read(bytes, 0, bytes.Length);
		}
		var buf = new ByteBuffer(bytes);
		var monster = Monster.GetRootAsMonster(buf);
		var hp = monster.Hp;
		var mana = monster.Mana;
		var name = monster.Name;
		Debug.Log(hp + " - " + mana + " - " + name);

		int invLength = monster.InventoryLength;
		var thirdItem = monster.Inventory(2);
		Debug.Log("Inventory: " + invLength + " - " + thirdItem);

		int weaponsLength = monster.WeaponsLength;
		var firstWeaponName = monster.Weapons(0).Value.Name;
		var firstWeaponDamage = monster.Weapons(0).Value.Damage;
		Debug.Log("Weapons: " + weaponsLength + " - " + firstWeaponName + " - " + firstWeaponDamage);

		var unionType = monster.EquippedType;
		if (unionType == Equipment.Weapon) {
			var weapon = monster.Equipped<Weapon>().Value;
			var weaponName = weapon.Name;
			var weaponDamage = weapon.Damage;
			Debug.Log("Equipped: " + weaponName + " - " + weaponDamage);
		}
	}
}
