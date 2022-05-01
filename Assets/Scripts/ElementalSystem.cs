using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalSystem : MonoBehaviour
{
    public float getElementalMultiplier(string element1, string element2)
    {
        // Returns whether element1 beats element2
        if (element1.Equals("Water"))
        {
            switch (element2)
            {
                case "Fire":
                    return 1.25f;
                case "Lightning":
                    return 0.75f;
            }
        } else if (element1.Equals("Fire"))
        {
            switch (element2)
            {
                case "Grass":
                    return 1.25f;
                case "Water":
                    return 0.75f;
            }
        }
        else if (element1.Equals("Grass"))
        {
            switch (element2)
            {
                case "Earth":
                    return 1.25f;
                case "Fire":
                    return 0.75f;
            }
        }
        else if (element1.Equals("Earth"))
        {
            switch (element2)
            {
                case "Lightning":
                    return 1.25f;
                case "Grass":
                    return 0.75f;
            }
        }
        else if (element1.Equals("Lightning"))
        {
            switch (element2)
            {
                case "Water":
                    return 1.25f;
                case "Earth":
                    return 0.75f;
            }
        }
        else if (element1.Equals("Light"))
        {
            switch (element2)
            {
                case "Dark":
                    return 1.25f;
            }
        }
        else if (element1.Equals("Dark"))
        {
            switch (element2)
            {
                case "Light":
                    return 1.25f;
            }
        }

        return 1f;
    }
}
