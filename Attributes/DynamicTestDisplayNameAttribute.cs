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
namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

/// <summary>
/// Attribute that enables dynamic test display names for theory tests by specifying a data source member.
/// </summary>
/// <remarks>
/// <para>
/// This attribute extends xUnit's data discovery by allowing dynamic generation of test display names
/// based on test data while maintaining a clean separation between test data and display logic.
/// </para>
/// <para>
/// The data source member must be a static method or property that returns <see cref="IEnumerable{T}"/> 
/// of <see cref="TheoryTestDataRow"/>.
/// </para>
/// </remarks>
/// <param name="dataSourceMemberName">The name of the static method or property that provides the test data</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class DynamicTestDisplayNameAttribute(string dataSourceMemberName) : DataAttribute
{
    #region Fields
    private readonly string _dataSourceMemberName = dataSourceMemberName;
    #endregion

    #region Exception Messages
    /// <summary>
    /// Error message when the test method has no declaring type.
    /// </summary>
    internal const string TestMethodHasNoDeclaringTypeMessage = "Test method has no declaring type";

    /// <summary>
    /// Gets the error message for invalid data source return type.
    /// </summary>
    /// <param name="data">The invalid data object</param>
    /// <returns>Formatted error message</returns>
    internal static string GetDataSourceNullOrInvalidTypeMessage(object? data)
    => $"Data source must return IEnumerable<TheoryTestDataRow>. " +
        $"Actual type: {data?.GetType().Name ?? "null"}";

    /// <summary>
    /// Gets the error message when the data source member is not found.
    /// </summary>
    /// <param name="declaringType">The type where the member was searched</param>
    /// <returns>Formatted error message</returns>
    internal string GetDataSourceMemberNotFoundMesssage(Type declaringType)
    => $"Data source member '{_dataSourceMemberName}' not found " +
        $"in type {declaringType.Name}. " +
        $"Expected a static method or property.";
    #endregion

    #region Methods
    /// <summary>
    /// Indicates whether this attribute supports discovery enumeration.
    /// </summary>
    /// <returns>Always returns true</returns>
    public override bool SupportsDiscoveryEnumeration() => true;

    /// <summary>
    /// Retrieves the theory test data from the specified data source member.
    /// </summary>
    /// <param name="testMethod">The test method being executed</param>
    /// <param name="disposalTracker">The disposal tracker for managing test resources</param>
    /// <returns>A collection of theory data rows with dynamically generated display names</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the test method has no declaring type
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// - The data source member is not found
    /// - The data source returns invalid data type
    /// </exception>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        Type? declaringType = testMethod.DeclaringType
            ?? throw new InvalidOperationException(TestMethodHasNoDeclaringTypeMessage);
        MethodInfo? dataSourceMethod = FindDataSourceMethod(declaringType)
            ?? throw new ArgumentException(GetDataSourceMemberNotFoundMesssage(declaringType));
        object? data = dataSourceMethod.Invoke(null, null);
        var namedDataRowList = GetNamedDataRowList(testMethod, data);

        return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(namedDataRowList);
    }

    /// <summary>
    /// Finds the data source method or property in the declaring type.
    /// </summary>
    /// <param name="declaringType">The type to search for the data source member</param>
    /// <returns>
    /// The MethodInfo for the data source member, or null if not found
    /// </returns>
    private MethodInfo? FindDataSourceMethod(Type declaringType)
    {
        const BindingFlags flags
            = BindingFlags.Static
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        return declaringType.GetMethod(_dataSourceMemberName, flags)
            ?? declaringType.GetProperty(_dataSourceMemberName, flags)?.GetMethod;
    }

    /// <summary>
    /// Converts the raw data into a list of theory data rows with generated display names.
    /// </summary>
    /// <param name="data">The data returned from the data source</param>
    /// <param name="testMethod">The test method information</param>
    /// <returns>A list of theory data rows with display names</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the data is not of type <see cref="IEnumerable{T}"/> of <see cref="TheoryTestDataRow"/>
    /// </exception>
    private static List<TheoryTestDataRow> GetNamedDataRowList(MethodInfo testMethod, object? data)
    {
        if (data is not IEnumerable<TheoryTestDataRow> dataRowList)
        {
            throw new ArgumentException(GetDataSourceNullOrInvalidTypeMessage(data));
        }

        var namedDataRowList = new List<TheoryTestDataRow>();

        foreach (TheoryTestDataRow? item in dataRowList)
        {
            var testData = item.TestData;
            var namedDataRow = new TheoryTestDataRow(testData, item.ArgsCode)
            {
                TestDisplayName = GetTestDisplayName(testMethod.Name, testData)
            };

            namedDataRowList.Add(namedDataRow);
        }

        return namedDataRowList;
    }
    #endregion
}