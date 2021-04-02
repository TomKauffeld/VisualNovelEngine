using System;
using System.IO;
using VisualNovelEngine;

namespace VisualNovelDirectory
{
    public class ScriptLoader<T> : IScriptLoader<T>
    {
        private readonly DirectoryInfo directory;
        private readonly IScriptParser<T> scriptParser;

        public ScriptLoader(string directory, IScriptParser<T> scriptParser)
        {
            this.scriptParser = scriptParser;
            this.directory = new DirectoryInfo(directory);
        }

        public IScript<T> LoadScript(string script_id)
        {
            try
            {
                FileInfo file = null;

                foreach(FileInfo f in directory.EnumerateFiles())
                {
                    if (f.Name.Substring(0, f.Name.Length - f.Extension.Length) == script_id)
                    {
                        file = f;
                        break;
                    }
                }
                if (file == null)
                    return null;
                string code = File.ReadAllText(file.FullName);
                return scriptParser.Parse(code, file.Extension.Substring(1));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Couldn't load the script {0}", script_id), e);
            }
        }
    }
}
