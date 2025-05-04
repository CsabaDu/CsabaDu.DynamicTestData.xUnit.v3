// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.xUnit.v3.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class MemberTestDataAttribute : MemberDataAttributeBase
{
    public MemberTestDataAttribute(string memberName, params object[] arguments)
    : base(memberName, arguments)
    {
        DisableDiscoveryEnumeration = true;
    }

    /// <inheritdoc/>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        var dataCollection = await base.GetData(testMethod, disposalTracker);
        if (dataCollection is IEnumerable<ITheoryTestDataRow> testDataCollection)
        {
            dataCollection =
                testDataCollection
                .Select(setTestDisplayNameIfArgsCodeProperties)
                .CastOrToReadOnlyCollection();
        }

        return await new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(dataCollection);

        #region Local methods
        ITheoryDataRow setTestDisplayNameIfArgsCodeProperties(ITheoryTestDataRow testDataRow)
        => testDataRow.ArgsCode == ArgsCode.Properties ?
            testDataRow.SetTestDisplayName(testMethod.Name)
            : testDataRow;
        #endregion
    }
}
