﻿using LiteEntitySystem.Internal;

namespace LiteEntitySystem
{
    /// <summary>
    /// Base class for Controller entities
    /// </summary>
    public abstract class ControllerLogic : InternalEntity
    {
        internal SyncVar<byte> InternalOwnerId;
        private SyncVar<EntitySharedReference> _controlledEntity;

        public byte OwnerId => InternalOwnerId;
        
        /// <summary>
        /// Is controller - AI controller
        /// </summary>
        public abstract bool IsBot { get; }

        public T GetControlledEntity<T>() where T : PawnLogic
        {
            return EntityManager.GetEntityById<T>(_controlledEntity);
        }

        internal override bool IsControlledBy(byte playerId)
        {
            return InternalOwnerId == playerId;
        }
        
        public virtual void BeforeControlledUpdate()
        {
            
        }

        public void DestroyWithControlledEntity()
        {
            GetControlledEntity<PawnLogic>()?.Destroy();
            _controlledEntity.Value = null;
            Destroy();
        }

        public void StartControl(PawnLogic target)
        {
            StopControl();
            _controlledEntity.Value = target;
            GetControlledEntity<PawnLogic>().Controller = this;
        }

        internal void OnControlledDestroy()
        {
            StopControl();
        }

        public void StopControl()
        {
            var controlledLogic = GetControlledEntity<PawnLogic>();
            if (controlledLogic == null)
                return;
            controlledLogic.Controller = null;
            _controlledEntity.Value = null;
        }
        
        protected ControllerLogic(EntityParams entityParams) : base(entityParams) { }
    }

    /// <summary>
    /// Base class for AI Controller entities
    /// </summary>
    [LocalOnly, UpdateableEntity]
    public abstract class AiControllerLogic : ControllerLogic
    {
        public override bool IsBot => true;
        
        protected AiControllerLogic(EntityParams entityParams) : base(entityParams) { }
    }

    /// <summary>
    /// Base class for AI Controller entities with typed ControlledEntity field
    /// </summary>
    [LocalOnly, UpdateableEntity]
    public abstract class AiControllerLogic<T> : AiControllerLogic where T : PawnLogic
    {
        public T ControlledEntity => GetControlledEntity<T>();
        
        protected AiControllerLogic(EntityParams entityParams) : base(entityParams) { }
    }

    /// <summary>
    /// Base class for human Controller entities
    /// </summary>
    [UpdateableEntity(true)]
    public abstract class HumanControllerLogic<TInput> : ControllerLogic where TInput : unmanaged
    {
        /// <summary>
        /// Called on client and server to read generated from <see cref="GenerateInput"/> input
        /// </summary>
        /// <param name="input"></param>
        public abstract void ReadInput(in TInput input);
        
        /// <summary>
        /// Called on client to generate input
        /// </summary>
        public abstract void GenerateInput(out TInput input);

        public override bool IsBot => false;
        
        protected HumanControllerLogic(EntityParams entityParams) : base(entityParams) { }
    }

    /// <summary>
    /// Base class for human Controller entities with typed ControlledEntity field
    /// </summary>
    public abstract class HumanControllerLogic<TInput, T> : HumanControllerLogic<TInput> where T : PawnLogic where TInput : unmanaged
    {
        public T ControlledEntity => GetControlledEntity<T>();

        protected HumanControllerLogic(EntityParams entityParams) : base(entityParams) { }
    }
}