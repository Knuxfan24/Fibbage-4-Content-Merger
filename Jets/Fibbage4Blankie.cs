namespace Fibbage4ContentMerger
{
    internal class Fibbage4Blankie
    {
        // Allow single line file loading.
        public Fibbage4Blankie() { }

        public Fibbage4Blankie(string file) => Deseralise(file);

        /// <summary>
        /// Content header.
        /// </summary>
        public class FormatData
        {
            [JsonProperty(Order = 1, PropertyName = "content")]
            public List<QuestionEntry> Content { get; set; } = new();
        }

        public class QuestionEntry
        {
            [JsonProperty(Order = 1, PropertyName = "alternateSpellings")]
            public string[] AlternateSpellings { get; set; } = Array.Empty<string>();

            [JsonProperty(Order = 2, PropertyName = "bumper")]
            public string Bumper { get; set; } = "None";

            [JsonProperty(Order = 3, PropertyName = "category")]
            public string Category { get; set; } = "";

            [JsonProperty(Order = 4, PropertyName = "correctText")]
            public string CorrectText { get; set; } = "";

            [JsonProperty(Order = 5, PropertyName = "extraCategories")]
            public string[] ExtraCategories { get; set; } = Array.Empty<string>();

            [JsonProperty(Order = 6, PropertyName = "id")]
            public string ID { get; set; } = "24242";

            [JsonProperty(Order = 7, PropertyName = "isValid")]
            public string IsValid { get; set; } = "";

            [JsonProperty(Order = 8, PropertyName = "personal")]
            public string PersonalQuestion { get; set; } = "This is the question the player sees.";

            [JsonProperty(Order = 9, PropertyName = "portrait")]
            public bool Portrait { get; set; } = false;

            [JsonProperty(Order = 10, PropertyName = "questionText")]
            public string Question { get; set; } = "This is the question the other players see.";

            [JsonProperty(Order = 11, PropertyName = "suggestions")]
            public string[] Suggestions { get; set; } = Array.Empty<string>();

            [JsonProperty(Order = 12, PropertyName = "us")]
            public bool USCentric { get; set; } = false;

            [JsonProperty(Order = 13, PropertyName = "x")]
            public bool Explicit { get; set; } = false;
        }

        // Basic setup.
        public FormatData Data = new();

        public void Deseralise(string file)
        {
            var origData = JsonConvert.DeserializeObject<FormatData>(File.ReadAllText(file));

            foreach (var prompt in origData.Content)
                Data.Content.Add(prompt);
        }
    }
}
