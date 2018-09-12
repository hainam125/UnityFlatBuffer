using System;
using System.Collections.Generic;
using System.IO;

public static class Helper {
	public static byte[] GetBytes(ArraySegment<byte>? arraySegment) {
		if (arraySegment.HasValue && arraySegment.Value.Count > 0) {
			var array = arraySegment.Value;
			var bytes = new byte[array.Count];
			Array.Copy(array.Array, array.Offset, bytes, 0, array.Count);
			return bytes;
		}
		return null;
	}

	public static DateTime RandomDate() {
		var start = new DateTime(1995, 1, 1);
		int range = (DateTime.Today - start).Days;
		return start.AddDays(UnityEngine.Random.Range(0, range));
	}

    public static void SaveMasterData<T>(List<T> tests, string pathMasterData) where T : MBaseTest
    {
        using (var writer = new BinaryWriter(File.Open(pathMasterData, FileMode.Create)))
        {
            foreach (var t in tests)
            {
                writer.Write(t.Id);
                var bytes = t.ToBytes();
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }
            UnityEngine.Debug.Log("Save Many!");
        }
    }
}
