
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectX.Logic
{
    public class EnemyStateMachine : FSMStateMachine<EnemyController>
    {
        public EnemyStateMachine(EnemyController entity) : base(entity)
        {
            EnemyStateFactory gameStateFactory = new EnemyStateFactory(false);
            gameStateFactory.Init();

            stateFactory = gameStateFactory;
        }
    }
}