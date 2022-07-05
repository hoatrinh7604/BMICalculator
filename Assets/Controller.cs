using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_InputField age;
    [SerializeField] TMP_Dropdown gender;
    [SerializeField] TMP_InputField height;
    [SerializeField] TMP_InputField weight;

    [SerializeField] TextMeshProUGUI bmi;
    [SerializeField] TextMeshProUGUI bmi_suff;
    [SerializeField] TextMeshProUGUI range;
    [SerializeField] TextMeshProUGUI wfh;
    [SerializeField] TextMeshProUGUI ponderalIndex;

    [SerializeField] GameObject listRangeAdult;
    [SerializeField] GameObject listRangeChild;

    [SerializeField] GameObject listFieldParent;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private int amount = 0;
    private double result = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 1;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
        gender.options.Clear();
        List<string> items = new List<string>();

        items.Add("Male");
        items.Add("Female");

        foreach(var item in items)
        {
            gender.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        gender.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
    }

    private void DropdownitemSelected()
    {
        if (gender.options[gender.value].text == "Male")
        {
            //SwitchToHourRate();
        }
        else
        {
            //SwitchToDayRate();
        }
    }


    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    private bool CheckValidate()
    {
        if (age.text == "" || height.text == "" || weight.text == "")
        {
            return false;
        }

        //return text.All(char.IsDigit);
        return true;
    }


    public void Sum()
    {
        CalWithAdult();
        listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        double age_m = double.Parse(age.text);
        double height_m = double.Parse(height.text);
        double weight_m = double.Parse(weight.text);
        var heightPow = (height_m / 100) * (height_m / 100);
        double bmi_m = weight_m / heightPow;
        bmi.text = bmi_m.ToString("0.00") + " kg/m2";
        bmi_suff.text = GetRange(bmi_m);

        range.text = "18.5 kg/m2 - 25 kg/m2";

        double targetMinHeight = 18.5f * heightPow;
        double targetMaxHeight = 25 * heightPow;

        wfh.text = targetMinHeight.ToString("0.00") + " kgs - " + targetMaxHeight.ToString("0.00") + " kgs";

        var PI = weight_m/(heightPow * (height_m / 100));
        ponderalIndex.text = PI.ToString("0.00") + " kg/m3";

        listRangeChild.SetActive(false);
    }

    private void CalWithChild()
    {
        double age_m = double.Parse(age.text);
        double height_m = double.Parse(height.text);
        double weight_m = double.Parse(weight.text);

        double bmi_m = height_m / ((weight_m / 100) * (weight_m / 100));
        bmi.text = bmi_m.ToString();
        range.text = GetRange(bmi_m);
    }

    private string GetRange(double value)
    {
        if(value < 16)
        {
            bmi_suff.color = listColor[0];
            return "Severe Thinness";
        }
        else if(value >= 16 && value < 17)
        {
            bmi_suff.color = listColor[1];
            return "Moderate Thinness";
        }
        else if (value >= 17 && value < 18.5f)
        {
            bmi_suff.color = listColor[2];
            return "Mild Thinness";
        }
        else if (value >= 18.5 && value < 25)
        {
            bmi_suff.color = listColor[3];
            return "Normal";
        }
        else if (value >= 25 && value < 30)
        {
            bmi_suff.color = listColor[4];
            return "Overweight";
        }
        else if (value >= 30 && value < 35)
        {
            bmi_suff.color = listColor[5];
            return "Obese Class I";
        }
        else if (value >= 35 && value < 40)
        {
            bmi_suff.color = listColor[6];
            return "Obese Class II";
        }
        else
        {
            bmi_suff.color = listColor[7];
            return "Obese Class III";
        }
    }

    public void Continue()
    {
        if(amount==0) return;
        double currentResult = result;
        Clear();
        list[0].GetComponent<TMP_InputField>().text = currentResult.ToString();
        listInt[0] = currentResult;
    }

    public void Clear()
    {
        //amount = 0;
        //result = 0;
        //UpdateAmount();
        //UpdateResult();
        //list.Clear();
        //listInt.Clear();
        //continueButton.interactable = false;
        //addButton.interactable = true;
        //for (int i = 0; i < listFieldParent.transform.childCount; i++)
        //{
        //    Destroy(listFieldParent.transform.GetChild(i).gameObject);
        //}
        listFieldParent.SetActive(false);

        age.text = "";
        gender.value = 0;
        height.text = "";
        weight.text = "";

        calButton.interactable = false;
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
