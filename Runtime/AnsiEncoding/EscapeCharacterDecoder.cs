using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal sealed class EscapeCharacterDecoder : IEscapeCharacterDecoder
    {
        private const byte EscapeCharacter = 0x1B; // ESC
        private const byte BellCharacter = 0x07; // BEL
        private const byte LeftBracketCharacter_ControlSequenceIntroducer = 0x5B; // [
        private const byte RightBracketCharacter_OperatingSystemCommandIntroducer = 0x5D; // ]
        private const byte XonCharacter = 17;
        private const byte XoffCharacter = 19;

        private enum State
        {
            Normal,
            Command,
        }

        private State _state;
        private System.Text.Encoding _encoding;
        private Decoder _decoder;
        private Encoder _encoder;
        private List<byte> _commandBuffer;
        private readonly bool _supportXonXOff;
        private bool _xOffReceived;
        private readonly List<byte[]> _outBuffer;
        private event Action<SequenceType, char, string> _processCommand;
        private event Action<byte[]> _processOutput;

        event Action<byte[]> IEscapeCharacterDecoder.ProcessOutput
        {
            add => _processOutput += value;
            remove => _processOutput -= value;
        }

        event Action<SequenceType, char, string> IEscapeCharacterDecoder.ProcessCommand
        {
            add => _processCommand += value;
            remove => _processCommand -= value;
        }

        public System.Text.Encoding Encoding
        {
            get => _encoding;
            private set
            {
                if (Equals(_encoding, value)) return;
                _encoding = value;
                _decoder = _encoding.GetDecoder();
                _encoder = _encoding.GetEncoder();
            }
        }

        public EscapeCharacterDecoder()
        {
            _state = State.Normal;
            Encoding = System.Text.Encoding.ASCII;
            _commandBuffer = new List<byte>();
            _supportXonXOff = true;
            _xOffReceived = false;
            _outBuffer = new List<byte[]>();
        }

        internal bool IsValidParameterCharacter(char c, bool isOscCommand, bool isCSI)
        {
            if (isOscCommand)
            {
                return c != BellCharacter || Char.IsNumber(c) || c == ';' || c == '"' || c == '?';
            }
            else if (isCSI)
                return Char.IsNumber(c) || c == ';' || c == '"' || c == '?' || c == '>' || c == '!';

            return false;
        }

        internal void AddToCommandBuffer(byte data)
        {
            if (_supportXonXOff)
            {
                if (data == XonCharacter || data == XoffCharacter)
                {
                    return;
                }
            }

            _commandBuffer.Add(data);
        }

        internal void AddToCommandBuffer(byte[] bytes)
        {
            if (_supportXonXOff)
            {
                foreach (byte b in bytes)
                {
                    if (!(b == XonCharacter || b == XoffCharacter))
                    {
                        _commandBuffer.Add(b);
                    }
                }
            }
            else
            {
                _commandBuffer.AddRange(bytes);
            }
        }

        internal bool IsValidOneCharacterCommand(char command)
        {
            return command is '6' or '7' or '8' or '9';
        }

        private void ProcessCommandBuffer()
        {
            _state = State.Command;

            if (_commandBuffer.Count > 1)
            {
                if (_commandBuffer[0] != EscapeCharacter)
                {
                    throw new Exception(
                        "Internal error, first command character _MUST_ be the escape character, please report this bug to the author.");
                }

                int start = 1;
                bool IsCsiCommand = false;
                bool isOscCommand = false;
                // Is this a one or two byte escape code?
                if ((IsCsiCommand = _commandBuffer[start] == LeftBracketCharacter_ControlSequenceIntroducer)
                    ||
                    (isOscCommand = _commandBuffer[start] == RightBracketCharacter_OperatingSystemCommandIntroducer))
                {
                    start++;
                    // It is a two byte escape code, but we still need more data
                    if (_commandBuffer.Count < 3)
                    {
                        return;
                    }
                }

                bool insideQuotes = false;
                int end = start;
                while (end < _commandBuffer.Count &&
                       (IsValidParameterCharacter((char)_commandBuffer[end], isOscCommand, IsCsiCommand) ||
                        insideQuotes))
                {
                    if (_commandBuffer[end] == '"')
                    {
                        insideQuotes = !insideQuotes;
                    }

                    if (_commandBuffer[end] == ']' || _commandBuffer[end] == BellCharacter)
                    {
                        isOscCommand = !isOscCommand;
                        if (isOscCommand)
                            start++;
                    }

                    end++;
                }

                if (_commandBuffer.Count == 2 && IsValidOneCharacterCommand((char)_commandBuffer[start]))
                {
                    end = _commandBuffer.Count - 1;
                }

                if (end == _commandBuffer.Count)
                {
                    // More data needed
                    return;
                }

                Decoder decoder = Encoding.GetDecoder();
                byte[] parameterData = new byte[end - start];
                for (int i = 0; i < parameterData.Length; i++)
                {
                    parameterData[i] = _commandBuffer[start + i];
                }

                int parameterLength = decoder.GetCharCount(parameterData, 0, parameterData.Length);
                char[] parameterChars = new char[parameterLength];
                decoder.GetChars(parameterData, 0, parameterData.Length, parameterChars, 0);
                String parameter = new String(parameterChars);

                byte command = _commandBuffer[end];

                try
                {
                    OnProcessCommand(GetSequenceType(IsCsiCommand, isOscCommand), command, parameter);
                }
                finally
                {
                    //System.Console.WriteLine ( "Remove the processed commands" );

                    // Remove the processed commands
                    if (_commandBuffer.Count == end - 1)
                    {
                        // All command bytes processed, we can go back to normal handling
                        _commandBuffer.Clear();
                        _state = State.Normal;
                    }
                    else
                    {
                        bool returnToNormalState = true;
                        for (int i = end + 1; i < _commandBuffer.Count; i++)
                        {
                            if (_commandBuffer[i] == EscapeCharacter)
                            {
                                _commandBuffer.RemoveRange(0, i);
                                ProcessCommandBuffer();
                                returnToNormalState = false;
                            }
                            else
                            {
                                ProcessNormalInput(_commandBuffer[i]);
                            }
                        }

                        if (returnToNormalState)
                        {
                            _commandBuffer.Clear();

                            _state = State.Normal;
                        }
                    }
                }
            }
        }

        private SequenceType GetSequenceType(bool isCsi, bool isOSC)
        {
            return (isCsi, isOSC) switch
            {
                (false, false) => SequenceType.ESC,
                (true, false) => SequenceType.CSI,
                (false, true) => SequenceType.OSC,
            };
        }

        internal void ProcessNormalInput(byte data)
        {
            if (data == EscapeCharacter)
            {
                throw new Exception(
                    "Internal error, ProcessNormalInput was passed an escape character, please report this bug to the author.");
            }

            if (_supportXonXOff)
            {
                if (data == XonCharacter || data == XoffCharacter)
                {
                    return;
                }
            }

            byte[] output = new byte[] { data };
            int charCount = _decoder.GetCharCount(output, 0, 1);

            if (charCount > 0)
            {
                OnProcessOutput(output);
            }
            else
            {
                //System.Console.WriteLine ( "char count was zero" );
            }
        }

        public void Decode(byte[] data)
        {
            if (data.Length == 0)
            {
                throw new ArgumentException("Input can not process an empty array.");
            }

            if (_supportXonXOff)
            {
                foreach (byte b in data)
                {
                    if (b == XoffCharacter)
                    {
                        _xOffReceived = true;
                    }
                    else if (b == XonCharacter)
                    {
                        _xOffReceived = false;
                        if (_outBuffer.Count > 0)
                        {
                            foreach (byte[] output in _outBuffer)
                            {
                                OnProcessOutput(output);
                            }
                        }
                    }
                }
            }

            switch (_state)
            {
                case State.Normal:
                    if (data[0] == EscapeCharacter)
                    {
                        AddToCommandBuffer(data);
                        ProcessCommandBuffer();
                    }
                    else
                    {
                        int i = 0;
                        while (i < data.Length && data[i] != EscapeCharacter)
                        {
                            ProcessNormalInput(data[i]);
                            i++;
                        }

                        if (i != data.Length)
                        {
                            while (i < data.Length)
                            {
                                AddToCommandBuffer(data[i]);
                                i++;
                            }

                            ProcessCommandBuffer();
                        }
                    }

                    break;

                case State.Command:
                    AddToCommandBuffer(data);
                    ProcessCommandBuffer();
                    break;
            }
        }

        public bool KeyPressed(KeyCode[] modifiers, KeyCode key)
        {
            return false;
        }

        public void Dispose()
        {
            _encoding = null;
            _decoder = null;
            _encoder = null;
            _commandBuffer = null;
        }

        private void OnProcessCommand(SequenceType sequenceType, byte command, string parameter)
        {
            _processCommand?.Invoke(sequenceType, (char)command, parameter);
        }

        private void OnProcessOutput(byte[] output)
        {
            if (_processOutput != null)
            {
                if (_supportXonXOff && _xOffReceived)
                {
                    _outBuffer.Add(output);
                }
                else
                {
                    _processOutput(output);
                }
            }
        }
    }
}

