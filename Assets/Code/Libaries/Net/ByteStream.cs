using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Code.Libaries.Net
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
                Application.Quit();
                throw new Exception("Broken bytestream.");
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
        
        public void addString(String s)
        {
            if (s == null)
                s = "null";
            addShort(s.Length);
            foreach (char c in s.ToCharArray()) {
                addByte((byte)c);
            }
        }


        public void addFloat4B(float f)
        {
            addInt((int)(f * 1000));
        }

        public void addFloat2B(float f)
        {
            addShort((int)(f * 100));
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

        public float getFloat4B()
        {
            return (float)getInt() / 1000f;
        }

        public float getFloat2B()
        {
            return (float)getShort() / 100f;
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

        public void addFlag(params bool[] updates)
        {
            int i = 0;
            if (updates.Length >= 1)
            {
                 i = (i | (updates[0] ? 0x01 : 0x00));
            }
            if (updates.Length >= 2)
            {
                i = (i | (updates[1] ? 0x02 : 0x00));
            }
            if (updates.Length >= 3)
            {
                i = (i | (updates[2] ? 0x04 : 0x00));
            }
            if (updates.Length >= 4)
            {
                i = (i | (updates[3] ? 0x08 : 0x00));
            }
            if (updates.Length >= 5)
            {
                i = (i | (updates[4] ? 0x10 : 0x00));
            }
            if (updates.Length >= 6)
            {
                i = (i | (updates[5] ? 0x20 : 0x00));
            }
            if (updates.Length >= 7)
            {
                i = (i | (updates[6] ? 0x40 : 0x00));
            }
            if (updates.Length >= 8)
            {
                i = (i | (updates[7] ? 0x80 : 0x00));
            }
            addByte(i);
        }

        public void addBytes(byte[] p)
        {
            stream.AddRange(p);
        }

        public byte[] getSubBuffer(int lenght)
        {
            byte[] bytes = new byte[lenght];
            for (int i = 0; i < lenght; i++)
            {
                bytes[i] = (byte) getByte();
            }
            return bytes;
        }

        public void addPosition6B(Vector3 Position)
        {
            addFloat2B(Position.x);
            addFloat2B(Position.y);
            addFloat2B(Position.z);
        }

        internal Vector3 getPosition6B()
        {
            return new Vector3(getFloat2B(),getFloat2B(),getFloat2B());
        }

        internal BitArray GetBitArray()
        {
            return new BitArray(new[] { getByte() });
        }

        public int GetSize()
        {
            return stream.Count;
        }
    }
}

