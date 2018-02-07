using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

//Currently not using this class because I couldn't figure out a good way to sort the array it returns
//That made using it pretty impractical
public static class ReflectionHelper {

	static Assembly assembly = Assembly.GetExecutingAssembly();
	static Type[] types = assembly.GetTypes();

	public static Type[] GetSubclasses(Type baseClass){
		List<Type> listOfTypes = new List<Type> ();

		for (int i = 0; i < types.Length; i++) {
			if (types[i].BaseType == baseClass) {
				listOfTypes.Add (types [i]);
			}
		}

		return listOfTypes.ToArray ();
	}
}
