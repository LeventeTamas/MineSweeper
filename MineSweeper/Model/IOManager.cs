using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Model
{
    public class IOManager
    {
        
        public static void SaveIGameModel(string path, IGameModel gameModel)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, gameModel);
            }
        }

        public static IGameModel LoadIGameModel(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (IGameModel)binaryFormatter.Deserialize(stream);
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static void IniWriteValue(string file, string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, file);
        }

        public static string IniReadValue(string file, string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, file);
            return temp.ToString();

        }

    }
}
