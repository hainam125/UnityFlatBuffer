using System;

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
}
