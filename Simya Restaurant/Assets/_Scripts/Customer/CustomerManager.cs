using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Seat
{
    public GameObject seatObj;
    public Vector3 Position => seatObj.transform.position;
    public bool isEmpty;

    public Seat(GameObject obj)
    {
        seatObj = obj;
        isEmpty = true;
    }
}

public class CustomerManager : MonoBehaviour
{
    [Header("Seats")]
    [SerializeField] private List<Seat> seats = new List<Seat>();
    private Queue<GameObject> customerQueue = new Queue<GameObject>();

    [Header("Customer Settings")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform lineStartPosition;
    [SerializeField] private float lineSpacing = 2.0f;
    [SerializeField] private float moveSpeed = 2.0f; // 이동 속도
    [SerializeField] private int maxCustomers = 3;

    private void Start()
    {
        InitializeSeats();
        InvokeRepeating(nameof(SpawnCustomer), 0f, 10f); // 10초마다 손님 스폰
    }

    private void InitializeSeats()
    {
        foreach (Seat seat in seats)
        {
            seat.isEmpty = true;
        }
    }

    private void SpawnCustomer()
    {
        if (customerQueue.Count >= maxCustomers) return;

        GameObject newCustomer = Instantiate(customerPrefab, lineStartPosition.position, Quaternion.identity);
        customerQueue.Enqueue(newCustomer);

        UpdateCustomerQueuePositions();
        MoveToSeat();
    }

    private void UpdateCustomerQueuePositions()
    {
        int index = 0;
        foreach (GameObject customer in customerQueue)
        {
            Vector3 queuePosition = lineStartPosition.position + Vector3.back * index * lineSpacing;
            customer.transform.position = queuePosition;
            index++;
        }
    }

    private Seat FindEmptySeat()
    {
        foreach (Seat seat in seats)
        {
            if (seat.isEmpty) return seat;
        }
        return null;
    }

    private void MoveToSeat()
    {
        if (customerQueue.Count > 0)
        {
            Seat emptySeat = FindEmptySeat();
            if (emptySeat != null)
            {
                GameObject customer = customerQueue.Dequeue();
                StartCoroutine(MoveCustomerToSeat(customer, emptySeat));
                emptySeat.isEmpty = false;
            }
        }
    }

    private IEnumerator MoveCustomerToSeat(GameObject customer, Seat seat)
    {
        Vector3 startPosition = customer.transform.position;
        Vector3 targetPosition = seat.Position;
        float elapsedTime = 0;
        float journeyTime = 1.5f;

        while (elapsedTime < journeyTime)
        {
            customer.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / journeyTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        customer.transform.position = targetPosition;
    }

}
