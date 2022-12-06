using System.IO;
using UnityEngine;

namespace ObjectManagement
{
    public class PersisentStorage : MonoBehaviour
    {
        private string _savePath;

        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        }


        public void Save(PersistableObject o, int version)
        {
            using (var writer = new BinaryWriter(File.Open(_savePath, FileMode.Create)))
            {
                writer.Write(-version);
                o.Save(new GameDataWriter(writer));
            }
        }

        public void Load(PersistableObject o)
        {
            ///协程会导致在文件流关闭后才进行读取数据的操作，所以先预读到内存中
            byte[] data = File.ReadAllBytes(_savePath);

            var reader = new BinaryReader(new MemoryStream(data));
        
            o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
    }
}