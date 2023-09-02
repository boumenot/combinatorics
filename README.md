# FORK

This is a fork of [eoincampbell/combinatorics](https://github.com/eoincampbell/combinatorics).  My changes are purely performance focused. My fork sacrifices the project in the name of performance and *potentially* some performance.  The original repo is probably the better choice.

In brief, the changes are:

1. Upgrade to VS 2022.
1. Support .NET 7 only.
1. Support WithoutRepetition **only**
1. Use array instead of (readonly) list.
1. Avoid allocations as much as possible.
1. Little more unit test coverage to ensure I did not regress anything.

Performance improved by (unscientifically) 3 to 5x.

# ORIGINAL

[![Build status](https://ci.appveyor.com/api/projects/status/tr38vj9ebhokwsyi?svg=true)](https://ci.appveyor.com/project/eoincampbell/combinatorics)

This project contains the Combinatorics Implementations of Adrian Akison, taken from his excellent CodeProject Article
http://www.codeproject.com/Articles/26050/Permutations-Combinations-and-Variations-using-C-G


"Combinatorics has many applications within computer science for solving complex problems. 
However, it is under-represented in libraries since there is little application of Combinatorics in business applications.
Fortunately, the science behind it has been studied by mathematicians for centuries, and is well understood and well documented. 
However, mathematicians are focused on how many elements will exist within a Combinatorics problem, and have little interest in actually going through the work of creating those lists.  Enter computer science to actually construct these massive collections."

You can [install the package from Nuget](https://www.nuget.org/packages/Nito.Combinatorics/)

> PM> Install-Package Combinatorics
