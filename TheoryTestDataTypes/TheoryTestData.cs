// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataTypes;

/// <summary>
/// Represents a collection of theory test data with configurable argument conversion strategy.
/// </summary>
/// <remarks>
/// <para>
/// This sealed class provides a strongly-typed container for theory test data rows,
/// inheriting from <see cref="TheoryDataBase{TTheoryDataRow, TTestData}"/> and implementing
/// <see cref="ITheoryTestData"/>.
/// </para>
/// </remarks>
/// <param name="argsCode">The strategy for converting test data to method arguments.</param>
public sealed class TheoryTestData(ArgsCode argsCode)
: TheoryDataBase<ITheoryTestDataRow, ITestData>, ITheoryTestData
{
    /// <inheritdoc cref="ITheoryTestData.ArgsCode"/>
    /// <remarks>The value is validated to be a defined enum value.</remarks>
    public ArgsCode ArgsCode { get; init; } = argsCode.Defined(nameof(argsCode));

    /// <inheritdoc cref="ITheoryTestData.TestDataType"/>
    public Type? TestDataType { get; private set; }

    /// <summary>
    /// Determines whether the specified <see cref="ITestData"/> instance is equal to the current instance.
    /// </summary>
    /// <param name="testData">The <see cref="ITestData"/> instance to compare with the current instance. Can be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the specified <see cref="ITestData"/> instance is of the same type as the current
    /// instance's <c>TestDataType</c>; otherwise, <see langword="false"/>.</returns>
    public bool Equals(ITestData? testData)
    => TestDataType is not null
        && testData?.GetType() == TestDataType;

    /// <summary>
    /// Converts test data into a theory test data row.
    /// </summary>
    /// <param name="testData">The test data to convert.</param>
    /// <returns>
    /// A new <see cref="TheoryTestDataRow"/> instance configured with the test data,
    /// and argument conversion strategy.
    /// </returns>
    protected override ITheoryTestDataRow Convert(ITestData testData)
    => new TheoryTestDataRow(testData, ArgsCode);

    /// <summary>
    /// Creates a strongly-typed instance of <see cref="ITheoryTestData"/> using the specified arguments and test data.
    /// </summary>
    /// <param name="argsCode">The code representing the arguments to be used for the test data.</param>
    /// <param name="testData">The test data instance used to initialize the <see cref="ITheoryTestData"/>. Cannot be <see langword="null"/>.</param>
    /// <returns>An instance of <see cref="ITheoryTestData"/> initialized with the provided arguments and test data type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="testData"/> is <see langword="null"/>.</exception>
    internal static TheoryTestData TypedTheoryTestData(
        ArgsCode argsCode,
        [NotNull] ITestData testData)
    => new(argsCode)
    {
        TestDataType = testData?.GetType()
            ?? throw new ArgumentNullException(nameof(testData)),
    };
}