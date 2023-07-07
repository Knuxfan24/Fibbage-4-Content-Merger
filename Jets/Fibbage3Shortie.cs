namespace Fibbage4ContentMerger
{
    internal class Fibbage3Shortie
    {
        // Allow single line file loading.
        public Fibbage3Shortie() { }

        public Fibbage3Shortie(string file) => Deseralise(file);

        /// <summary>
        /// Content header.
        /// </summary>
        public class FormatData
        {
            [JsonProperty(Order = 1, PropertyName = "episodeid")]
            public int ID { get; set; } = 24242;

            [JsonProperty(Order = 2, PropertyName = "content")]
            public List<QuestionEntry> Content { get; set; } = new();
        }

        public class QuestionEntry
        {
            [JsonProperty(Order = 1, PropertyName = "x")]
            public bool Explicit { get; set; } = false;

            [JsonProperty(Order = 2, PropertyName = "personal")]
            public string PersonalQuestion { get; set; } = "This is the question the player sees.";

            [JsonProperty(Order = 3, PropertyName = "id")]
            public int ID { get; set; } = 24242;

            [JsonProperty(Order = 4, PropertyName = "portrait")]
            public bool Portrait { get; set; } = false;

            [JsonProperty(Order = 5, PropertyName = "category")]
            public string Category { get; set; } = "";

            [JsonProperty(Order = 6, PropertyName = "bumper")]
            public string Bumper { get; set; } = "";

            [JsonProperty(Order = 7, PropertyName = "us")]
            public bool USCentric { get; set; } = false;
        }

        // Basic setup.
        public FormatData Data = new();

        public void Deseralise(string file)
        {
            var origData = JsonConvert.DeserializeObject<FormatData>(File.ReadAllText(file));

            Data.ID = origData.ID;

            foreach (var prompt in origData.Content)
                Data.Content.Add(prompt);
        }
    }
}
