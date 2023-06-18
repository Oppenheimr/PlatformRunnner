using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class AnimatorExtensions
    {
        public static HumanBodyBones GetBone(this Animator animator, Transform transform)
        {
            var bones = animator.GetBones();
            foreach (var bone in bones.Where(bone => transform == bone.Key))
                return bone.Value;
            return HumanBodyBones.Chest;
        }

        public static void TryAddBone(this Dictionary<Transform, HumanBodyBones> dict, Animator animator, HumanBodyBones humanBodyBones)
        {
            //Null check
            dict ??= new Dictionary<Transform, HumanBodyBones>();
            //Try Add
            Transform bone = null;
            try
            {
                bone = animator.GetBoneTransform(humanBodyBones);
            }
            catch (Exception)
            {
                //ignored
            }
            
            if(bone != null)
                dict.Add(bone, humanBodyBones);
        }
        
        public static Dictionary<Transform, HumanBodyBones> GetBones(this Animator animator)
        {
            
            Dictionary<Transform, HumanBodyBones> dict = new Dictionary<Transform, HumanBodyBones>();
            
            dict.TryAddBone(animator,HumanBodyBones.Hips);
            dict.TryAddBone(animator, HumanBodyBones.LeftUpperLeg);
            dict.TryAddBone(animator, HumanBodyBones.RightUpperLeg);
            dict.TryAddBone(animator, HumanBodyBones.LeftLowerLeg);
            dict.TryAddBone(animator, HumanBodyBones.RightLowerLeg);
            dict.TryAddBone(animator, HumanBodyBones.LeftFoot);
            dict.TryAddBone(animator, HumanBodyBones.RightFoot);
            dict.TryAddBone(animator, HumanBodyBones.Spine);
            dict.TryAddBone(animator, HumanBodyBones.Chest);
            dict.TryAddBone(animator, HumanBodyBones.Neck);
            dict.TryAddBone(animator, HumanBodyBones.Head);
            dict.TryAddBone(animator, HumanBodyBones.LeftShoulder);
            dict.TryAddBone(animator, HumanBodyBones.RightShoulder);
            dict.TryAddBone(animator, HumanBodyBones.LeftUpperArm);
            dict.TryAddBone(animator, HumanBodyBones.RightUpperArm);
            dict.TryAddBone(animator, HumanBodyBones.LeftLowerArm);
            dict.TryAddBone(animator, HumanBodyBones.RightLowerArm);
            dict.TryAddBone(animator, HumanBodyBones.LeftHand);
            dict.TryAddBone(animator, HumanBodyBones.RightHand);
            dict.TryAddBone(animator, HumanBodyBones.LeftToes);
            dict.TryAddBone(animator, HumanBodyBones.RightToes);
            dict.TryAddBone(animator, HumanBodyBones.LeftEye);
            dict.TryAddBone(animator, HumanBodyBones.RightEye);
            dict.TryAddBone(animator, HumanBodyBones.Jaw);
            dict.TryAddBone(animator, HumanBodyBones.LeftThumbProximal);
            dict.TryAddBone(animator, HumanBodyBones.LeftThumbIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.LeftThumbDistal);
            dict.TryAddBone(animator, HumanBodyBones.LeftIndexProximal);
            dict.TryAddBone(animator, HumanBodyBones.LeftIndexIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.LeftIndexDistal);
            dict.TryAddBone(animator, HumanBodyBones.LeftMiddleProximal);
            dict.TryAddBone(animator, HumanBodyBones.LeftMiddleIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.LeftMiddleDistal);
            dict.TryAddBone(animator, HumanBodyBones.LeftRingProximal);
            dict.TryAddBone(animator, HumanBodyBones.LeftRingIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.LeftRingDistal);
            dict.TryAddBone(animator, HumanBodyBones.LeftLittleProximal);
            dict.TryAddBone(animator, HumanBodyBones.LeftLittleIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.LeftLittleDistal);
            dict.TryAddBone(animator, HumanBodyBones.RightThumbProximal);
            dict.TryAddBone(animator, HumanBodyBones.RightThumbIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.RightThumbDistal);
            dict.TryAddBone(animator, HumanBodyBones.RightIndexProximal);
            dict.TryAddBone(animator, HumanBodyBones.RightIndexIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.RightIndexDistal);
            dict.TryAddBone(animator, HumanBodyBones.RightMiddleProximal);
            dict.TryAddBone(animator, HumanBodyBones.RightMiddleIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.RightMiddleDistal);
            dict.TryAddBone(animator, HumanBodyBones.RightRingProximal);
            dict.TryAddBone(animator, HumanBodyBones.RightRingIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.RightRingDistal);
            dict.TryAddBone(animator, HumanBodyBones.RightLittleProximal);
            dict.TryAddBone(animator, HumanBodyBones.RightLittleIntermediate);
            dict.TryAddBone(animator, HumanBodyBones.RightLittleDistal);
            dict.TryAddBone(animator, HumanBodyBones.UpperChest);
            dict.TryAddBone(animator, HumanBodyBones.LastBone);
            
            return dict;
        }
    }
}