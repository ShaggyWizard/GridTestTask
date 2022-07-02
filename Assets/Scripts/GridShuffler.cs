using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace GridTest
{
    public static class GridShuffler
    {
        public enum ShuffleType
        {
            RandomType,
            Random,
            Vertical,
            Horizontal,
            Vortex,
            CrissCross,
            NumberOfTypes
        }
        public static Letter[,] Shuffle(Letter[,] letters, ShuffleType type = ShuffleType.RandomType)
        {
            if (type == ShuffleType.RandomType)
            {
                type = (ShuffleType)Random.Range(0, (int)ShuffleType.NumberOfTypes);
            }
            switch (type)
            {
                case ShuffleType.Random:
                    RandomPoints(ref letters);
                    break;
                case ShuffleType.Vertical:
                    Vertical(ref letters);
                    break;
                case ShuffleType.Horizontal:
                    Horizontal(ref letters);
                    break;
                case ShuffleType.Vortex:
                    Vortex(ref letters);
                    break;
                case ShuffleType.CrissCross:
                    CrissCross(ref letters);
                    break;
                default:
                    RandomPoints(ref letters);
                    break;
            }
            return letters;
        }

        private static void RandomPoints(ref Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            List<Letter> lettersList = new List<Letter>();

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    lettersList.Add(letters[x, y]);
                }
            }

            Stack<Letter> lettersStack = GetShuffledStack(lettersList);

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    letters[x, y] = lettersStack.Pop();
                }
            }
        }
        private static void Vertical(ref Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            for (int x = 0; x < resolution.x; x++)
            {
                List<Letter> lettersList = new List<Letter>();

                for (int y = 0; y < resolution.y; y++)
                {
                    lettersList.Add(letters[x, y]);
                }

                Stack<Letter> letterStack = GetShuffledStack(lettersList);

                for (int y = 0; y < resolution.y; y++)
                {
                    letters[x, y] = letterStack.Pop();
                }
            }
        }
        private static void Horizontal(ref Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            for (int y = 0; y < resolution.y; y++)
            {
                List<Letter> lettersList = new List<Letter>();

                for (int x = 0; x < resolution.x; x++)
                {
                    lettersList.Add(letters[x, y]);
                }

                Stack<Letter> letterStack = GetShuffledStack(lettersList);

                for (int x = 0; x < resolution.x; x++)
                {
                    letters[x, y] = letterStack.Pop();
                }
            }
        }
        private static void Vortex(ref Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            Stack<Letter>[] randomizedStacks = GetShuffledStacks(letters);

            Stack<Letter>[] finalStacks = CloneStacks(randomizedStacks);

            OverlayStack(ref finalStacks[0], randomizedStacks[2]);
            OverlayStack(ref finalStacks[6], randomizedStacks[0]);
            OverlayStack(ref finalStacks[8], randomizedStacks[6]);
            OverlayStack(ref finalStacks[2], randomizedStacks[8]);

            OverlayStack(ref finalStacks[1], randomizedStacks[5]);
            OverlayStack(ref finalStacks[3], randomizedStacks[1]);
            OverlayStack(ref finalStacks[7], randomizedStacks[3]);
            OverlayStack(ref finalStacks[5], randomizedStacks[7]);

            FillLetters(ref letters, MergeStacks(finalStacks, resolution));
        }
        private static void CrissCross(ref Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            Stack<Letter>[] randomizedStacks = GetShuffledStacks(letters);
            Stack<Letter>[] finalStack = CloneStacks(randomizedStacks);

            for (int i = 0; i < randomizedStacks.Length; i++)
            {
                OverlayStack(ref finalStack[i], randomizedStacks[randomizedStacks.Length - i - 1]);
            }

            FillLetters(ref letters, MergeStacks(finalStack, resolution));
        }



        //Fills letters array with given stack
        private static void FillLetters(ref Letter[,] letters, Stack<Letter> reverseStack)
        {
            if (letters.Length != reverseStack.Count)
            {
                Debug.LogError("Sizes don't match");
                return;
            }

            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    letters[x, y] = reverseStack.Pop();
                }
            }
        }

        //Shuffles letters in stack
        private static Stack<Letter> GetShuffledStack(List<Letter> list)
        {
            Stack<Letter> stack = new Stack<Letter>();

            int length = list.Count;

            bool[] occupied = new bool[length];

            for (int i = 0; i < length; i++)
            {
                int rand = UnityEngine.Random.Range(0, length);
                while (occupied[rand])
                {
                    rand = rand + 1 < length ? rand + 1 : 0;
                }
                occupied[rand] = true;
                stack.Push(list[rand]);
            }
            return stack;
        }

        //Divides array into 9 stacks and shuffles letters in them
        private static Stack<Letter>[] GetShuffledStacks(Letter[,] letters)
        {
            Vector2Int resolution = new Vector2Int(letters.GetLength(0), letters.GetLength(1));
            Vector2Int half = resolution / 2;
            Vector2Int center = new Vector2Int(resolution.x % 2 == 0 ? -1 : half.x,
                                               resolution.y % 2 == 0 ? -1 : half.y);

            int stacksCount = 9;
            Stack<Letter>[] stacks = new Stack<Letter>[stacksCount];
            for (int i = 0; i < stacksCount; i++)
            {
                stacks[i] = new Stack<Letter>();
            }

            // 6 7 8
            // 3 4 5
            // 0 1 2

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    bool centerX = x == center.x;
                    bool centerY = y == center.y;

                    bool left = x < half.x;
                    bool bot = y < half.y;

                    int i = left ? 0 : centerX ? 1 : 2;
                    i += bot ? 0 : centerY ? 3 : 6;

                    stacks[i].Push(letters[x, y]);
                }
            }

            for (int i = 0; i < stacksCount; i++)
            {
                stacks[i] = GetShuffledStack(new List<Letter>(stacks[i]));
            }

            return stacks;
        }

        private static Stack<Letter>[] CloneStacks(Stack<Letter>[] randomizedStacks)
        {
            Stack<Letter>[] clone = new Stack<Letter>[randomizedStacks.Length];
            for (int i = 0; i < randomizedStacks.Length; i++)
            {
                clone[i] = new Stack<Letter>(new Stack<Letter>(randomizedStacks[i]));
            }
            return clone;
        }

        private static void OverlayStack(ref Stack<Letter> target, Stack<Letter> source)
        {
            int range = Mathf.Min(target.Count, source.Count);
            for (int i = 0; i < range; i++)
            {
                target.Pop();
            }
            for (int i = 0; i < range; i++)
            {
                target.Push(source.Pop());
            }
        }

        private static Stack<Letter> MergeStacks(Stack<Letter>[] stacks, Vector2Int resolution)
        {
            Vector2Int half = resolution / 2;
            Vector2Int center = new Vector2Int(resolution.x % 2 == 0 ? -1 : half.x,
                                               resolution.y % 2 == 0 ? -1 : half.y);

            Stack<Letter> stack = new Stack<Letter>();

            for (int x = resolution.x - 1; x >= 0; x--)
            {
                for (int y = resolution.y - 1; y >= 0; y--)
                {
                    bool centerX = x == center.x;
                    bool centerY = y == center.y;

                    bool left = x < half.x;
                    bool bot = y < half.y;

                    int i = left ? 0 : centerX ? 1 : 2;
                    i += bot ? 0 : centerY ? 3 : 6;

                    stack.Push(stacks[i].Pop());
                }
            }
            return stack;
        }
    }
}