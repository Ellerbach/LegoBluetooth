// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    public class PortBufferingStateMachine
    {
        /// <summary>
        /// Represents the states of the buffering state machine.
        /// </summary>
        public enum State
        {
            Idle,
            BusyEmpty,
            BusyFull
        }

        /// <summary>
        /// Represents the events that affect the state machine.
        /// </summary>
        public enum Event
        {
            CommandForImmediateExecution,
            CommandForBuffering,
            CommandCompleted,
            Interrupt
        }

        /// <summary>
        /// Represents the feedback message as bit-fields.
        /// </summary>
        [Flags]
        public enum FeedbackMessage : byte
        {
            BufferEmptyCommandInProgress = 0x01,
            BufferEmptyCommandCompleted = 0x02,
            CurrentCommandsDiscarded = 0x04,
            Idle = 0x08,
            BusyFull = 0x10
        }

        /// <summary>
        /// Gets the current state of the state machine.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortBufferingStateMachine"/> class.
        /// </summary>
        public PortBufferingStateMachine()
        {
            CurrentState = State.Idle;
        }

        /// <summary>
        /// Handles the specified event and transitions the state machine to the new state.
        /// </summary>
        /// <param name="event">The event to handle.</param>
        /// <returns>The feedback message indicating the new state and any additional information.</returns>
        public FeedbackMessage HandleEvent(Event @event)
        {
            FeedbackMessage feedback = 0;

            switch (CurrentState)
            {
                case State.Idle:
                    if (@event == Event.CommandForImmediateExecution || @event == Event.CommandForBuffering)
                    {
                        CurrentState = State.BusyEmpty;
                        feedback = FeedbackMessage.BufferEmptyCommandInProgress;
                    }
                    break;

                case State.BusyEmpty:
                    if (@event == Event.CommandForImmediateExecution)
                    {
                        feedback = FeedbackMessage.BufferEmptyCommandInProgress | FeedbackMessage.CurrentCommandsDiscarded;
                    }
                    else if (@event == Event.CommandForBuffering)
                    {
                        CurrentState = State.BusyFull;
                        feedback = FeedbackMessage.BusyFull;
                    }
                    else if (@event == Event.CommandCompleted)
                    {
                        CurrentState = State.Idle;
                        feedback = FeedbackMessage.Idle | FeedbackMessage.BufferEmptyCommandCompleted;
                    }
                    else if (@event == Event.Interrupt)
                    {
                        CurrentState = State.Idle;
                        feedback = FeedbackMessage.Idle | FeedbackMessage.CurrentCommandsDiscarded;
                    }
                    break;

                case State.BusyFull:
                    if (@event == Event.CommandForImmediateExecution)
                    {
                        CurrentState = State.BusyEmpty;
                        feedback = FeedbackMessage.BufferEmptyCommandInProgress | FeedbackMessage.CurrentCommandsDiscarded;
                    }
                    else if (@event == Event.CommandCompleted)
                    {
                        CurrentState = State.BusyEmpty;
                        feedback = FeedbackMessage.BufferEmptyCommandCompleted;
                    }
                    else if (@event == Event.Interrupt)
                    {
                        CurrentState = State.Idle;
                        feedback = FeedbackMessage.Idle | FeedbackMessage.CurrentCommandsDiscarded;
                    }
                    break;
            }

            return feedback;
        }
    }
}
