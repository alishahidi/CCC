namespace Core;

public class Class1
{

    public static void Main(string[] args)
    {
        var compiler = new ExpressionCompiler();

// Example 1: Simple comparison
        var result1 = compiler.CompileToCSharp("OrderAmount > 500");
// Output: "OrderAmount > 500"

// Example 2: String comparison
        var result2 = compiler.CompileToCSharp("level == \"Gold\"");
// Output: "level == \"Gold\""

// Example 3: Complex logical expression
        var result3 = compiler.CompileToCSharp("(Age >= 18 && Status == \"Active\") || IsAdmin");
// Output: "(Age >= 18 && Status == \"Active\") || IsAdmin"

// Example 4: With boolean literals
        var result4 = compiler.CompileToCSharp("IsValid == true && HasLicense != false");
// Output: "IsValid == true && HasLicense != false"

        System.Console.WriteLine(result1);
        System.Console.WriteLine(result2);
        System.Console.WriteLine(result3);
        System.Console.WriteLine(result4);

        Console.ReadKey();
    }
}