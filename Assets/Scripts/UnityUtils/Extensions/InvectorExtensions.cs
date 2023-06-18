using UnityEngine;
#if INVECTOR_BASIC || INVECTOR_MELEE || INVECTOR_SHOOTER || INVECTOR_AI_TEMPLATE
using Inherited.Inherited_CyberBullet;
using Inherited.Inherited_Lovatto.InGame.AI;
using Invector.vCharacterController.AI;
using Invector.vCharacterController.AI.FSMBehaviour;
#endif

namespace UnityUtils.Extensions
{
#if (INVECTOR_BASIC || INVECTOR_MELEE || INVECTOR_SHOOTER || INVECTOR_AI_TEMPLATE)
    public static class InvectorExtensions
    {
        public static ControlAIShooter ControlAIShooter(this vIControlAI controlAI) 
            => (ControlAIShooter)controlAI;
        
        public static ControlAIShooter ControlAIShooter(this vIFSMBehaviourController behaviour) 
            => (ControlAIShooter)behaviour.aiController;
        
        public static Vector3 BasePosition(this vIFSMBehaviourController behaviour) 
            => ((ControlAIShooter)behaviour.aiController).BaseTransform.position;
        
        public static AIShooterReferences TargetBoss(this vIFSMBehaviourController behaviour) 
            => ((ControlAIShooter)behaviour.aiController).TargetBoss;
        
        public static Transform TargetBossTransform(this vIFSMBehaviourController behaviour) 
            => ((ControlAIShooter)behaviour.aiController).TargetBoss.transform;
        
        public static Vector3 TargetBossPosition(this vIFSMBehaviourController behaviour) 
            => ((ControlAIShooter)behaviour.aiController).TargetBoss.transform.position;
    }
#endif
}