using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStar;
using AStar.Options;

public class MarkPath : MonoBehaviour
{
    public static void Mark(BasicMazeGenerator maze, Vector2[] points)
    {
        // Create a tile matrix from the maze to feed into A*
        short[,] tiles = new short[maze.RowCount * 3, maze.ColumnCount * 3];

        for (int row = 0; row < maze.RowCount; row++)
        {
            for (int col = 0; col < maze.ColumnCount; col++)
            {
                MazeCell cell = maze.GetMazeCell(row, col);

                int y = row * 3 + 1;
                int x = col * 3 + 1;

                // Center is always unoccupied
                tiles[y, x] = 1;

                // Walls
                tiles[y, x - 1] = (short)(cell.WallLeft  ? 0 : 1); // Left
                tiles[y, x + 1] = (short)(cell.WallRight ? 0 : 1); // Right
                tiles[y + 1, x] = (short)(cell.WallFront ? 0 : 1); // Front
                tiles[y - 1, x] = (short)(cell.WallBack  ? 0 : 1); // Back

                // Fill corners
                tiles[y - 1, x - 1] = 0;
                tiles[y + 1, x - 1] = 0;
                tiles[y - 1, x + 1] = 0;
                tiles[y + 1, x + 1] = 0;
            }
        }

        // Perform A* from one coin to all others
        WorldGrid world = new WorldGrid(tiles);
        
        PathFinderOptions pathfinderOptions = new PathFinderOptions
        {
            PunishChangeDirection = true,
            UseDiagonals = false,
        };

        PathFinder pathfinder = new PathFinder(world, pathfinderOptions);

        // Path from each point to the points ahead of it
        for (int i = 0; i < points.Length; i++) {
            for (int j = i + 1; j < points.Length; j++)
            {
                Position from = new Position((int) points[i].x * 3 + 1, (int) points[i].y * 3 + 1);
                Position to = new Position((int) points[j].x * 3 + 1, (int) points[j].y * 3 + 1);

                // Position is row, col
                Position[] path = pathfinder.FindPath(from, to);

                if (path.Length == 0)
                {
                    Debug.Log("No Path Found!");
                }

                // Make selective areas poison if they're not in any path
                foreach (Position p in path)
                {
                    // If position is a cell
                    if (((p.Row - 1) % 3 == 0) && ((p.Column - 1) % 3 == 0))
                    {
                        int cellRow = (p.Row - 1) / 3;
                        int cellCol = (p.Column - 1) / 3;

                        // Mark it as in path
                        maze.GetMazeCell(cellRow, cellCol).InPath = true;
                    }
                }
            }
        }
    }
}
