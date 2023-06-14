using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Class for a lock that requires specific toggles to be on & others off
/// </summary>
public class GridToggleLock : MonoBehaviour
{
    [Tooltip("Organize the toggles under row parents, supply the parent of the row objects here.")]
    [SerializeField] private GameObject parentOfRowsOfToggles;

    [VectorLabels("Row", "Column")]
    [Tooltip("One based list of row & column coords of the correct toggles. Top left is row 1 column 1.")]
    [SerializeField] private Vector2Int[] correctTogglesAsRowColumnCoords;

    private Activatable[,] togglesGrid;
    private bool isCorrectCombination = false;

    private void Awake()
    {
        FetchToggles();
        foreach (var toggle in togglesGrid)
            toggle.PropertyChanged += UpdateCombinationOnActiveChange;

        void FetchToggles()
        {
            int numOfRows = parentOfRowsOfToggles.transform.childCount;
            if (numOfRows == 0)
                throw new Exception("At least one row is needed!");

            int numOfColumns = parentOfRowsOfToggles.transform.GetChild(0).childCount;
            if (numOfColumns == 0)
                throw new Exception("At least one column is needed!");

            togglesGrid = new Activatable[numOfRows, numOfColumns];
            for (int rowIndex = 0; rowIndex < parentOfRowsOfToggles.transform.childCount; rowIndex++)
            {
                Transform rowOfToggles = parentOfRowsOfToggles.transform.GetChild(rowIndex);
                for (int columnIndex = 0; columnIndex < rowOfToggles.childCount; columnIndex++)
                    togglesGrid[rowIndex, columnIndex] = rowOfToggles.GetChild(columnIndex).GetComponentInChildren<Activatable>();
            }
        }

        void UpdateCombinationOnActiveChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Activatable.isActivated))
                UpdateCombination();
        }
    }

    private void UpdateCombination()
    {
        bool wasCorrectCOmbination = isCorrectCombination;
        isCorrectCombination = CheckCombination();

        if (wasCorrectCOmbination == isCorrectCombination)
            return;
        else if (!wasCorrectCOmbination && isCorrectCombination)
            Debug.Log("Correct combination!");
        else if (wasCorrectCOmbination && !isCorrectCombination)
            Debug.Log("You had the correct combination, but now it's wrong...");
    }

    private bool CheckCombination()
    {
        for (int rowIndex = 0; rowIndex < togglesGrid.GetLength(0); rowIndex++)
            for (int columnIndex = 0; columnIndex < togglesGrid.GetLength(1); columnIndex++)
                if (togglesGrid[rowIndex, columnIndex].isActivated != correctTogglesAsRowColumnCoords.Contains(new(rowIndex + 1, columnIndex + 1)))
                    return false;

        return true;
    }
}
