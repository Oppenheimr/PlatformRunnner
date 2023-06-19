using System.Collections;
using UnityEngine;


namespace Editor
{
    /// <summary>
    /// A class for automatic ragdoll generation developed by Lovatto Studios.
    /// </summary>
    public class AutoRagdoller
    {
        private static Animator Anim;
        private static Transform pelvis;
        private static Transform leftHips = (Transform)null;
        private static Transform leftKnee = (Transform)null;
        private static Transform rightHips = (Transform)null;
        private static Transform rightKnee = (Transform)null;
        private static Transform leftArm = (Transform)null;
        private static Transform leftElbow = (Transform)null;
        private static Transform rightArm = (Transform)null;
        private static Transform rightElbow = (Transform)null;
        private static Transform middleSpine = (Transform)null;
        private static Transform head = (Transform)null;
        private static float totalMass = 20f;
        private static Vector3 right = Vector3.right;
        private static Vector3 up = Vector3.up;
        private static Vector3 forward = Vector3.forward;
        private static Vector3 worldRight = Vector3.right;
        private static Vector3 worldUp = Vector3.up;
        private static Vector3 worldForward = Vector3.forward;
        private static bool flipForward = false;
        private static ArrayList bones;
        private static AutoRagdoller.BoneInfo rootBone;

        private static string CheckConsistency()
        {
            AutoRagdoller.PrepareBones();
            Hashtable hashtable = new Hashtable();
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if ((bool)(UnityEngine.Object)bone.anchor)
                {
                    if (hashtable[(object)bone.anchor] != null)
                    {
                        AutoRagdoller.BoneInfo boneInfo = (AutoRagdoller.BoneInfo)hashtable[(object)bone.anchor];
                        return string.Format("{0} and {1} may not be assigned to the same bone.", (object)bone.name,
                            (object)boneInfo.name);
                    }

                    hashtable[(object)bone.anchor] = (object)bone;
                }
            }

            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if ((UnityEngine.Object)bone.anchor == (UnityEngine.Object)null)
                    return string.Format("{0} has not been assigned yet.\n", (object)bone.name);
            }

