using System;
using System.Collections;
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

    public static long RandomLongDate() {
        return RandomDate().Ticks;
    }

    public static List<long> RandomList(int length, int max)
    {
        List<long> r = new List<long>();
        int count = UnityEngine.Random.Range(0, length);
        for (int i = 0; i < count; i++)
        {
            long item = (long)UnityEngine.Random.Range(0, max);
            r.Add(item);
        }
        return r;
    }

    public static string RandomName(string prefix)
    {
        int count = UnityEngine.Random.Range(10, 25);
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[count];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }

        return prefix + " " + new string(stringChars);
    }
}
public static class DataHelper
{
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

    public static IList LoadMaster(string pathMasterData, Type type)
    {
        var listType = typeof(List<>).MakeGenericType(type);
        var list = (IList)Activator.CreateInstance(listType);

        var reader = new BinaryReader(File.Open(pathMasterData, FileMode.Open));
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            var id = reader.ReadInt64();
            var length = reader.ReadInt32();
            var bytes = reader.ReadBytes(length);
            var test = (BaseFB)System.Activator.CreateInstance(type);
            test.Update(bytes);
            list.Add(test);
        }
        reader.Close();
        return list;
    }

    public static List<T> LoadMaster<T>(string pathMasterData) where T : MBaseTest
    {
        var reader = new BinaryReader(File.Open(pathMasterData, FileMode.Open));
        var tests = new List<T>();
        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
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
