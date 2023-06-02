using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static BrickController;

public class BridgeController : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] BrickColor BCScriptable;
    [SerializeField] Vector3 offset = new Vector3(0f, 0.1f, 0.5f);

    public BrickType type = BrickType.White;

    public Material GetMaterial(BrickType type)
    {
        switch (type)
        {
            case BrickType.Green:
                return BCScriptable.Mats[0];
            case BrickType.Red:
                return BCScriptable.Mats[1];
            case BrickType.Purple:
                return BCScriptable.Mats[2];
            case BrickType.Yellow:
                return BCScriptable.Mats[3];
            case BrickType.White:
                return BCScriptable.Mats[4];
            case BrickType.Gray:
                return BCScriptable.Mats[5];
            default:
                return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Building(other);
        
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    PlayerController _pCon = other.GetComponent<PlayerController>();
    //    _pCon.CanMoveForward = true;
    //}

    public void DestroyStack(Collider other, int count, Character comp)
    {
        BrickController[] gameObject = other.gameObject.GetComponentsInChildren<BrickController>();
        comp.BrickList.RemoveAt(count - 1);
        //gameObject[count - 1].gameObject.SetActive(false);
        gameObject[count - 1].Release();
    }

    public void Building(Collider other)
    {
        //Player
        PlayerController _pCon = other.GetComponent<PlayerController>();
        Material _pMat = other.gameObject.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;

        Material _mat = GetComponent<MeshRenderer>().material;

        if (_pCon)
        {
            if (_pCon.BrickList.Count > 0)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                Debug.Log(_mat.ToString());
                if (type != _pCon._BrickType)
                {
                    type = _pCon._BrickType;
                    DestroyStack(other, _pCon.BrickList.Count, _pCon);
                    GetComponent<MeshRenderer>().material = _pMat;
                }
            }
            else if (type == _pCon._BrickType)
            {
                _pCon.CanMoveForward = true;
            }
            else
            {
                _pCon.CanMoveForward = false;
            }
        }

        //AI
        AI _AI = other.GetComponent<AI>();
        if (_AI)
        {
            if (_AI.GetComponent<AI>().BrickList.Count > 0)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;

                if (type != _AI._BrickType)
                {
                    type = _AI._BrickType;
                    DestroyStack(other, _AI.BrickList.Count, _AI);
                    GetComponent<MeshRenderer>().material = _pMat;
                    
                }
                _AI.MoveTo(transform.position + offset);
            }
            else
            {
                _AI.GetClosestBrick();
            }
        }
    }

}
