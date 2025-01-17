using System;
using System.Runtime.InteropServices;

namespace ParquetSharp
{
    /// <summary>
    /// Represents the metadata for a row group in a Parquet file.
    /// </summary>
    /// <remarks>
    /// A row group is a logical horizontal partition of a Parquet file that contains a set of column chunks.
    /// It provides information about the columns and rows in the row group.
    /// </remarks>
    public sealed class RowGroupMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RowGroupMetaData"/> class.
        /// </summary>
        /// <param name="handle">A handle to the native row group metadata.</param>
        /// <remarks>
        /// This constructor is intended to be used by the ParquetSharp library.
        /// </remarks>
        internal RowGroupMetaData(IntPtr handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// Gets the number of columns in the row group.
        /// </summary>
        public int NumColumns => ExceptionInfo.Return<int>(_handle, RowGroupMetaData_Num_Columns);
        /// <summary>
        /// Gets the number of rows in the row group.
        /// </summary>
        public long NumRows => ExceptionInfo.Return<long>(_handle, RowGroupMetaData_Num_Rows);
        /// <summary>
        /// Gets the schema descriptor for the row group.
        /// </summary>
        public SchemaDescriptor Schema => _schema ??= new SchemaDescriptor(ExceptionInfo.Return<IntPtr>(_handle, RowGroupMetaData_Schema));
        /// <summary>
        /// Gets the total byte size of the row group.
        /// </summary>
        public long TotalByteSize => ExceptionInfo.Return<long>(_handle, RowGroupMetaData_Total_Byte_Size);

        /// <summary>
        /// Gets the metadata for the column chunk at the specified index.
        /// </summary>
        /// <param name="i">The index of the column chunk.</param>
        /// <returns>The metadata for the column chunk.</returns>
        /// <remarks>
        /// Column chunks are stored in the same order as the columns in the schema.
        /// </remarks>
        public ColumnChunkMetaData GetColumnChunkMetaData(int i)
        {
            return new ColumnChunkMetaData(ExceptionInfo.Return<int, IntPtr>(_handle, i, RowGroupMetaData_Get_Column_Chunk_Meta_Data));
        }

        [DllImport(ParquetDll.Name)]
        private static extern IntPtr RowGroupMetaData_Get_Column_Chunk_Meta_Data(IntPtr rowGroupMetaData, int i, out IntPtr columnChunkMetaData);

        [DllImport(ParquetDll.Name)]
        private static extern IntPtr RowGroupMetaData_Num_Columns(IntPtr rowGroupMetaData, out int numColumns);

        [DllImport(ParquetDll.Name)]
        private static extern IntPtr RowGroupMetaData_Num_Rows(IntPtr rowGroupMetaData, out long numRows);

        [DllImport(ParquetDll.Name)]
        private static extern IntPtr RowGroupMetaData_Schema(IntPtr rowGroupMetaData, out IntPtr schemaDescriptor);

        [DllImport(ParquetDll.Name)]
        private static extern IntPtr RowGroupMetaData_Total_Byte_Size(IntPtr rowGroupMetaData, out long totalByteSize);

        private readonly IntPtr _handle;
        private SchemaDescriptor? _schema;
    }
}
