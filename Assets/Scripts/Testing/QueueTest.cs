using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Testing
{
    public class QueueTest : MonoBehaviour
    {
        [SerializeField]
        private List<ResolveObject> queueElements = new List<ResolveObject>();

        [SerializeField] private float timeToResolveCard = 0.4f;
        
        public void Awake()
        {
            GameManager.Instance.OnResolve += AddToResolveQueue;
            GameManager.Instance.OnResolveFinished += ResolveFinished;
        }
        
        private void AddToResolveQueue(ResolveObject resolve)
        {
            queueElements.Add(resolve);
        }

        private void ResolveFinished(bool finished)
        {
            if (finished)
            {
                StartCoroutine(StartCoolAnimation());
            }
            else
            {
                queueElements.Clear();
            }
            Debug.Log(finished);
        }

        IEnumerator StartCoolAnimation()
        {
            Debug.Log("Starting cool animation");

            foreach (ResolveObject resolveObject in queueElements)
            {
                switch (resolveObject.ResolveType)
                {
                    case ResolveType.Interfere:
                        Debug.Log("Interfere");
                        break;
                    case ResolveType.Defend:
                        Debug.Log("Defend");
                        break;
                    case ResolveType.Attack:
                        Debug.Log("Attack");
                        break;
                    case ResolveType.Damage:
                        Debug.Log("Damage");
                        GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].TakeDamage(resolveObject.Number);
                        break;
                    case ResolveType.Heal:
                        Debug.Log("Heal");
                        GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].HealLife(resolveObject.Number);
                        break;
                    case ResolveType.CardDraw:
                        Debug.Log("CardDraw");
                        break;
                }
                yield return new WaitForSeconds(timeToResolveCard);
            }
            GameManager.Instance.ProceedToNextState();
            Debug.Log("Ending cool animation");
        }
        
    }
}