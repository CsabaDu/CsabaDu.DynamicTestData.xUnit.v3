// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

using CsabaDu.DynamicTestData.TestDataTypes;
using CsabaDu.DynamicTestData.xUnit.v3.TheoryTestDataHolders;

namespace CsabaDu.DynamicTestData.xUnit.v3.TestDataHolders
{
    public class TheoryTestData<TTestData>
    : TheoryDataBase<ITheoryTestDataRow<TTestData>, TTestData>,
    ITheoryTestData
    where TTestData : notnull, ITestData
    {
        public TheoryTestData(
            TTestData testData,
            IDataStrategy dataStrategy)
        : this(dataStrategy)
        => Add(testData);

        public TheoryTestData(ITheoryTestDataRow<TTestData> row)
        : this(row?.DataStrategy)
        => Add(row!);

        private TheoryTestData(IDataStrategy? dataStrategy)
        {
            Guard.ArgumentNotNull(dataStrategy);
            DataStrategy = dataStrategy;
        }

        public TheoryTestData(IEnumerable<TheoryTestDataRow<TTestData>> rows)
        : this(rows?.FirstOrDefault()?.DataStrategy)
        => AddRange(rows!);

        public Type TestDataType => typeof(TTestData);

        public IDataStrategy DataStrategy { get; init; }

        public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName)
        {
            return GetRows(testMethodName, null);

            //if (string.IsNullOrEmpty(testMethodName))
            //{
            //    return GetRows();
            //}

            //return GetRows()?.Select(ttdr => new TheoryTestDataRow<TTestData>(
            //    (ttdr as TheoryTestDataRow<TTestData>)!,
            //    ttdr.GetDataStrategy().ArgsCode,
            //    testMethodName));
        }

        public IEnumerable<ITheoryTestDataRow>? GetNamedRows(string? testMethodName, ArgsCode? argsCode)
        {
            return GetRows(testMethodName, argsCode);

            //if (string.IsNullOrEmpty(testMethodName))
            //{
            //    return GetRows(argsCode);
            //}

            //if (!argsCode.HasValue || argsCode.Value == DataStrategy.ArgsCode)
            //{
            //    return GetNamedRows(testMethodName);
            //}

            //return GetRows()?.Select(ttdr => new TheoryTestDataRow<TTestData>(
            //    (ttdr as TheoryTestDataRow<TTestData>)!,
            //    argsCode.Value,
            //    testMethodName));
        }

        public IEnumerable<ITheoryTestDataRow>? GetRows()
        {
            return GetRows(null, null);
        }

        public IEnumerable<ITheoryTestDataRow>? GetRows(ArgsCode? argsCode)
        {
            return GetRows(null, argsCode);

            //if (!argsCode.HasValue || argsCode.Value == DataStrategy.ArgsCode)
            //{
            //    return GetRows();
            //}

            //return GetRows()?.Select(ttdr => new TheoryTestDataRow<TTestData>(
            //    (ttdr as TheoryTestDataRow<TTestData>)!,
            //    argsCode.Value,
            //    null));
        }

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
        => new TheoryTestDataRow<TTestData>(testData, DataStrategy);
    }
}
