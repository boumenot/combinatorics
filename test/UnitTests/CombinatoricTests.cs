using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using Combinatorics.Collections;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Tests Cases &amp; Examples for Combinations, Permutations &amp; Variations with & without repetition in the output sets.
    /// </summary>
    public class CombinatoricTests
    {
        private static string ToHexString(byte[] bs)
        {
            return string.Join(string.Empty, bs.Select(x => x.ToString("x2")));
        }

        [Fact]
        public void Performance()
        {
            var integers = Enumerable.Range(0, 30).ToList();
            var c = new Combinations<int>(integers, 5);

            Assert.True(c.All(x => x.ToArray().Length > 0));
        }

        [Theory]
        [InlineData(3, 2, "af769638713fb50c0ac50b98b17e9f1091531fa9")]
        [InlineData(3, 3, "4ada414edc3ad469e73520dac1f4e9beff703796")]
        [InlineData(6, 1, "7c68f6ce264a9eb359c5d83bab71a1390b2ac6ee")]
        [InlineData(6, 2, "f0c4188916d55195315895801fe596e9c2b349c7")]
        [InlineData(6, 3, "fbac2bc02c4c9598123500def57354e57e624686")]
        [InlineData(6, 4, "729826ddf7b501245a8b2cadd37e12e433af01f7")]
        [InlineData(6, 5, "92e9fc64fe4fc54f52a8e34055a31410d97a6ac8")]
        [InlineData(6, 6, "7c68f6ce264a9eb359c5d83bab71a1390b2ac6ee")]
        [InlineData(10, 3, "d974e1834f959a29a7c9f83fba2f6da5eb765aa6")]
        [InlineData(20, 5, "def651f69c3dcb1578b231b87c3d31711e85a117")]
        public void Combination_Sha1(int count, int size, string expectedChecksum)
        {
            var byteCount = size * sizeof(int);
            int total = 0;
            var integers = Enumerable.Range(1, count).ToArray();
            var testSubject = new Combinations<int>(integers, size);

            using (var sha = SHA1.Create())
            {
                foreach (var comb in testSubject)
                {
                    ++total;
                    sha.TransformBlock(comb.SelectMany(BitConverter.GetBytes).ToArray(), 0, byteCount, null, 0);
                }

                sha.TransformFinalBlock(new byte[] { 0xbe, 0xef }, 0, 2);
                var hash = ToHexString(sha.Hash);

                Assert.Equal(expectedChecksum, hash);
                Assert.Equal(total, testSubject.Count);
            }
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