            return "";
        }

        private static void GetBones()
        {
            AutoRagdoller.pelvis = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.Hips);
            AutoRagdoller.leftHips = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            AutoRagdoller.leftKnee = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            AutoRagdoller.rightHips = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            AutoRagdoller.rightKnee = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            AutoRagdoller.leftArm = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            AutoRagdoller.leftElbow = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            AutoRagdoller.rightArm = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
            AutoRagdoller.rightElbow = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            AutoRagdoller.middleSpine = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.Spine);
            AutoRagdoller.head = AutoRagdoller.Anim.GetBoneTransform(HumanBodyBones.Head);
        }

        private static void DecomposeVector(
            out Vector3 normalCompo,
            out Vector3 tangentCompo,
            Vector3 outwardDir,
            Vector3 outwardNormal)
        {
            outwardNormal = outwardNormal.normalized;
            normalCompo = outwardNormal * Vector3.Dot(outwardDir, outwardNormal);
            tangentCompo = outwardDir - normalCompo;
        }

        private static void CalculateAxes()
        {
            if ((UnityEngine.Object)AutoRagdoller.head != (UnityEngine.Object)null &&
                (UnityEngine.Object)AutoRagdoller.pelvis != (UnityEngine.Object)null)
                AutoRagdoller.up =
                    AutoRagdoller.CalculateDirectionAxis(
                        AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.head.position));
            if ((UnityEngine.Object)AutoRagdoller.rightElbow != (UnityEngine.Object)null &&
                (UnityEngine.Object)AutoRagdoller.pelvis != (UnityEngine.Object)null)
            {
                Vector3 tangentCompo;
                AutoRagdoller.DecomposeVector(out Vector3 _, out tangentCompo,
                    AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.rightElbow.position), AutoRagdoller.up);
                AutoRagdoller.right = AutoRagdoller.CalculateDirectionAxis(tangentCompo);
            }

            AutoRagdoller.forward = Vector3.Cross(AutoRagdoller.right, AutoRagdoller.up);
            if (!AutoRagdoller.flipForward)
                return;
            AutoRagdoller.forward = -AutoRagdoller.forward;
        }

        private static void PrepareBones()
        {
            if ((bool)(UnityEngine.Object)AutoRagdoller.pelvis)
            {
                AutoRagdoller.worldRight = AutoRagdoller.pelvis.TransformDirection(AutoRagdoller.right);
                AutoRagdoller.worldUp = AutoRagdoller.pelvis.TransformDirection(AutoRagdoller.up);
                AutoRagdoller.worldForward = AutoRagdoller.pelvis.TransformDirection(AutoRagdoller.forward);
            }

            AutoRagdoller.bones = new ArrayList();
            AutoRagdoller.rootBone = new AutoRagdoller.BoneInfo();
            AutoRagdoller.rootBone.name = "Pelvis";
            AutoRagdoller.rootBone.anchor = AutoRagdoller.pelvis;
            AutoRagdoller.rootBone.parent = (AutoRagdoller.BoneInfo)null;
            AutoRagdoller.rootBone.density = 2.5f;
            AutoRagdoller.bones.Add((object)AutoRagdoller.rootBone);
            AutoRagdoller.AddMirroredJoint("Hips", AutoRagdoller.leftHips, AutoRagdoller.rightHips, "Pelvis",
                AutoRagdoller.worldRight, AutoRagdoller.worldForward, -20f, 70f, 30f, typeof(CapsuleCollider), 0.3f,
                1.5f);
            AutoRagdoller.AddMirroredJoint("Knee", AutoRagdoller.leftKnee, AutoRagdoller.rightKnee, "Hips",
                AutoRagdoller.worldRight, AutoRagdoller.worldForward, -80f, 0.0f, 0.0f, typeof(CapsuleCollider), 0.25f,
                1.5f);
            AutoRagdoller.AddJoint("Middle Spine", AutoRagdoller.middleSpine, "Pelvis", AutoRagdoller.worldRight,
                AutoRagdoller.worldForward, -20f, 20f, 10f, (System.Type)null, 1f, 2.5f);
            AutoRagdoller.AddMirroredJoint("Arm", AutoRagdoller.leftArm, AutoRagdoller.rightArm, "Middle Spine",
                AutoRagdoller.worldUp, AutoRagdoller.worldForward, -70f, 10f, 50f, typeof(CapsuleCollider), 0.25f, 1f);
            AutoRagdoller.AddMirroredJoint("Elbow", AutoRagdoller.leftElbow, AutoRagdoller.rightElbow, "Arm",
                AutoRagdoller.worldForward, AutoRagdoller.worldUp, -90f, 0.0f, 0.0f, typeof(CapsuleCollider), 0.2f, 1f);
            AutoRagdoller.AddJoint("Head", AutoRagdoller.head, "Middle Spine", AutoRagdoller.worldRight,
                AutoRagdoller.worldForward, -40f, 25f, 25f, (System.Type)null, 1f, 1f);
        }

        public static bool Build(Animator _anim)
        {
            AutoRagdoller.Anim = _anim;
            AutoRagdoller.GetBones();
            AutoRagdoller.CheckConsistency();
            AutoRagdoller.CalculateAxes();
            AutoRagdoller.Cleanup();
            AutoRagdoller.BuildCapsules();
            AutoRagdoller.AddBreastColliders();
            AutoRagdoller.AddHeadCollider();
            AutoRagdoller.BuildBodies();
            AutoRagdoller.BuildJoints();
            AutoRagdoller.CalculateMass();
            return true;
        }

        private static AutoRagdoller.BoneInfo FindBone(string name)
        {
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if (bone.name == name)
                    return bone;
            }

            return (AutoRagdoller.BoneInfo)null;
        }

        private static void AddMirroredJoint(
            string name,
            Transform leftAnchor,
            Transform rightAnchor,
            string parent,
            Vector3 worldTwistAxis,
            Vector3 worldSwingAxis,
            float minLimit,
            float maxLimit,
            float swingLimit,
            System.Type colliderType,
            float radiusScale,
            float density)
        {
            AutoRagdoller.AddJoint("Left " + name, leftAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit,
                maxLimit, swingLimit, colliderType, radiusScale, density);
            AutoRagdoller.AddJoint("Right " + name, rightAnchor, parent, worldTwistAxis, worldSwingAxis, minLimit,
                maxLimit, swingLimit, colliderType, radiusScale, density);
        }

        private static void AddJoint(
            string name,
            Transform anchor,
            string parent,
            Vector3 worldTwistAxis,
            Vector3 worldSwingAxis,
            float minLimit,
            float maxLimit,
            float swingLimit,
            System.Type colliderType,
            float radiusScale,
            float density)
        {
            AutoRagdoller.BoneInfo boneInfo = new AutoRagdoller.BoneInfo();
            boneInfo.name = name;
            boneInfo.anchor = anchor;
            boneInfo.axis = worldTwistAxis;
            boneInfo.normalAxis = worldSwingAxis;
            boneInfo.minLimit = minLimit;
            boneInfo.maxLimit = maxLimit;
            boneInfo.swingLimit = swingLimit;
            boneInfo.density = density;
            boneInfo.colliderType = colliderType;
            boneInfo.radiusScale = radiusScale;
            if (AutoRagdoller.FindBone(parent) != null)
                boneInfo.parent = AutoRagdoller.FindBone(parent);
            else if (name.StartsWith("Left"))
                boneInfo.parent = AutoRagdoller.FindBone("Left " + parent);
            else if (name.StartsWith("Right"))
                boneInfo.parent = AutoRagdoller.FindBone("Right " + parent);
            boneInfo.parent.children.Add((object)boneInfo);
            AutoRagdoller.bones.Add((object)boneInfo);
        }

        private static void BuildCapsules()
        {
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if (bone.colliderType == typeof(CapsuleCollider))
                {
                    int direction;
                    float distance;
                    if (bone.children.Count == 1)
                    {
                        Vector3 position = ((AutoRagdoller.BoneInfo)bone.children[0]).anchor.position;
                        AutoRagdoller.CalculateDirection(bone.anchor.InverseTransformPoint(position), out direction,
                            out distance);
                    }
                    else
                    {
                        Vector3 position = bone.anchor.position - bone.parent.anchor.position + bone.anchor.position;
                        AutoRagdoller.CalculateDirection(bone.anchor.InverseTransformPoint(position), out direction,
                            out distance);
                        if (bone.anchor.GetComponentsInChildren(typeof(Transform)).Length > 1)
                        {
                            Bounds bounds = new Bounds();
                            foreach (Transform componentsInChild in bone.anchor.GetComponentsInChildren(
                                         typeof(Transform)))
                                bounds.Encapsulate(bone.anchor.InverseTransformPoint(componentsInChild.position));
                            Vector3 vector3;
                            if ((double)distance > 0.0)
                            {
                                vector3 = bounds.max;
                                distance = vector3[direction];
                            }
                            else
                            {
                                vector3 = bounds.min;
                                distance = vector3[direction];
                            }
                        }
                    }

                    CapsuleCollider capsuleCollider = bone.anchor.gameObject.AddComponent<CapsuleCollider>();
                    capsuleCollider.direction = direction;
                    Vector3 zero = Vector3.zero;
                    zero[direction] = distance * 0.5f;
                    capsuleCollider.center = zero;
                    capsuleCollider.height = Mathf.Abs(distance);
                    capsuleCollider.radius = Mathf.Abs(distance * bone.radiusScale);
                }
            }
        }

        private static void Cleanup()
        {
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if ((bool)(UnityEngine.Object)bone.anchor)
                {
                    foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(typeof(Joint)))
                        UnityEngine.Object.DestroyImmediate(componentsInChild);
                    foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(
                                 typeof(Rigidbody)))
                        UnityEngine.Object.DestroyImmediate(componentsInChild);
                    foreach (UnityEngine.Object componentsInChild in bone.anchor.GetComponentsInChildren(
                                 typeof(Collider)))
                        UnityEngine.Object.DestroyImmediate(componentsInChild);
                }
            }
        }

        private static void BuildBodies()
        {
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                bone.anchor.gameObject.AddComponent<Rigidbody>();
                bone.anchor.GetComponent<Rigidbody>().mass = bone.density;
            }
        }

        private static void BuildJoints()
        {
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
            {
                if (bone.parent != null)
                {
                    CharacterJoint characterJoint = bone.anchor.gameObject.AddComponent<CharacterJoint>();
                    bone.joint = characterJoint;
                    characterJoint.axis =
                        AutoRagdoller.CalculateDirectionAxis(bone.anchor.InverseTransformDirection(bone.axis));
                    characterJoint.swingAxis =
                        AutoRagdoller.CalculateDirectionAxis(bone.anchor.InverseTransformDirection(bone.normalAxis));
                    characterJoint.anchor = Vector3.zero;
                    characterJoint.connectedBody = bone.parent.anchor.GetComponent<Rigidbody>();
                    characterJoint.enablePreprocessing = false;
                    characterJoint.enableProjection = true;
                    SoftJointLimit softJointLimit = new SoftJointLimit();
                    softJointLimit.contactDistance = 0.0f;
                    softJointLimit.limit = bone.minLimit;
                    characterJoint.lowTwistLimit = softJointLimit;
                    softJointLimit.limit = bone.maxLimit;
                    characterJoint.highTwistLimit = softJointLimit;
                    softJointLimit.limit = bone.swingLimit;
                    characterJoint.swing1Limit = softJointLimit;
                    softJointLimit.limit = 0.0f;
                    characterJoint.swing2Limit = softJointLimit;
                }
            }
        }

        private static void CalculateMassRecurse(AutoRagdoller.BoneInfo bone)
        {
            float mass = bone.anchor.GetComponent<Rigidbody>().mass;
            foreach (AutoRagdoller.BoneInfo child in bone.children)
            {
                AutoRagdoller.CalculateMassRecurse(child);
                mass += child.summedMass;
            }

            bone.summedMass = mass;
        }

        private static void CalculateMass()
        {
            AutoRagdoller.CalculateMassRecurse(AutoRagdoller.rootBone);
            float num = AutoRagdoller.totalMass / AutoRagdoller.rootBone.summedMass;
            foreach (AutoRagdoller.BoneInfo bone in AutoRagdoller.bones)
                bone.anchor.GetComponent<Rigidbody>().mass *= num;
            AutoRagdoller.CalculateMassRecurse(AutoRagdoller.rootBone);
        }

        private static void CalculateDirection(Vector3 point, out int direction, out float distance)
        {
            direction = 0;
            if ((double)Mathf.Abs(point[1]) > (double)Mathf.Abs(point[0]))
                direction = 1;
            if ((double)Mathf.Abs(point[2]) > (double)Mathf.Abs(point[direction]))
                direction = 2;
            distance = point[direction];
        }

        private static Vector3 CalculateDirectionAxis(Vector3 point)
        {
            int direction = 0;
            float distance;
            AutoRagdoller.CalculateDirection(point, out direction, out distance);
            Vector3 zero = Vector3.zero;
            zero[direction] = (double)distance <= 0.0 ? -1f : 1f;
            return zero;
        }

        private static int SmallestComponent(Vector3 point)
        {
            int index = 0;
            if ((double)Mathf.Abs(point[1]) < (double)Mathf.Abs(point[0]))
                index = 1;
            if ((double)Mathf.Abs(point[2]) < (double)Mathf.Abs(point[index]))
                index = 2;
            return index;
        }

        private static int LargestComponent(Vector3 point)
        {
            int index = 0;
            if ((double)Mathf.Abs(point[1]) > (double)Mathf.Abs(point[0]))
                index = 1;
            if ((double)Mathf.Abs(point[2]) > (double)Mathf.Abs(point[index]))
                index = 2;
            return index;
        }

        private static int SecondLargestComponent(Vector3 point)
        {
            int num1 = AutoRagdoller.SmallestComponent(point);
            int num2 = AutoRagdoller.LargestComponent(point);
            if (num1 < num2)
            {
                int num3 = num2;
                num2 = num1;
                num1 = num3;
            }

            if (num1 == 0 && num2 == 1)
                return 2;
            return num1 == 0 && num2 == 2 ? 1 : 0;
        }

        private static Bounds Clip(
            Bounds bounds,
            Transform relativeTo,
            Transform clipTransform,
            bool below)
        {
            int index = AutoRagdoller.LargestComponent(bounds.size);
            if ((double)Vector3.Dot(AutoRagdoller.worldUp, relativeTo.TransformPoint(bounds.max)) >
                (double)Vector3.Dot(AutoRagdoller.worldUp, relativeTo.TransformPoint(bounds.min)) == below)
            {
                Vector3 min = bounds.min;
                min[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
                bounds.min = min;
            }
            else
            {
                Vector3 max = bounds.max;
                max[index] = relativeTo.InverseTransformPoint(clipTransform.position)[index];
                bounds.max = max;
            }

            return bounds;
        }

        private static Bounds GetBreastBounds(Transform relativeTo)
        {
            Bounds breastBounds = new Bounds();
            breastBounds.Encapsulate(relativeTo.InverseTransformPoint(AutoRagdoller.leftHips.position));
            breastBounds.Encapsulate(relativeTo.InverseTransformPoint(AutoRagdoller.rightHips.position));
            breastBounds.Encapsulate(relativeTo.InverseTransformPoint(AutoRagdoller.leftArm.position));
            breastBounds.Encapsulate(relativeTo.InverseTransformPoint(AutoRagdoller.rightArm.position));
            Vector3 size = breastBounds.size;
            size[AutoRagdoller.SmallestComponent(breastBounds.size)] =
                size[AutoRagdoller.LargestComponent(breastBounds.size)] / 2f;
            breastBounds.size = size;
            return breastBounds;
        }

        private static void AddBreastColliders()
        {
            if ((UnityEngine.Object)AutoRagdoller.middleSpine != (UnityEngine.Object)null &&
                (UnityEngine.Object)AutoRagdoller.pelvis != (UnityEngine.Object)null)
            {
                Bounds bounds = AutoRagdoller.Clip(AutoRagdoller.GetBreastBounds(AutoRagdoller.pelvis),
                    AutoRagdoller.pelvis, AutoRagdoller.middleSpine, false);
                BoxCollider boxCollider1 = AutoRagdoller.pelvis.gameObject.AddComponent<BoxCollider>();
                boxCollider1.center = bounds.center;
                boxCollider1.size = bounds.size;
                bounds = AutoRagdoller.Clip(AutoRagdoller.GetBreastBounds(AutoRagdoller.middleSpine),
                    AutoRagdoller.middleSpine, AutoRagdoller.middleSpine, true);
                BoxCollider boxCollider2 = AutoRagdoller.middleSpine.gameObject.AddComponent<BoxCollider>();
                boxCollider2.center = bounds.center;
                boxCollider2.size = bounds.size;
            }
            else
            {
                Bounds bounds = new Bounds();
                bounds.Encapsulate(AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.leftHips.position));
                bounds.Encapsulate(AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.rightHips.position));
                bounds.Encapsulate(AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.leftArm.position));
                bounds.Encapsulate(AutoRagdoller.pelvis.InverseTransformPoint(AutoRagdoller.rightArm.position));
                Vector3 size = bounds.size;
                size[AutoRagdoller.SmallestComponent(bounds.size)] =
                    size[AutoRagdoller.LargestComponent(bounds.size)] / 2f;
                BoxCollider boxCollider = AutoRagdoller.pelvis.gameObject.AddComponent<BoxCollider>();
                boxCollider.center = bounds.center;
                boxCollider.size = size;
            }
        }

        private static void AddHeadCollider()
        {
            if ((bool)(UnityEngine.Object)AutoRagdoller.head.GetComponent<Collider>())
                UnityEngine.Object.Destroy((UnityEngine.Object)AutoRagdoller.head.GetComponent<Collider>());
            float num = Vector3.Distance(AutoRagdoller.leftArm.transform.position,
                AutoRagdoller.rightArm.transform.position) / 4f;
            SphereCollider sphereCollider = AutoRagdoller.head.gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = num;
            Vector3 zero = Vector3.zero;
            int direction;
            float distance;
            AutoRagdoller.CalculateDirection(AutoRagdoller.head.InverseTransformPoint(AutoRagdoller.pelvis.position),
                out direction, out distance);
            zero[direction] = (double)distance <= 0.0 ? num : -num;
            sphereCollider.center = zero;
        }

        private class BoneInfo
        {
            public string name;
            public Transform anchor;
            public CharacterJoint joint;
            public AutoRagdoller.BoneInfo parent;
            public float minLimit;
            public float maxLimit;
            public float swingLimit;
            public Vector3 axis;
            public Vector3 normalAxis;
            public float radiusScale;
            public System.Type colliderType;
            public ArrayList children = new ArrayList();
            public float density;
            public float summedMass;
        }
    }
}