using System;
using System.Collections.Generic;
using UnityEngine;

namespace OldBlood.Code.Libaries.Net
{
    public class ByteStream
    {
        private List<byte> stream;
        private int _offset = 0;

        public int Offset{
            get{return _offset;}
            set{_offset = value;}
        } 

        public ByteStream()
        {
            stream = new List<byte>();
        }

        public ByteStream(int lenght)
        {
            stream = new List<byte>(lenght);
        }

        public ByteStream(byte[] bytes)
        {
            stream = new List<byte>(bytes.Length);
            foreach (var _b in bytes)
            {
                addByte(_b);
            }
        }

        public void addByte(int value) {
            addByte(value, this._offset++);
        }
        
        public void addByte(int value, int pos) {
            if (pos < stream.Count) {
                stream[pos] = (byte) value;
            } else {
                stream.Add((byte) value);
            }
        }

        public int getByte() {
            if (stream.Count > _offset) {
                return (int)stream[_offset++]; 
            }else{
                Debug.LogError("Readigngdsg streamsize: "+stream.Count+" off: "+Offset);
                return 0;
            }
        }

        public void addShort(int i) {
             addByte(i >> 8);
            addByte(i);
        }
        
        public void addInt(int i) {
            addByte(i >> 24);
            addByte(i >> 16);
            addByte(i >> 8);
            addByte(i);
        }
        
        public void addLong(long l) {
            addByte((int) (l >> 56));
            addByte((int) (l >> 48));
            addByte((int) (l >> 40));
            addByte((int) (l >> 32));
            addByte((int) (l >> 24));
            addByte((int) (l >> 16));
            addByte((int) (l >> 8));
            addByte((int) l);
        }
        
        public void addString(String s) {
            addShort(s.Length);
            foreach (char c in s.ToCharArray()) {
                addByte((byte)c);
            }
        }

        
        public void addFloat(float f) {
            addInt((int)(f * 1000));
        }
        
        public int getUnsignedByte() {
            return getByte() & 0xFF;
        }
        
        public int getShort() {
            int i = (getUnsignedByte() << 8) + getUnsignedByte();
            if (i > 32767) {
                 i -= 65536;
            }
            return i;
        }
        
        public int getShortBE() {
            int i = getUnsignedShortBE();
            if (i > 32767) {
                i -= 65536;
            }
            return i;
        }
        
        public int getUnsignedShort() {
            return (getUnsignedByte() << 8) + getUnsignedByte();
        }
        
        public int getUnsignedShortBE() {
            return getUnsignedByte() + (getUnsignedByte() << 8);
        }
        
        public int getInt() {
             return (getUnsignedByte() << 24) + (getUnsignedByte() << 16) + (getUnsignedByte() << 8) + getUnsignedByte();
        }
        
        public int getIntBE() {
            return getUnsignedByte() + (getUnsignedByte() << 8) + (getUnsignedByte() << 16) + (getUnsignedByte() << 24);
        }
        
        public long getLong() {
            long l = getInt() & 0xFFFFFFFF;
            long l1 = getInt() & 0xFFFFFFFF;
            return (l << 32) + l1;
        }
        
        public String getString() {
            String s = "";
            int size = getUnsignedShort();
            for (int i = 0; i < size; i++)
            {
                s += (char) getByte();
            }
            return s;
        }
        
        public float getFloat() {
            return (float) getInt() / 1000f;
        }

        public byte[] GetBuffer()
        {
            return stream.ToArray();
        }

        public int Length {
            get {
                return stream.Count;
            }
            set {
                stream.Capacity = value;
            }
        }

        private static byte[] GetBytesOfString(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}

