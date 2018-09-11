using System.Reflection;

public static class Extension {
	public static bool IsType<T>(this PropertyInfo propertyInfo) {
		return propertyInfo.PropertyType == typeof(T);
	}
}