// #define PUNITY_ANSI_DEBUG
// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace HamerSoft.PuniTY.AnsiEncoding
// {
//     internal abstract class EscapeCharacterDecoder
//     {
//         internal const byte EscapeCharacter = 0x1B; // [
//         internal const byte LeftBracketCharacter = 0x5B; // [
//         internal const byte RightBracketCharacter = 0x5D; // ]
//         internal const byte BellCharacter = 0x07; // BEL
//         private readonly System.Text.Encoding _encoding;
//         private List<byte> _inputBuffer;
//
//         public EscapeCharacterDecoder()
//         {
//             _encoding = System.Text.Encoding.ASCII;
//         }
//
//         public void AddData(List<byte> data)
//         {
//             _inputBuffer.AddRange(data);
//         }
//
//         public void ProcessCommands()
//         {
//             if (_inputBuffer.Count > 1)
//             {
//                 if (IsEscapeCharacter(_inputBuffer[0]))
//                 {
//                     throw new Exception(
//                         "First character of a command must start with Escape Character 0x1B ='['.");
//                 }
//
//                 int start = 1;
//                 if (IsTwoByteEscapeCode(start))
//                 {
//                     start++;
//                     if (_inputBuffer.Count < 3)
//                     {
//                         return;
//                     }
//                 }
//
//                 int end = start;
//                 while (!IsValidParameterCharacter((char)_inputBuffer[end]))
//                 {
//                     end++;
//                 }
//             }
//         }
//
//         private bool IsEscapeCharacter(byte character)
//         {
//             return character == EscapeCharacter;
//         }
//
//         public void ProcessCommand()
//         {
//             if (_inputBuffer.Count > 1)
//             {
//                 if (_inputBuffer[0] != EscapeCharacter)
//                 {
//                     throw new Exception(
//                         "First character of a command must start with Escape Character 0x1B ='['.");
//                 }
//
//                 int start = 1;
//                 if (IsTwoByteEscapeCode(start))
//                 {
//                     start++;
//                     if (_inputBuffer.Count < 3)
//                     {
//                         return;
//                     }
//                 }
//                 else if (IsOSCEscapeCode(start))
//                 {
//                     start++;
//                     var end = 1;
//                     while (_inputBuffer[end] != BellCharacter)
//                         end++;
// #if PUNITY_ANSI_DEBUG
//                     var parameter = GetParameters(end, start);
//                     Debug.Log($"OSC command detected and ignored: {parameter}");
// #elif PUNITY_LOGGING
//                     Debug.Log("OSC command detected and ignored!");
// #endif
//                     _inputBuffer.RemoveRange(0, start + end);
//                 }
//                 else
//                 {
//                     bool insideQuotes = false;
//                     int end = start;
//                     while (end < _inputBuffer.Count &&
//                            (IsValidParameterCharacter((char)_inputBuffer[end]) || insideQuotes))
//                     {
//                         if (_inputBuffer[end] == '"')
//                         {
//                             insideQuotes = !insideQuotes;
//                         }
//
//                         end++;
//                     }
//
//                     // if (_inputBuffer.Count == 2 && IsValidOneCharacterCommand((char)m_commandBuffer[start]))
//                     // {
//                     //     end = m_commandBuffer.Count - 1;
//                     // }
//
//                     if (end == _inputBuffer.Count)
//                     {
//                         // More data needed
//                         return;
//                     }
//
//
//                     var parameter = GetParameters(end, start);
//
//                     byte command = _inputBuffer[end];
//                     ProcessCommand((char)command, parameter);
//                 }
//             }
//         }
//
//         protected virtual bool IsValidParameterCharacter(char _c)
//         {
//             // return (Char.IsNumber( _c ) || _c == '(' || _c == ')' || _c == ';' || _c == '"' || _c == '?');
//             return (Char.IsNumber(_c) || _c == ';' || _c == '"' || _c == '?');
//         }
//
//         /// <summary>
//         /// Get parameters from encoded string
//         /// </summary>
//         /// <param name="end">end index in buffer</param>
//         /// <param name="start">start index in buffer</param>
//         /// <returns>parameters as string</returns>
//         private string GetParameters(int end, int start)
//         {
//             var commandBuffer = new byte[end - start];
//             for (int i = 0; i < commandBuffer.Length; i++)
//                 commandBuffer[i] = _inputBuffer[start + i];
//
//             int parameterLength = _encoding.GetCharCount(commandBuffer, 0, commandBuffer.Length);
//             char[] parameterChars = new char[parameterLength];
//             _encoding.GetChars(commandBuffer, 0, commandBuffer.Length, parameterChars, 0);
//             String parameter = new String(parameterChars);
//             return parameter;
//         }
//
//         /// <summary>
//         /// check if this is a OSC escape code
//         /// </summary>
//         /// <remarks>https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences#screen-colors</remarks>
//         /// <param name="index">index to check in the buffer</param>
//         /// <returns>true when ] detected</returns>
//         private bool IsOSCEscapeCode(int index)
//         {
//             return _inputBuffer[index] == RightBracketCharacter;
//         }
//
//         /// <summary>
//         /// check if the escape code has double [
//         /// </summary>
//         /// <param name="index">index to check in the buffer</param>
//         /// <returns>true when [ detected</returns>
//         private bool IsTwoByteEscapeCode(int index)
//         {
//             return _inputBuffer[index] == LeftBracketCharacter;
//         }
//
//         private
//             protected abstract void ProcessCommand(char character, string parameters);
//     }
// }