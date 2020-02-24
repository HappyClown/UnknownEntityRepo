using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public float tileSize;
    // The room size = amount of cells.
    public int minRoomTilesX, maxRoomTilesX;
    public int minRoomTilesY, maxRoomTilesY;
    int roomTilesX, roomTilesY;
    // Multiple of the room size as well as matching the tilegrid tile size?
    public int minAmntRoomsX, maxAmntRoomsX;
    public int minAmntRoomsY, maxAmntRoomsY;
    int roomsX, roomsY;
    Vector2 centeredLvlBotLeft;
    Room[,] rooms;
    // tileSize * amount of cells / 2 = middle of room
    // tileSize / 2 = middle of cell
    // tileSize * amount of cells * amnt of room = max grid
    public void CreateLevel() {
        roomsX = Random.Range(minAmntRoomsX, maxAmntRoomsX+1);
        roomsY = Random.Range(minAmntRoomsY, maxAmntRoomsY+1);
        roomTilesX = Random.Range(minRoomTilesX, maxRoomTilesX+1);
        roomTilesY = Random.Range(minRoomTilesY, maxRoomTilesY+1);
        float roomWidth = roomTilesX * tileSize;
        float roomHeight = roomTilesY * tileSize;
        rooms = new Room[roomsX, roomsY];
        centeredLvlBotLeft = (Vector2)this.transform.position - Vector2.right*roomsX*roomWidth/2 - Vector2.up*roomsY*roomHeight/2;

        for (int x = 0; x < roomsX; x++) {
            for (int y = 0; y < roomsY; y++) {
                Vector2 botLeftPos = new Vector2(centeredLvlBotLeft.x + x*roomWidth, centeredLvlBotLeft.y + y*roomHeight);
                rooms[x, y] = new Room(roomTilesX, roomTilesY, tileSize, botLeftPos);
                rooms[x, y].CreateRoom();
            }
        }
    }

    void OnDrawGizmos() {
        // Draw the rooms.
        Vector2 roomMidOffset = new Vector2(tileSize * roomTilesX / 2, tileSize * roomTilesY / 2);
        Vector2 roomSize = new Vector2(tileSize * roomTilesX, tileSize * roomTilesY);
        for (int x = 0; x < roomsX; x++) {
            for (int y = 0; y < roomsY; y++) {
                Vector3 center = new Vector3(centeredLvlBotLeft.x + x*roomSize.x + roomMidOffset.x, centeredLvlBotLeft.y + y*roomSize.y + roomMidOffset.y, 0);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(center, roomSize * 0.9f);
                // Draw the tiles.
                Vector2 roomBotLeft = new Vector2(center.x-roomMidOffset.x, center.y-roomMidOffset.y);
                float midOffset = tileSize / 2;
                Vector3 tileSizeVector = new Vector3(tileSize, tileSize, tileSize);
                for (int tileX = 0; tileX < roomTilesX; tileX++) {
                    for (int tileY = 0; tileY < roomTilesY; tileY++) {
                        Vector3 tileCenter = new Vector3(roomBotLeft.x+(tileX*tileSize)+midOffset, roomBotLeft.y+(tileY*tileSize)+midOffset, -10);
                        //PickGizmosColor(tileX, tileY);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(tileCenter, tileSizeVector * 0.8f);
                    }
                }
            }
        }
    }
}
