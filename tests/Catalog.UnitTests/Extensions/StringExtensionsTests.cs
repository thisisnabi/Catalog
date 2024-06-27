using Catalog.Infrastructure.Extensions;
namespace Catalog.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("Convert This Title To Kebab-Case!", "convert-this-title-to-kebab-case")]
    [InlineData("Hello World", "hello-world")]
    [InlineData(" Hello World ", "hello-world")]
    [InlineData("Multiple     Spaces", "multiple-spaces")]
    [InlineData("Special@#Characters!", "specialcharacters")]
    [InlineData("Already-Kebab-Case", "already-kebab-case")]
    public void ConvertToKebabCase_ShouldReturnCorrectKebabCase(string input, string expected)
    {
        // Act
        string result = input.ToKebabCase();

        // Assert
        result.Should().Be(expected);
    }
}
