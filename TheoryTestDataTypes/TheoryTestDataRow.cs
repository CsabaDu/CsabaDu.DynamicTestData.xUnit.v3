/*
 * MIT License
 * 
 * Copyright (c) 2025. Csaba Dudas (CsabaDu)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

public sealed record TheoryTestDataRow(TestData TestData, ArgsCode ArgsCode, string? TestDisplayName = null)
: ITheoryTestDataRow, ISetTheoryDataRow<TheoryTestDataRow>
{
    #region Constants
    internal const string ArgsCodePropertyHasInvalidValue_ = "ArgsCode property has invalid value: ";
    #endregion

    #region Properties
    [NotNull]
    public TestData TestData { get; init; } = Guard.ArgumentNotNull(TestData, nameof(TestData));

    public ArgsCode ArgsCode { get; init; } = ArgsCode.Defined(nameof(ArgsCode));

    public bool? Explicit { get; init; } = null;

    public string? Skip { get; init; } = null;

    public int? Timeout { get; init; } = null;

    public Dictionary<string, HashSet<string>>? Traits { get; init; } = [];
    #endregion

    #region Methods
    internal static string? GetTestDisplayName(string? testMethodName, TestData testData)
    {
        return testMethodName is not null ?
            GetDisplayName(testMethodName, testData.TestCase)
            : null;
    }

    public TheoryTestDataRow SetTestDisplayName(string? testMethodName)
    => this with { TestDisplayName = GetTestDisplayName(testMethodName, TestData) };

    public TheoryTestDataRow SetExplicit(bool? explicitValue)
    => this with { Explicit = explicitValue };

    public TheoryTestDataRow SetSkip(string? skipValue)
    => this with { Skip = skipValue };

    public TheoryTestDataRow SetTimeout(int? timeoutValue)
    => this with { Timeout = timeoutValue };

    public TheoryTestDataRow SetTraits(string traitName, string traitValue)
    {
        Guard.ArgumentNotNullOrEmpty(nameof(traitName), traitName);
        Guard.ArgumentNotNullOrEmpty(nameof(traitValue), traitValue);

        if (Traits == null)
        {
            var traits = new Dictionary<string, HashSet<string>>()
            {
                { traitName, [traitValue] }
            };

            return this with { Traits = traits };
        }

        if (Traits!.TryGetValue(traitName, out HashSet<string>? traitvalues))
        {
            _ = traitvalues.Add(traitValue);
        }
        else
        {
            Traits.Add(traitName, [traitValue]);
        }

        return this;
    }

    public object?[] GetData() => ArgsCode switch
    {
        ArgsCode.Instance => [TestData],
        ArgsCode.Properties => string.IsNullOrEmpty(TestData.ExitMode) ?
            TestDataPropertiesToArgs(2)
            : TestDataPropertiesToArgs(1),
        _ => throw new InvalidOperationException(ArgsCodePropertyHasInvalidValue_ + (int)ArgsCode)
    };

    private object?[] TestDataPropertiesToArgs(int startIndex)
    => TestData.ToArgs(ArgsCode.Properties)[startIndex..];
    #endregion
}
