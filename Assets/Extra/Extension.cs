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
}
