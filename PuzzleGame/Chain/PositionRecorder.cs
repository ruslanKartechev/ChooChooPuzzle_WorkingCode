using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class PositionRecorder
    {
        private List<PositionData> _data = new List<PositionData>();
        public List<PositionData> Data { get { return _data; } }
        public void RecordPosision(PositionData prevPosition)
        {
            _data.Add(prevPosition);
        //    UnityEngine.Debug.Log("Recorded position");
                
        }
        public void RemoveAt(int num)
        {
            PositionData res = _data.Find(x => x.num == num);
            if (res != null)
                _data.Remove(res);
        }
        public PositionData GetLast()
        {
            PositionData res = null;
            if (_data.Count > 0)
                res = _data[_data.Count - 1];
            return res;
        }
        public void RemoveLast()
        {
            if (_data.Count > 0)
            {
                _data.RemoveAt(_data.Count - 1);
            }
        }
    }
}
