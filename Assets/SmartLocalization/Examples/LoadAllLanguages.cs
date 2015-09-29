//
//  LoadAllLanguages.cs
//
//
// Written by Niklas Borglund and Jakob Hillerström
//

namespace SmartLocalization
{
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadAllLanguages : MonoBehaviour 
{
	private Dictionary<string,string> currentLanguageValues;
	private List<SmartCultureInfo> availableLanguages;
	private LanguageManager languageManager;
	private Vector2 valuesScrollPosition = Vector2.zero;
	private Vector2 languagesScrollPosition = Vector2.zero;
    private byte languageSign = 0; // 0 - eng, 1 - rus

	void Start () 
	{
		languageManager = LanguageManager.Instance;
		
		SmartCultureInfo systemLanguage = languageManager.GetSupportedSystemLanguage();
		if(systemLanguage != null)
		{
            languageManager.ChangeLanguage(systemLanguage);	
		}
		
		if(languageManager.NumberOfSupportedLanguages > 0)
		{
			currentLanguageValues = languageManager.RawTextDatabase;
			availableLanguages = languageManager.GetSupportedLanguages();
		}
		else
		{
			Debug.LogError("No languages are created!, Open the Smart Localization plugin at Window->Smart Localization and create your language!");
		}

		LanguageManager.Instance.OnChangeLanguage += OnLanguageChanged;
	}

	void OnDestroy()
	{
		if(LanguageManager.HasInstance)
		{
			LanguageManager.Instance.OnChangeLanguage -= OnLanguageChanged;
		}
	}

	void OnLanguageChanged(LanguageManager languageManager)
	{
		currentLanguageValues = languageManager.RawTextDatabase;
	}
	
	void OnGUI() 
	{
		if(languageManager.NumberOfSupportedLanguages > 0)
		{
                if (Application.loadedLevel == 0)
                {
                    Button nextSceneBtn = GameObject.Find("nextSceneBtn").GetComponent<Button>();
                    nextSceneBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.nextScene");

                    Button screenshotBtn = GameObject.Find("screenshotBtn").GetComponent<Button>();
                    screenshotBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.screenshot");

                    Button colorBtn = GameObject.Find("colorBtn").GetComponent<Button>();
                    colorBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.color");
                }
                
                if (Application.loadedLevel == 1)
                {
                    Button nextSceneBtn = GameObject.Find("nextSceneBtn").GetComponent<Button>();
                    nextSceneBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.nextScene");

                    Button screenshotBtn = GameObject.Find("screenshotBtn").GetComponent<Button>();
                    screenshotBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.screenshot");

                    Button newSphereBtn = GameObject.Find("newSphereBtn").GetComponent<Button>();
                    newSphereBtn.GetComponentInChildren<Text>().text = languageManager.GetTextValue("SmartLocalization.sphere");
                }

                
languagesScrollPosition = GUILayout.BeginScrollView (languagesScrollPosition);
			foreach(SmartCultureInfo language in availableLanguages)
			{
				if(GUILayout.Button(language.nativeName, GUILayout.Width(200)))
				{
					languageManager.ChangeLanguage(language);
				}
			}

			GUILayout.EndScrollView();
            }
	}
}
}//namespace SmartLocalization
