﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueGraph
    {
        private Dictionary<int, DialogueNode> _nodes;
        private int _currentNodeId = 0;

        public string Id { get; set; }

        public DialogueNode CurrentNode
        {
            get { return _nodes[CurrentNodeId]; }
        }

        public int CurrentNodeId
        {
            get { return _currentNodeId; }
            set { _currentNodeId = value; }
        }

        public override bool Equals(object obj)
        {
            var dialogue = obj as DialogueGraph;
            if (dialogue == null)
            {
                return false;
            }
            return Enumerable.SequenceEqual(_nodes, dialogue._nodes) && CurrentNodeId == dialogue.CurrentNodeId;
        }

        public override string ToString()
        {
            var result = "[\n";

            foreach (var node in _nodes.Values)
            {
                result += node.ToString() + ",\n";
            }
            result += "]\n}";
            return result;
        }

        public void ChooseAnswer(uint index)
        {
            CurrentNodeId = CurrentNode.Answers[index].Next;
        }

        public DialogueGraph()
        {
            this._nodes = new Dictionary<int, DialogueNode>();
        }

        public DialogueGraph(Dictionary<int, DialogueNode> nodes)
        {
            this._nodes = nodes;
        }

        public DialogueGraph(string id)
        {
            this._nodes = new Dictionary<int, DialogueNode>();
            Id = id;
        }

        public DialogueGraph(Dictionary<int, DialogueNode> nodes, string id)
        {
            this._nodes = nodes;
            Id = id;
        }


        public void addNode(int index, DialogueNode node)
        {
            _nodes.Add(index, node);
        }
    }
}