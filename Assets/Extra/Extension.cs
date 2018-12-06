using System;
using System.Collections.Generic;
using System.Reflection;

public static class Extension {
	public static bool IsType<T>(this PropertyInfo propertyInfo) {
		return propertyInfo.PropertyType == typeof(T);
	}

    public static bool IsGenericList<T>(this PropertyInfo propertyInfo)
    {
        return propertyInfo.IsGenericListType() && propertyInfo.PropertyType.GetTypeInfo().GenericTypeArguments[0].IsSubclassOf(typeof(T));
    }

    public static bool IsGenericListType(this PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
    }

    public static byte[] GetBytes(this ArraySegment<byte>? arraySegment)
    {
        if (arraySegment.HasValue && arraySegment.Value.Count > 0)
        {
            var array = arraySegment.Value;
            var bytes = new byte[array.Count];
            Array.Copy(array.Array, array.Offset, bytes, 0, array.Count);
            return bytes;
        }
        return null;
    }
}
