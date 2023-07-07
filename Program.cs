global using Newtonsoft.Json;
using Fibbage4ContentMerger.Jets;

namespace Fibbage4ContentMerger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:\n" +
                                  "Fibbage4ContentMerger.exe {path to Fibbage 3's content folder (fig 1)} {path to Fibbage 4's content\\en folder (fig 2)} [-merge]}\n" +
                                  "fig 1: \"D:\\Steam\\steamapps\\common\\The Jackbox Party Pack 4\\games\\Fibbage3\\content\"\n" +
                                  "fig 2: \"D:\\Steam\\steamapps\\common\\The Jackbox Party Pack 9\\games\\Fibbage4\\content\\en\"\n\n" +
                                  "To include the original Fibbage 3 questions, then add the -merge argument after the directories:\n\n" +
                                  "Example:\n" +
                                  "Fibbage4ContentMerger.exe \"D:\\Steam\\steamapps\\common\\The Jackbox Party Pack 4\\games\\Fibbage3\\content\" \"D:\\Steam\\steamapps\\common\\The Jackbox Party Pack 9\\games\\Fibbage4\\content\\en\" -merge\n\n" +
                                  "Press any key to exit.");
                Console.ReadKey();
                return;
            }

            // Set up the directories for each game.
            string f3Directory = args[0];
            string f4Directory = args[1];

            // Load the Fibbage 4 Enough About You Blankie file.
            Console.WriteLine("Loading Fibbage 4's Blankie file.");
            Fibbage4Blankie f4 = new($@"{f4Directory}\eayblankie.jet");

            // Set up a Fibbage 3 Enough About You Shortie file.
            Fibbage3Shortie f3 = new();

            // Set up the merge bool.
            bool merge = false;

            // Check if the user has specified the merge argument.
            if (args.Length > 2)
                if (args[2] == "-merge")
                    merge = true;

            // If we're merging, then load the Fibbage 3 Enough About You Shortie file.
            if (merge)
            {
                Console.WriteLine("Loading Fibbage 3's Shortie file for merging.");
                f3 = new($@"{f3Directory}\tmishortie.jet");
            }

            // Loop through each Fibbage 4 question and convert them to the Fibbage 3 style.
            foreach (Fibbage4Blankie.QuestionEntry f4Question in f4.Data.Content)
            {
                Console.WriteLine($"Converting question with ID {f4Question.ID}.");
                f3.Data.Content.Add(new()
                {
                    Explicit = f4Question.Explicit,
                    PersonalQuestion = f4Question.PersonalQuestion.Replace("{{BLANK}}", "<BLANK>"),
                    ID = int.Parse(f4Question.ID),
                    Portrait = f4Question.Portrait,
                    Category = f4Question.Category,
                    Bumper = f4Question.Bumper,
                    USCentric = f4Question.USCentric
                });
            }

            // Backup the original file if a backup doesn't already exist.
            if (!File.Exists($@"{f3Directory}\tmishortie.orig"))
            {
                Console.WriteLine("Backing up Fibbage 3's original Shortie file.");
                File.Move($@"{f3Directory}\tmishortie.jet", $@"{f3Directory}\tmishortie.orig");
            }

            // Write the new Fibbage 3 Enough About You Shortie file.
            Console.WriteLine("Saving new Fibbage 3 Shortie file.");
            File.WriteAllText($@"{f3Directory}\tmishortie.jet", JsonConvert.SerializeObject(f3.Data, Formatting.Indented));

            // Loop through each Fibbage 4 Question's data.
            foreach (string f4Question in Directory.GetDirectories($@"{f4Directory}\eayblankie"))
            {
                // Get the ID of this question.
                string directory = f4Question[(f4Question.LastIndexOf('\\') + 1)..];

                Console.WriteLine($"Converting data for question with ID {directory}.");

                // Create this question's directory in Fibbage 3.
                Directory.CreateDirectory($@"{f3Directory}\tmishortie\{directory}");

                // Copy the ogg files for this question to Fibbage 3.
                foreach (string oggFile in Directory.GetFiles(f4Question, "*.ogg"))
                    File.Copy(oggFile, $@"{f3Directory}\tmishortie\{directory}\{Path.GetFileName(oggFile)}", true);

                // Read this question's data.
                FibbageQuestionData f4QuestionData = new($@"{f4Question}\data.jet");

                // Find the tags that Fibbage 3 uses that also exist in Fibbage 4.
                var hasBumperAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "HasBumperAudio");
                var hasKeywordAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "HasKeywordAudio");
                var hasCorrectAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "HasCorrectAudio");
                var hasQuestionAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "HasQuestionAudio");
                var bumperAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "BumperAudio");
                var pic = f4QuestionData.Data.Fields.Find(x => x.Name == "Pic");
                var correctAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "CorrectAudio");
                var questionAudio = f4QuestionData.Data.Fields.Find(x => x.Name == "QuestionAudio");

                // Find this question in Fibbage 4's Blankie file.
                var f4AlternateQuestionData = f4.Data.Content.Find(x => x.ID == directory);

                // Set up a question for Fibbage 3.
                FibbageQuestionData f3QuestionData = new();

                // Add all the needed fields to the Fibbage 3 question.
                f3QuestionData.Data.Fields.Add(hasBumperAudio);
                f3QuestionData.Data.Fields.Add(hasKeywordAudio);
                f3QuestionData.Data.Fields.Add(AddField("B", "false", "HasBumperType", null));
                f3QuestionData.Data.Fields.Add(hasCorrectAudio);
                f3QuestionData.Data.Fields.Add(hasQuestionAudio);
                f3QuestionData.Data.Fields.Add(AddField("S", string.Join(',', f4AlternateQuestionData.Suggestions), "Suggestions", null));
                f3QuestionData.Data.Fields.Add(AddField("S", f4AlternateQuestionData.PersonalQuestion.Replace("{{BLANK}}", "<BLANK>").Replace("{{PLAYER}}", "<PLAYER>"), "PersonalQuestionText", null));
                f3QuestionData.Data.Fields.Add(AddField("S", f4AlternateQuestionData.Category, "Category", null));
                f3QuestionData.Data.Fields.Add(AddField("S", f4AlternateQuestionData.CorrectText, "CorrectText", null));
                f3QuestionData.Data.Fields.Add(AddField("S", f4AlternateQuestionData.Bumper, "BumperType", null));
                f3QuestionData.Data.Fields.Add(AddField("S", f4AlternateQuestionData.Question.Replace("{{BLANK}}", "<BLANK>").Replace("{{PLAYER}}", "<PLAYER>"), "QuestionText", null));
                f3QuestionData.Data.Fields.Add(AddField("S", "", "SocialMediaDate", null));
                f3QuestionData.Data.Fields.Add(AddField("S", "", "KeywordResponse", null));
                f3QuestionData.Data.Fields.Add(AddField("S", "", "SocialMediaName", null));
                f3QuestionData.Data.Fields.Add(AddField("S", "", "AlternateSpellings", null));
                f3QuestionData.Data.Fields.Add(AddField("A", null, "KeywordResponseAudio", "[category=host]"));
                f3QuestionData.Data.Fields.Add(bumperAudio);
                f3QuestionData.Data.Fields.Add(pic);
                f3QuestionData.Data.Fields.Add(correctAudio);
                f3QuestionData.Data.Fields.Add(questionAudio);

                // Write this question's data.
                File.WriteAllText($@"{f3Directory}\tmishortie\{directory}\data.jet", JsonConvert.SerializeObject(f3QuestionData.Data, Formatting.Indented));
            }

            Console.WriteLine("\nDone!\n" +
                              "Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds a field to a Fibbage Question Data.
        /// </summary>
        private static FibbageQuestionData.FieldData AddField(string tag, string? value, string name, string? subtitle)
        {
            FibbageQuestionData.FieldData data = new()
            {
                Tag = tag,
                Value = value,
                Name = name,
                Subtitle = subtitle
            };
            return data;
        }
    }
}