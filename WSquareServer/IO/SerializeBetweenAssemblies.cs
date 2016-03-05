using System;
using System.Runtime.Serialization;
using System.Reflection;

namespace Personal.IO
{
	sealed class SerializeBetweenAssemblies:SerializationBinder
	{



			public override Type BindToType(string assemblyName, string typeName)
			{
				Type typeToDeserialize = null;

				String currentAssembly = Assembly.GetExecutingAssembly().FullName;

				// In this case we are always using the current assembly
				assemblyName = currentAssembly;

				// Get the type using the typeName and assemblyName
				typeToDeserialize = Type.GetType(String.Format("{0}, {1}",typeName, assemblyName));
				
				return typeToDeserialize;
			}
		
	}
}

