using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Seats")]
    [SerializeField] private GameObject[] seats;
	[SerializeField] private Vector3[] seatPositions;
	[SerializeField] private bool[] isSeatOccupied;

	[Header("Customer Prefab")]
	[SerializeField] private GameObject customerPrefab;
	[SerializeField] private bool canSeat;

	void Start()
	{
		seatPositions = new Vector3[seats.Length];
		isSeatOccupied = new bool[seats.Length];

		int index = 0;
		foreach (GameObject seat in seats)
		{
			seatPositions[index] = seat.transform.position;
			isSeatOccupied[index] = false;
			index++;
		}
	}

	void CheckEmptySeat()
	{

	}
}
