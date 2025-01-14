// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

/// <summary>
/// Represents the value format for a LEGO Bluetooth device.
/// </summary>
namespace LegoBluetooth
{
    public class ValueFormat
    {
        /// <summary>
        /// Gets or sets the number of datasets.
        /// </summary>
        public byte NumberOfDatasets { get; set; }

        /// <summary>
        /// Gets or sets the data type of the datasets.
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// Gets or sets the total number of figures.
        /// </summary>
        public byte TotalFigures { get; set; }

        /// <summary>
        /// Gets or sets the number of decimal places.
        /// </summary>
        public byte Decimals { get; set; }

        /// <summary>
        /// Decodes a byte array to create a <see cref="ValueFormat"/> instance.
        /// </summary>
        /// <param name="data">The byte array containing the value format data.</param>
        /// <returns>A <see cref="ValueFormat"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the data array is null or its length is less than 4 bytes.</exception>
        public static ValueFormat Decode(byte[] data)
        {
            if (data == null || data.Length < 4)
            {
                throw new ArgumentException("Invalid data array. Must contain at least 4 bytes.", nameof(data));
            }

            return new ValueFormat
            {
                NumberOfDatasets = data[0],
                Type = (DataType)data[1],
                TotalFigures = data[2],
                Decimals = data[3]
            };
        }

        /// <summary>
        /// Encodes the <see cref="ValueFormat"/> instance to a byte array.
        /// </summary>
        /// <returns>A byte array representing the <see cref="ValueFormat"/> instance.</returns>
        public byte[] Encode()
        {
            return new byte[]
            {
                NumberOfDatasets,
                (byte)Type,
                TotalFigures,
                Decimals
            };
        }
    }
}

