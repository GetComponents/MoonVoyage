using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SectionLoader : MonoBehaviour
{
    public static SectionLoader Instance;
    private int currentIndex;
    [SerializeField]
    private int startIndex;
    [SerializeField]
    List<SectionScript> AllSections;
    SectionScript leftSection, middleSection, rightSection;

    [SerializeField]
    List<SectionScript> nearbySections = new List<SectionScript>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //currentIndex = startIndex;
        //middleSection = Instantiate(AllSections[startIndex]);
        //if (startIndex - 1 >= 0)
        //{
        //    leftSection = Instantiate(AllSections[startIndex - 1]);
        //}
        //if (startIndex + 1 <= AllSections.Count)
        //{
        //    rightSection = Instantiate(AllSections[startIndex + 1]);
        //}


        SpawnSectionsViaIndex(AllSections[startIndex].nearbyIndices);
    }

    public void LoadSection(int index)
    {
        if (index < 0 || index > AllSections.Count)
        {
            Debug.Log("Section Index Invalid");
            return;
        }
        SectionScript newSection = GetNearbySectionViaIndex(index);
        //if (newSection == null)
        //{
        //    int[] io = new int[1] { index };
        //    newSection = SpawnSectionsViaIndex(io);
        //}
        if (newSection != null)
        {
            nearbySections = FindOverlappingSections(newSection.nearbyIndices, out List<int> overlappingIndices, out List<int> missingIndices);
            foreach (int missingIndex in missingIndices)
            {
                nearbySections.Add(Instantiate(AllSections[missingIndex]));
            }
        }
        else { Debug.Log("New section is not adjacent"); }












        //if (index > currentIndex)
        //{
        //    if (leftSection != null)
        //        Destroy(leftSection.gameObject);
        //    leftSection = middleSection;
        //    if (rightSection != null)
        //        middleSection = rightSection;
        //    if (index >= AllSections.Count - 1)
        //    {
        //        //rightSection = Instantiate(AllSections[0]).GetComponent<SelectionScript>();
        //    }
        //    else
        //    {
        //        rightSection = Instantiate(AllSections[index + 1]).GetComponent<SectionScript>();
        //    }
        //}
        //if (index < currentIndex)
        //{
        //    if (rightSection != null)
        //        Destroy(rightSection.gameObject);
        //    rightSection = middleSection;
        //    if (leftSection != null)
        //        middleSection = leftSection;
        //    if (index == 0)
        //    {
        //        //GameObject tmp = Instantiate(AllSections[AllSections.Count].gameObject);
        //        //leftSection = Instantiate(AllSections[AllSections.Count - 1]).GetComponent<SelectionScript>();
        //    }
        //    else
        //    {
        //        leftSection = Instantiate(AllSections[index - 1]).GetComponent<SectionScript>();
        //    }
        //}
        //currentIndex = index;
    }

    private void SpawnSectionsViaIndex(int[] indices)
    {
        SectionScript section;
        for (int i = 0; i < indices.Length; i++)
        {
            section = Instantiate(AllSections[indices[i]]);
            nearbySections.Add(section);
        }
    }

    private SectionScript GetNearbySectionViaIndex(int index)
    {
        foreach (SectionScript section in nearbySections)
        {
            if (section.SectionIndex == index)
            {
                return section;
            }
        }
        return null;
    }

    private List<SectionScript> FindOverlappingSections(int[] indices, out List<int> overlappingIndices, out List<int> missingIndices)
    {
        overlappingIndices = new List<int>();
        missingIndices = indices.ToList();
        List<SectionScript> tmp = new List<SectionScript>();

        List<SectionScript> sectionsToDestroy = new List<SectionScript>();
        bool foundIndex = false;

        for (int j = 0; j < nearbySections.Count; j++)
        {
            foundIndex = false;
            for (int i = 0; i < indices.Length; i++)
            {
                if (nearbySections[j].SectionIndex == indices[i])
                {
                    foundIndex = true;
                    tmp.Add(nearbySections[j]);
                    overlappingIndices.Add(indices[i]);
                    missingIndices.Remove(indices[i]);
                    break;
                }
            }
            if (!foundIndex)
            {
                sectionsToDestroy.Add(nearbySections[j]);
            }
        }
        SectionScript[] deadSections = sectionsToDestroy.ToArray();
        for (int i = 0; i < deadSections.Length; i++)
        {
            Destroy(sectionsToDestroy[i].gameObject);
        }
        return tmp;
    }

    //private List<SectionScript> GetNearbySections(int[] indices)
    //{
    //    List<SectionScript> tmp = new List<SectionScript>();
    //    List<SectionScript> sectionsToDestroy = new List<SectionScript>();
    //    bool foundIndex = false;
    //    for (int j = 0; j < nearbySections.Count; j++)
    //    {
    //        foundIndex = false;
    //        for (int i = 0; i < indices.Length; i++)
    //        {
    //            if (nearbySections[j].SectionIndex == indices[i])
    //            {
    //                tmp.Add(nearbySections[j]);
    //                break;
    //            }
    //        }
    //        if (!foundIndex)
    //        {
    //            sectionsToDestroy.Add(nearbySections[j]);
    //        }
    //    }
    //    SectionScript[] deadSections = sectionsToDestroy.ToArray();
    //    for (int i = 0; i < deadSections.Length; i++)
    //    {
    //        Destroy(sectionsToDestroy[i].gameObject);
    //    }
    //    return tmp;
    //}
}
