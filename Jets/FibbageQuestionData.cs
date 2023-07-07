namespace Fibbage4ContentMerger.Jets
{
    internal class FibbageQuestionData
    {
        // Allow single line file loading.
        public FibbageQuestionData() { }

        public FibbageQuestionData(string file) => Deseralise(file);

        /// <summary>
        /// Content header.
        /// </summary>
        public class FormatData
        {
            [JsonProperty(Order = 1, PropertyName = "fields")]
            public List<FieldData> Fields { get; set; } = new();
        }

        public class FieldData
        {
            [JsonProperty(Order = 1, PropertyName = "t")]
            public string Tag { get; set; } = "";

            [JsonProperty(Order = 2, PropertyName = "v")]
            public string? Value { get; set; }

            [JsonProperty(Order = 3, PropertyName = "n")]
            public string Name { get; set; } = "";

            [JsonProperty(Order = 4, PropertyName = "s")]
            public string? Subtitle { get; set; }

            public override string ToString() => Name;
        }

        // Basic setup.
        public FormatData Data = new();

        public void Deseralise(string file)
        {
            var origData = JsonConvert.DeserializeObject<FormatData>(File.ReadAllText(file));

            foreach (var prompt in origData.Fields)
                Data.Fields.Add(prompt);
        }
    }
}
