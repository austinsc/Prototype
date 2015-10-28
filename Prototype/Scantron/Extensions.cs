// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    Extensions.cs
// Modified On: 10/28/2015 9:41 AM
// Modified By: Austin, Stephen (saustin)

using System;
using System.Collections.Generic;

namespace Prototype.Scantron
{
    internal static class Extensions
    {
        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source,
                                                           int chunkSize)
        {
            // Validate parameters.
            if(source == null) throw new ArgumentNullException(nameof(source));
            if(chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "The chunkSize parameter must be a positive value.");

            // Call the internal implementation.
            return source.ChunkInternal(chunkSize);
        }

        private static IEnumerable<IEnumerable<T>> ChunkInternal<T>(this IEnumerable<T> source, int chunkSize)
        {
            // Get the enumerator.  Dispose of when done.
            using(var enumerator = source.GetEnumerator())
                do
                {
                    // Move to the next element.  If there's nothing left
                    // then get out.
                    if(!enumerator.MoveNext()) yield break;

                    // Return the chunked sequence.
                    yield return ChunkSequence(enumerator, chunkSize);
                }
                while(true);
        }

        private static IEnumerable<T> ChunkSequence<T>(IEnumerator<T> enumerator, int chunkSize)
        {
            // The count.
            var count = 0;

            // There is at least one item.  Yield and then continue.
            do
            {
                // Yield the item.
                yield return enumerator.Current;
            }
            while(++count < chunkSize && enumerator.MoveNext());
        }
    }
}