using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace CodeBase.SpawnPoints
{
    public class EnumerableRandomizer
    {
        public IEnumerable<T> Randomize<T>(IEnumerable<T> target)
        {
            var sourceArray = target as T[] ?? target.ToArray();
            for (var i = 0; i < sourceArray.Length; i++)
            {
                int swapIndex = GetRandomIndex(sourceArray.Length);
                SwapElements(i, swapIndex, ref sourceArray);
            }
            return sourceArray;
        }

        private int GetRandomIndex(int collectionLength)
            => new Random().Next(0, collectionLength);

        private void SwapElements<T>(int index1, int index2, ref T[] array)
            => (array[index1], array[index2]) = (array[index2], array[index1]);
    }
}