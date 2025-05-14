// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

/// <summary>
/// Provides a data source for a theory test, with the data coming from a member of the test class.
/// Extends <see cref="MemberDataAttributeBase"/> with additional functionality.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class MemberTestDataAttribute : MemberDataAttributeBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberTestDataAttribute"/> class.
    /// <remarks>
    /// Constructor extension to set <see cref="MemberDataAttributeBase.DisableDiscoveryEnumeration"/> to true.
    /// </remarks>
    /// </summary>
    /// <param name="memberName">The name of the public member that will provide the test data.</param>
    /// <param name="arguments">The arguments to be passed to the member (only supported for static members).</param>
    public MemberTestDataAttribute(string memberName, params object[] arguments)
    : base(memberName, arguments)
    => DisableDiscoveryEnumeration = true;

    /// <summary>
    /// Retrieves the data to be used to test the theory.
    /// <remarks>
    /// For data rows implementing <see cref="ITheoryTestDataRow"/> where <see cref="ITheoryTestDataRow.ArgsCode"/>
    /// equals <see cref="ArgsCode.Properties"/>, this method sets the <see cref="ITheoryDataRow.TestDisplayName"/>
    /// using the test method name and the <see cref="ITheoryTestDataRow.TestData.TestCase"/> property.
    /// </remarks>
    /// </summary>
    /// <param name="testMethod">The test method decorated with the <see cref="TheoryAttribute"/>.</param>
    /// <param name="disposalTracker">The tracker for disposable objects created during test data generation.</param>
    /// <returns>A collection of test data rows to be used for the theory test.</returns>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        var dataCollection =
            await base.GetData(testMethod, disposalTracker)
            .ConfigureAwait(false);
        var result = new List<ITheoryDataRow>();

        foreach (var dataRow in dataCollection)
        {
            if (dataRow is ITheoryTestDataRow testDataRow)
            {
                var namedDataRow = testDataRow.SetTestDisplayName(testMethod.Name);
                result.Add(namedDataRow);
            }
            else
            {
                result.Add(dataRow);
            }
        }

        return result.CastOrToReadOnlyCollection();
    }

    /// <summary>
    /// Returns <paramref name="dataRow"/> if it is an <see cref="ITheoryTestDataRow"/>,
    /// otherwise calls the base method.
    /// </summary>
    /// <inheritdoc cref="DataAttribute.ConvertDataRow(object)"/>
    protected override ITheoryDataRow ConvertDataRow(object dataRow)
    => dataRow is ITheoryTestDataRow testDataRow ?
        testDataRow
        : base.ConvertDataRow(dataRow);
}