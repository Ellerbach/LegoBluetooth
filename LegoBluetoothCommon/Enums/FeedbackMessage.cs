// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

/// <summary>
/// Represents the feedback message as bit-fields.
/// </summary>
[Flags]
public enum FeedbackMessage : byte
{
    /// <summary>
    /// Buffer Empty + Command In Progress.
    /// </summary>
    BufferEmptyCommandInProgress = 0x01,

    /// <summary>
    /// Buffer Empty + Command Completed.
    /// </summary>
    BufferEmptyCommandCompleted = 0x02,

    /// <summary>
    /// Current Command(s) Discarded.
    /// </summary>
    CurrentCommandsDiscarded = 0x04,

    /// <summary>
    /// Idle.
    /// </summary>
    Idle = 0x08,

    /// <summary>
    /// Busy/Full.
    /// </summary>
    BusyFull = 0x10
}
