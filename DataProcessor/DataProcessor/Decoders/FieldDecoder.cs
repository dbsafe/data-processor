﻿using DataProcessor.Contracts;
using DataProcessor.Models;
using System;
using System.Text.RegularExpressions;

namespace DataProcessor.Decoders
{
    public abstract class FieldDecoder : IFieldDecoder
    {
        private string _pattern;
        private Regex _regex;
        public bool IsMatch { get; private set; }
        public string Pattern
        {
            get => _pattern;
            set
            {

                _pattern = value;
                _regex = new Regex($"^{_pattern}$", RegexOptions.Compiled);
            }
        }
        public ValidationResultType FailValidationResult { get; set; }

        public virtual void Decode(Field field)
        {
            EnsureThatPropertiesAreInitialized();
            IsMatch = _regex.IsMatch(field.Raw);
        }

        protected void EnsureThatPropertiesAreInitialized()
        {
            EnsureThatPropertyIsInitialized(nameof(Pattern), Pattern);
            EnsureThatPropertyIsInitialized(nameof(FailValidationResult), FailValidationResult);
        }

        protected void EnsureThatPropertyIsInitialized(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Property {name} cannot be empty or null");
            }
        }

        protected void EnsureThatPropertyIsInitialized(string name, ValidationResultType value)
        {
            if (value == 0)
            {
                throw new InvalidOperationException($"Property {name} must be set");
            }
        }
    }
}
