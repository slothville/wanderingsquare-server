using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Personal.IO
{
	public static class SerializationHelper
	{
		public static byte[] SerializedObjectToArray(Object obj)
		{
			if(obj==null){return new byte[0];}

			using (var ms = new MemoryStream())
			{

				(new BinaryFormatter()).Serialize(ms, obj);

				return ms.ToArray();
				
			}

		}

		public static T DeserializeObject<T>(byte[] serializedobject)
		{
			if(serializedobject==null){return (default(T));}

			using (var ms = new MemoryStream(serializedobject))
			{
					BinaryFormatter formatter = new BinaryFormatter ();
		
					formatter.Binder = new SerializeBetweenAssemblies ();

					return (T)formatter.Deserialize (ms);

			}
		
		}
//		public static object DeserializeObject(byte[] serializedobject)
//		{
//
//			using (var ms = new MemoryStream(serializedobject))
//			{
//				BinaryFormatter formatter = new BinaryFormatter ();
//
//				formatter.Binder = new SerializeBetweenAssemblies ();
//
//
//				return formatter.Deserialize (ms);
//			}
//
//		}
	}
}

