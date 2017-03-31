﻿using System;

namespace DialogueSystem
{
    public class DialogueAnswer
    {

        private string _message;
        private int _next;
        private Func<bool> _condition;

        public string Message {
			get { return _message;}
        }

        public int Next
        {
            get { return _next; }
        }

        public bool IsVisible
        {
            get { return _condition(); }
        }

        public void SetCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public DialogueAnswer(string message, int next)
        {
            this._message = message;
            this._next = next;
            this._condition = () => true;
        }
    }
}