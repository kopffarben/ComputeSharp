﻿using ComputeSharp.Shaders.Renderer.Models.Fields.Abstract;

namespace ComputeSharp.Shaders.Renderer.Models.Fields
{
    /// <summary>
    /// A <see langword="class"/> that contains info on a readonly buffer field
    /// </summary>
    internal sealed class ReadOnlyBufferFieldInfo : FieldInfoBase
    {
        /// <summary>
        /// Gets whether or not the current <see cref="FieldInfoBase"/> instance represents a readonly buffer (always <see langword="true"/>)
        /// </summary>
        public bool IsReadOnlyBuffer { get; } = true;

        /// <summary>
        /// Gets the index of the current constant buffer field
        /// </summary>
        public int ReadOnlyBufferIndex { get; }

        /// <summary>
        /// Creates a new <see cref="ReadOnlyBufferFieldInfo"/> instance with the specified parameters
        /// </summary>
        /// <param name="fieldType">The type of the current field</param>
        /// <param name="fieldName">The name of the current field</param>
        /// <param name="bufferIndex">The index of the current readonly buffer field</param>
        public ReadOnlyBufferFieldInfo(string fieldType, string fieldName, int bufferIndex) : base(fieldType, fieldName)
        {
            ReadOnlyBufferIndex = bufferIndex;
        }
    }
}
