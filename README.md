# gistify
A Roslyn experiment.

A hassle free tool that intelligently touches-up and uploads your code snippets.

Selected snippet of code is enhanced with information about the types and origin of objects used within.

Read the **[Getting Started guide](https://github.com/CodeConnect/gistify/wiki)**

### Example

Input:

![image](https://cloud.githubusercontent.com/assets/1673956/10238743/b03d8254-6878-11e5-8f9a-bf4f465b928f.png)

Output:

![image](https://cloud.githubusercontent.com/assets/1673956/10238742/ac9f2f62-6878-11e5-9444-d77e0716b057.png)

### Contribute

Gistify is using some elements of the Roslyn compiler, namely the `SemanticModel` and `CSharpSyntaxWalker` in class [CodeConnect.Gistify.Engine.DiscoveryWalker](https://github.com/CodeConnect/gistify/blob/master/CodeConnect.Gistify.Engine/DiscoveryWalker.cs). 

To get started with Roslyn, see the [Learn Roslyn Now](https://joshvarty.wordpress.com/learn-roslyn-now/) tutorial series.

Gistify has some rough edges and lots of room for improvement, feel free to contribute!
