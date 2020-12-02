using System;
using System.Text.RegularExpressions;
using System.Linq;

public enum PasswordPolicy
{
    OldCompany,
    Toboggan,
}

public record PasswordRecord
{
    private static Regex regex = new Regex("(?<min>(\\d){1,2})-(?<max>(\\d){1,2})\\s(?<char>\\w):\\s(?<password>\\w+)");

    private PasswordPolicy policy;

    public PasswordRecord(string line, PasswordPolicy policy)
    {
        Min = Convert.ToInt32(regex.Matches(line)[0].Groups["min"].Value);
        Max = Convert.ToInt32(regex.Matches(line)[0].Groups["max"].Value);
        Character = Convert.ToChar(regex.Matches(line)[0].Groups["char"].Value);
        Password = regex.Matches(line)[0].Groups["password"].Value;
        this.policy = policy;
    }

    public char Character { get; }
    public int Min { get; }
    public int Max { get; }
    public string Password { get; }

    private bool Version1Validation()
    {
        var count = Password.Where(_ => _ == Character).Count();
        return (count >= Min && count <= Max);
    }

    private bool Version2Validation() {
        return Password.SafeCheck(Min, Character) ^ Password.SafeCheck(Max, Character);
    }

    public bool Valid()
    {
        switch (policy)
        {
            case PasswordPolicy.OldCompany:
                return Version1Validation();

            case PasswordPolicy.Toboggan:
                return Version2Validation();
        }

        throw new Exception("Literally impossible");
    }
}

public static class StringExtensions {
    public static bool SafeCheck(this String str, int index, char compare) {
        var modifiedIndex = index - 1;
        if (str.Length < modifiedIndex) {
            return false;
        }

        var charAtIndex = str[modifiedIndex];
        return charAtIndex == compare;
    }
}