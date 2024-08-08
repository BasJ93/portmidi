using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PortMidi
{
    public class MidiInput : MidiStream
    {
        public MidiInput(IntPtr stream, Int32 inputDevice)
            : base(stream, inputDevice)
        {
        }

        public bool HasData => PortMidiMarshal.Pm_Poll(stream) == MidiErrorType.GotData;

        public int Read(PmEvent[] buffer, int index, int length)
        {
            //var gch = GCHandle.Alloc(buffer);
            try
            {
                //var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, index);
                int numberOfMessages = PortMidiMarshal.Pm_Read(stream, buffer, length);
                if (numberOfMessages < 0)
                {
                    throw new MidiException((MidiErrorType) numberOfMessages,
                        PortMidiMarshal.Pm_GetErrorText((MidiErrorType) numberOfMessages));
                }
                return numberOfMessages;
            }
            finally
            {
                //gch.Free();
            }
        }

        public Event ReadEvent(PmEvent[] buffer, int index, int length)
        {
            //var gch = GCHandle.Alloc(buffer);
            try
            {
                //var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, index);
                int size = PortMidiMarshal.Pm_Read(stream, buffer, length);
                if (size < 0)
                {
                    throw new MidiException((MidiErrorType) size,
                        PortMidiMarshal.Pm_GetErrorText((MidiErrorType) size));
                }

                return new Event(buffer[0]);
                //return new Event(Marshal.PtrToStructure<PmEvent>(ptr));
            }
            finally
            {
                //gch.Free();
            }
        }
    }
}