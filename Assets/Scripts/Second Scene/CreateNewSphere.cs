using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateNewSphere : MonoBehaviour {

    private GameObject sphere;
    private ArrayList listOfSpheres;

    void Start()
    {
        listOfSpheres = new ArrayList();
    }

    public void addNewSphere()
    {
        // delete spheres if there are more then 10 of them on screen
        if (listOfSpheres.Count > 10)
        {
            foreach (GameObject createdSphere in listOfSpheres)
            {
                Destroy(createdSphere);
            }
            listOfSpheres.Clear();
        }
            
        int sliderPosition = (int) GameObject.Find("sphereSizeSlider") .GetComponent<Slider>() .value;

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(Random.value * 300 - Random.value * 350 - 500, Random.value * 400 - 300, 800);
        sphere.transform.localScale = new Vector3(50 * sliderPosition, 50 * sliderPosition, 50 * sliderPosition);

        listOfSpheres.Add(sphere);
    }
}
