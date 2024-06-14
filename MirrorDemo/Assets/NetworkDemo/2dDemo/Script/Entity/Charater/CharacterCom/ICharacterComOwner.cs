using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public interface ICharacterComOwner
    {
        T GetCom<T>() where T : CharacterComponent;

        Transform Transform { get; }

        void RefreshHpPercent(float percent);

    }
}