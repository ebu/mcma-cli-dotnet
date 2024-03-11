namespace Mcma.Tools.Cli;

public static class OptionsHelper
{
    public static IEnumerable<(string Name, string Value)> ToNameValuePairs(this IEnumerable<string> optionValues)
        =>
            optionValues
                .Select(optionValue =>
                {
                    var nameAndValue = optionValue.Split(":");
                    if (nameAndValue.Length != 2)
                        throw new Exception($"Invalid option value '{optionValue}'. Value should be in format '[name]:[value]'.");
                    return (nameAndValue[0], nameAndValue[1]);
                });
}