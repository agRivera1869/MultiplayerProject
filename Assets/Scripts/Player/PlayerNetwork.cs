using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Vector3> _netScale = new(writePerm: NetworkVariableWritePermission.Owner);
    void Update()
    {
        //if player controls this object
        if (IsOwner)
        {
            _netPos.Value = transform.position;
            _netScale.Value = transform.localScale;
        }
        //if they dont
        else
        {
            transform.position = _netPos.Value;
            transform.localScale = _netScale.Value;
        }
    }

    struct PlayerNetworkData
    {
        private float _x, _y;   //track x and y position
        private short _xScale;  //check x scale for flipping sprite

        //update position on the x and y axis
        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }
        //update scale on the x axis
        internal Vector3 Scale
        {
            get => new Vector3(_xScale, 1, 1);
            set => _xScale = (short)value.x;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _xScale);
        }
    }
}
