using System;
using System.Collections.Generic;
using System.Linq;
using Markov;
using Newtonsoft.Json;
using ReviewsGenerator.Reviews;
using GroupDocs.Classification;

namespace ReviewsGenerator.MarkovBase
{
    public class MarkovBase
    {
        public MarkovBase()
        {

        }
        public MarkovBase(string input)
        {
            Input = input;
        }

        private static MarkovChain<char> ReviewerNameChain;
        private static List<string> AsinList; //Product List
        private static MarkovChain<string> AsinChain;
        private static MarkovChain<string> SummaryTextChain;
        private static MarkovChain<string> ReviewTextChain;
        private static MarkovChain<string> ReviewTimeChain;
        private static List<int[]> HelpfulList = new List<int[]>();
        private static Dictionary<string, string> ReviewerIDs = new Dictionary<string, string>();
        private static string Input;
        private int[] Order = new int[] { 1, 2, 1 };

        private static List<Review> reviewList = new List<Review>();

        public void Learn()
        {
            reviewList = (List<Review>)JsonConvert.DeserializeObject(Input, typeof(List<Review>));

            Random random = new Random();
            ReviewerNameChain = new MarkovChain<char>(Order[random.Next(Order.Length)]);
            AsinList = new List<string>();
            SummaryTextChain = new MarkovChain<string>(Order[random.Next(Order.Length)]);
            ReviewTextChain = new MarkovChain<string>(Order[random.Next(Order.Length)]);
            ReviewTimeChain = new MarkovChain<string>(Order[random.Next(Order.Length)]);
            AsinChain = new MarkovChain<string>(Order[random.Next(Order.Length)]);

            //add each review and its components to corresponding markov chain
            foreach (var review in reviewList)
            {
                if (review.Asin != null && review.ReviewerName != null &&
                    review.ReviewText != null && review.ReviewTime != null && review.Summary != null && review.Helpful != null
                    )
                {
                    string reviewTime = review.ReviewTime;
                    reviewTime = reviewTime.Replace(" ", "/");
                    reviewTime = reviewTime.Replace(",", string.Empty);

                    ReviewerNameChain.Add(review.ReviewerName);
                    AsinList.Add(review.Asin);
                    SummaryTextChain.Add(review.Summary.Split(new char[] { ' ' }));
                    ReviewTextChain.Add(review.ReviewText.Split(new char[] { ' ' }));
                    ReviewTimeChain.Add(reviewTime.Split(new char[] { ' ' }));
                    HelpfulList.Add(review.Helpful);
                    AsinChain.Add(review.Asin.Split(new char[] { ' ' }));//asin = Amazon Standard Identification Number


                }

            }
        }
        public Review GenerateReview()
        {
            Random random = new Random();
            Review review = new Review();
            string reviewerName = new string(ReviewerNameChain.Chain(random).ToArray());


            review.ReviewerName = reviewerName;
            review.Summary = string.Join(" ", SummaryTextChain.Chain(random));
            review.ReviewText = string.Join(" ", ReviewTextChain.Chain(random));

            //analyze sentiment based on summary
            var sentimentClassifier = new SentimentClassifier();
            var sentiment = review.Summary;
            int charCount = sentiment.Count(x => Char.IsLetter(x));
            if (charCount > 100)
            {
                sentiment = sentiment.Substring(0, 100);
            }
            //returns number between 0 and 1
            //generate star rating / overall based on positive probability
            var positiveProbability = sentimentClassifier.PositiveProbability(sentiment);
            review.Overall = Math.Round((positiveProbability * 10) / 2) > 1 ? Math.Round((positiveProbability * 10) / 2) : 1;

            var helpful = HelpfulList.ElementAt(random.Next(HelpfulList.Count));
            review.ReviewTime = string.Join(" ", ReviewTimeChain.Chain(random));
            review.Asin = string.Join(" ", AsinChain.Chain(random));
            review.HelpfulRatio = helpful[0] + " Yes , " + helpful[1] + " No";
            return review;
        }

    }
}
