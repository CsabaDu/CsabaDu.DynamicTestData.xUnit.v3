// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class MemberTestDataAttribute : MemberDataAttributeBase
{
    /// <inheritdoc/>
    /// <remarks>
    /// Constructor extension to set <see cref="MemberDataAttributeBase.DisableDiscoveryEnumeration"/> to true.
    /// </remarks>
    public MemberTestDataAttribute(string memberName, params object[] arguments)
    : base(memberName, arguments)
    {
        DisableDiscoveryEnumeration = true;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Sets <see cref="ITheoryDataRow.TestDisplayName"/> if elements are <see cref="ITheoryTestDataRow"/>
    /// and their <see cref="ITheoryTestDataRow.ArgsCode"/> property is <see cref="ArgsCode.Properties"/>.
    /// </remarks>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        var dataCollection = await base.GetData(testMethod, disposalTracker).ConfigureAwait(false);
        if (dataCollection is IEnumerable<ITheoryTestDataRow> testDataCollection)
        {
            dataCollection =
                testDataCollection
                .Select(setTestDisplayNameIfArgsCodeProperties)
                .CastOrToReadOnlyCollection();
        }

        return dataCollection;

        #region Local methods
        ITheoryDataRow setTestDisplayNameIfArgsCodeProperties(ITheoryTestDataRow testDataRow)
        => testDataRow.ArgsCode == ArgsCode.Properties ?
            testDataRow.SetTestDisplayName(testMethod.Name)
            : testDataRow;
        #endregion
    }
}
