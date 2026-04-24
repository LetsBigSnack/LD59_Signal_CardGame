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
        }

        IEnumerator StartCoolAnimation()
        {
            Debug.Log("Starting cool animation");

            foreach (ResolveObject resolveObject in queueElements)
            {
                switch (resolveObject.ResolveType)
                {
                    case ResolveType.Interfere:
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Interfere Side: {resolveObject.OppositeSlot.PlayerSide.ToString()} | Slot: {resolveObject.OppositeSlot.SlotPosition.ToString()} with priority {resolveObject.Number}");
                        break;
                    case ResolveType.Defend:
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Defending for {resolveObject.Number}");
                        break;
                    case ResolveType.Attack:
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Attacking for {resolveObject.Number}");
                        break;
                    case ResolveType.Damage:
                        GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].TakeDamage(resolveObject.Number);
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Taking Damage for {resolveObject.Number}");
                        break;
                    case ResolveType.Heal:
                        GameManager.Instance.Players[resolveObject.OwnSlot.PlayerSide].HealLife(resolveObject.Number);
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Healing for {resolveObject.Number}");
                        break;
                    case ResolveType.CardDraw:
                        Debug.Log($"Side: {resolveObject.OwnSlot.PlayerSide.ToString()} | Slot: {resolveObject.OwnSlot.SlotPosition.ToString()}  is Drawing Card for {resolveObject.Number}");
                        break;
                }
                yield return new WaitForSeconds(timeToResolveCard);
            }
            GameManager.Instance.ProceedToNextState();
            Debug.Log("Ending cool animation");
        }
        
    }
}