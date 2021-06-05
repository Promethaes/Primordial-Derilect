using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum PacketType {
    Welcome = 1,

    PlayerMovement,
}

public class Packet : IDisposable {

    public Packet() { }
    public Packet(int id) { WriteInt(id); }
    public Packet(PacketType id) { WriteInt((int)id); }

    //UDP packet constructor
    //public Packet(ClientPacket messageID, int lobbyID, int clientID, int playerID){
    //    WriteInt((int)messageID);
    //    WriteInt(lobbyID);
    //    WriteInt(clientID);
    //    WriteInt(playerID);
    //}
    public Packet(byte[] data) { SetBytes(data); }

    private List<byte> _buffer = new List<byte>();
    private byte[] _readableBuffer;
    private int _readPos = 0;

    public byte[] Buffer { get { return _readableBuffer = _buffer.ToArray(); } }
    public int Length { get { return _buffer.Count; } }
    public int UnreadLength { get { return Length - _readPos; } }

    public void Reset(bool shouldReset = true) {
        if (!shouldReset) {
            _readPos -= 4;
            return;
        }

        _buffer.Clear();
        _readableBuffer = null;
        _readPos = 0;
    }

    public void SetBytes(byte[] data) {
        WriteByteArray(data);
        _readableBuffer = _buffer.ToArray();
    }

    public void WriteLength() {
        _buffer.InsertRange(0, BitConverter.GetBytes(_buffer.Count));
    }

    public void InsertID(int value) {
        _buffer.InsertRange(0, BitConverter.GetBytes(value));
    }

    //use when setting up lobbies
    public void InsertAllID(int lobbyID, int clientID, int playerID) {
        _buffer.InsertRange(0, BitConverter.GetBytes(playerID));
        _buffer.InsertRange(0, BitConverter.GetBytes(clientID));
        _buffer.InsertRange(0, BitConverter.GetBytes(lobbyID));
    }

    public void WriteByte(byte value) { _buffer.Add(value); }
    public void WriteByteArray(byte[] value) { _buffer.AddRange(value); }
    public void WriteBool(bool value) { _buffer.AddRange(BitConverter.GetBytes(value)); }
    public void WriteInt(int value) { _buffer.AddRange(BitConverter.GetBytes(value)); }
    public void WriteFloat(float value) { _buffer.AddRange(BitConverter.GetBytes(value)); }
    public void WriteString(string value) { WriteInt(value.Length); _buffer.AddRange(Encoding.ASCII.GetBytes(value)); }

    public void WriteVec2(Vector2 value) { WriteFloat(value.x); WriteFloat(value.y); }
    public void WriteVec3(Vector3 value) { WriteFloat(value.x); WriteFloat(value.y); WriteFloat(value.z); }
    public void WriteVec4(Vector4 value) { WriteFloat(value.x); WriteFloat(value.y); WriteFloat(value.z); WriteFloat(value.w); }
    public void WriteQuat(Quaternion value) { WriteFloat(value.x); WriteFloat(value.y); WriteFloat(value.z); WriteFloat(value.w); }

    public byte ReadByte(bool moveReadPos = true) {

        byte value = _readableBuffer[_readPos];
        if (moveReadPos) _readPos += sizeof(byte);
        return value;
    }

    public byte[] ReadBytes(int length, bool moveReadPos = true) {

        byte[] value = _buffer.GetRange(_readPos, length).ToArray();
        if (moveReadPos) _readPos += length;
        return value;
    }

    public bool ReadBool(bool moveReadPos = true) {

        bool value = BitConverter.ToBoolean(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += sizeof(bool);
        return value;
    }

    public int ReadInt(bool moveReadPos = true) {

        int value = BitConverter.ToInt32(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += sizeof(int);
        return value;
    }

    public float ReadFloat(bool moveReadPos = true) {

        float value = BitConverter.ToSingle(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += sizeof(float);
        return value;
    }

    public string ReadString(bool moveReadPos = true) {
        try {
            int len = ReadInt();
            string value = Encoding.ASCII.GetString(_readableBuffer, _readPos, len);
            if (moveReadPos && value.Length > 0) _readPos += len;
            return value;
        } catch {
            throw new Exception($"Could not read value at read position {_readPos} as type \"string\"!");
        }
    }

    public Vector2 ReadVector2(bool moveReadPos = true) {
        var x = ReadFloat();
        var y = ReadFloat();
        return new Vector2(x, y);
    }

    public Vector3 ReadVector3(bool moveReadPos = true) {
        var x = ReadFloat();
        var y = ReadFloat();
        var z = ReadFloat();
        return new Vector3(x, y, z);
    }

    public Vector4 ReadVector4(bool moveReadPos = true) {
        var x = ReadFloat();
        var y = ReadFloat();
        var z = ReadFloat();
        var w = ReadFloat();
        return new Vector4(x, y, z, w);
    }

    public Quaternion ReadQuaternion(bool moveReadPos = true) {
        var x = ReadFloat();
        var y = ReadFloat();
        var z = ReadFloat();
        var w = ReadFloat();
        return new Quaternion(x, y, z, w);
    }


    private bool _disposed = false;

    protected virtual void Dispose(bool disposing) {
        if (_disposed) {
            return;
        }

        if (disposing) {
            _buffer = null;
            _readableBuffer = null;
            _readPos = 0;
        }

        _disposed = true;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}