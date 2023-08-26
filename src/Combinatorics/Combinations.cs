using System.Collections;
using System.Numerics;

namespace Combinatorics.Collections
{
    /// <summary>
    /// Combinations defines a sequence of all possible subsets of a particular size from the set of values.
    /// Within the returned set, there is no prescribed order.
    /// This follows the mathematical concept of choose.
    /// For example, put <c>10</c> dominoes in a hat and pick <c>5</c>.
    /// The number of possible combinations is defined as "10 choose 5", which is calculated as <c>(10!) / ((10 - 5)! * 5!)</c>.
    /// </summary>
    /// <remarks>
    /// When given a input collect {A B C} and lower index of 2, the following sets are generated:
    /// MetaCollectionType.WithoutRepetition =>
    /// {A B}, {A C}, {B C}
    /// 
    /// Input sets with multiple equal values will generate redundant combinations in proportion
    /// to the likelihood of outcome.  For example, {A A B B} and a lower index of 3 will generate:
    /// {A A B} {A A B} {A B B} {A B B}
    /// </remarks>
    /// <typeparam name="T">The type of the values within the list.</typeparam>
    public sealed class Combinations<T> : IEnumerable<T[]>
    {
        public Combinations(IEnumerable<T> values, int lowerIndex)
            : this(values.ToArray(), lowerIndex)
        {
        }

        /// <summary>
        /// Create a combination set from the provided list of values.
        /// The upper index is calculated as values.Count, the lower index is specified.
        /// </summary>
        /// <param name="values">List of values to select combinations from.</param>
        /// <param name="lowerIndex">The size of each combination set to return.</param>
        public Combinations(T[] values, int lowerIndex)
        {
            // Copy the array and parameters and then create a map of booleans that will 
            // be used by a permutations object to reference the subset.
            // 
            // A map of upper index elements is created with lower index false's.  
            // E.g. 8 choose 3 generates:
            // Map: {1 1 1 1 1 0 0 0}
            // Note: For sorting reasons, false denotes inclusion in output.

            this.LowerIndex = lowerIndex;
            this._myValues = values;
            var myMap = new List<bool>(this._myValues.Length);
            myMap.AddRange(_myValues.Select((t, i) => i < this._myValues.Length - LowerIndex));

            _myPermutations = new Permutations<bool>(myMap);
        }

        /// <summary>
        /// Gets an enumerator for collecting the list of combinations.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T[]> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// The enumerator that enumerates each meta-collection of the enclosing Combinations class.
        /// </summary>
        public sealed class Enumerator : IEnumerator<T[]>
        {
            /// <summary>
            /// Construct a enumerator with the parent object.
            /// </summary>
            /// <param name="source">The source combinations object.</param>
            public Enumerator(Combinations<T> source)
            {
                _myParent = source;
                _myPermutationsEnumerator = (Permutations<bool>.Enumerator)_myParent._myPermutations.GetEnumerator();
                this._myCurrentIndex = -1;
                this._myCurrentList = new T[this._myParent.LowerIndex]; 
            }

            void IEnumerator.Reset() => throw new NotSupportedException();

            /// <summary>
            /// Advances to the next combination of items from the set.
            /// </summary>
            /// <returns>True if successfully moved to next combination, False if no more unique combinations exist.</returns>
            /// <remarks>
            /// The heavy lifting is done by the permutations object, the combination is generated
            /// by creating a new list of those items that have a true in the permutation parallel array.
            /// </remarks>
            public bool MoveNext()
            {
                var ret = _myPermutationsEnumerator.MoveNext();
                this._myCurrentIndex = -1;
                return ret;
            }

            /// <summary>
            /// The current combination
            /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
            public T[] Current
#pragma warning restore CA1819 // Properties should not return arrays
            {
                get
                {
                    ComputeCurrent();
                    return _myCurrentList!;
                }
            }

            object IEnumerator.Current => Current;

            /// <inheritdoc />
            public void Dispose() => _myPermutationsEnumerator.Dispose();

            /// <summary>
            /// The only complex function of this entire wrapper, ComputeCurrent() creates
            /// a list of original values from the bool permutation provided.  
            /// The exception for accessing current (InvalidOperationException) is generated
            /// by the call to .Current on the underlying enumeration.
            /// </summary>
            /// <remarks>
            /// To compute the current list of values, the underlying permutation object
            /// which moves with this enumerator, is scanned differently based on the type.
            /// The items have only two values, true and false, which have different meanings:
            /// 
            /// The output is a straightforward subset of the input array.  
            /// E.g. 6 choose 3 without repetition
            /// Input array:   {A B C D E F}
            /// Permutations:  {0 1 0 0 1 1}
            /// Generates set: {A   C D    }
            /// Note: size of permutation is equal to upper index.
            /// </remarks>
            private void ComputeCurrent()
            {
                if (this._myCurrentIndex >= 0)
                    return;

                var index = 0;
                this._myCurrentIndex = 0;

                var currentPermutation = _myPermutationsEnumerator.Current;
                foreach (var p in currentPermutation)
                {
                    if (!p)
                    {
                        _myCurrentList[this._myCurrentIndex] = _myParent._myValues[index];
                        ++index;
                        ++this._myCurrentIndex;
                    }
                    else
                    {
                        ++index;
                    }
                }
            }

            /// <summary>
            /// Parent object this is an enumerator for.
            /// </summary>
            private Combinations<T> _myParent;

            private int _myCurrentIndex;

            /// <summary>
            /// The current list of values, this is lazy evaluated by the Current property.
            /// </summary>
            private T[] _myCurrentList;

            /// <summary>
            /// An enumerator of the parents list of lexicographic orderings.
            /// </summary>
            private readonly Permutations<bool>.Enumerator _myPermutationsEnumerator;
        }

        /// <summary>
        /// The number of unique combinations that are defined in this meta-collection.
        /// This value is mathematically defined as Choose(M, N) where M is the set size
        /// and N is the subset size.  This is M! / (N! * (M-N)!).
        /// </summary>
        public BigInteger Count => _myPermutations.Count;

        /// <summary>
        /// The upper index of the meta-collection, equal to the number of items in the initial set.
        /// </summary>
        public int UpperIndex => _myValues.Length;

        /// <summary>
        /// The lower index of the meta-collection, equal to the number of items returned each iteration.
        /// </summary>
        public int LowerIndex { get; }

        /// <summary>
        /// Copy of values object is initialized with, required for enumerator reset.
        /// </summary>
        private T[] _myValues;

        /// <summary>
        /// Permutations object that handles permutations on booleans for combination inclusion.
        /// </summary>
        private Permutations<bool> _myPermutations;
    }
}
