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
    public static CustomerManager instance;


    [Header("Seats State")]
    [SerializeField] private List<Seat> seats = new List<Seat>();
    private Queue<GameObject> customerQueue = new Queue<GameObject>();

    [Space(10)]
    [Header("Customer Settings")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform lineStartPosition;
    [SerializeField] private float lineSpacing = 2.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int maxCustomers = 3;

    [Space(10)]
    [Header("Get Customer List")]
    [SerializeField] private List<GameObject> customerList = new List<GameObject>();

    [Space(10)]
    [Header("Others")]
    [SerializeField] private Transform servingButton;

    [Space(10)]
    [Header("Final Price")]
    public float totalPrice;
    public float dangerFruitTartTotalPrice;
    public float flowerTeaTotalPrice;
    public float slimeJamBreadTotalPrice;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeSeats();
        InvokeRepeating(nameof(SpawnCustomer), 0f, 10f);
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

        Transform servingButton = newCustomer.transform.GetChild(0).GetChild(3);
        servingButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            newCustomer.GetComponent<Customer>().ServeMenu();
        });

        customerList.Add(newCustomer);
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