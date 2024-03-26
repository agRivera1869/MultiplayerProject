using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Vector3> _netScale = new(writePerm: NetworkVariableWritePermission.Owner);
    void Update()
    {
        transform.position = _netPos.Value;
        transform.localScale = _netScale.Value;
    }

    struct EnemyNetworkData
    {
        private float _x, _y;
        private short _xScale;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {
                _x = value.x;
                _y = value.y;
            }
        }

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
