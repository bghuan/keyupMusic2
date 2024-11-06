class Program
{
    static void Main()
    {
        KeyPress(new string[] { "a", "s", "d" });
        Console.ReadLine();
    }
    public static void KeyPress(string[] key)
    {
        var inputs = new string[key.Length * 2];

        for (int i = 0; i < key.Length; i++)
        {
            inputs[i] = key[i];
            Console.WriteLine(inputs[i]);
        }
        for (int i = 0; i < key.Length; i++)
        {
            inputs[i + key.Length] = key[key.Length - 1 - i];
            Console.WriteLine(inputs[i + key.Length]);
        }
        Console.WriteLine(inputs);
    }
}