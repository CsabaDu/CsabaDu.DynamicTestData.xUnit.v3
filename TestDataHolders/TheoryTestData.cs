// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataHolders;

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders
{
    public class TheoryTestData<TTestData>
    : TheoryDataBase<ITheoryTestDataRow<TTestData>, TTestData>,
    ITheoryTestData
    where TTestData : notnull, ITestData
    {
        private TheoryTestData(IDataStrategy? dataStrategy, string paramName)
        => DataStrategy = dataStrategy
            ?? throw new ArgumentNullException(paramName);

        public TheoryTestData(
            TTestData testData,
            IDataStrategy dataStrategy)
        : this(dataStrategy, nameof(dataStrategy))
        => Add(testData);

        public TheoryTestData(ITheoryTestDataRow<TTestData> row)
        : this(row?.DataStrategy, nameof(row))
        => Add(row!);

        public TheoryTestData(IEnumerable<TheoryTestDataRow<TTestData>> rows)
        : this(rows?.FirstOrDefault()?.DataStrategy, nameof(rows))
        => AddRange(rows!);

        public Type TestDataType => typeof(TTestData);

        public IDataStrategy DataStrategy { get; init; }

        public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName)
        => GetRows(testMethodName, null);

        public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName, ArgsCode? argsCode)
        => GetRows(testMethodName, argsCode);

        public IEnumerable<ITheoryTestDataRow>? GetRows()
        => GetRows(null, null);

        public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
        => GetRows(null, argsCode);

        private IEnumerable<ITheoryTestDataRow>? GetRows(
            string? testMethodName,
            ArgsCode? argsCode)
        {
            argsCode ??= DataStrategy.ArgsCode;

            if (string.IsNullOrEmpty(testMethodName)
                && argsCode == DataStrategy.ArgsCode)
            {
                return this;
            }

            return this.Select(ttdr => new TheoryTestDataRow<TTestData>(
                (ttdr as TheoryTestDataRow<TTestData>)!,
                argsCode.Value,
                testMethodName));
        }

        protected override ITheoryTestDataRow<TTestData> Convert(TTestData testData)
        => new TheoryTestDataRow<TTestData>(
            testData,
            DataStrategy);
    }
}
