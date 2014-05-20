using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonUtils
{
    public static class ObjectExtensionMethods
    {
        public static T DeepCopy<T>(this T a)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, a);
                memoryStream.Position = 0;
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }
    }


}
