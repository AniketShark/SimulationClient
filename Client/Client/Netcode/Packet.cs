using System.Text;

namespace Client.Netcode
{
	public class Packet : IDisposable
	{
		private List<byte> _buffer;
		private byte[] _readBuffer;
		private int _readPosition;
		private int _length;
		private bool _bufferUpdated;

		public Packet()
		{
			_buffer = new List<byte>();
			_readPosition = 0;
			_length = 0;
		}

		public int GetReadPos()
		{
			return _readPosition;
		}

		public byte[] ToArray()
		{
			return _buffer.ToArray();
		}
		public int Count()
		{
			return _buffer.Count();
		}
		public int LengthRemaining()
		{
			return Count() - _readPosition;
		}

		public void Clear()
		{
			_buffer.Clear();
			_readPosition = 0;
		}
		public void WriteLength()
		{
			_buffer.InsertRange(0, BitConverter.GetBytes(_buffer.Count));
		}

		public void WriteByte(byte input)
		{
			_buffer.Add(input);
			_bufferUpdated = true;
		}
		public void WriteBytes(byte[] input)
		{
			_buffer.AddRange(input);
			_bufferUpdated = true;
		}
		public void WriteShort(short value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value));
			_bufferUpdated = true;
		}
		public void WriteInt(int value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value));
			_bufferUpdated = true;
		}
		public void WriteLong(long value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value));
			_bufferUpdated = true;
		}
		public void WriteFloat(float value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value));
			_bufferUpdated = true;
		}
		public void WriteBool(bool value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value));
			_bufferUpdated = true;
		}
		public void WriteString(string value)
		{
			_buffer.AddRange(BitConverter.GetBytes(value.Length));
			_buffer.AddRange(Encoding.ASCII.GetBytes(value));
			_bufferUpdated = true;
		}
		public byte ReadByte(bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				byte value = _readBuffer[_readPosition];

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 1;
				}
				return value;
			}
			else
			{
				throw new Exception("You are not trying to read out a 'byte'");
			}

		}
		public byte[] ReadBytes(int length, bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				byte[] value = _buffer.GetRange(_readPosition, length).ToArray();

				if (peek)
				{
					_readPosition += length;
				}
				return value;
			}
			else
			{
				throw new Exception("You are not trying to read out a 'byte[]'");
			}
		}
		public short ReadShort(bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				short value = BitConverter.ToInt16(_readBuffer, _readPosition);

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 2;
				}
				return value;
			}
			else
			{
				throw new Exception("You are trying to read out a 'short'");
			}
		}
		public int ReadInt(bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				int value = BitConverter.ToInt32(_readBuffer, _readPosition);

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 4;
				}
				return value;
			}
			else
			{
				throw new Exception("You are trying to read out a 'int'");
			}
		}
		public long ReadLong(bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				long value = BitConverter.ToInt64(_readBuffer, _readPosition);

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 8;
				}
				return value;
			}
			else
			{
				throw new Exception("You are trying to read out a 'long'");
			}
		}
		public float ReadFloat(bool peek = true)
		{

			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				float value = BitConverter.ToSingle(_readBuffer, _readPosition);

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 4;
				}
				return value;
			}
			else
			{
				throw new Exception("You are trying to read out a 'float'");
			}
		}
		public bool ReadBool(bool peek = true)
		{
			if (_buffer.Count > _readPosition)
			{
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}

				bool value = BitConverter.ToBoolean(_readBuffer, _readPosition);

				if (peek & _buffer.Count > _readPosition)
				{
					_readPosition += 1;
				}
				return value;
			}
			else
			{
				throw new Exception("You are trying to read out a 'bool'");
			}
		}
		public string ReadString(bool peek = true)
		{
			try
			{
				int length = ReadInt(peek);
				if (_bufferUpdated)
				{
					_readBuffer = _buffer.ToArray();
					_bufferUpdated = false;
				}
				string value = Encoding.ASCII.GetString(_readBuffer, _readPosition, length);

				if (peek & _buffer.Count > _readPosition)
				{
					if (value.Length > 0)
						_readPosition += length;
				}
				return value;
			}
			catch (Exception ex)
			{
				throw new Exception("You are not trying to read 'string'");
			}
		}

		private bool _disposeValue = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				_buffer.Clear();
				_readPosition = 0;
				_disposeValue = true;
			}

		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

}
