using Combinatorics.Collections;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Tests Cases &amp; Examples for Combinations, Permutations &amp; Variations with & without repetition in the output sets.
    /// </summary>
    public class CombinatoricTests
    {
        [Fact]
        public void Performance()
        {
            var integers = Enumerable.Range(0, 30).ToList();
            var c = new Combinations<int>(integers, 5);

            Assert.True(c.All(x => x.ToArray().Length > 0));
        }


        /// <summary>
        /// Standard permutations simply provide every single ordering of the input set.
        /// Permutations of {A B C}: {A B C}, {A C B}, {B A C}, {B C A}, {C A B}, {C B A}
        /// The number of Permutations can be easily shown to be P(n) = n!, where n is the number of items. 
        /// In the above example, the input set contains 3 items, and the size is 3! = 6. 
        /// This means that the number of permutations grows exponentially with n. 
        /// Even a small n can create massive numbers of Permutations; for example, the number of ways to randomly 
        /// shuffle a deck of cards is 52! or approximately 8.1E67.
        /// </summary>
        [Fact]
        public void Generate_Permutations_Without_Repetition_On_3_Unique_Input_Items_Should_Create_6_Output_Permutations()
        {
            var integers = new List<int> { 1, 2, 3 };

            var p = new Permutations<int>(integers);

            foreach (var v in p)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(6, p.Count); 
        }

        /// <summary>
        /// Combinations are subsets of a given size taken from a given input set. 
        /// The size of the set is known as the Upper Index (n) and the size of the subset is known as the Lower Index (k). 
        /// When counting the number of combinations, the terminology is generally "n choose k", 
        /// and is known as the Binomial Coefficient [3]. Unlike permutations, combinations do not have any order 
        /// in the output set. Combinations without Repetition are would be similar to drawing balls from a lottery drum.
        /// Each ball can only be drawn once but the order they are drawn in is unimportant.
        /// </summary>
        [Fact]
        public void Generate_Combinations_of_3_Without_Repetition_On_6_Input_Items_Should_Create_20_Output_Items()
        {
            var integers = new List<int> {1, 2, 3, 4, 5, 6};

            var c = new Combinations<int>(integers, 3);

            foreach (var v in c)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", v));
            }

            Assert.Equal(20, c.Count);
        }

        /// <summary>
        /// Variations combine features of combinations and permutations, they are the set of all ordered 
        /// combinations of items to make up a subset. Like combinations, the size of the set is known as 
        /// the Upper Index (n) and the size of the subset is known as the Lower Index (k). And, the 
        /// generation of variations can be based on the repeating of output items. These are called Variations.
        /// 
        /// Variations are permutations of combinations. That is, a variation of a set of n items choose k, 
        /// is the ordered subsets of size k. For example:
        /// Variations of {A B C} choose 2: {A B}, {A C}, {B A}, {B C}, {C A}, {C B}
        /// The number of outputs in this particular example is similar to the number of combinations of 
        /// n choose k divided by the permutations of k. 
        /// 
        /// It can be calculated as V(n, k) = C(n, k) * P(k) = (n! / ( k! * (n - k)! )) * k! = n! / (n - k)!. 
        /// </summary>
        [Fact]
        public void Generate_Variations_of_3_Without_Repetition_On_6_Input_Items_Should_Create_120_Output_Items()
        {
            var integers = new List<int> {1, 2, 3, 4, 5, 6};

            var v = new Variations<int>(integers, 3);

            foreach (var vv in v)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", vv));
            }

            Assert.Equal(120, v.Count);
        }
    }
}
