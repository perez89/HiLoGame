//namespace HiLoClient.ConsoleCmds;

//public class ConsoleInteraction
//{
//    bool Check(int numbevr) {
//        if (numbevr > 10)
//            return true;
//        return false;
//    }
//    public void Test() {

//        var x = Read<int, int>("write one number", "this number does not seem valid", new int[] { 1, 2, 3, 4 }, Check);


//        var y = Read<int, int>("write one number", "this number does not seem valid", new int[] { 1, 2, 3, 4 }, (number) => (number > 10));


//        var y = Read<string, bool>("write one number", "this number does not seem valid", new string[] { "yes", "y", "no", "n" }, (val) => (val.Equals("yes") || val.Equals("y") return true));

//    }

//    public R Read<T, R>(string initialMessage, string retryMessage, T[] validRps, Func<T, bool> myMethodName)
//    {
//        Console.WriteLine("Write your nickname: ");

//        var input = Console.ReadLine();

//        if (!string.IsNullOrEmpty(input))
//        {
//            input = input.Trim();

//            var validInput = false;
//            if (validRps != null && validRps.Any())
//            {
//                if (!validRps.Select(t => (t + "").ToLowerInvariant()).Contains(input.ToLowerInvariant()))
//                {
//                    validInput = true;
//                }
//            }
//            else {
//                validInput = true;
//            }

//            if (validInput)
//            {
//                T x = GetValue<T>(input);

//                if (myMethodName(x))
//                {
//                    return x;
//                }
//            }         
//        };

//        Console.WriteLine("");
//        Console.WriteLine("Please, write a nickname with at least 3 characters");

//        return Read(initialMessage, retryMessage, myMethodName);
//    }

//    public static T GetValue<T>(String value)
//    {
//        return (T)Convert.ChangeType(value, typeof(T));
//    }
//}
