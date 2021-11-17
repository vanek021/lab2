using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class StaticHaffmanAlgorithm
    {
        private Dictionary<char, int> frequencyArray = new Dictionary<char, int>();
        private BinaryTree binaryTree = new BinaryTree();
        private List<BinaryTree> nodes = new List<BinaryTree>();

        public void ResetDictionary(string message)
        {
            frequencyArray = new Dictionary<char, int>();
            foreach (var e in message)
            {
                if (!frequencyArray.Keys.Contains(e))
                    frequencyArray.Add(e, 1);
                else
                    frequencyArray[e]++;

            }

            foreach (var symbol in frequencyArray)
            {
                nodes.Add(new BinaryTree() { Value = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<BinaryTree> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    List<BinaryTree> taken = orderedNodes.Take(2).ToList();

                    var parent = new BinaryTree()
                    {
                        Value = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                binaryTree = nodes.FirstOrDefault();

            }
        }

        public BitArray EncodeMessage(string message)
        {
            ResetDictionary(message);
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < message.Length; i++)
            {
                List<bool> encodedSymbol = binaryTree.Add(message[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public string Decode(BitArray bits)
        {
            BinaryTree current = binaryTree;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                        current = current.Right;
                }
                else
                {
                    if (current.Left != null)
                        current = current.Left;
                }

                if (current.IsLeaf())
                {
                    decoded += current.Value;
                    current = binaryTree;
                }
            }

            return decoded;
        }
    }

    class BinaryTree
    {
        public BinaryTree Left;
        public BinaryTree Right;
        public int Frequency;
        public char Value;

        public List<bool> Add(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                if (symbol.Equals(Value))
                    return data;
                else
                    return null;
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Add(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Add(symbol, rightPath);
                }

                if (left != null)
                    return left;
                else
                    return right;
            }
        }

        public bool IsLeaf()
        {
            return (Left == null && Right == null);
        }
    }
}
