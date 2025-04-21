namespace Core;

public class Class1
{

    public static void Main(string[] args)
    {
        var compiler = new ExpressionCompiler();

        List<string> experssions = new List<string>
        {
            "OrderAmount > 500",
            "level == \"Gold\"", 
            "(Age >= 18 && Status == \"Active\") || IsAdmin",
            "IsValid == true && HasLicense != false",
            "Age > 21",
            "Price <= 100.50",
            "IsActive == true",
            "Name == \"John\"",
            "Department != \"HR\"",
            "Age > 18 && IsStudent",
            "IsPremium || Balance >= 1000",
            "!(IsBlocked)",
            "(Age >= 21 || IsVeteran) && IsVerified",
            "A && (B || C)",
            "((A && B) || C) && D",
            "(CreditScore > 700 && Income >= 50000) || DownPayment >= 0.2",
            "(Status == \"Gold\" && YearsMember > 5) || IsEmployee",
            "42",
            "true",
            "\"hello\"",
            "Age == ",
            "A > > B",
            "MiddleName == null",
            "Result != null && Result > 0",
            "(Total * 1.08) > 100",
            "Quantity * UnitPrice - Discount > 500"
        };

        foreach (var exp in experssions)
        {
            try
            {
                Console.WriteLine("---------------------------------------");
                System.Console.WriteLine(compiler.CompileToCSharp(exp));
                Console.WriteLine("---------------------------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("*************************************************");
                Console.WriteLine("Err: " + exp);
                Console.WriteLine(e);
                Console.WriteLine("*************************************************\n");
            }
        }
        
        Console.ReadKey();
    }
}