using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAI.Shared.Solvers
{
    public class GeniusSquareSolutionFactory
    {
        // Let's embrace .NET 5.0 by using a record!
        public record Shape(int Id, int Height, int Width, int[] Offsets);

        const int ShapeSize = 4; // We can represent all shapes in a 4 x 4 grid
        const int GridSize = 6;

        private Dictionary<int, Shape[]> shapeMap;

        // Scale gene value (an int from 0 to 359) to an index dependent on available values.
        // E.g. The T shape can be rotated 4 ways so a value from 90 to 179 may represent a 90° rotation.
        // Similarly, the square shape has a height and width of 2 so it's X and Y values must be
        // scaled to fall in the range 0 to 4 so that it does not fall off the board.
        int ScaleValue(int value, int elementsInGroup)
        {
            return value / (GeniusSquareSolver.MaxValue / elementsInGroup);
        }

        // Get cell indexes that shape will occupy based on it's orientation, column & row
        public int[] GetIndexes(int shapeId, int shapeValue, int xValue, int yValue)
        {
            var shapeGroup = shapeMap[shapeId];

            var shapeIndex = ScaleValue(shapeValue, shapeGroup.Length);

            var shape = shapeGroup[shapeIndex];

            var x = ScaleValue(xValue, (GridSize + 1) - shape.Width);
            var y = ScaleValue(yValue, (GridSize + 1) - shape.Height);

            var idx = y * GridSize + x;

            return shape.Offsets.Select(o => idx + o).ToArray();
        }

        // Read 4x4 shape representations (including all orientations) into a Shape
        // object that we can store in our dictionary
        Shape ParseShape(int id, string[] rows)
        {
            var offsets = new List<int>();
            int width = 0;
            int height = 0;

            for (int row = 0; row < ShapeSize; row++)
            {
                for (int col = 0; col < ShapeSize; col++)
                {
                    if (rows[row][col] == '⬛')
                    {
                        offsets.Add(row * GridSize + col);

                        height = Math.Max(height, row);
                        width = Math.Max(width, col);
                    }
                }
            }

            return new Shape(id, height + 1, width + 1, offsets.ToArray());
        }

        Shape[] ParseShapeGroup(string s, int id)
        {
            var rows = s.Split(Environment.NewLine);

            var groups =
                rows
                .Select(x => x.TrimStart().Split(' '))
                .Transpose()
                .Select(x => x.ToArray())
                .ToArray();

            return groups.Select(x => ParseShape(id, x)).ToArray();
        }

        public GeniusSquareSolutionFactory()
        {
            shapeMap =
                shapes
                .Select(ParseShapeGroup)
                .Index()
                .ToDictionary(x => x.Key, x => x.Value);
        }

        // There's probably a better way of doing this, but I found
        // it strangely satisfying typing them out like this :)
        // Try to arrange in descending order of difficulty to place
        string[] shapes =
            new[]
            {
                // 2
                @"⬛⬛⬜⬜
                  ⬛⬛⬜⬜
                  ⬜⬜⬜⬜
                  ⬜⬜⬜⬜",

                // 3
                @"⬛⬛⬛⬛ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬛⬜⬜⬜",

                // 4
                @"⬛⬜⬜⬜ ⬜⬛⬛⬜ ⬜⬛⬜⬜ ⬛⬛⬜⬜
                  ⬛⬛⬜⬜ ⬛⬛⬜⬜ ⬛⬛⬜⬜ ⬜⬛⬛⬜
                  ⬜⬛⬜⬜ ⬜⬜⬜⬜ ⬛⬜⬜⬜ ⬜⬜⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜",

                // 5
                @"⬛⬜⬜⬜ ⬛⬛⬛⬜ ⬜⬛⬜⬜ ⬜⬛⬜⬜
                  ⬛⬛⬜⬜ ⬜⬛⬜⬜ ⬛⬛⬜⬜ ⬛⬛⬛⬜
                  ⬛⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬛⬜⬜ ⬜⬜⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜",

                // 6
                @"⬜⬜⬛⬜ ⬛⬛⬜⬜ ⬛⬛⬛⬜ ⬛⬜⬜⬜ ⬛⬜⬜⬜ ⬛⬛⬜⬜ ⬛⬛⬛⬜ ⬜⬛⬜⬜
                  ⬛⬛⬛⬜ ⬜⬛⬜⬜ ⬛⬜⬜⬜ ⬛⬜⬜⬜ ⬛⬛⬛⬜ ⬛⬜⬜⬜ ⬜⬜⬛⬜ ⬜⬛⬜⬜
                  ⬜⬜⬜⬜ ⬜⬛⬜⬜ ⬜⬜⬜⬜ ⬛⬛⬜⬜ ⬜⬜⬜⬜ ⬛⬜⬜⬜ ⬜⬜⬜⬜ ⬛⬛⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜",

                // 7
                @"⬛⬛⬛⬜ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬛⬜⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜",

                // 8
                @"⬛⬛⬜⬜ ⬛⬛⬜⬜ ⬜⬛⬜⬜ ⬛⬜⬜⬜
                  ⬛⬜⬜⬜ ⬜⬛⬜⬜ ⬛⬛⬜⬜ ⬛⬛⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜
                  ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜ ⬜⬜⬜⬜",

                // Easier just to drop double and single squares in when three gaps remains
                // 9
                //@"⬛⬛⬜⬜ ⬛⬜⬜⬜
                //  ⬜⬜⬜⬜ ⬛⬜⬜⬜
                //  ⬜⬜⬜⬜ ⬜⬜⬜⬜
                //  ⬜⬜⬜⬜ ⬜⬜⬜⬜"

                // 10
                //@"⬛⬜⬜⬜
                //  ⬜⬜⬜⬜
                //  ⬜⬜⬜⬜
                //  ⬜⬜⬜⬜"
            };
    }
}
