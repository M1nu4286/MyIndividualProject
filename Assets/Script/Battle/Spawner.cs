using GameData.Types;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class Spawner
{
    SpeedHeap speedHeap;

    public void InitHeap(int maxCapacity)
    {
        speedHeap = new SpeedHeap(maxCapacity + 1);
    }
    public void LoadStageData(ref RuntimeEntity[] _stageDatas, EntityEntry[] entities, int entityIndex, EntityType type)
    {
        if (entities == null) return;

        int tempIndex = 0;
        for (int i = 0; i < entities.Length && tempIndex < entityIndex; i++)
        {
            if (entities[i].Type == type)
            {
                _stageDatas[tempIndex] = new RuntimeEntity(entities[i], tempIndex);
                tempIndex++;
            }
        }

        Debug.Log($"{type} 타입 유닛 {_stageDatas.Length}마리 로드 완료.");
    }

    public void GetSortOrder(ref int[] orderList, int entityIndex, RuntimeEntity[] runtimeEntities)
    {
        speedHeap.heapSize = 0;
        for (int i = 0; i < entityIndex; i++)
        {
            if (runtimeEntities[i].isAlive)
            {

                InsertHeap(ref speedHeap, i, runtimeEntities[i].currentSpeed);
            }
        }
        int activeCount = speedHeap.heapSize;
        Debug.Log("턴 순서 계산 완료. " + activeCount + "마리의 유닛이 행동합니다.");

        for (int i = 0; i < activeCount; i++)
        {
            orderList[i] = PopMaxHeap(ref speedHeap).index;
        }


    }

    private struct SpeedHeap
    {
        public SpeedHeapNode[] heap; //구조체는 값타입인데 지금 힙배열 <- 참조타입
        public int heapSize;

        public SpeedHeap(int heapLength)
        {
            heap = new SpeedHeapNode[heapLength];
            heapSize = 0;
        }
    }

    private struct SpeedHeapNode
    {
        public int index;
        public int speed;
        public SpeedHeapNode(int index, int currentSpeed)
        {

            this.index = index;
            this.speed = currentSpeed;
        }
    }

    private void InsertHeap(ref SpeedHeap speedHeap, int nodeIndex, int currentSpeed)
    {
        int index;
        SpeedHeapNode node = new SpeedHeapNode(nodeIndex, currentSpeed);
        index = ++speedHeap.heapSize;

        while ((index != 1) && (node.speed > speedHeap.heap[index / 2].speed))
        {
            speedHeap.heap[index] = speedHeap.heap[index / 2];
            index /= 2;
        }
        speedHeap.heap[index] = node;
    }

    private SpeedHeapNode PopMaxHeap(ref SpeedHeap speedHeap)
    {
        if (speedHeap.heapSize <= 0)
        {
            Debug.LogWarning("Heap is empty! Check your logic.");
            return default; // 모든 필드가 0/null인 빈 구조체 반환
        }

        SpeedHeapNode root = speedHeap.heap[1]; // 힙의 루트 노드(최대값)를 저장
        SpeedHeapNode last = speedHeap.heap[speedHeap.heapSize--]; // 힙의 마지막 노드를 저장하고 힙 크기를 감소

        int parent = 1; // 부모 노드의 인덱스
        int child = 2; // 자식 노드의 인덱스

        while (child <= speedHeap.heapSize) //자식 노드가 힙 크기 내에 있는 동안
        {
            if (child < speedHeap.heapSize && speedHeap.heap[child].speed < speedHeap.heap[child + 1].speed)
            {
                child++;// 오른쪽 자식 노드가 더 크면 오른쪽 자식 노드로 이동
            }
            if (last.speed >= speedHeap.heap[child].speed)
            {
                break; // 마지막 노드가 자식 노드보다 크거나 같으면 힙 속성 만족
            }
            else
            {
                speedHeap.heap[parent] = speedHeap.heap[child]; // 자식 노드를 부모 노드로 이동
                parent = child; // 부모 노드의 인덱스를 자식 노드의 인덱스로 이동
                child *= 2; // 왼쪽 자식 노드의 인덱스로 이동
            }
        }
        speedHeap.heap[parent] = last;
        return root;// 힙의 루트 노드(최대값)를 반환
    }
}

