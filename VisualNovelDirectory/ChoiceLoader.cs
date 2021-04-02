using System;
using System.Collections.Generic;
using System.IO;
using VisualNovelConsole;
using VisualNovelEngine;

namespace VisualNovelDirectory
{
    public class ChoiceLoader : IChoiceLoader
    {
        private readonly DirectoryInfo directory;
        private readonly IScriptLoader<bool> scriptBoolLoader;


        public ChoiceLoader(string directory, IScriptLoader<bool> scriptBoolLoader)
        {
            this.directory = new DirectoryInfo(directory);
            this.scriptBoolLoader = scriptBoolLoader;
        }

        private IList<string> GetChoices(string node_id)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(directory.FullName, node_id, "choices"));
                if (!dir.Exists)
                    return new List<string>();
                List<string> choices = new List<string>();
                foreach (DirectoryInfo d in dir.EnumerateDirectories())
                    choices.Add(d.Name);
                return choices;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Couldn't load the choice ids for {0}", node_id), e);
            }
        }


        private Choice LoadChoice(string node_id, string choice_id)
        {
            try
            {
                DirectoryInfo mainDir = new DirectoryInfo(Path.Combine(directory.FullName, node_id, "choices", choice_id));
                FileInfo changeFile = new FileInfo(Path.Combine(mainDir.FullName, "change.txt"));
                FileInfo requirementFile = new FileInfo(Path.Combine(mainDir.FullName, "requirements.txt"));
                FileInfo textFile = new FileInfo(Path.Combine(mainDir.FullName, "text.txt"));

                IScript<bool> change = null;
                IScript<bool> requirements = null;

                if (!mainDir.Exists)
                    throw new Exception("choice doesn't exist");
                if (!textFile.Exists)
                    throw new Exception("no text file found");

                if (changeFile.Exists)
                {
                    string script_id = File.ReadAllText(changeFile.FullName);
                    change = scriptBoolLoader.LoadScript(script_id);
                }
                if (requirementFile.Exists)
                {
                    string script_id = File.ReadAllText(requirementFile.FullName);
                    requirements = scriptBoolLoader.LoadScript(script_id);
                }
                string text = File.ReadAllText(textFile.FullName);
                Screen screen = new Screen(text);
                return new Choice(screen, change, requirements);
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Couldn't load the choice {0}.{1}", node_id, choice_id), e);
            }
        }

        

        public IList<Choice> LoadChoices(string node_id)
        {
            try
            {
                List<Choice> choices = new List<Choice>();
                foreach (string choice in GetChoices(node_id))
                    choices.Add(LoadChoice(node_id, choice));
                return choices;
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Couldn't load the choices for {0}", node_id), e);
            }
        }
    }
}
