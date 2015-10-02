# gistify
A Roslyn experiment.
A hassle free tool that intelligently touches-up and uploads your code snippets.
Selected snippet of code is enhanced with information about the types and origin of objects used within.

### Example

Input:

Output:


### Contribute

Gistify is using some elements of the Roslyn compiler, namely the `SemanticModel` and `CSharpSyntaxWalker` in class `CodeConnect.Gistify.Engine.DiscoveryWalker`. 

To get started with Roslyn, see the [Learn Roslyn Now](https://joshvarty.wordpress.com/learn-roslyn-now/) tutorial series.
Gistify has some rough edges and room for improvement, feel free to contribute!
