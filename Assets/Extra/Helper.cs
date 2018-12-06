using System;
using System.Collections.Generic;

public static class Helper {
	public static DateTime RandomDate() {
		var start = new DateTime(1995, 1, 1);
		int range = (DateTime.Today - start).Days;
		return start.AddDays(UnityEngine.Random.Range(0, range));
	}

	public static long RandomLongDate() {
		return RandomDate().Ticks;
	}

	public static List<long> RandomList(int length, int max) {
		List<long> r = new List<long>();
		int count = UnityEngine.Random.Range(0, length);
		for (int i = 0; i < count; i++) {
			long item = (long)UnityEngine.Random.Range(0, max);
			r.Add(item);
		}
		return r;
	}

	public static string RandomName(string prefix) {
		int count = UnityEngine.Random.Range(10, 25);
		var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		var stringChars = new char[count];

		for (int i = 0; i < stringChars.Length; i++) {
			stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
		}

		return prefix + " " + new string(stringChars);
	}
}
