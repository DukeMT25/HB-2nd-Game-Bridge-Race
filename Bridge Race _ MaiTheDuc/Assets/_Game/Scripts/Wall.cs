using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        None
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            ConstrainPlayerMove(CalculateRelativePosition(player), player, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player)
        {
            ConstrainPlayerMove(CalculateRelativePosition(player), player, true);
        }

        player.CanMoveForward = true;
        player.CanMoveBackward = true;
        player.CanMoveLeft = true;
        player.CanMoveRight = true;
    }

    private void ConstrainPlayerMove(Direction direction, PlayerController player, bool canMove)
    {
        switch (direction)
        {
            case Direction.Forward:
                player.CanMoveForward = canMove;
                break;

            case Direction.Backward:
                player.CanMoveBackward = canMove;
                break;

            case Direction.Left:
                player.CanMoveLeft = canMove;
                break;

            case Direction.Right:
                player.CanMoveRight = canMove;
                break;

            default:
                break;
        }
    }

    private Direction CalculateRelativePosition(PlayerController player)
    {
        if (transform.rotation == Quaternion.identity)
        {
            if (player.transform.position.x > transform.position.x)
            {
                return Direction.Left;
            }
            else if (player.transform.position.x <= transform.position.x)
            {
                return Direction.Right;
            }
        }
        else
        {
            if (player.transform.position.z > transform.position.z)
            {
                return Direction.Backward;
            }
            else if (player.transform.position.z <= transform.position.z)
            {
                return Direction.Forward;
            }
        }

        return Direction.None;
    }
}