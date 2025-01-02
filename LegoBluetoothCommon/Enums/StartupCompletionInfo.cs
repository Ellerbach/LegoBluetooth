// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

/// <summary>
/// Represents the startup and completion information.
/// </summary>
[Flags]
public enum StartupCompletionInfo : byte
{
    /// <summary>
    /// Buffer if necessary.
    /// </summary>
    BufferIfNecessary = 0x00,

    /// <summary>
    /// Execute immediately.
    /// </summary>
    ExecuteImmediately = 0x10,

    /// <summary>
    /// No action.
    /// </summary>
    NoAction = 0x00,

    /// <summary>
    /// Command feedback (Status).
    /// </summary>
    CommandFeedback = 0x01
}