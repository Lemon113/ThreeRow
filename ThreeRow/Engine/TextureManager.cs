using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace ThreeRow.Engine
{
    class TextureManager
    {
        private static TextureManager _INSTANCE;
        private Dictionary<String, Texture2D> _textures;

        private TextureManager()
        {

        }

        public static void Init(ContentManager Content)
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = new TextureManager();
            }
            _INSTANCE._textures = LoadContent<Texture2D>(Content, "Textures");
        }

        public static Texture2D GetTexture(String name)
        {
            Texture2D texture = null;
            _INSTANCE._textures.TryGetValue(name, out texture);
            return texture;
        }

        public static Dictionary<String, T> LoadContent<T>(ContentManager contentManager, string contentFolder)
        {
            //Load directory info, abort if none
            DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            //Init the resulting list
            Dictionary<String, T> result = new Dictionary<String, T>();

            //Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
            //Return the result
            return result;
        }
    }
}
