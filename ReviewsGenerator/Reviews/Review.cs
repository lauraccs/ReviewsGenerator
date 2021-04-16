using System;
using System.ComponentModel;
using Newtonsoft.Json;
namespace ReviewsGenerator.Reviews
{
    public class Review
    {
        [JsonProperty]
        [DisplayName("Amazon Product ID")]
        public string Asin { get; set; }
        [JsonProperty]
        public string ReviewerName { get; set; }
        [JsonProperty]
        public int[] Helpful { get; set; }
        public string HelpfulRatio { get; set; }
        [JsonProperty]
        public string ReviewText { get; set; }
        [JsonProperty]
        [DisplayName("Star Rating")]
        public double Overall { get; set; }
        [JsonProperty]
        public string Summary { get; set; }
        [JsonProperty]
        public string ReviewTime { get; set; }




    }
}
