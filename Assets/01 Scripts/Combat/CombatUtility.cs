using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harpaesis.Combat
{
    public static class CombatUtility
    {
        public static int TranslateFormula(string _formula)
        {
            _formula.ToLower();

            if (_formula.Contains("d"))
            {
                string[] s = _formula.Split('d');

                int _times;
                int _maxRoll;

                if(int.TryParse(s[0], out _times) && int.TryParse(s[1], out _maxRoll))
                {
                    return Roll(_times, _maxRoll);
                }
            }
            else
            {
                int _number;

                if(int.TryParse(_formula, out _number))
                {
                    return _number;
                }
            }

            throw new System.Exception($"Error: The given formula is not valid. Given formula: {_formula}." +
                $" Formula should be in \"#d#\" format or should just be a number.");
        }

        public static int Roll(int _times, int _maxRoll)
        {
            int _sum = 0;

            for (int i = 0; i < _times; i++)
            {
                _sum += Random.Range(1, _maxRoll + 1);
            }

            return _sum;
        }
    }
}