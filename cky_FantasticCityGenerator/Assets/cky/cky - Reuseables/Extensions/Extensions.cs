using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cky.Reuseables.Extension
{
    public static class Extensions
    {
        public static bool TryGetRaycastHit(this Camera camera, out Ray ray, out RaycastHit hit, float rayDistance)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            ray = camera.ScreenPointToRay(screenCenter);
            return Physics.Raycast(ray, out hit, rayDistance);
        }

        public static void AlignToNearestAngle(this Transform transform, float angleStep)
        {
            float currentAngle = transform.eulerAngles.y;

            float nearestAngle = Mathf.Round(currentAngle / angleStep) * angleStep;

            transform.rotation = Quaternion.Euler(0, nearestAngle, 0);
        }


        public static bool IsCarParked(this Transform carTr, Transform parkingTr, float distance, float viewAngle)
        {
            var dist = Vector3.Distance(carTr.position, parkingTr.position);

            if (dist > distance)
            {
                return false;
            }

            if (!carTr.InCloseDirection(parkingTr, viewAngle))
            {
                Debug.Log(Vector3.Angle(carTr.forward, parkingTr.forward));
                return false;
            }

            return true;
        }

        public static bool InCloseDirection(this Transform actorTr, Transform objectTr, float viewAngle)
        {
            if (Vector3.Angle(actorTr.forward, objectTr.forward) < viewAngle * 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool InSight(this Transform actorTr, Transform objectTr, float viewAngle)
        {
            var actorPosition = actorTr.position;
            var objectPosition = objectTr.position;
            var dir = (objectPosition - actorPosition);
            dir.y = 0.0f;
            dir.Normalize();

            if (Vector3.Angle(actorTr.forward, dir) < viewAngle * 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool InSight_Direction(this Transform actorTr, Vector3 lookDirection, Transform objectTr, float viewAngle)
        {
            var actorPosition = actorTr.position;
            var objectPosition = objectTr.position;
            var dir = (objectPosition - actorPosition);
            dir.y = 0.0f;
            dir.Normalize();

            if (Vector3.Angle(lookDirection, dir) < viewAngle * 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }





        public static Vector3 WorldUnitDirectionX(this Transform actorTr, Vector3 targetPos)
        {
            var direction = targetPos - actorTr.position;
            direction.y = 0.0f;
            direction.z = 0.0f;
            direction.Normalize();

            return direction;
        }
        public static Vector3 LocalUnitDirectionX(this Transform actorTr, Vector3 targetPos)
        {
            var direction = targetPos - actorTr.localPosition;
            direction.y = 0.0f;
            direction.z = 0.0f;
            direction.Normalize();

            return direction;
        }
        public static void TurnToThis(this Transform actorTr, Vector3 targetPos, float rotationSpeed)
        {
            Vector3 direction = targetPos - actorTr.position;
            direction.y = 0;

            float angle = Vector3.Angle(direction, actorTr.forward);
            actorTr.rotation = Quaternion.Slerp(actorTr.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotationSpeed);
        }



        public static float TurnToDirectionWithAngleFeedback(this Transform actorTr, Vector3 targetForward, float rotationSpeed)
        {
            //Vector3 direction = targetForward - stateMachineAI.transform.position;
            Vector3 direction = targetForward;
            direction.y = 0;

            actorTr.rotation = Quaternion.Slerp(actorTr.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotationSpeed);

            float angle = Vector3.Angle(direction, actorTr.forward);

            return angle;
        }

        public static void TurnToDirection(this Transform actorTr, Vector3 targetForward, float rotationSpeed)
        {
            //Vector3 direction = targetForward - stateMachineAI.transform.position;
            Vector3 direction = targetForward;
            direction.y = 0;

            float angle = Vector3.Angle(direction, actorTr.forward);
            actorTr.rotation = Quaternion.Slerp(actorTr.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * rotationSpeed);
        }

        public static void TurnToAngle(this Transform actorTr, Vector3 targetAngle, float rotationSpeed)
        {
            actorTr.rotation = Quaternion.Slerp(actorTr.rotation,
                                                Quaternion.LookRotation(targetAngle),
                                                Time.deltaTime * rotationSpeed);
        }

        public static float Angle(this Transform actorTr, Vector3 targetForward)
        {
            return Vector3.Angle(actorTr.forward, targetForward);
        }

        public static void MoveWithVelocity(this Rigidbody rb, Vector3 targetPos, float moveSpeed, float rotationSpeed)
        {
            var actorTr = rb.transform;

            actorTr.TurnToThis(targetPos, rotationSpeed);

            rb.velocity = actorTr.forward * moveSpeed;
        }

        public static bool CloseToThisXZ(this Transform actorTr, Vector3 targetPos, float targetDistance)
        {
            var actorPos = actorTr.position;
            targetPos.y = actorPos.y;

            var distance = Vector3.Distance(actorPos, (Vector3)targetPos);

            if (distance < targetDistance)
                return true;

            return false;
        }

        public static T RandomFromArray<T>(this IEnumerable<T> array)
        {
            var random = UnityEngine.Random.Range(0, array.Count());
            return array.ElementAt(random);
        }



        private static readonly System.Random RandomGenerator = new System.Random();

        public static void Shuffle<T>(this IList<T> shuffleList)
        {
            int count = shuffleList.Count;
            while (count > 1)
            {
                count--;
                int randomValue = RandomGenerator.Next(count + 1);
                T value = shuffleList[randomValue];
                shuffleList[randomValue] = shuffleList[count];
                shuffleList[count] = value;
            }
        }

    }
}