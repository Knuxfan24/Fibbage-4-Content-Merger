# Fibbage4ContentMerger
A command line tool to merge the questions from the Fibbage 4 Enough About You gamemode into Fibbage 3 Enough About You. Made purely because we were disappointed with Fibbage 4's EaY lacking a final round and just abruptly ending.

# Building
To build this project, simply clone the repository and use Visual Studio to compile it.

# Usage
As this is a command line tool, a command prompt needs to be opened and used to run the application. Two arguments are required with a third optional one. These are:

The path to your Fibbage 3 Content folder. For me, this path is `D:\Steam\steamapps\common\The Jackbox Party Pack 4\games\Fibbage3\content`

The path to your Fibbage 4 Content folder. For me, this path is `D:\Steam\steamapps\common\The Jackbox Party Pack 9\games\Fibbage4\content\en` (the language tag may be interchangable, but I have only tested English.)

And the optional third argument is `-merge`, which preserves the original Fibbage 3 questions rather than overwriting them.

***

So for me to merge the Fibbage 4 Enough About You questions with the original Fibbage 3 Enough About You questions, I would run `Fibbage4ContentMerger.exe "D:\Steam\steamapps\common\The Jackbox Party Pack 4\games\Fibbage3\content" "D:\Steam\steamapps\common\The Jackbox Party Pack 9\games\Fibbage4\content\en" -merge`