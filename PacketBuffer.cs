using System;
using System.Collections.Generic;
using System.Text;

// classes common to the client and server should be in this namespace
namespace One_Click_Server
{
    // defines a basic network packet buffer for the client and server
    class PacketBuffer : IDisposable
    {
        byte[] _readBuffer; // buffer we read in. as a byte array
        List<byte> _bufferList; // our byte array buffer as a list
        int _readPos; // position we are currently reading from
        bool _bufferUpdate = false; // are we currently updating buffer

        // default constructor
        public PacketBuffer()
        {
            _bufferList = new List<byte>();
            _readPos = 0;
        }

        // member function to convert PacketBuffer to byte array
        public byte[] ToArray() { return _bufferList.ToArray(); }

        // add input bytes to buffer
        public void WriteBytes(byte[] inBytes)
        {
            _bufferList.AddRange(inBytes);
            _bufferUpdate = true;
        }

        // add single byte to buffer
        public void WriteByte(byte inputByte)
        {
            _bufferList.Add(inputByte);
            _bufferUpdate = true;
        }

        // add int to buffer
        public void WriteInt(int inputInt) { _bufferList.AddRange(BitConverter.GetBytes(inputInt)); }

        // add float to buffer
        public void WriteFloat(float inputFloat) { _bufferList.AddRange(BitConverter.GetBytes(inputFloat)); }

        // add string to buffer
        public void WriteString(string inputString)
        {
            _bufferList.AddRange(BitConverter.GetBytes(inputString.Length));
            _bufferList.AddRange(Encoding.ASCII.GetBytes(inputString));
        }

        // read integer from buffer
        public int ReadInteger(bool peek = true)
        {
            if (_readPos < _bufferList.Count) {
                if (_bufferUpdate) {
                    _readBuffer = _bufferList.ToArray();
                    _bufferUpdate = false;
                }

                int value = BitConverter.ToInt32(_readBuffer, _readPos);

                if (peek & _bufferList.Count > _readPos) _readPos += 4;

                return value;
            } else throw new Exception("Buffer attempted to read out of bounds. ");
        }

        // read float from buffer
        public float ReadFloat(bool peek = true)
        {
            // check we are reading within bounds of received data buffer
            if (_readPos < _bufferList.Count) {
                if (_bufferUpdate) {
                    _readBuffer = _bufferList.ToArray();
                    _bufferUpdate = false;
                }

                float value = BitConverter.ToSingle(_readBuffer, _readPos);

                if (peek & _bufferList.Count > _readPos) _readPos += 4;

                return value;
            } else throw new Exception("Buffer attempted to read out of bounds. ");
        }

        // read byte array from buffer
        public byte[] ReadByteArray(int length, bool peek = true)
        {
            if (_bufferUpdate) {
                _readBuffer = _bufferList.ToArray();
                _bufferUpdate = false;
            }

            byte[] value = _bufferList.GetRange(_readPos, length).ToArray();

            if (peek & _bufferList.Count > _readPos)
                _readPos += length;

            return value;
        }

        // read single byte from buffer
        public byte ReadByte(bool peek = true)
        {
            // check we are reading within bounds of received data buffer
            if (_readPos < _bufferList.Count) {
                if (_bufferUpdate) {
                    _readBuffer = _bufferList.ToArray();
                    _bufferUpdate = false;
                }

                byte value = _readBuffer[_readPos];

                if (peek & _bufferList.Count > _readPos) _readPos += 1;

                return value;
            } else throw new Exception("Buffer attempted to read out of bounds. ");
        }

        public string ReadString(bool peek = true)
        {
            int length = ReadInteger(true);

            if (_bufferUpdate) {
                _readBuffer = _bufferList.ToArray();
                _bufferUpdate = false;
            }

            string value = Encoding.ASCII.GetString(_readBuffer, _readPos, length);

            if (peek & _bufferList.Count > _readPos) _readPos += length;

            return value;
        }

        // garbage collection / disposing our buffer

        private bool bufferNotDisposed = true;

        protected virtual void Dispose(bool disposing)
        {
            if (bufferNotDisposed) {
                if (disposing) _bufferList.Clear();

                _readPos = 0;
            }

            // flag as disposed
            bufferNotDisposed = false;
        }

        public void Dispose()
        {
            // call our disposer
            Dispose(true);

            // tell garbage collection not to finalize
            GC.SuppressFinalize(this);
        }
    }
}
