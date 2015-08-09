# gistify
Hassle free tool that intelligently touches-up and uploads your code snippets

### Goal:

To enhance a snippet of code with information about the types and origin of objects used within.

### Method of operation:

- Take all objects from the snippet
- Check where object was declared
- Get object's namespace
- Create syntax for using the assembly
- If object was declared outside:
-- Get object's type
-- Create syntax that copies the declaration (type)
- Put together all syntices
- Upload combined code to github

### Required elements:

- A walker that will provide declaration syntax
- A model that will provide type and namespace information based on syntax
- Get object's namespace
- Get object's type
- Create syntax for "using"
- Create syntax for declaration
- Upload to github